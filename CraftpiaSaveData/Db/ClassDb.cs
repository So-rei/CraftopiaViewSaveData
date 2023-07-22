using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData
{
    public class ClassDb
    {
        public string id { get; set; }
        public string value { get; set; }

        public ClassDb(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
