using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPXList
    {
        public List<CPItemInBox> Value { get; set; }

        public string ListName { get; private set; }

        public CPXList(string _listname) => ListName = _listname;

        public override string ToString()
        {
            string ret = "\""+ ListName + "\":[";
            foreach (var item in Value)
            {
                ret += "{" + item.ToString() + "},";
            }
            ret.TrimEnd(',');

            ret += "]";

            return ret;
        }
    }
}
