using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPItemInBox
    {
        public List<CPItemInBoxValue> Value { get; set; } = new List<CPItemInBoxValue>();

        public override string ToString()
        {
            string ret = "\"itemInBox\":[";
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
