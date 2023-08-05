using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class _CPInventorySaveData
    {
        /// CPXList -> ItemInBox -> ItemInBoxValue -> Item
        public Dictionary<string, CPXList<CPItemInBox>> paramsList;
        /// CPXList -> "enchantFragmentList"
        public CPXList<CPEnchant> enchantList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public _CPInventorySaveData()
        {
            paramsList = new Dictionary<string, CPXList<CPItemInBox>>();
            foreach (string s in Enum.GetNames(typeof(CommonConst.itemListName)))
            {
                paramsList.Add(s, new CPXList<CPItemInBox>(s));
            }
            enchantList = new CPXList<CPEnchant>(CommonConst.enchantFragmentList, true);
        }

        /// <summary>
        /// コピー用コンストラクタ
        /// </summary>
        public _CPInventorySaveData(_CPInventorySaveData sc)
        {
            paramsList = new Dictionary<string, CPXList<CPItemInBox>>();
            foreach (var d in sc.paramsList)
            {
                paramsList.Add(d.Key, (CPXList<CPItemInBox>)(d.Value));
            }
            enchantList = sc.enchantList;
        }

        public override string ToString()
        {
            string ret = "{\"" + CommonConst.inventorySaveData + "\":{";
            foreach (var li in paramsList)
            {
                ret += li.Value.ToString() + ",";
            }
            ret = ret.TrimEnd(',') + "}";

            return ret;
        }
    }
}
