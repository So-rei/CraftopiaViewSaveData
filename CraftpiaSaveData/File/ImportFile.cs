using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CraftpiaViewSaveData.File
{
    public static class ImportFile
    {
        public static byte[] Import(string filepath)
        {
            //バイナリのまま取る
            using (System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                //ファイルを読み込むバイト型配列を作成する
                byte[] bs = new byte[fs.Length];
                //ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length);
                return bs;
            }
        }

        public static CraftpiaParams GetList(byte[] bytedata, string savePath)
        {
            //string savePath = $@"{Path.GetDirectoryName(filepath)}\{Path.GetFileNameWithoutExtension(filepath)}.json";

            //64kbごとに謎の空白が4byte入っているので飛ばす
            //0～2FFF(先頭30kb)と最後10KBぐらいまではヘッダ？一時領域?なので飛ばす?
            //飛ばした中身は「◯◯List、◯◯List、・・・キャラデータ」の順で並んでるっぽい?
            int bc = bytedata.Count();
            int tmpstart = 0;
            var headerKeysIndex = new Dictionary<int, (int index, string listname)>();
            int listindex = 0;
            var endIndex = bc;
            for (int i = 11000; i < 12288; i++)
            {
                if (bytedata[i] == (byte)0)
                {
                    if (bytedata.Skip(i).Take(8).Select(p => Convert.ToInt32(p)).Sum() == 0)
                        tmpstart = i;
                }
            }
            for (int i = tmpstart; i < bc; i++)
            {
                if (bytedata[i] == Convert.ToByte('L') && new string(bytedata.Skip(i).Take(4).Select(p => (char)p).ToArray()) == "List")
                {
                    int idx = i;
                    while (bytedata[idx - 1] != '"')
                        idx--;
                    string lname = new string(bytedata.Skip(idx).Take(4 + i - idx).Select(p => (char)p).ToArray());
                    if (CommonConst.listname.ToList().IndexOf(lname) != -1)
                    {
                        headerKeysIndex.Add(listindex, (idx - 1, new string(bytedata.Skip(idx).Take(4 + i - idx).Select(p => (char)p).ToArray())));
                        listindex++;
                    }
                }
            }
            for (int i = bc - 1024; i > 0; i--)
            {
                if (bytedata[i] == Convert.ToByte('P') && new string(bytedata.Skip(i).Take(10).Select(p => (char)p).ToArray()) == "PlayerSave")
                {
                    headerKeysIndex.Add(listindex, (i, "PlayerSave"));
                    for (int j = i; j < bc; j++)
                    {
                        if (bytedata[j] == (byte)0)
                        {
                            headerKeysIndex.Add(listindex + 1, (j, "PlayerSave:End"));
                            endIndex = j;
                            break;
                        }
                    }
                    break;
                }
            }

            //一番上のツリー
            var mainTree = new CraftpiaParams(0, "MOM");
            //各"◯◯List"のツリー
            int listi = 1;
            //List以下
            var pParams = new Stack<CraftpiaParams>();
            pParams.Push(new CraftpiaParams(headerKeysIndex.First().Value.index, "mom-0"));
            var tmpindex = -1;
            bool commaflg = false;
            string tmpValue = "", elemname = ""; //内容,項目名

            for (int idx = headerKeysIndex.First().Value.index; idx <= endIndex; idx++)
            {
                try
                {
                    //次の◯◯Listに来た
                    //前のList確定したので保存,状態リセット,次のListの準備
                    if (headerKeysIndex[listi].index == idx)
                    {
                        if (headerKeysIndex[listi].listname == "PlayerSave:End")
                        {
                            //playersaveだけは構造上、直下の値がそのまま入る
                            mainTree.innerParams.Add(pParams.Pop());
                            break; //最後なので終了
                        }
                        else
                        {
                            //親List - 子　を結合してmainTreeに追加
                            var childParam = new List<CraftpiaParams>();
                            int child_count = pParams.Count() - 2;
                            for (int t = 0; t < child_count; t++)
                                childParam.Add(pParams.Pop());
                            var parentParam = pParams.Pop();
                            parentParam.innerParams = childParam;
                            mainTree.innerParams.Add(parentParam);
                        }

                        //次の準備
                        tmpindex = -1;
                        commaflg = false;
                        tmpValue = elemname = "";

                        pParams.Clear();
                        pParams.Push(new CraftpiaParams(headerKeysIndex.First().Value.index, "mom-" + listi.ToString()));
                        listi++;
                    }

                    if (idx % 65536 < 4) continue;  //64kbごとに謎の空白が4byte入っているので飛ばす
                    var b = bytedata[idx];
                    if (b < 32 || b > 126) continue; //制御文字は飛ばす

                    switch ((char)b)
                    {
                        case '{': //Param一層追加
                            {
                                if (tmpValue == "") //名前なし{}はオブジェクト作る
                                {
                                    var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                    pParams.Push(ne);
                                }
                                else
                                    pParams.Peek().name = tmpValue;

                                tmpindex = idx + 1; //次のidx  
                                elemname = tmpValue = "";
                                break;
                            }
                        case '}': //直前のParamsが確定、一層減
                            {
                                if (tmpValue != "") //コレクション最後の,省略}だった場合のみ、中身の確定処理
                                {
                                    var bf = pParams.Pop();
                                    bf.value = tmpValue;
                                    bf.oldlength = tmpValue.Length;
                                    pParams.Peek().innerParams.Add(bf);
                                }
                                //一層浅く    
                                var bf2 = pParams.Pop();
                                pParams.Peek().innerParams.Add(bf2);
                                tmpindex = idx + 1; //次のidx    
                                elemname = tmpValue = "";
                                break;
                            }
                        case ':': //名称確定
                            {
                                var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                pParams.Push(ne);
                                tmpindex = idx + 1; //次のidx  
                                elemname = tmpValue = "";
                                break;
                            }
                        case ',':
                            {
                                //配列[]の中の,である
                                if (pParams.Peek().isArray && tmpValue != "")
                                {
                                    //配列内の名前なし項目の場合は、paramsが生成されていないので、独立で作成する
                                    var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                    pParams.Peek().innerParams.Add(ne);
                                }
                                else if (tmpValue != "")//配列が終わった直後の,は何もしない
                                {
                                    //直前の項目の値が確定
                                    var bf3 = pParams.Pop();
                                    bf3.value = tmpValue;
                                    pParams.Peek().innerParams.Add(bf3);
                                }
                                tmpindex = idx + 1; //次のidx  
                                elemname = tmpValue = "";
                                break;
                            }
                        case '[': //Param追加(array)
                            {
                                //var ne = new CraftpiaParams(tmpindex, elemname, tmpValue, true);
                                pParams.Peek().isArray = true;
                                tmpindex = idx + 1; //次のidx  
                                elemname = tmpValue = "";
                                break;
                            }
                        case ']': //直前のParamsが確定、一層減
                            {
                                if (tmpValue != "") //コレクション最後の]だった場合のみ、paramsが生成されていないので、独立で作成する
                                {
                                    var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                    pParams.Peek().innerParams.Add(ne);
                                }
                                //一層浅く    
                                var bf4 = pParams.Pop();
                                pParams.Peek().innerParams.Add(bf4);
                                tmpindex = idx + 1; //次のidx    
                                elemname = tmpValue = "";
                                break;
                            }
                        case '"':
                            {
                                commaflg = !commaflg;
                                break;
                            }
                        default: //その他内容またはキー名称
                            {
                                if (commaflg)
                                    elemname += ((char)b).ToString();
                                else
                                    tmpValue += ((char)b).ToString();
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    //エラーの出たリストはすべて飛ばす（仮）
                    Console.WriteLine("ERR." + headerKeysIndex[listi].listname + ":" + idx + "[" + (listi == 0 ? "0" : headerKeysIndex[listi - 1].index.ToString()) + "～" + headerKeysIndex[listi].index.ToString() + "]");
                    tmpindex = -1;
                    commaflg = false;
                    tmpValue = elemname = "";

                    pParams.Clear();
                    pParams.Push(new CraftpiaParams(headerKeysIndex.First().Value.index, "mom-" + listi.ToString()));

                    idx = headerKeysIndex[listi].index;
                    listi++;
                }
            }

            return mainTree;
        }
    }
}