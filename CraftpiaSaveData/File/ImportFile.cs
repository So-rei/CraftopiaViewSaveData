using System;
using System.Collections.Generic;
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
        //public CraftpiaParams Import(string filepath)
        public static byte[] Import(string filepath)
        {

            ////"C:\test\1.txt"をShift-JISコードとして開く
            //using (System.IO.StreamReader sr = new System.IO.StreamReader(@filepath, System.Text.Encoding.GetEncoding("shift_jis")))
            //{
            //    //内容をすべて読み込む
            //    string s = sr.ReadToEnd();
            //    return ConvertJson(s, savePath);
            //}

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
            var startListIndex = new List<int>();
            var endIndex = 0;
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
                    startListIndex.Add(idx - 1);
                }
            }
            for (int i = bc - 1024; i > 0; i--)
            {
                if (bytedata[i] == Convert.ToByte('P') && new string(bytedata.Skip(i).Take(10).Select(p => (char)p).ToArray()) == "PlayerSave")
                {
                    startListIndex.Add(i);
                    break;
                }
            }
            for (int i = startListIndex.Last(); i < bc; i++)
            {
                if (bytedata[i] == (byte)0)
                {
                    endIndex = i;
                    break;
                }
            }

            var cparams = new Dictionary<int, CraftpiaParams>();
            var layername = new Stack<(int kagi,int kakko,string key) >(); //[の層、{の層、キー
            var tmpindex = -1;
            var kagicnt = 0;
            var kakkocnt = 0;
            bool commaflg = false;
            string tmpname = "", elemname = "";
            int listindex = 0;

            for (int idx = startListIndex.First(); idx < endIndex; idx++)
            {
                if (idx % 65536 < 4)
                {
                    //64kbごとに謎の空白が4byte入っているので飛ばす
                    continue;
                }

                //次の◯◯Listに来た...状態リセット
                if (startListIndex[listindex + 1] == idx)
                {
                    tmpindex = -1;
                    kagicnt = 0;
                    kakkocnt = 0;
                    commaflg = false;
                    tmpname = elemname = "";
                    layername.Clear();
                    listindex++;
                }


                var b = bytedata[idx];
                //制御文字は飛ばす
                if (b < 32 || b > 126)
                {
                    continue;
                }

                switch ((char)b)
                {
                    case '{':
                        if (elemname == ""　&& ((char)b).ToString() != ":")
                            layername.Push((kagicnt, kakkocnt, "*"));//名前なし{}が複数の時対策({}配列)
                        //一層深く
                        kakkocnt++;
                        tmpindex = idx + 1;
                        //if (kagiflg) kagikakkoflg = true;
                        tmpname = ""; //名前クリア
                        break;
                    case '}'://tmp確定
                        if (layername.Peek().kakko == kakkocnt && tmpname != "") //コレクション最後の,省略}だった場合のみ内容確定処理
                        {
                            cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p.key)), 0, tmpname));
                            elemname = tmpname = "";
                            tmpindex = idx + 1; //次のidx    
                            layername.Pop();
                        }
                        kakkocnt--;
                        //if (kagiflg) kagikakkoflg = false;
                        //一層浅く             
                        layername.Pop();
                        break;
                    case ':':
                        break;
                    case ',':
                        //if (kagiflg && !kagikakkoflg) //[の中身の場合(その中にある{内}ではない)は配列なので、そのまま保存する
                        if (tmpname != "" && layername.Peek().kagi != kagicnt)
                        {
                            tmpname += ",";
                            continue;
                        }
                        if (tmpname == "")
                        {
                            tmpindex = idx + 1; //次のidx  
                            continue;//(}や]直後の,はとばす
                        }
                        //内容確定
                        cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p.key)), 0, tmpname));
                        layername.Pop();
                        elemname = tmpname = "";
                        tmpindex = idx + 1; //次のidx  
                        break;
                    case '[':
                        kagicnt++;
                        //kagikakkoflg = false;
                        //kagiflg = true;//[の配列フラグon
                        tmpindex = idx + 1;
                        break;
                    case ']':
                        //[配列のときは内容確定
                        //if (kagiflg)
                        if (layername.Peek().kagi == kagicnt) //コレクション最後の]だった場合のみ内容確定処理
                        {
                            if (layername.Peek().key == "*")
                                layername.Pop();
                            cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p.key)), 0, tmpname));
                            elemname = tmpname = "";
                            tmpindex = idx + 1; //次のidx    
                            //kagiflg = false;
                            //kagikakkoflg = false;
                            layername.Pop();
                        }
                        kagicnt--;
                        //layername.Pop();
                        break;
                    case '"':
                        commaflg = !commaflg;
                        Console.WriteLine(idx);
                        if (!commaflg)
                        {
                            //名称確定
                            layername.Push((kagicnt, kakkocnt, elemname));
                            elemname = "";
                            tmpname = "";
                        }
                        break;
                    default: //その他内容またはキー名称
                        if (commaflg)
                            elemname += ((char)b).ToString();
                        else
                            tmpname += ((char)b).ToString();
                        break;
                }
            }
            //不要なバイナリをカットしてjsonファイルにできないか？

            return cparams;
        }
        public static Dictionary<int, CraftpiaParams> GetList20230712(byte[] bytedata, string savePath)
        {
            //string savePath = $@"{Path.GetDirectoryName(filepath)}\{Path.GetFileNameWithoutExtension(filepath)}.json";

            //最初の情報は除く
            //「PlayerSave」の手前と「Ingame」の手前のデータがなんか壊れてるっぽい?ので飛ばすフラグを作る
            var skipflg = new List<int>();//奇数個目～偶数個目までが壊れてるゾーン
            skipflg.Add(0);
            int bc = bytedata.Count();
            for (int i = 0; i < bc; i++)
            {
                if (i % 65536 < 4)
                {
                    //64kbごとに謎の空白が4byte入っているので飛ばす
                    continue;
                }
                //制御文字
                if (skipflg.Count() % 2 == 0 && (bytedata[i] < 32 || bytedata[i] > 126))
                {
                    skipflg.Add(i);
                }
                if (bytedata[i] == Convert.ToByte('P'))
                {
                    if (i < bc - 11 && new string(bytedata.Skip(i).Take(11).Select(p => (char)p).ToArray()) == "PlayerSave{")
                        skipflg.Add(i + 11);
                }
                if (bytedata[i] == Convert.ToByte('I'))
                {
                    if (i < bc - 6 && new string(bytedata.Skip(i).Take(6).Select(p => (char)p).ToArray()) == "InGame")
                        skipflg.Add(i + 6);
                }
            }

            int skip_flg_idx = 0;

            var cparams = new Dictionary<int, CraftpiaParams>();
            var layername = new Stack<string>();
            var tmpname = "";
            var tmpindex = -1;
            bool kagiflg = false, kagikakkoflg = false;
            for (int idx = 0; idx < bc; idx++)
            {
                if (idx % 65536 < 4)
                {
                    //64kbごとに謎の空白が4byte入っているので飛ばす
                    continue;
                }
                if (skipflg[skip_flg_idx] <= idx)
                {
                    if (skip_flg_idx == skipflg.Count() - 1) break;
                    idx++;
                    tmpname = "";
                    kagiflg = false;
                    idx = skipflg[skip_flg_idx + 1] - 1;
                    layername.Clear();
                    tmpindex = idx + 1;
                    skip_flg_idx += 2;
                    continue;
                }
                var b = bytedata[idx];

                switch ((char)b)
                {
                    case '{':
                        //一層深く
                        tmpindex = idx + 1;
                        if (kagiflg) kagikakkoflg = true;
                        break;
                    case '}'://tmp確定
                        if (kagiflg) kagikakkoflg = false;
                        if (tmpname != "") //コレクション最後の}だった場合は内容確定
                        {
                            cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p)), 0, tmpname));
                            tmpname = "";
                            tmpindex = idx + 1; //次のidx    
                        }
                        //一層浅く             
                        layername.Pop();
                        break;
                    case ':':
                        //名称確定
                        layername.Push(tmpname);
                        tmpname = "";
                        break;
                    case ',':
                        if (kagiflg && !kagikakkoflg) //[の中身の場合(その中にある{内}ではない)は配列なので、そのまま保存する
                        {
                            tmpname += ",";
                            continue;
                        }  
                        if (tmpname == "")
                        {
                            tmpindex = idx + 1; //次のidx  
                            continue;//(}や]直後の,はとばす
                        }
                        //内容確定
                        cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p)), 0, tmpname));
                        layername.Pop();
                        tmpname = "";
                        tmpindex = idx + 1; //次のidx  
                        break;
                    case '[':
                        kagikakkoflg = false;
                        kagiflg = true;//[の配列フラグon
                        tmpindex = idx + 1;
                        break;
                    case ']':
                        //[配列のときは内容確定
                        if (kagiflg)
                        {
                            cparams.Add(tmpindex, new CraftpiaParams(tmpindex, string.Join("-", layername.Reverse().Select(p => p)), 0, tmpname));
                            tmpname = "";
                            tmpindex = idx + 1; //次のidx    
                            kagiflg = false;
                            kagikakkoflg = false;
                        }
                        layername.Pop();
                        break;
                    default: //その他内容またはキー名称
                        tmpname += ((char)b).ToString();
                        break;
                }
            }
            //不要なバイナリをカットしてjsonファイルにできないか？

            return cparams;
        }
    }
}