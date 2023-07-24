using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPItemInBoxValue
    {
        public CPItem item { get; set; } = new CPItem();
        public int count { get; set; }
        public int[] assignedHotkeySlot { get; set; } = new int[3] { -1, -1, -1 }; //装備系のときのみ
        public int assignedEquipSlot { get; set; }

        public override string ToString()
        {
            string ret = "{";
            ret += "\"item\":" + item.ToString() + ",";
            ret += "\"count\":" + count.ToString() + ",";
            if (assignedHotkeySlot[0] != -1)
            {
                ret += "\"assignedHotkeySlot\":[" + assignedHotkeySlot[0].ToString() + "," + assignedHotkeySlot[1].ToString() + "," + assignedHotkeySlot[2].ToString() + "],";
            }
            ret += "\"assignedEquipSlot\":" + assignedEquipSlot.ToString();
            ret += "}";

            return ret;
        }
    }
}
