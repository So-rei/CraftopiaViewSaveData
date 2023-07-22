using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class _CPInventorySaveData
    {
        public List<CPXList> paramsList;

        public _CPInventorySaveData()
        {
            foreach (string s in Enum.GetNames(typeof(CommonConst.itemListName)))
            {
                paramsList.Add(new CPXList(s));
            }
        }

        public override string ToString()
        {
            string ret = "\"inventorySaveData\":{";
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
