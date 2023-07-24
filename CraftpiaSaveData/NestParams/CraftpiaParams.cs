using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData.NestParams
{
    public class CraftpiaParams : IEnumerable<CraftpiaParams>
    {
        public int index { get; private set; }                      //何文字目にあったか
        public string name { get; set; }                            //名称(名前なしの場合もある)
        public List<CraftpiaParams> innerParams { get; set; }       //内容が入れ子や配列の場合
        public bool isArray { get; set; }                           //内容が配列[]の場合True,入れ子{}の場合False
        public int x { get; set; }                                  //"itemInBox"など、おなじ項目名で中身が複数ある場合、アイテムの何番目なのかのインデックス
        public string value { get; set; }                           //内容が値の場合
        public int oldlength { get; private set; }                  //内容が値の場合、入っていた内容の長さ

        public CraftpiaParams(int _index, string _name, string _value = "", bool _isArray = false)
        {
            this.index = _index;
            this.name = _name;
            this.value = _value;
            this.oldlength = value.Length;
            this.innerParams = new List<CraftpiaParams>();
            this.isArray = _isArray;
            this.x = 1;
        }

        public override string ToString()
        {
            string s = name + "," + x.ToString() + "," + value.ToString();
            return s;
        }

        public bool TryGetChildParams(out List<CraftpiaParams> childParams)
        {
            childParams = this.innerParams;
            return this.innerParams != null;
        }

        public Dictionary<string, CraftpiaParams> GetChildParamsString(CraftpiaParams targetParam = null)
        {
            if (targetParam == null)
                targetParam = this;

            var ret = new Dictionary<string, CraftpiaParams>();
            foreach (var p1 in targetParam.innerParams)
            {
                var c2 = GetChildParamsString(p1);
                foreach (var dic in c2)
                {
                    string keyname = targetParam.name + "_" + targetParam .x + "-" +dic.Key;
                    ret.Add(keyname, dic.Value);
                }
            }

            if (ret.Count() == 0)
                ret.Add(targetParam.name + "_" + targetParam.x, targetParam);

            return ret;
        }

        public IEnumerator<CraftpiaParams> GetEnumerator()
        {
            foreach (var p in innerParams)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.innerParams;
        }
    }
}
