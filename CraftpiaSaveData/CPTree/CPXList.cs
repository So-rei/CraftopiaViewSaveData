using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftpiaViewSaveData.CPTree
{
    public class CPXList<T> where T : class
    {
        public virtual List<T> Child { get; set; } = new List<T>();     //中身入れ子<T>        
        public string ListName { get; set; }                    //json内部名
        public bool isArray { get; set; }                       //配列かどうか

        public CPXList(string _listname = "", bool _isArray = false) 
        {
            ListName = _listname;
            isArray = _isArray;
        }

        public override string ToString() => ToStr();

        public virtual string ToStr()
        {
            string ret = "\"" + ListName + "\":[";
            if (isArray)
                ret += string.Join(",", Child.Select(p => p.ToString()).ToArray());
            else
                ret += string.Join(",", Child.Select(p => "{" + p.ToString() + "}").ToArray());

            return ret + "]";
        }
    }
}
