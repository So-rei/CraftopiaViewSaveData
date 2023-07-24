using CraftpiaViewSaveData.CPTree;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CraftpiaViewSaveData.CommonConst;

namespace CraftpiaViewSaveData.NestParams
{
    public static class ConvertCraftpiaParams
    {
        public static CraftpiaParams JsonStrToCraftpiaParams(string value, string savePath)
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
                                var ne = new CraftpiaParams(tmpindex, elemname, tmpValue);
                                pParams.Push(ne);

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

                                // 同じ名称のものがあったら、xを加算する
                                int samecount = pParams.Peek().innerParams.Count(p => p.name == bf2.name);
                                bf2.x += samecount;

                                pParams.Peek().innerParams.Add(bf2);
                                tmpindex = idx + 1; //次のidx    
                                elemname = tmpValue = "";
                                break;
                            }
                        case ':': //名称確定
                            {
                                break;
                            }
                        case ',':
                            {
                                if (tmpValue != "")
                                {
                                    if (pParams.Peek().isArray)
                                    {
                                        //配列内の名前なし項目の場合は、paramsが生成されていないので、独立で作成する
                                        var ne = new CraftpiaParams(tmpindex, "ary", tmpValue);

                                        // 同じ名称のものがあったら、xを加算する
                                        int samecount = pParams.Peek().innerParams.Count(p => p.name == "ary");
                                        ne.x += samecount;

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
                                    var ne = new CraftpiaParams(tmpindex, "ary", tmpValue);

                                    // 同じ名称のものがあったら、xを加算する
                                    int samecount = pParams.Peek().innerParams.Count(p => p.name == "ary");
                                    ne.x += samecount;

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

        /// <summary>
        /// json文字列から取ったCraftpiaParamsをデータ構造にはめ込む
        /// ※アイテム系統のみ
        /// </summary>
        /// <param name="cparams"></param>
        /// <returns></returns>
        public static _CPInventorySaveData CraftpiaParamsToCPTree(CraftpiaParams cparams)
        {
            var inventoryparams = cparams.innerParams[0].innerParams.First();
            if (inventoryparams.name != CommonConst.inventorySaveData) return null;

            var ret = new _CPInventorySaveData();
            foreach (var child in inventoryparams.innerParams)
            {
                if (!ret.paramsList.TryGetValue(child.name, out var cpx)) continue;
                if (!child.TryGetChildParams(out var cc)) continue;

                foreach (var items in cc)
                {
                    var itemInBox = new CPItemInBox();

                    foreach (var _iteminbox in items)
                    {
                        var itemInBoxValue = new CPItemInBoxValue();
                        foreach (var _iteminboxvalue in _iteminbox)
                        {
                            //item,count,assignedHotkeySlot,assignedEquipSlot の順
                            //item詳細
                            foreach (var _item in _iteminboxvalue.innerParams[0])
                            {
                                switch (_item.name)
                                {
                                    case "itemId":
                                        itemInBoxValue.item.itemId = Convert.ToInt32(_item.value);
                                        break;
                                    case "itemLevel":
                                        itemInBoxValue.item.itemLevel = Convert.ToInt32(_item.value);
                                        break;
                                    case "enchantIds":
                                        itemInBoxValue.item.enchantIds = Array.ConvertAll(_item.Select(p => p.value).ToArray(), e => (int)Convert.ChangeType(e, typeof(int)));
                                        break;
                                    case "proficient":
                                        itemInBoxValue.item.proficient = Convert.ToDouble(_item.value);
                                        break;
                                    case "petID":
                                        itemInBoxValue.item.petID = Convert.ToInt32(_item.value);
                                        break;
                                    case "saveLock":
                                        itemInBoxValue.item.saveLock = Convert.ToBoolean(_item.value);
                                        break;
                                    case "bulletNum":
                                        itemInBoxValue.item.bulletNum = Convert.ToInt32(_item.value);
                                        break;
                                    case "bulletId":
                                        itemInBoxValue.item.bulletId = Convert.ToInt32(_item.value);
                                        break;
                                    case "dataVersion":
                                        itemInBoxValue.item.dataVersion = Convert.ToInt32(_item.value);
                                        break;
                                }
                            }
                            itemInBoxValue.count = Convert.ToInt32(_iteminboxvalue.innerParams[1].value);

                            //assignedHotkeySlotは装備系のみ
                            if (child.name == itemListName.equipmentList.ToString() && _iteminboxvalue.innerParams[2].Count() == 3)
                            {
                                itemInBoxValue.assignedHotkeySlot = Array.ConvertAll(_iteminboxvalue.innerParams[2].Select(p => p.value).ToArray(), e => (int)Convert.ChangeType(e, typeof(int)));
                            }
                            itemInBoxValue.assignedEquipSlot = Convert.ToInt32(_iteminboxvalue.innerParams[3].value);
                        }
                        itemInBox.Value.Add(itemInBoxValue);
                    }

                    cpx.Value.Add(itemInBox);
                }
            }

            return ret;
        }
    }
}
