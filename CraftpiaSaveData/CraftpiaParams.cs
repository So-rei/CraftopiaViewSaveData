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
        public string name { get; private set; } //名称(名前なしの場合もある)
        public List<CraftpiaParams> innerParams { get; private set; }        //内容が入れ子や配列の場合
        public bool isArray { get; private set; }                            //内容が配列[]の場合True,入れ子{}の場合False
        public int x { get; set; }//"itemInBox"などの場合、アイテムの何番目なのかのインデックス
        public string value { get; set; } //内容が値の場合
        public int oldlength { get; set; }//内容が値の場合、入っていた内容の長さ

        public CraftpiaParams(int _index, string _name, string value = "", bool _isArray = false)
        {
            this.index = _index;
            this.name = _name;
            this.innerParams = new List<CraftpiaParams>();
            this.isArray = _isArray;
            this.x = -1;
        }

        public override string ToString()
        {
            string s = name + "," + x.ToString() + "," + value.ToString();
            return s;
        }
    }
}
