using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPEnchant
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("個数")]
        public int value { get; set; }
        [DisplayName("エンチャント名")]
        public string enchantName { get; set; }

        public CPEnchant(int _id, int _value)
        {
            id = _id;
            value = _value;
            enchantName = "";
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
