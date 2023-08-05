using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.File
{
    public class ClsResource
    {
        public string id { get; set; }
        public string value { get; set; }
        public string param { get; set; }

        public ClsResource(string id, string value, string param)
        {
            this.id = id;
            this.value = value;
            this.param = param;
        }
    }
}
