using CraftpiaViewSaveData.File;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CraftpiaViewSaveData.CommonConst;

namespace CraftpiaViewSaveData
{
    public partial class MainForm : Form
    {
        byte[] originalData;
        CraftpiaParams convertData;

        public MainForm()
        {
            InitializeComponent();
        }

        #region "データ取得関係"
        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var ocss = files.Where(f => Path.GetExtension(f) == ".db" || Path.GetExtension(f) == ".json");
            if (ocss.Count() != 1) return;

            originalData = ImportFile.Import(ocss.First());
            convertData = ImportFile.GetList(originalData, ocss.First());

            //dgvにセットしていく...
            var itemIndexCount = new Dictionary<string, Dictionary<string,int>>();//アイテム等のときのindexふり用dictionary
            CommonConst.listname.ToList().ForEach(p => itemIndexCount.Add(p, new Dictionary<string, int>()));
            dgv1.Rows.Clear();
            var viewlist = GetResourceFile.GetFile();
            int row = 0;
            //foreach (var data in convertData)
            //{
            //    //以下の構造である
            //    //value -- {charaMakeData,plStatusSaveData,inventorySaveData}
            //    //inventorySaveData -- {equipmentList,buildingList,consumptionList,personalChestList,petList,materialList,petChestList}
            //    //                      └-- itemInBox -- {(item,count) * 最大4枠}
            //    var lastkey = data.Value.keys.Last();
            //    string keystr = String.Join(",", data.Value.keys);

            //    dgv1.Rows.Add();
            //    dgv1.Rows[row].Cells[(int)rowindex.CategoryName].Value = "-";
            //    dgv1.Rows[row].Cells[(int)rowindex.DetailName].Value = lastkey;
            //    dgv1.Rows[row].Cells[(int)rowindex.FullName].Value = keystr;

            //    //名称をtxtから取得
            //    if (viewlist.ContainsKey(data.Value.listName + "." + lastkey))
            //    {
            //        dgv1.Rows[row].Cells[(int)rowindex.DetailName_Ja].Value = viewlist[data.Value.listName + "." + lastkey];
            //    }
            //    //アイテム系の場合
            //    if (data.Value.listName != null && data.Value.listName != "")
            //    {
            //        //カテゴリ名称
            //        dgv1.Rows[row].Cells[0].Value = CommonConst.listnameja[CommonConst.listname.ToList().IndexOf(data.Value.listName)];

            //        //何個目のアイテムか,Noを発番
            //        if (itemIndexCount[data.Value.listName].ContainsKey(keystr))
            //        {
            //            itemIndexCount[data.Value.listName][keystr]++;
            //        }
            //        else
            //        {
            //            itemIndexCount[data.Value.listName].Add(keystr, 1);
            //        }
            //        dgv1.Rows[row].Cells[(int)rowindex.Numbering].Value = itemIndexCount[data.Value.listName][keystr];
            //        data.Value.x = itemIndexCount[data.Value.listName][keystr];
            //    }

            //    dgv1.Rows[row].Cells[(int)rowindex.Value].Value = data.Value.value; //値
            //    dgv1.Rows[row].Cells[(int)rowindex.Index].Value = data.Value.index; //index
            //    dgv1.Rows[row].Cells[(int)rowindex.HexIndex].Value = Convert.ToString(data.Value.index, 16); //index0x
            //    row++;
            //}
        }
        #endregion
        #region "アイテム詳細関係"

        private void panelEquipment_content_Click(object sender, EventArgs e)
        {
            //c#にはLike文が無いので面倒
            var PanelArray1 = panelEquipment.Controls.OfType<Panel>()
                                  .Where(p => System.Text.RegularExpressions.Regex.IsMatch(p.Name, "^p1_.*", RegexOptions.Singleline))
                                  .OrderBy(q => q.TabIndex).ToArray();

            var name = ((Panel)sender).Name;
            var categoryIndex = 1;
            var itemIndex = Convert.ToInt32(name.Split('_').Last());

            DisplayItemDetail(categoryIndex, itemIndex);
        }

        void DisplayItemDetail(int categoryIndex, int itemindex)
        {

        }
        #endregion
    }
}
