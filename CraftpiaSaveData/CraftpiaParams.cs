using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData
{
    public class CraftpiaParams
    {
        public int index { get; private set; }//何文字目にあったか
        public List<string> keys { get; private set; } //項目の名称
        public string listName { get; private set; }//項目がどのリストに属する項目であるか

        public int x { get; set; }//"itemInBox"などの場合、アイテムの何番目なのかのインデックス
        public string value { get; set; } //入っている内容(変更したときはここに上書き)
        public int oldlength { get; private set; }//入っていた内容の長さ

        public CraftpiaParams(int index, string key, int x, string value)
        {
            this.index = index;
            this.keys = key.Split('-').Select(p => p.Trim('"').Trim(',').Trim('"')).ToList();
            this.x = x;
            this.value = value;
            this.oldlength = value.Length;

            foreach (var k in keys)
            {
                if (CommonConst.listname.ToList().LastIndexOf(k) != -1)
                {
                    this.listName = k;
                    break;
                }
            }
        }
    }
}
