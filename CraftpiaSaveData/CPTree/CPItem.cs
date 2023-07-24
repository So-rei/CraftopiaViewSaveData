using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPItem
    {
        public int itemId { get; set; }
        public int itemLevel { get; set; }
        public int[] enchantIds { get; set; } = new int[4];
        public double proficient { get; set; }
        public int petID { get; set; }
        public bool saveLock { get; set; }
        public int bulletNum { get; set; }
        public int bulletId { get; set; }
        public int dataVersion { get; set; }


        public override string ToString()
        {
            string ret = "";
            ret += "\"item\":{";
            ret += "\"itemId\":" + itemId.ToString() + ",";
            ret += "\"enchantIds\":[" + enchantIds[0].ToString() + "," + enchantIds[1].ToString() + "," + enchantIds[2].ToString() + "," + enchantIds[3].ToString() + "],";
            ret += "\"proficient\":" + proficient.ToString() + ",";
            ret += "\"petID\":" + petID.ToString() + ",";
            ret += "\"saveLock\":" + saveLock.ToString() + ",";
            ret += "\"bulletNum\":" + bulletNum.ToString() + ",";
            ret += "\"bulletId\":" + bulletId.ToString() + ",";
            ret += "\"dataVersion\":" + dataVersion.ToString();
            ret += "}";

            return ret;
        }
    }
}
