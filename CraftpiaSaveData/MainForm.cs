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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

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
            public String ItemEtc { get; set; }
            public ComboBoxItemSet(string id, string name, string etc = "")
            {
                ItemValue = Convert.ToInt32(id);
                ItemDisp = id + " : " + name;
                ItemEtc = etc;
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

            setCboBox(cboEnchant1_1, clist);
            setCboBox(cboEnchant1_2, clist);
            setCboBox(cboEnchant1_3, clist);
            setCboBox(cboEnchant1_4, clist);
            setCboBox(cboEnchant2_1, clist);
            setCboBox(cboEnchant2_2, clist);
            setCboBox(cboEnchant2_3, clist);
            setCboBox(cboEnchant2_4, clist);
            setCboBox(cboEnchant3_1, clist);
            setCboBox(cboEnchant3_2, clist);
            setCboBox(cboEnchant3_3, clist);
            setCboBox(cboEnchant3_4, clist);
            setCboBox(cboEnchant4_1, clist);
            setCboBox(cboEnchant4_2, clist);
            setCboBox(cboEnchant4_3, clist);
            setCboBox(cboEnchant4_4, clist);
        }
        void ItemComboBoxSet()
        {
            List<ComboBoxItemSet> clist = new List<ComboBoxItemSet>();

            var itemParams = GetResourceFile.GetFile("ItemParams.txt");
            foreach (var d in itemParams)
            {
                clist.Add(new ComboBoxItemSet(d.Key, d.Value));
            }

            setCboBox(cboItem1, clist);
            setCboBox(cboItem2, clist);
            setCboBox(cboItem3, clist);
            setCboBox(cboItem4, clist);
        }
        void setCboBox(ComboBox cbo, List<ComboBoxItemSet> source)
        {
            cbo.DataSource = new List<ComboBoxItemSet>(source);
            cbo.DisplayMember = "ItemDisp";
            cbo.ValueMember = "ItemValue";
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

            originalData = CrudDb.Read(ocss.First());
            convertData = ConvertCraftpiaParams.JsonStrToCraftpiaParams(originalData.Where(p => p.id == PPSave_ID_InGame).First().value, ocss.First());
            CPInventorySaveData = CraftpiaParamsToCPTree(convertData);

            //1ページ1番目をロード
            SetItemDetailToDisp(selectType.ToString(), 1);
#if DEBUGX
            HiddenViewString();
#endif
        }
        #endregion

#if DEBUGX
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
            SetItemDetailToDisp(selectType.ToString(), 1);
        }

        private void panel_ItemNo_Click(object sender, EventArgs e)
        {
            //c#にはLike文が無いので面倒
            var PanelArray1 = panelItemNo.Controls.OfType<Panel>()
                                  .Where(p => System.Text.RegularExpressions.Regex.IsMatch(p.Name, "^p1_.*", RegexOptions.Singleline))
                                  .OrderBy(q => q.TabIndex).ToArray();

            var itemIndex = Convert.ToInt32(new string((((Panel)sender).Name).Skip(3).ToArray())) - 1;

            SetItemDetailToDisp(selectType.ToString(), itemIndex);
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetItemDetailToDisp(selectType.ToString(), 1);
        }

        void SetItemDetailToDisp(string categoryName, int itemindex)
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

            //アイテム1(左上)--------------------------------------------------------------------
            //基本属性
            textItemId1.Text = target.Value[0].item.itemId.ToString();
            textItemLevel1.Text = target.Value[0].item.itemLevel.ToString();
            textEnchantIds1_1.Text = target.Value[0].item.enchantIds[0].ToString();
            textEnchantIds1_2.Text = target.Value[0].item.enchantIds[1].ToString();
            textEnchantIds1_3.Text = target.Value[0].item.enchantIds[2].ToString();
            textEnchantIds1_4.Text = target.Value[0].item.enchantIds[3].ToString();
            textProficient1.Text = target.Value[0].item.proficient.ToString();
            textPetID1.Text = target.Value[0].item.petID.ToString();
            chkSaveLock1.Checked = target.Value[0].item.saveLock;
            textBulletNum1.Text = target.Value[0].item.bulletNum.ToString();
            textBulletId1.Text = target.Value[0].item.bulletId.ToString();
            chkDataVersion1.Checked = target.Value[0].item.dataVersion == 0;
            //個数など外部属性
            textCount1.Text = target.Value[0].count.ToString();
            textAssignedHotkeySlot1_1.Text = target.Value[0].assignedHotkeySlot[0].ToString();
            textAssignedHotkeySlot1_2.Text = target.Value[0].assignedHotkeySlot[1].ToString();
            textAssignedHotkeySlot1_3.Text = target.Value[0].assignedHotkeySlot[2].ToString();
            textAssignedEquipSlot1.Text = target.Value[0].assignedEquipSlot.ToString();
            // コンボボックス連動
            cboItem1.SelectedValue = target.Value[0].item.itemId;
            cboEnchant1_1.SelectedValue = target.Value[0].item.enchantIds[0];
            cboEnchant1_2.SelectedValue = target.Value[0].item.enchantIds[1];
            cboEnchant1_3.SelectedValue = target.Value[0].item.enchantIds[2];
            cboEnchant1_4.SelectedValue = target.Value[0].item.enchantIds[3];
            //アイテム2(右上)--------------------------------------------------------------------
            if (target.Value.Count() > 1)
            {
                //基本属性
                textItemId2.Text = target.Value[1].item.itemId.ToString();
                textItemLevel2.Text = target.Value[1].item.itemLevel.ToString();
                textEnchantIds2_1.Text = target.Value[1].item.enchantIds[0].ToString();
                textEnchantIds2_2.Text = target.Value[1].item.enchantIds[1].ToString();
                textEnchantIds2_3.Text = target.Value[1].item.enchantIds[2].ToString();
                textEnchantIds2_4.Text = target.Value[1].item.enchantIds[3].ToString();
                textProficient2.Text = target.Value[1].item.proficient.ToString();
                textPetID2.Text = target.Value[1].item.petID.ToString();
                chkSaveLock2.Checked = target.Value[1].item.saveLock;
                textBulletNum2.Text = target.Value[1].item.bulletNum.ToString();
                textBulletId2.Text = target.Value[1].item.bulletId.ToString();
                chkDataVersion2.Checked = target.Value[1].item.dataVersion == 0;
                //個数など外部属性
                textCount2.Text = target.Value[1].count.ToString();
                textAssignedHotkeySlot2_1.Text = target.Value[1].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot2_2.Text = target.Value[1].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot2_3.Text = target.Value[1].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot2.Text = target.Value[1].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem2.SelectedValue = target.Value[1].item.itemId;
                cboEnchant2_1.SelectedValue = target.Value[1].item.enchantIds[0];
                cboEnchant2_2.SelectedValue = target.Value[1].item.enchantIds[1];
                cboEnchant2_3.SelectedValue = target.Value[1].item.enchantIds[2];
                cboEnchant2_4.SelectedValue = target.Value[1].item.enchantIds[3];
            }
            //アイテム3(左下)--------------------------------------------------------------------
            if (target.Value.Count() > 2)
            {
                //基本属性
                textItemId3.Text = target.Value[2].item.itemId.ToString();
                textItemLevel3.Text = target.Value[2].item.itemLevel.ToString();
                textEnchantIds3_1.Text = target.Value[2].item.enchantIds[0].ToString();
                textEnchantIds3_2.Text = target.Value[2].item.enchantIds[1].ToString();
                textEnchantIds3_3.Text = target.Value[2].item.enchantIds[2].ToString();
                textEnchantIds3_4.Text = target.Value[2].item.enchantIds[3].ToString();
                textProficient3.Text = target.Value[2].item.proficient.ToString();
                textPetID3.Text = target.Value[2].item.petID.ToString();
                chkSaveLock3.Checked = target.Value[2].item.saveLock;
                textBulletNum3.Text = target.Value[2].item.bulletNum.ToString();
                textBulletId3.Text = target.Value[2].item.bulletId.ToString();
                chkDataVersion3.Checked = target.Value[2].item.dataVersion == 0;
                //個数など外部属性
                textCount3.Text = target.Value[2].count.ToString();
                textAssignedHotkeySlot3_1.Text = target.Value[2].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot3_2.Text = target.Value[2].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot3_3.Text = target.Value[2].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot3.Text = target.Value[2].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem3.SelectedValue = target.Value[2].item.itemId;
                cboEnchant3_1.SelectedValue = target.Value[2].item.enchantIds[0];
                cboEnchant3_2.SelectedValue = target.Value[2].item.enchantIds[1];
                cboEnchant3_3.SelectedValue = target.Value[2].item.enchantIds[2];
                cboEnchant3_4.SelectedValue = target.Value[2].item.enchantIds[3];
            }
            //アイテム3(左下)--------------------------------------------------------------------
            if (target.Value.Count() > 3)
            {
                //基本属性
                textItemId4.Text = target.Value[3].item.itemId.ToString();
                textItemLevel4.Text = target.Value[3].item.itemLevel.ToString();
                textEnchantIds4_1.Text = target.Value[3].item.enchantIds[0].ToString();
                textEnchantIds4_2.Text = target.Value[3].item.enchantIds[1].ToString();
                textEnchantIds4_3.Text = target.Value[3].item.enchantIds[2].ToString();
                textEnchantIds4_4.Text = target.Value[3].item.enchantIds[3].ToString();
                textProficient4.Text = target.Value[3].item.proficient.ToString();
                textPetID4.Text = target.Value[3].item.petID.ToString();
                chkSaveLock4.Checked = target.Value[3].item.saveLock;
                textBulletNum4.Text = target.Value[3].item.bulletNum.ToString();
                textBulletId4.Text = target.Value[3].item.bulletId.ToString();
                chkDataVersion3.Checked = target.Value[3].item.dataVersion == 0;
                //個数など外部属性
                textCount4.Text = target.Value[3].count.ToString();
                textAssignedHotkeySlot4_1.Text = target.Value[3].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot4_2.Text = target.Value[3].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot4_3.Text = target.Value[3].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot4.Text = target.Value[3].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem4.SelectedValue = target.Value[3].item.itemId;
                cboEnchant4_1.SelectedValue = target.Value[3].item.enchantIds[0];
                cboEnchant4_2.SelectedValue = target.Value[3].item.enchantIds[1];
                cboEnchant4_3.SelectedValue = target.Value[3].item.enchantIds[2];
                cboEnchant4_4.SelectedValue = target.Value[3].item.enchantIds[3];
            }
        }
        #endregion
    }
}
