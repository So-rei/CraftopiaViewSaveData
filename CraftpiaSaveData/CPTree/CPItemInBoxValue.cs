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
        public int[] assignedHotkeySlot { get; set; } //装備系のみ。装備以外だと配列ごとnullになる
        public int assignedEquipSlot { get; set; }

        public override string ToString()
        {
            //if (count == 0) return ""; //空の名前なし{}を排除する

            string ret = "";
            ret += "\"item\":" + item.ToString() + ",";
            ret += "\"count\":" + count.ToString() + ",";
            if (assignedHotkeySlot != null)
            {
                ret += "\"assignedHotkeySlot\":[" + assignedHotkeySlot[0].ToString() + "," + assignedHotkeySlot[1].ToString() + "," + assignedHotkeySlot[2].ToString() + "],";
            }
            else
            {
                ret += "\"assignedHotkeySlot\":null,";
            }
            ret += "\"assignedEquipSlot\":" + assignedEquipSlot.ToString();

            return ret;
        }
    }
}
