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
    internal static class GetResourceFile
    {
        public static Dictionary<string , ClsResource> GetFile(string filename)
        {
            var dic = new Dictionary<string , ClsResource>();

            var apppath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + "\\" + filename;
            //"C:\test\1.txt"をShift-JISコードとして開く
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@apppath, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                //内容を読み込む
                while (sr.Peek() > -1)
                {
                    var line = sr.ReadLine();
                    if (line == "" || line[0] == '#') continue;

                    var sx = line.Split(',');
                    dic.Add(sx[0], new ClsResource(sx[0], sx.Count() > 1 ? sx[1] : "", sx.Count() > 2 ? sx[2] : ""));
                }
            }
            return dic;
        }
    }
}
