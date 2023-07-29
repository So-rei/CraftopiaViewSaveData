using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPItemInBox
    {
        public List<CPItemInBoxValue> Child { get; set; } = new List<CPItemInBoxValue>();

        public override string ToString()
        {
            string ret = "\"itemInBox\":[";
            foreach (CPItemInBoxValue item in Child)
            {
                ret += "{" + item.ToString() + "},";
            }
            ret = ret.TrimEnd(',');

            ret += "]";

            return ret;
        }
    }
}
