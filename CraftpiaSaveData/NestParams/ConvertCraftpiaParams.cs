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
        public static CraftpiaParams JsonStrToCraftpiaParams(string value)
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
                //情報がある場合、情報をクラスにセットしていく...
                if (!child.TryGetChildParams(out var cc)) continue;

                //エンチャント型
                if (child.name == CommonConst.enchantFragmentList)
                {
                    foreach (var enchants in cc)
                    {
                        var enchant = new CPEnchant(enchants.x, Convert.ToInt32(enchants.value));
                        ret.enchantList.Child.Add(enchant);
                    }

                    continue;
                }

                //アイテム型
                //表示したい●●List以外は飛ばす
                if (!ret.paramsList.TryGetValue(child.name, out var cpx)) continue;

                foreach (var items in cc)
                {
                    var itemInBox = new CPItemInBox();

                    foreach (var _iteminbox in items)
                    {
                        foreach (var _iteminboxvalue in _iteminbox)
                        {
                            var itemInBoxValue = new CPItemInBoxValue();
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
                            //if (child.name == itemListName.equipmentList.ToString() && _iteminboxvalue.innerParams[2].Count() == 3)
                            if (_iteminboxvalue.innerParams[2].Count() == 3)
                            {
                                itemInBoxValue.assignedHotkeySlot = Array.ConvertAll(_iteminboxvalue.innerParams[2].Select(p => p.value).ToArray(), e => (int)Convert.ChangeType(e, typeof(int)));
                            }
                            itemInBoxValue.assignedEquipSlot = Convert.ToInt32(_iteminboxvalue.innerParams[3].value);

                            itemInBox.Child.Add(itemInBoxValue);
                        }
                    }

                    cpx.Child.Add(itemInBox);
                }
            }

            return ret;
        }

        /// <summary>
        /// importantListとpersonalChestListの間にunlockedPet,quickSlotSkill
        /// personalChestListの後にenchantFragmentList、skillSaveDatas、missionSaveDatas、mainMissionCleared、questSaveDatas、missionCategoryTookDatas、statisticalSaveDatas
        /// petSaveDatas,recipeUnlocked,licenseUnlocked,totalPlayTime,creativeTutorial,soulOrbPicked,soulOrbExchangeCount,equipMysetSaveDatas,lookMysetSaveDatas,skillMysetSaveDatas,mapFogDraw,visualTutorialSaveData,timelineSaveData
        /// が入っているのでそれを適宜セットし、最終的にDBに入れる文字列にする
        /// </summary>
        /// <param name="cpTree">変更した_CPInventorySaveData</param>
        /// <returns>Json形式String</returns>
        public static string ConcatOtherParams(string bf, _CPInventorySaveData cpTree)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"" + CommonConst.inventorySaveData + "\":{");

            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.equipmentList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.buildingList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.consumptionList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.materialList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.petList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.importantList.ToString()])).ToString() + ",");

            int idx1 = bf.IndexOf("unlockedPet");
            int idx2 = bf.IndexOf(itemListName.personalChestList.ToString());
            string hazama = bf.Substring(idx1 - 1, idx2 - idx1);
            sb.Append(hazama);

            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.personalChestList.ToString()])).ToString() + ",");
            sb.Append(((CPXList<CPItemInBox>)(cpTree.paramsList[itemListName.petChestList.ToString()])).ToString() + ",");

            sb.Append(((CPXList<CPEnchant>)(cpTree.enchantList)).ToString() + "},");
            int idx3 = bf.IndexOf("skillSaveDatas");
            sb.Append(bf.Substring(idx3 - 1));

            return sb.ToString();
        }
    }
}
