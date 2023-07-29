using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class _CPInventorySaveData
    {
        public Dictionary<string, CPXList> paramsList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public _CPInventorySaveData()
        {
            paramsList = new Dictionary<string, CPXList>();
            foreach (string s in Enum.GetNames(typeof(CommonConst.itemListName)))
            {
                paramsList.Add(s, new CPXList(s));
            }
        }
        /// <summary>
        /// コピー用コンストラクタ
        /// </summary>
        public _CPInventorySaveData(_CPInventorySaveData sc)
        {
            foreach (var d in sc.paramsList)
            {
                paramsList.Add(d.Key, d.Value);
            }
        }

        public override string ToString()
        {
            string ret = "\"" + CommonConst.inventorySaveData + "\":{";
            foreach (var li in paramsList)
            {
                ret += li.ToString() + ",";
            }
            ret.TrimEnd(',');

            ret += "}";

            return ret;
        }
    }
}
