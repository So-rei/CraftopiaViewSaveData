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

        public static Dictionary<int, CraftpiaParams> GetList(byte[] bytedata, string savePath)
        {
            //string savePath = $@"{Path.GetDirectoryName(filepath)}\{Path.GetFileNameWithoutExtension(filepath)}.json";

            //64kbごとに謎の空白が4byte入っているので飛ばす
            //0～2FFF(先頭30kb)と最後10KBぐらいまではヘッダ？一時領域?なので飛ばす?
            //飛ばした中身は「◯◯List、◯◯List、・・・キャラデータ」の順で並んでるっぽい?
            int bc = bytedata.Count();
            int tmpstart = 0;
            var startListIndex = new Dictionary<int, (int index, string listname)>();
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
                    startListIndex.Add(listindex, (idx - 1, new string(bytedata.Skip(idx).Take(4 + i - idx).Select(p => (char)p).ToArray())));
                    listindex++;
                }
            }
            for (int i = bc - 1024; i > 0; i--)
            {
                if (bytedata[i] == Convert.ToByte('P') && new string(bytedata.Skip(i).Take(10).Select(p => (char)p).ToArray()) == "PlayerSave")
                {
                    startListIndex.Add(listindex, (i, "PlayerSave"));
                    for (int j = i; j < bc; j++)
                    {
                        if (bytedata[j] == (byte)0)
                        {
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
            var pParams = new Stack<List<CraftpiaParams>>();
            pParams.Push(new List<CraftpiaParams>() { new CraftpiaParams(startListIndex.First().Value.index, "mom-0") });
            var tmpindex = -1;
            bool commaflg = false;
            string tmpValue = "", elemname = ""; //内容,項目名

            for (int idx = startListIndex.First().Value.index; idx < endIndex; idx++)
            {
                //次の◯◯Listに来た
                //前のList確定したので保存,状態リセット,次のListの準備
                if (startListIndex[listi].index == idx)
                {
                    tmpindex = -1;
                    commaflg = false;
                    tmpValue = elemname = "";

                    var childParam = pParams.Pop().First(); //List直下は必ず１個
                    mainTree.innerParams.Add(childParam);
                    pParams.Push(new List<CraftpiaParams>() { new CraftpiaParams(startListIndex.First().Value.index, "mom-" + listi.ToString()) });
                    listi++;
                }

                if (idx % 65536 < 4) continue;  //64kbごとに謎の空白が4byte入っているので飛ばす
                var b = bytedata[idx];
                if (b < 32 || b > 126) continue; //制御文字は飛ばす

                switch ((char)b)
                {
                    case '{': //Param一層追加
                        {
                            var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                            pParams.Push(new List<CraftpiaParams>() { ne });
                            tmpindex = idx + 1; //次のidx  
                            elemname = tmpValue = "";
                            break;
                        }
                    case '}': //直前のParamsが確定、一層減
                        {
                            if (pParams.Peek().Count() == 1 && tmpValue != "") //コレクション最後の,省略}だった場合のみ、中身の確定処理
                            {
                                var bf = pParams.Pop();
                                bf.Last().value = tmpValue;
                                bf.Last().oldlength = tmpValue.Length;
                                pParams.Peek().Last().innerParams.AddRange(bf);
                            }
                            //一層浅く    
                            var bf2 = pParams.Pop();
                            pParams.Peek().Last().innerParams.AddRange(bf2);
                            tmpindex = idx + 1; //次のidx    
                            elemname = tmpValue = "";
                            break;
                        }
                    case ':': //名称確定
                        {
                            var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                            pParams.Push(new List<CraftpiaParams>() { ne });
                            tmpindex = idx + 1; //次のidx  
                            elemname = tmpValue = "";
                            break;
                        }
                    case ',':
                        {
                            //配列[]の中の,である
                            if (pParams.Peek().Last().isArray)
                            {
                                //[{～},{～}]　の","は特に何もしない
                                //if ((char)bytedata[idx - 1] == '}') continue;
                                //if (tmpValue == "") continue;

                                //配列内の名前なし項目の場合は、paramsが生成されていないので、独立で作成する
                                var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                pParams.Peek().Last().innerParams.Add(ne);
                            }
                            else
                            {
                                //直前の項目の値が確定
                                var bf3 = pParams.Pop();
                                bf3.Last().value = tmpValue;
                                pParams.Peek().Last().innerParams.AddRange(bf3);
                            }
                            tmpindex = idx + 1; //次のidx  
                            elemname = tmpValue = "";
                            break;
                        }
                    case '[': //Param一層追加(array)
                        {
                            var ne = new CraftpiaParams(tmpindex, elemname, tmpValue, true);
                            pParams.Push(new List<CraftpiaParams>() { ne });
                            tmpindex = idx + 1; //次のidx  
                            elemname = tmpValue = "";
                            break;
                        }
                    case ']': //直前のParamsが確定、一層減
                        {
                            if (pParams.Peek().Count() == 1 && tmpValue != "") //コレクション最後の]だった場合のみ、中身の確定処理
                            {
                                var bf = pParams.Pop();
                                bf.Last().value = tmpValue;
                                bf.Last().oldlength = tmpValue.Length;
                                pParams.Peek().Last().innerParams.AddRange(bf);
                            }
                            //一層浅く    
                            var bf4 = pParams.Pop();
                            pParams.Peek().Last().innerParams.AddRange(bf4);
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

            //最後のキャラデータをAdd
            mainTree.innerParams.Add(pParams.Pop().First());

            return null;
        }
    }
}