using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData
{
    public class StrToCraftpiaParams
    {
        public static CraftpiaParams GetList(string value, string savePath)
        {
            //jsonファイル to 入れ子クラス
            int bc = value.Count();

            //List以下
            var pParams = new Stack<CraftpiaParams>();
            pParams.Push(new CraftpiaParams(-1, "MOM"));
            var tmpindex = -1;
            bool commaflg = false;
            string tmpValue = "", elemname = ""; //内容,項目名

            for (int idx = 0; idx < bc; idx++)
            {
                try
                {
                    var b = value[idx];

                    switch ((char)b)
                    {
                        case '{': //Param一層追加
                            {
                                if (true) //名前なし{}はオブジェクト作る
                                //if (tmpValue == "") //名前なし{}はオブジェクト作る
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
                                if (tmpValue != "") //コレクション最後の,省略}だった場合のみ、中身の"〇〇":▲ の確定処理
                                {
                                    var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                    pParams.Peek().innerParams.Add(ne);
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
                                //var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                //pParams.Push(ne);
                                //tmpindex = idx + 1; //次のidx  
                                //elemname = tmpValue = "";
                                break;
                            }
                        case ',':
                            {
                                if (tmpValue != "")
                                {
                                    if (pParams.Peek().isArray)
                                    {
                                        //配列内の名前なし項目の場合は、paramsが生成されていないので、独立で作成する
                                        var ne = new CraftpiaParams(tmpindex, "", tmpValue);
                                        pParams.Peek().innerParams.Add(ne);
                                    }
                                    else if (elemname != "")
                                    {
                                        //１項目の"〇〇":▲ が終わったあとである
                                        var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                        pParams.Peek().innerParams.Add(ne);
                                    }
                                }
                                tmpindex = idx + 1; //次のidx  
                                elemname = tmpValue = "";
                                break;
                            }
                        case '[': //Param追加(array)
                            {
                                //var ne = new CraftpiaParams(tmpindex, elemname, tmpValue, true);
                                var ne = new CraftpiaParams(tmpindex, elemname, tmpValue, true);
                                pParams.Push(ne);
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
                                var bf2 = pParams.Pop();
                                pParams.Peek().innerParams.Add(bf2);
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
                    Console.WriteLine("ERR");
                    tmpindex = -1;
                    commaflg = false;
                    tmpValue = elemname = "";

                    pParams.Clear();
                    pParams.Push(new CraftpiaParams(idx, "mom-"));
                }
            }

            return pParams.Last();
        }
    }
}
