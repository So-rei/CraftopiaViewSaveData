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
using static CraftpiaViewSaveData.NestParams.ConvertCraftpiaParams;
using CraftpiaViewSaveData.NestParams;
using CraftpiaViewSaveData.File;
using CraftpiaViewSaveData.CPTree;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace CraftpiaViewSaveData
{
    public partial class MainForm : Form
    {
        List<ClassDb> originalData;
        CraftpiaParams convertData;
        _CPInventorySaveData CPInventorySaveData;
        itemListName selectType { get { return (itemListName)tabControl1.SelectedIndex; } }
        public class ComboBoxItemSet
        {
            public int ItemValue { get; set; }
            public String ItemDisp { get; set; }
            public ComboBoxItemSet(string id, string name)
            {
                ItemValue = Convert.ToInt32(id);
                ItemDisp = id + " : " + name;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            EnchantComboBoxSet();
            ItemComboBoxSet();
        }

        void EnchantComboBoxSet()
        {
            List<ComboBoxItemSet> clist = new List<ComboBoxItemSet>();

            var enchantParams = GetResourceFile.GetFile("EnchantParams.txt");
            foreach (var d in enchantParams)
            {
                clist.Add(new ComboBoxItemSet(d.Key, d.Value));
            }

            cboEnchant1.DataSource = new List<ComboBoxItemSet>(clist);
            cboEnchant1.DisplayMember = "ItemDisp";
            cboEnchant1.ValueMember = "ItemValue";
            cboEnchant2.DataSource = new List<ComboBoxItemSet>(clist);
            cboEnchant2.DisplayMember = "ItemDisp";
            cboEnchant2.ValueMember = "ItemValue";
            cboEnchant3.DataSource = new List<ComboBoxItemSet>(clist);
            cboEnchant3.DisplayMember = "ItemDisp";
            cboEnchant3.ValueMember = "ItemValue";
            cboEnchant4.DataSource = new List<ComboBoxItemSet>(clist);
            cboEnchant4.DisplayMember = "ItemDisp";
            cboEnchant4.ValueMember = "ItemValue";
        }
        void ItemComboBoxSet()
        {
            List<ComboBoxItemSet> clist = new List<ComboBoxItemSet>();

            var itemParams = GetResourceFile.GetFile("ItemParams.txt");
            foreach (var d in itemParams)
            {
                clist.Add(new ComboBoxItemSet(d.Key, d.Value));
            }

            cboItem.DataSource = clist;
            cboItem.DisplayMember = "ItemDisp";
            cboItem.ValueMember = "ItemValue";
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

            //originalData = ImportFile.Import(ocss.First());
            //convertData = ImportFile.GetList(originalData, ocss.First());
            originalData = CrudDb.Read(ocss.First());
            convertData = ConvertCraftpiaParams.JsonStrToCraftpiaParams(originalData.Where(p => p.id == PPSave_ID_InGame).First().value, ocss.First());
            CPInventorySaveData = CraftpiaParamsToCPTree(convertData);

#if DEBUG
            HiddenViewString();
#endif
        }
        #endregion

#if DEBUG
        //画面で確認用
        private void HiddenViewString()
        {
            //dgvにセットしていく...
            Dictionary<string, CraftpiaParams> hiddenViewString = convertData.GetChildParamsString();
            dgv1.Rows.Clear();

            int row = 0;
            foreach (var data in hiddenViewString)
            {
                var dataTree = data.Key.Split('-').ToList();

                dgv1.Rows.Add();
                dgv1.Rows[row].Cells[(int)rowindex.CategoryName].Value = dataTree[2];
                dgv1.Rows[row].Cells[(int)rowindex.DetailName].Value = data.Value.name;
                dgv1.Rows[row].Cells[(int)rowindex.FullName].Value = data.Key;

                //何個目のアイテムか,Noを発番
                var no_str = "";
                foreach (var d in dataTree)
                {
                    if (int.TryParse(d, out int i))
                        no_str += "-" + i.ToString();
                }
                dgv1.Rows[row].Cells[(int)rowindex.Numbering].Value = no_str == "" ? "1" : no_str.TrimStart('-');

                dgv1.Rows[row].Cells[(int)rowindex.Value].Value = data.Value.value; //値
                dgv1.Rows[row].Cells[(int)rowindex.Index].Value = data.Value.index; //index
                dgv1.Rows[row].Cells[(int)rowindex.HexIndex].Value = Convert.ToString(data.Value.index, 16); //index0x
                row++;
            }
        }
#endif

        #region "アイテム詳細関係"
        private void label1_Click(object sender, EventArgs e)
        {
            DisplayItemDetail(selectType.ToString(), 1, 1);
        }

        private void panelEquipment_content_Click(object sender, EventArgs e)
        {
            //c#にはLike文が無いので面倒
            var PanelArray1 = panelEquipment.Controls.OfType<Panel>()
                                  .Where(p => System.Text.RegularExpressions.Regex.IsMatch(p.Name, "^p1_.*", RegexOptions.Singleline))
                                  .OrderBy(q => q.TabIndex).ToArray();

            var itemIndex = Convert.ToInt32(new string((((Panel)sender).Name).Skip(3).ToArray())) - 1;

            DisplayItemDetail(selectType.ToString(), itemIndex, 1);
        }

        void DisplayItemDetail(string categoryName, int itemindex, int iteminboxindex)
        {
            if (CPInventorySaveData == null)
            {
                MessageBox.Show("セーブデータがセットされていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            //アイテム所持数未開放
            if (CPInventorySaveData.paramsList[categoryName].Value.Count() <= itemindex)
                return;

            var target = CPInventorySaveData.paramsList[categoryName].Value[itemindex];
            textItemId.Text = target.Value[0].item.itemId.ToString();
            textItemLevel.Text = target.Value[0].item.itemLevel.ToString();
            textEnchantIds_1.Text = target.Value[0].item.enchantIds[0].ToString();
            textEnchantIds_2.Text = target.Value[0].item.enchantIds[1].ToString();
            textEnchantIds_3.Text = target.Value[0].item.enchantIds[2].ToString();
            textEnchantIds_4.Text = target.Value[0].item.enchantIds[3].ToString();
            textProficient.Text = target.Value[0].item.proficient.ToString();
            textPetID.Text = target.Value[0].item.petID.ToString();
            chkSaveLock.Checked = target.Value[0].item.saveLock;
            textBulletNum.Text = target.Value[0].item.bulletNum.ToString();
            textBulletId.Text = target.Value[0].item.bulletId.ToString();
            chkDataVersion.Checked = target.Value[0].item.dataVersion == 0;

            textCount.Text = target.Value[0].count.ToString();
            textAssignedHotkeySlot1.Text = target.Value[0].assignedHotkeySlot[0].ToString();
            textAssignedHotkeySlot2.Text = target.Value[0].assignedHotkeySlot[1].ToString();
            textAssignedHotkeySlot3.Text = target.Value[0].assignedHotkeySlot[2].ToString();
            textAssignedEquipSlot.Text = target.Value[0].assignedEquipSlot.ToString();

            // コンボボックス連動
            cboItem.SelectedValue = target.Value[0].item.itemId;
            cboEnchant1.SelectedValue = target.Value[0].item.enchantIds[0];
            cboEnchant2.SelectedValue = target.Value[0].item.enchantIds[1];
            cboEnchant3.SelectedValue = target.Value[0].item.enchantIds[2];
            cboEnchant4.SelectedValue = target.Value[0].item.enchantIds[3];
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayItemDetail(selectType.ToString(), 1, 1);
        }
    }
    }
