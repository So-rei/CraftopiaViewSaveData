﻿using System;
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
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace CraftpiaViewSaveData
{
    public partial class MainForm : Form
    {
        #region 共通関数、クラス
        //開いているセーブデータの値を保持する
        string dbPath { get; set; }
        List<ClassDb> originalData;
        CraftpiaParams convertData;
        _CPInventorySaveData CPInventorySaveDataBackUp;
        _CPInventorySaveData CPInventorySaveData;

        //コンボボックスの値（初回ロード時設定）
        Dictionary<string, ClsResource> enchantComboBoxParams;

        /// <summary>
        /// 今開いているアイテム種類タブ
        /// </summary>
        itemListName bfSelectType { get; set; }
        /// <summary>
        /// 今開いているパネルNo
        /// </summary>
        int selectPanelNo { get; set; }
        /// <summary>
        /// 今開いているページタブNo
        /// </summary>
        int itemPageNo { get { return tabcontrol2.SelectedIndex; } }

        //面倒なのでコントロール配列にする
        TextBox[] textItemIds;
        TextBox[] textItemLevels;
        TextBox[][] textEnchantIds;
        TextBox[] textProficients;
        TextBox[] textPetIDs;
        CheckBox[] chkSaveLocks;
        TextBox[] textBulletNums;
        TextBox[] textBulletIds;
        CheckBox[] chkDataVersions;
        TextBox[] textCounts;
        TextBox[][] textAssignedHotkeySlots;
        TextBox[] textAssignedEquipSlots;
        ComboBox[] cboItems;
        ComboBox[][] cboEnchants;
        Panel[] itemPanels;

        //ロード中フラグ
        public bool isLoading { get; private set; } = true;

        public class ComboBoxItemSet
        {
            public int ItemValue { get; set; }
            public String ItemDisp { get; set; }
            public string[] ItemEtc { get; set; }

            //コンボボックス初期化(エンチャント用)
            public ComboBoxItemSet(string id, string name, int rank, params string[] etc)
            {
                ItemValue = Convert.ToInt32(id);
                ItemDisp = id + " : " + "★" + rank + " : " + name + " : " + etc[0];
                ItemEtc = etc.Skip(1).ToArray();
            }
            //コンボボックス初期化(アイテム用)
            public ComboBoxItemSet(string id, string name, params string[] etc)
            {
                ItemValue = Convert.ToInt32(id);
                ItemDisp = id + " : " + name;
                ItemEtc = etc.ToArray();
            }
        }
        #endregion

        #region 初期処理

        /// <summary>
        /// イニシャライズ
        /// </summary>
        public MainForm()
        {
            isLoading = true;

            InitializeComponent();
            EnchantComboBoxSet();
            ItemComboBoxSet();
            SetControlArray();

            isLoading = false;
        }

        //コンボボックス設定（エンチャント）
        private void EnchantComboBoxSet()
        {
            List<ComboBoxItemSet> clist = new List<ComboBoxItemSet>();

            enchantComboBoxParams = GetResourceFile.GetFileEnchant("EnchantParams.txt");
            foreach (var d in enchantComboBoxParams)
            {
                clist.Add(new ComboBoxItemSet(d.Key, d.Value.value, d.Value.rank, new string[] { d.Value.param }));
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
        private void ItemComboBoxSet()
        {
            List<ComboBoxItemSet> clist = new List<ComboBoxItemSet>();

            var itemParams = GetResourceFile.GetFile("ItemParams.txt");
            foreach (var d in itemParams)
            {
                clist.Add(new ComboBoxItemSet(d.Key, d.Value.value, new string[] { d.Value.param }));
            }

            setCboBox(cboItem1, clist);
            setCboBox(cboItem2, clist);
            setCboBox(cboItem3, clist);
            setCboBox(cboItem4, clist);
        }
        private void setCboBox(ComboBox cbo, List<ComboBoxItemSet> source)
        {
            cbo.DataSource = new List<ComboBoxItemSet>(source);
            cbo.DisplayMember = "ItemDisp";
            cbo.ValueMember = "ItemValue";
        }

        /// <summary>
        /// コントロール配列のセット
        /// </summary>
        private void SetControlArray()
        {
            textItemIds = new TextBox[] { textItemId1, textItemId2, textItemId3, textItemId4 };
            textItemLevels = new TextBox[] { textItemLevel1, textItemLevel2, textItemLevel3, textItemLevel4 };
            textEnchantIds = new TextBox[][] { new []{ textEnchantIds1_1, textEnchantIds1_2, textEnchantIds1_3, textEnchantIds1_4 },
             new []{ textEnchantIds2_1, textEnchantIds2_2, textEnchantIds2_3, textEnchantIds2_4 },
             new []{ textEnchantIds3_1, textEnchantIds3_2, textEnchantIds3_3, textEnchantIds3_4 },
             new []{ textEnchantIds4_1, textEnchantIds4_2, textEnchantIds4_3, textEnchantIds4_4 }};
            textProficients = new TextBox[] { textProficient1, textProficient2, textProficient3, textProficient4 };
            textPetIDs = new TextBox[] { textPetID1, textPetID2, textPetID3, textPetID4 };
            chkSaveLocks = new CheckBox[] { chkSaveLock1, chkSaveLock2, chkSaveLock3, chkSaveLock4 };
            textBulletNums = new TextBox[] { textBulletNum1, textBulletNum2, textBulletNum3, textBulletNum4 };
            textBulletIds = new TextBox[] { textBulletId1, textBulletId2, textBulletId3, textBulletId4 };
            chkDataVersions = new CheckBox[] { chkDataVersion1, chkDataVersion2, chkDataVersion3, chkDataVersion4 };
            textCounts = new TextBox[] { textCount1, textCount2, textCount3, textCount4 };
            textAssignedHotkeySlots = new TextBox[][] { new[] { textAssignedHotkeySlot1_1, textAssignedHotkeySlot1_2, textAssignedHotkeySlot1_3 } ,
             new[] { textAssignedHotkeySlot2_1, textAssignedHotkeySlot2_2, textAssignedHotkeySlot2_3 },
             new[] { textAssignedHotkeySlot3_1, textAssignedHotkeySlot3_2, textAssignedHotkeySlot3_3 },
             new[] { textAssignedHotkeySlot4_1, textAssignedHotkeySlot4_2, textAssignedHotkeySlot4_3 }};
            textAssignedEquipSlots = new TextBox[] { textAssignedEquipSlot1, textAssignedEquipSlot2, textAssignedEquipSlot3, textAssignedEquipSlot4 };
            cboItems = new ComboBox[] { cboItem1, cboItem2, cboItem3, cboItem4 };
            cboEnchants = new ComboBox[][] { new ComboBox[] { cboEnchant1_1, cboEnchant1_2, cboEnchant1_3, cboEnchant1_4 },
            new ComboBox[] { cboEnchant2_1, cboEnchant2_2, cboEnchant2_3, cboEnchant2_4 },
            new ComboBox[] { cboEnchant3_1, cboEnchant3_2, cboEnchant3_3, cboEnchant3_4 },
            new ComboBox[] { cboEnchant4_1, cboEnchant4_2, cboEnchant4_3, cboEnchant4_4 }};
            itemPanels = new Panel[] { p1_1, p1_2, p1_3, p1_4, p1_5, p1_6, p1_7, p1_8, p1_9, p1_10, p1_11, p1_12, p1_13, p1_14, p1_15, p1_16, p1_17, p1_18, p1_19, p1_20,
                p1_21, p1_22, p1_23, p1_24, p1_25, p1_26, p1_27, p1_28, p1_29, p1_30, p1_31, p1_32, p1_33, p1_34, p1_35, p1_36, p1_37, p1_38, p1_39, p1_40 };
        }
        #endregion

        #region "セーブデータ取得関係"
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
            setItemDetailToDisp(bfSelectType.ToString(), 0);

            //エンチャントロード
            dgvLoadEnchant();
#if DEBUGX
            HiddenViewString();
#endif

            //ページアクティブ
            this.tabControl1.Enabled = true;
            this.tabcontrol2.Enabled = true;
        }

        //アイテム上限数（解放も込み）を視覚的にわかるようにする
        private void setDispView()
        {
            int page_limit = CPInventorySaveData.paramsList[bfSelectType.ToString()].Child.Count();
            Color colOK = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            Color colNG = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            for (int i = 0; i < itemPanels.Count(); i++)
                itemPanels[i].BackColor = page_limit > i ? colOK : colNG;
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
        #endregion

        #region エンチャ関係
        private void dgvLoadEnchant()
        {
            int enchantCount = CPInventorySaveData.enchantList.Child.Count();
            for (int i = 0; i < enchantCount; i++)
            {
                if (enchantComboBoxParams.TryGetValue(CPInventorySaveData.enchantList.Child[i].id.ToString(), out ClsResource v))
                {
                    CPInventorySaveData.enchantList.Child[i].enchantName = v.value;
                    CPInventorySaveData.enchantList.Child[i].enchantEffect = v.param;
                }
            }
            BindingList<CPEnchant> ds = new BindingList<CPEnchant>(CPInventorySaveData.enchantList.Child);
            dgvEnchant.DataSource = ds;
        }

        #endregion

        #region "アイテム詳細関係"
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //直前の内容を保存する
            SaveItems(bfSelectType);
            //場所更新
            bfSelectType = (itemListName)tabControl1.SelectedIndex;

            //アイテム属性タブ遷移
            if (bfSelectType.ToString() == itemListName.petChestList.ToString())
            {
                MessageBox.Show("ペットチェストは現在設定不可です。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //アイテム上限数（解放も込み）を視覚的にわかるようにする
            setDispView();
            //初期位置（左上１個目）を選択
            setItemDetailToDisp(bfSelectType.ToString(), 0);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            //アイテム選択
            setItemDetailToDisp(bfSelectType.ToString(), 0);
        }

        private void panel_ItemNo_Click(object sender, EventArgs e)
        {
            if (isLoading) return;

            //アイテム選択1-40
            //c#にはLike文が無いので面倒
            var PanelArray1 = panelItemNo.Controls.OfType<Panel>()
                                  .Where(p => System.Text.RegularExpressions.Regex.IsMatch(p.Name, "^p1_.*", RegexOptions.Singleline))
                                  .OrderBy(q => q.TabIndex).ToArray();

            var itemIndex = Convert.ToInt32(new string((((Panel)sender).Name).Skip(3).ToArray())) - 1;

            //直前の内容を保存する
            SaveItems(bfSelectType);

            //選択Noのアイテムを選択
            setItemDetailToDisp(bfSelectType.ToString(), itemIndex);

        }
        #endregion

        #region "個別アイテム関係"

        /// <summary>
        /// アイテムデータ　→　画面
        /// </summary>
        /// <param name="categoryName">属性名</param>
        /// <param name="itemIndex">アイテムNo（左上１行目から１，２，・・・）</param>
        void setItemDetailToDisp(string categoryName, int itemIndex)
        {
            selectPanelNo = itemIndex;
            if (CPInventorySaveData == null)
            {
                MessageBox.Show("セーブデータがセットされていません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //アイテム所持数未開放
            if (CPInventorySaveData.paramsList[categoryName].Child.Count() <= selectPanelNo)
                return;

            var target = CPInventorySaveData.paramsList[categoryName].Child[selectPanelNo];

            //アイテム1(左上),アイテム2(右上),アイテム3(左下),アイテム3(左下)----------------------------------
            for (int i = 0; i < 4; i++)
            {
                if (target.Child.Count() > i)
                    SetItemDetail(i, target);
                else
                    SetItemDetailInit(i);
            }
        }

        /// <summary>
        /// 入れ子アイテムデータをセットする
        /// </summary>
        /// <param name="no">入れ子No</param>
        void SetItemDetail(int no, CPItemInBox setData)
        {
            //基本属性
            textItemIds[no].Text = setData.Child[no].item.itemId.ToString();
            textItemLevels[no].Text = setData.Child[no].item.itemLevel.ToString();
            textEnchantIds[no][0].Text = setData.Child[no].item.enchantIds[0].ToString();
            textEnchantIds[no][1].Text = setData.Child[no].item.enchantIds[1].ToString();
            textEnchantIds[no][2].Text = setData.Child[no].item.enchantIds[2].ToString();
            textEnchantIds[no][3].Text = setData.Child[no].item.enchantIds[3].ToString();
            textProficients[no].Text = setData.Child[no].item.proficient.ToString();
            textPetIDs[no].Text = setData.Child[no].item.petID.ToString();
            chkSaveLocks[no].Checked = setData.Child[no].item.saveLock;
            textBulletNums[no].Text = setData.Child[no].item.bulletNum.ToString();
            textBulletIds[no].Text = setData.Child[no].item.bulletId.ToString();
            chkDataVersions[no].Checked = setData.Child[no].item.dataVersion == 0;
            //個数など外部属性
            textCounts[no].Text = setData.Child[no].count.ToString();
            if (setData.Child[no].assignedHotkeySlot != null)
            {
                textAssignedHotkeySlots[no][0].Text = setData.Child[no].assignedHotkeySlot[0].ToString();
                textAssignedHotkeySlots[no][1].Text = setData.Child[no].assignedHotkeySlot[1].ToString();
                textAssignedHotkeySlots[no][2].Text = setData.Child[no].assignedHotkeySlot[2].ToString();
            }
            textAssignedEquipSlots[no].Text = setData.Child[no].assignedEquipSlot.ToString();
            // コンボボックス連動
            cboItems[no].SelectedValue = setData.Child[no].item.itemId;
            cboEnchants[no][0].SelectedValue = setData.Child[no].item.enchantIds[0];
            cboEnchants[no][1].SelectedValue = setData.Child[no].item.enchantIds[1];
            cboEnchants[no][2].SelectedValue = setData.Child[no].item.enchantIds[2];
            cboEnchants[no][3].SelectedValue = setData.Child[no].item.enchantIds[3];
        }

        /// <summary>
        /// 入れ子アイテムがない場合、入れ子ページを初期化する
        /// </summary>
        /// <param name="no">入れ子No(2,3,4)</param>
        void SetItemDetailInit(int no)
        {
            //基本属性
            textItemIds[no].Text = textItemId1.Text;
            textItemLevels[no].Text = "0";
            textEnchantIds[no][0].Text = "0";
            textEnchantIds[no][1].Text = "0";
            textEnchantIds[no][2].Text = "0";
            textEnchantIds[no][3].Text = "0";
            textProficients[no].Text = "0";
            textPetIDs[no].Text = "65535";
            chkSaveLocks[no].Checked = chkSaveLock1.Checked;
            textBulletNums[no].Text = "0";
            textBulletIds[no].Text = "0";
            chkDataVersions[no].Checked = chkDataVersion1.Checked;
            //個数など外部属性
            textCounts[no].Text = "0";
            textAssignedHotkeySlots[no][0].Text = "0";
            textAssignedHotkeySlots[no][1].Text = "0";
            textAssignedHotkeySlots[no][2].Text = "0";
            textAssignedEquipSlots[no].Text = "0";
            // コンボボックス連動
            cboItems[no].SelectedValue = "0";
            cboEnchants[no][0].SelectedValue = "0";
            cboEnchants[no][1].SelectedValue = "0";
            cboEnchants[no][2].SelectedValue = "0";
            cboEnchants[no][3].SelectedValue = "0";
        }


        /// <summary>
        /// コンボボックスの対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboItem1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            int no = itemPageNo;
            if (cboItems[no].SelectedValue != null)
                textItemIds[no].Text = cboItems[no].SelectedValue.ToString();
        }
        /// <summary>
        /// コンボボックスの対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboEnchants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            int no = itemPageNo;
            var enchantNo = Convert.ToInt32(new string((((ComboBox)sender).Name).Skip("cboEnchant*_".Length).ToArray())) - 1;
            if (cboEnchants[no][enchantNo].SelectedValue != null)
                textEnchantIds[no][enchantNo].Text = cboEnchants[no][enchantNo].SelectedValue.ToString();
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
                CPInventorySaveData = ConvertCraftpiaParams.CraftpiaParamsToCPTree(convertData);
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
                //直前の内容を保存する
                SaveItems(bfSelectType);
                //ファイルにセーブ処理開始
                if (!SetItemDetailToFile())
                {
                    MessageBox.Show("セーブに失敗しました。");
                    return;
                }




                MessageBox.Show("セーブ成功");
            }
        }

        private void SaveItems(itemListName saveSelectType)
        {
            //未ロード
            if (CPInventorySaveData == null)
                return;

            //アイテム所持数未開放
            if (CPInventorySaveData.paramsList[saveSelectType.ToString()].Child.Count() <= selectPanelNo)
                return;

            var target = CPInventorySaveData.paramsList[saveSelectType.ToString()].Child[selectPanelNo];
            int no = itemPageNo;

            //アイテム入れ子の枠外（追加もできると思うけど今はキャンセルする
            if (target.Child.Count() <= no)
            {
                MessageBox.Show("アイテム枠のツリー('田'構造)を追加しようとしています。未実装", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //基本属性
            if (int.TryParse(textItemIds[no].Text, out int _itemid1))
                ((CPItemInBoxValue)target.Child[no]).item.itemId = _itemid1;
            if (int.TryParse(textItemLevels[no].Text, out int _itemlevel1))
                target.Child[no].item.itemLevel = _itemlevel1;

            if (int.TryParse(textEnchantIds[no][0].Text, out int _enchantids1_1))
                target.Child[no].item.enchantIds[0] = _enchantids1_1;
            if (int.TryParse(textEnchantIds[no][1].Text, out int _enchantids1_2))
                target.Child[no].item.enchantIds[1] = _enchantids1_2;
            if (int.TryParse(textEnchantIds[no][2].Text, out int _enchantids1_3))
                target.Child[no].item.enchantIds[2] = _enchantids1_3;
            if (int.TryParse(textEnchantIds[no][3].Text, out int _enchantids1_4))
                target.Child[no].item.enchantIds[3] = _enchantids1_4;
            if (int.TryParse(textProficients[no].Text, out int _proficient1))
                target.Child[no].item.proficient = _proficient1;

            if (int.TryParse(textPetIDs[no].Text, out int _petid1))
                target.Child[no].item.petID = _petid1;
            target.Child[no].item.saveLock = chkSaveLocks[no].Checked;
            if (int.TryParse(textBulletNums[no].Text, out int _bulletnum1))
                target.Child[no].item.bulletNum = _bulletnum1;
            if (int.TryParse(textBulletIds[no].Text, out int _bulletId))
                target.Child[no].item.bulletId = _bulletId;
            target.Child[no].item.dataVersion = chkDataVersion1.Checked ? 0 : 1;

            //個数など外部属性
            if (int.TryParse(textCounts[no].Text, out int _count1))
                target.Child[no].count = _count1;
            if (target.Child[no].assignedHotkeySlot != null)
            {
                if (int.TryParse(textAssignedHotkeySlots[no][0].Text, out int _assignedhotkeyslot1_1))
                    target.Child[no].assignedHotkeySlot[0] = _assignedhotkeyslot1_1;
                if (int.TryParse(textAssignedHotkeySlots[no][1].Text, out int _assignedhotkeyslot1_2))
                    target.Child[no].assignedHotkeySlot[1] = _assignedhotkeyslot1_2;
                if (int.TryParse(textAssignedHotkeySlots[no][2].Text, out int _assignedhotkeyslot1_3))
                    target.Child[no].assignedHotkeySlot[2] = _assignedhotkeyslot1_3;
            }
            if (int.TryParse(textAssignedEquipSlots[no].Text, out int _assignedEquipSlot))
                target.Child[no].assignedEquipSlot = _assignedEquipSlot;
        }

        /// <summary>
        /// セーブ処理
        /// </summary>
        /// <returns></returns>
        bool SetItemDetailToFile()
        {
            try
            {
                string jsonStr = ConvertCraftpiaParams.ConcatOtherParams(originalData[2].value, CPInventorySaveData);

#if TEST
                //テスト 生成データと一致するかどうかdiff確認--
                string originalstr = originalData[2].value;
                for (int i = 30000; i < jsonStr.Count(); i += 100)
                {
                    if (jsonStr.Substring(i, Math.Min(jsonStr.Count() - i, 100)) != originalstr.Substring(i, Math.Min(jsonStr.Count() - i, 100)))
                    {
                        MessageBox.Show(i.ToString());
                        return false;
                    }
                }
#endif

                originalData[2].value = jsonStr;
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
