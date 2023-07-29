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
        string dbPath { get; set; }
        List<ClassDb> originalData;
        CraftpiaParams convertData;
        _CPInventorySaveData CPInventorySaveDataBackUp;
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
        /// <summary>
        /// イニシャライズ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            EnchantComboBoxSet();
            ItemComboBoxSet();
        }

        //コンボボックス設定（エンチャント）
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
        //コンボボックス設定（アイテム）
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
            dbPath = ocss.First();

            originalData = CrudDb.Read(dbPath);
            convertData = ConvertCraftpiaParams.JsonStrToCraftpiaParams(originalData.Where(p => p.id == PPSave_ID_InGame).First().value);
            CPInventorySaveDataBackUp = ConvertCraftpiaParams.CraftpiaParamsToCPTree(convertData);
            CPInventorySaveData = new _CPInventorySaveData(CPInventorySaveDataBackUp);

            //アイテム上限数（解放も込み）を視覚的にわかるようにする
            setDispView();
            //1ページ1番目をロード
            setItemDetailToDisp(selectType.ToString(), 0);
#if DEBUGX
            HiddenViewString();
#endif
        }
        #endregion

        //アイテム上限数（解放も込み）を視覚的にわかるようにする
        private void setDispView()
        {
            int page_limit = CPInventorySaveData.paramsList[selectType.ToString()].Child.Count();
            Color colOK = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            Color colNG = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            p1_1.BackColor = page_limit > 0 ? colOK : colNG;
            p1_2.BackColor = page_limit > 1 ? colOK : colNG;
            p1_3.BackColor = page_limit > 2 ? colOK : colNG;
            p1_4.BackColor = page_limit > 3 ? colOK : colNG;
            p1_5.BackColor = page_limit > 4 ? colOK : colNG;
            p1_6.BackColor = page_limit > 5 ? colOK : colNG;
            p1_7.BackColor = page_limit > 6 ? colOK : colNG;
            p1_8.BackColor = page_limit > 7 ? colOK : colNG;
            p1_9.BackColor = page_limit > 8 ? colOK : colNG;
            p1_10.BackColor = page_limit > 9 ? colOK : colNG;
            p1_11.BackColor = page_limit > 10 ? colOK : colNG;
            p1_12.BackColor = page_limit > 11 ? colOK : colNG;
            p1_13.BackColor = page_limit > 12 ? colOK : colNG;
            p1_14.BackColor = page_limit > 13 ? colOK : colNG;
            p1_15.BackColor = page_limit > 14 ? colOK : colNG;
            p1_16.BackColor = page_limit > 15 ? colOK : colNG;
            p1_17.BackColor = page_limit > 16 ? colOK : colNG;
            p1_18.BackColor = page_limit > 17 ? colOK : colNG;
            p1_19.BackColor = page_limit > 18 ? colOK : colNG;
            p1_20.BackColor = page_limit > 19 ? colOK : colNG;
            p1_21.BackColor = page_limit > 20 ? colOK : colNG;
            p1_22.BackColor = page_limit > 21 ? colOK : colNG;
            p1_23.BackColor = page_limit > 22 ? colOK : colNG;
            p1_24.BackColor = page_limit > 23 ? colOK : colNG;
            p1_25.BackColor = page_limit > 24 ? colOK : colNG;
            p1_26.BackColor = page_limit > 25 ? colOK : colNG;
            p1_27.BackColor = page_limit > 26 ? colOK : colNG;
            p1_28.BackColor = page_limit > 27 ? colOK : colNG;
            p1_29.BackColor = page_limit > 28 ? colOK : colNG;
            p1_30.BackColor = page_limit > 29 ? colOK : colNG;
            p1_31.BackColor = page_limit > 30 ? colOK : colNG;
            p1_32.BackColor = page_limit > 31 ? colOK : colNG;
            p1_33.BackColor = page_limit > 32 ? colOK : colNG;
            p1_34.BackColor = page_limit > 33 ? colOK : colNG;
            p1_35.BackColor = page_limit > 34 ? colOK : colNG;
            p1_36.BackColor = page_limit > 35 ? colOK : colNG;
            p1_37.BackColor = page_limit > 36 ? colOK : colNG;
            p1_38.BackColor = page_limit > 37 ? colOK : colNG;
            p1_39.BackColor = page_limit > 38 ? colOK : colNG;
            p1_40.BackColor = page_limit > 39 ? colOK : colNG;
        }
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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //アイテム属性タブ遷移
            if (selectType.ToString() == itemListName.petChestList.ToString())
            {
                MessageBox.Show("ペットチェストは現在設定不可です。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //アイテム上限数（解放も込み）を視覚的にわかるようにする
            setDispView();
            setItemDetailToDisp(selectType.ToString(), 0);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            //アイテム選択
            setItemDetailToDisp(selectType.ToString(), 0);
        }

        private void panel_ItemNo_Click(object sender, EventArgs e)
        {
            //アイテム選択1-40
            //c#にはLike文が無いので面倒
            var PanelArray1 = panelItemNo.Controls.OfType<Panel>()
                                  .Where(p => System.Text.RegularExpressions.Regex.IsMatch(p.Name, "^p1_.*", RegexOptions.Singleline))
                                  .OrderBy(q => q.TabIndex).ToArray();

            var itemIndex = Convert.ToInt32(new string((((Panel)sender).Name).Skip(3).ToArray())) - 1;

            setItemDetailToDisp(selectType.ToString(), itemIndex);
        }
        #endregion
        #region "個別アイテム関係"

        /// <summary>
        /// アイテムデータ　→　画面
        /// </summary>
        /// <param name="categoryName">属性名</param>
        /// <param name="itemindex">アイテムNo（左上１行目から１，２，・・・）</param>
        void setItemDetailToDisp(string categoryName, int itemindex)
        {
            if (CPInventorySaveData == null)
            {
                MessageBox.Show("セーブデータがセットされていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            //アイテム所持数未開放
            if (CPInventorySaveData.paramsList[categoryName].Child.Count() <= itemindex)
                return;

            var target = CPInventorySaveData.paramsList[categoryName].Child[itemindex];

            //アイテム1(左上)--------------------------------------------------------------------
            //基本属性
            textItemId1.Text = target.Child[0].item.itemId.ToString();
            textItemLevel1.Text = target.Child[0].item.itemLevel.ToString();
            textEnchantIds1_1.Text = target.Child[0].item.enchantIds[0].ToString();
            textEnchantIds1_2.Text = target.Child[0].item.enchantIds[1].ToString();
            textEnchantIds1_3.Text = target.Child[0].item.enchantIds[2].ToString();
            textEnchantIds1_4.Text = target.Child[0].item.enchantIds[3].ToString();
            textProficient1.Text = target.Child[0].item.proficient.ToString();
            textPetID1.Text = target.Child[0].item.petID.ToString();
            chkSaveLock1.Checked = target.Child[0].item.saveLock;
            textBulletNum1.Text = target.Child[0].item.bulletNum.ToString();
            textBulletId1.Text = target.Child[0].item.bulletId.ToString();
            chkDataVersion1.Checked = target.Child[0].item.dataVersion == 0;
            //個数など外部属性
            textCount1.Text = target.Child[0].count.ToString();
            textAssignedHotkeySlot1_1.Text = target.Child[0].assignedHotkeySlot[0].ToString();
            textAssignedHotkeySlot1_2.Text = target.Child[0].assignedHotkeySlot[1].ToString();
            textAssignedHotkeySlot1_3.Text = target.Child[0].assignedHotkeySlot[2].ToString();
            textAssignedEquipSlot1.Text = target.Child[0].assignedEquipSlot.ToString();
            // コンボボックス連動
            cboItem1.SelectedValue = target.Child[0].item.itemId;
            cboEnchant1_1.SelectedValue = target.Child[0].item.enchantIds[0];
            cboEnchant1_2.SelectedValue = target.Child[0].item.enchantIds[1];
            cboEnchant1_3.SelectedValue = target.Child[0].item.enchantIds[2];
            cboEnchant1_4.SelectedValue = target.Child[0].item.enchantIds[3];
            //アイテム2(右上)--------------------------------------------------------------------
            if (target.Child.Count() > 1)
            {
                //基本属性
                textItemId2.Text = target.Child[1].item.itemId.ToString();
                textItemLevel2.Text = target.Child[1].item.itemLevel.ToString();
                textEnchantIds2_1.Text = target.Child[1].item.enchantIds[0].ToString();
                textEnchantIds2_2.Text = target.Child[1].item.enchantIds[1].ToString();
                textEnchantIds2_3.Text = target.Child[1].item.enchantIds[2].ToString();
                textEnchantIds2_4.Text = target.Child[1].item.enchantIds[3].ToString();
                textProficient2.Text = target.Child[1].item.proficient.ToString();
                textPetID2.Text = target.Child[1].item.petID.ToString();
                chkSaveLock2.Checked = target.Child[1].item.saveLock;
                textBulletNum2.Text = target.Child[1].item.bulletNum.ToString();
                textBulletId2.Text = target.Child[1].item.bulletId.ToString();
                chkDataVersion2.Checked = target.Child[1].item.dataVersion == 0;
                //個数など外部属性
                textCount2.Text = target.Child[1].count.ToString();
                textAssignedHotkeySlot2_1.Text = target.Child[1].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot2_2.Text = target.Child[1].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot2_3.Text = target.Child[1].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot2.Text = target.Child[1].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem2.SelectedValue = target.Child[1].item.itemId;
                cboEnchant2_1.SelectedValue = target.Child[1].item.enchantIds[0];
                cboEnchant2_2.SelectedValue = target.Child[1].item.enchantIds[1];
                cboEnchant2_3.SelectedValue = target.Child[1].item.enchantIds[2];
                cboEnchant2_4.SelectedValue = target.Child[1].item.enchantIds[3];
            }
            else
            {
                SetItemDetailInit(2);
            }
            //アイテム3(左下)--------------------------------------------------------------------
            if (target.Child.Count() > 2)
            {
                //基本属性
                textItemId3.Text = target.Child[2].item.itemId.ToString();
                textItemLevel3.Text = target.Child[2].item.itemLevel.ToString();
                textEnchantIds3_1.Text = target.Child[2].item.enchantIds[0].ToString();
                textEnchantIds3_2.Text = target.Child[2].item.enchantIds[1].ToString();
                textEnchantIds3_3.Text = target.Child[2].item.enchantIds[2].ToString();
                textEnchantIds3_4.Text = target.Child[2].item.enchantIds[3].ToString();
                textProficient3.Text = target.Child[2].item.proficient.ToString();
                textPetID3.Text = target.Child[2].item.petID.ToString();
                chkSaveLock3.Checked = target.Child[2].item.saveLock;
                textBulletNum3.Text = target.Child[2].item.bulletNum.ToString();
                textBulletId3.Text = target.Child[2].item.bulletId.ToString();
                chkDataVersion3.Checked = target.Child[2].item.dataVersion == 0;
                //個数など外部属性
                textCount3.Text = target.Child[2].count.ToString();
                textAssignedHotkeySlot3_1.Text = target.Child[2].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot3_2.Text = target.Child[2].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot3_3.Text = target.Child[2].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot3.Text = target.Child[2].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem3.SelectedValue = target.Child[2].item.itemId;
                cboEnchant3_1.SelectedValue = target.Child[2].item.enchantIds[0];
                cboEnchant3_2.SelectedValue = target.Child[2].item.enchantIds[1];
                cboEnchant3_3.SelectedValue = target.Child[2].item.enchantIds[2];
                cboEnchant3_4.SelectedValue = target.Child[2].item.enchantIds[3];
            }
            else
            {
                SetItemDetailInit(3);
            }
            //アイテム3(左下)--------------------------------------------------------------------
            if (target.Child.Count() > 3)
            {
                //基本属性
                textItemId4.Text = target.Child[3].item.itemId.ToString();
                textItemLevel4.Text = target.Child[3].item.itemLevel.ToString();
                textEnchantIds4_1.Text = target.Child[3].item.enchantIds[0].ToString();
                textEnchantIds4_2.Text = target.Child[3].item.enchantIds[1].ToString();
                textEnchantIds4_3.Text = target.Child[3].item.enchantIds[2].ToString();
                textEnchantIds4_4.Text = target.Child[3].item.enchantIds[3].ToString();
                textProficient4.Text = target.Child[3].item.proficient.ToString();
                textPetID4.Text = target.Child[3].item.petID.ToString();
                chkSaveLock4.Checked = target.Child[3].item.saveLock;
                textBulletNum4.Text = target.Child[3].item.bulletNum.ToString();
                textBulletId4.Text = target.Child[3].item.bulletId.ToString();
                chkDataVersion3.Checked = target.Child[3].item.dataVersion == 0;
                //個数など外部属性
                textCount4.Text = target.Child[3].count.ToString();
                textAssignedHotkeySlot4_1.Text = target.Child[3].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlot4_2.Text = target.Child[3].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlot4_3.Text = target.Child[3].assignedHotkeySlot[2].ToString();
                textAssignedEquipSlot4.Text = target.Child[3].assignedEquipSlot.ToString();
                // コンボボックス連動
                cboItem4.SelectedValue = target.Child[3].item.itemId;
                cboEnchant4_1.SelectedValue = target.Child[3].item.enchantIds[0];
                cboEnchant4_2.SelectedValue = target.Child[3].item.enchantIds[1];
                cboEnchant4_3.SelectedValue = target.Child[3].item.enchantIds[2];
                cboEnchant4_4.SelectedValue = target.Child[3].item.enchantIds[3];
            }
            else
            {
                SetItemDetailInit(4);
            }
        }

        /// <summary>
        /// 入れ子アイテムがない場合、入れ子ページを初期化する
        /// </summary>
        /// <param name="no">入れ子No(2,3,4)</param>
        void SetItemDetailInit(int no)
        {
            if (no ==2)
            {
                //基本属性
                textItemId2.Text = textItemId1.Text;
                textItemLevel2.Text = "0";
                textEnchantIds2_1.Text = "0";
                textEnchantIds2_2.Text = "0";
                textEnchantIds2_3.Text = "0";
                textEnchantIds2_4.Text = "0";
                textProficient2.Text = "0";
                textPetID2.Text = "65535";
                chkSaveLock2.Checked = chkSaveLock1.Checked;
                textBulletNum2.Text = "0";
                textBulletId2.Text = "0";
                chkDataVersion2.Checked = chkDataVersion1.Checked;
                //個数など外部属性
                textCount2.Text = "0";
                textAssignedHotkeySlot2_1.Text = "0";
                textAssignedHotkeySlot2_2.Text = "0";
                textAssignedHotkeySlot2_3.Text = "0";
                textAssignedEquipSlot2.Text = "0";
                // コンボボックス連動
                cboItem2.SelectedValue = "0";
                cboEnchant2_1.SelectedValue = "0";
                cboEnchant2_2.SelectedValue = "0";
                cboEnchant2_3.SelectedValue = "0";
                cboEnchant2_4.SelectedValue = "0";
            }
            if (no == 3)
            {
                //基本属性
                textItemId3.Text = textItemId1.Text;
                textItemLevel3.Text = "0";
                textEnchantIds3_1.Text = "0";
                textEnchantIds3_2.Text = "0";
                textEnchantIds3_3.Text = "0";
                textEnchantIds3_4.Text = "0";
                textProficient3.Text = "0";
                textPetID3.Text = "65535";
                chkSaveLock3.Checked = chkSaveLock1.Checked;
                textBulletNum3.Text = "0";
                textBulletId3.Text = "0";
                chkDataVersion3.Checked = chkDataVersion1.Checked;
                //個数など外部属性
                textCount3.Text = "0";
                textAssignedHotkeySlot3_1.Text = "0";
                textAssignedHotkeySlot3_2.Text = "0";
                textAssignedHotkeySlot3_3.Text = "0";
                textAssignedEquipSlot3.Text = "0";
                // コンボボックス連動
                cboItem3.SelectedValue = "0";
                cboEnchant3_1.SelectedValue = "0";
                cboEnchant3_2.SelectedValue = "0";
                cboEnchant3_3.SelectedValue = "0";
                cboEnchant3_4.SelectedValue = "0";
            }
            if (no == 4)
            {
                //基本属性
                textItemId4.Text = textItemId1.Text;
                textItemLevel4.Text = "0";
                textEnchantIds4_1.Text = "0";
                textEnchantIds4_2.Text = "0";
                textEnchantIds4_3.Text = "0";
                textEnchantIds4_4.Text = "0";
                textProficient4.Text = "0";
                textPetID4.Text = "65535";
                chkSaveLock4.Checked = chkSaveLock1.Checked;
                textBulletNum4.Text = "0";
                textBulletId4.Text = "0";
                chkDataVersion4.Checked = chkDataVersion1.Checked;
                //個数など外部属性
                textCount4.Text = "0";
                textAssignedHotkeySlot4_1.Text = "0";
                textAssignedHotkeySlot4_2.Text = "0";
                textAssignedHotkeySlot4_3.Text = "0";
                textAssignedEquipSlot4.Text = "0";
                // コンボボックス連動
                cboItem4.SelectedValue = "0";
                cboEnchant4_1.SelectedValue = "0";
                cboEnchant4_2.SelectedValue = "0";
                cboEnchant4_3.SelectedValue = "0";
                cboEnchant4_4.SelectedValue = "0";
            }
        }
        #endregion

        #region 保存関係

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (CPInventorySaveData == null)
            {
                MessageBox.Show("セーブデータがセットされていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("本当にリセットしますか？(画面状態を最後にドラッグした時の状態に戻します)", "警告", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                //リセット
                CPInventorySaveData = CPInventorySaveDataBackUp;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (CPInventorySaveData == null)
            {
                MessageBox.Show("セーブデータがセットされていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("セーブデータを上書きします。本当によろしいですか？", "警告", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                //セーブ処理開始
                if (!SetItemDetailToFile())
                {
                    MessageBox.Show("セーブに失敗しました。");
                }
            }
        }
        
        /// <summary>
        /// セーブ処理
        /// </summary>
        /// <returns></returns>
        bool SetItemDetailToFile()
        {
            try
            {
                string jsonStr = ConvertCraftpiaParams.CPTreeToJsonStr(CPInventorySaveData);

                //テスト 生成データと一致するかどうかdiff確認中--
                string originalstr = originalData[2].value;

                for (int i = 0; i < jsonStr.Count(); i++)
                {
                    if (jsonStr[i] != originalstr[i])
                    {
                        MessageBox.Show(i.ToString());
                        return false;
                    }
                }

                return true;
                //--TODO--
                originalData.Where(p => p.id == PPSave_ID_InGame).First().value = jsonStr;
                if (!CrudDb.Update(dbPath, originalData))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SetItemDetailToFile-err : " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
