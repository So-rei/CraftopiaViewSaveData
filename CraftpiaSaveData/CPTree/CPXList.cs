using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPXList<T> where T : class
    {
        public List<T> Child { get; set; } = new List<T>();

        public string ListName { get; set; }

        public CPXList(string _listname = "") 
        {
            ListName = _listname;
        }

        public override string ToString()
        {
            string ret = "\""+ ListName + "\":[";
            foreach (T item in Child)
                ret += "{" + item.ToString() + "},";

            return ret.TrimEnd(',') + "]";
        }
    }
}
