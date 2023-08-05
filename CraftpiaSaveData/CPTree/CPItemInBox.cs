using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    /// <summary>
    /// CPXList -> ItemInBox -> ItemInBoxValue -> Item
    /// </summary>
    public class CPItemInBox : CPXList<CPItemInBoxValue>
    {
        public List<CPItemInBoxValue> Child { get; set; } = new List<CPItemInBoxValue>();
        //public string ListName { get; private set; }

        public CPItemInBox()
        {
            ListName = "itemInBox";
        }

        //public override string ToString()
    }
}
