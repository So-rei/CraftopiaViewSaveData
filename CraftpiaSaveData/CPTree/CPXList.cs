using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPXList
    {
        public List<CPItemInBox> Child { get; set; } = new List<CPItemInBox>();

        public string ListName { get; private set; }

        public CPXList(string _listname)
        {
            ListName = _listname;
        }

        public override string ToString()
        {
            string ret = "\""+ ListName + "\":[";
            foreach (CPItemInBox item in Child)
            {
                ret += "{" + item.ToString() + "},";
            }
            ret = ret.TrimEnd(',') + "]";

            return ret;
        }
    }
}
