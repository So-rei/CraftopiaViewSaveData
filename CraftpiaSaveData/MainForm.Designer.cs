namespace CraftpiaViewSaveData
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConvertOcs2Json_Label = new System.Windows.Forms.Label();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.p1_1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.装備 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panelEquipment = new System.Windows.Forms.Panel();
            this.p1_24 = new System.Windows.Forms.Panel();
            this.p1_16 = new System.Windows.Forms.Panel();
            this.p1_23 = new System.Windows.Forms.Panel();
            this.p1_8 = new System.Windows.Forms.Panel();
            this.p1_22 = new System.Windows.Forms.Panel();
            this.p1_15 = new System.Windows.Forms.Panel();
            this.p1_21 = new System.Windows.Forms.Panel();
            this.p1_20 = new System.Windows.Forms.Panel();
            this.p1_7 = new System.Windows.Forms.Panel();
            this.p1_19 = new System.Windows.Forms.Panel();
            this.p1_14 = new System.Windows.Forms.Panel();
            this.p1_18 = new System.Windows.Forms.Panel();
            this.p1_13 = new System.Windows.Forms.Panel();
            this.p1_17 = new System.Windows.Forms.Panel();
            this.p1_6 = new System.Windows.Forms.Panel();
            this.p1_12 = new System.Windows.Forms.Panel();
            this.p1_5 = new System.Windows.Forms.Panel();
            this.p1_11 = new System.Windows.Forms.Panel();
            this.p1_4 = new System.Windows.Forms.Panel();
            this.p1_10 = new System.Windows.Forms.Panel();
            this.p1_3 = new System.Windows.Forms.Panel();
            this.p1_9 = new System.Windows.Forms.Panel();
            this.p1_2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.属性名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.項目名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.項目名all = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.日本語説明 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.値 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indexh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.p1_1.SuspendLayout();
            this.panelEquipment.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Controls.Add(this.ConvertOcs2Json_Label);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1168, 127);
            this.panel1.TabIndex = 0;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            // 
            // ConvertOcs2Json_Label
            // 
            this.ConvertOcs2Json_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConvertOcs2Json_Label.AutoSize = true;
            this.ConvertOcs2Json_Label.Font = new System.Drawing.Font("MS UI Gothic", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ConvertOcs2Json_Label.Location = new System.Drawing.Point(409, 35);
            this.ConvertOcs2Json_Label.Name = "ConvertOcs2Json_Label";
            this.ConvertOcs2Json_Label.Size = new System.Drawing.Size(453, 40);
            this.ConvertOcs2Json_Label.TabIndex = 1;
            this.ConvertOcs2Json_Label.Text = "ここにセーブファイルをドロップ";
            // 
            // dgv1
            // 
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.属性名,
            this.項目名,
            this.項目名all,
            this.日本語説明,
            this.No,
            this.値,
            this.index,
            this.indexh});
            this.dgv1.Location = new System.Drawing.Point(19, 532);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowTemplate.Height = 21;
            this.dgv1.Size = new System.Drawing.Size(1161, 289);
            this.dgv1.TabIndex = 1;
            // 
            // p1_1
            // 
            this.p1_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_1.Controls.Add(this.label1);
            this.p1_1.Location = new System.Drawing.Point(20, 59);
            this.p1_1.Name = "p1_1";
            this.p1_1.Size = new System.Drawing.Size(55, 55);
            this.p1_1.TabIndex = 2;
            this.p1_1.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "ここを\r\nクリック\r\n";
            // 
            // 装備
            // 
            this.装備.AutoSize = true;
            this.装備.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.装備.Location = new System.Drawing.Point(3, 13);
            this.装備.Name = "装備";
            this.装備.Size = new System.Drawing.Size(122, 21);
            this.装備.TabIndex = 3;
            this.装備.Text = "装備アイテム";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(32, 194);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(12, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "選択アイテム詳細";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(170, 194);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 6;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(311, 194);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 20);
            this.comboBox3.TabIndex = 7;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(453, 194);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 20);
            this.comboBox4.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(35, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "エンチャ1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(172, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "エンチャ2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(313, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "エンチャ3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(454, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "エンチャ4";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(770, 195);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(34, 19);
            this.textBox1.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(598, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "アイテム名";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(601, 194);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(163, 20);
            this.comboBox5.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(767, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "＋";
            // 
            // panelEquipment
            // 
            this.panelEquipment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelEquipment.Controls.Add(this.p1_24);
            this.panelEquipment.Controls.Add(this.p1_16);
            this.panelEquipment.Controls.Add(this.p1_23);
            this.panelEquipment.Controls.Add(this.p1_8);
            this.panelEquipment.Controls.Add(this.p1_22);
            this.panelEquipment.Controls.Add(this.p1_15);
            this.panelEquipment.Controls.Add(this.p1_21);
            this.panelEquipment.Controls.Add(this.p1_20);
            this.panelEquipment.Controls.Add(this.p1_7);
            this.panelEquipment.Controls.Add(this.p1_19);
            this.panelEquipment.Controls.Add(this.p1_14);
            this.panelEquipment.Controls.Add(this.p1_18);
            this.panelEquipment.Controls.Add(this.p1_13);
            this.panelEquipment.Controls.Add(this.p1_17);
            this.panelEquipment.Controls.Add(this.p1_6);
            this.panelEquipment.Controls.Add(this.p1_12);
            this.panelEquipment.Controls.Add(this.p1_5);
            this.panelEquipment.Controls.Add(this.p1_11);
            this.panelEquipment.Controls.Add(this.p1_4);
            this.panelEquipment.Controls.Add(this.p1_10);
            this.panelEquipment.Controls.Add(this.p1_3);
            this.panelEquipment.Controls.Add(this.p1_9);
            this.panelEquipment.Controls.Add(this.p1_2);
            this.panelEquipment.Controls.Add(this.p1_1);
            this.panelEquipment.Controls.Add(this.装備);
            this.panelEquipment.Location = new System.Drawing.Point(12, 277);
            this.panelEquipment.Name = "panelEquipment";
            this.panelEquipment.Size = new System.Drawing.Size(510, 249);
            this.panelEquipment.TabIndex = 17;
            // 
            // p1_24
            // 
            this.p1_24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_24.Location = new System.Drawing.Point(447, 181);
            this.p1_24.Name = "p1_24";
            this.p1_24.Size = new System.Drawing.Size(55, 55);
            this.p1_24.TabIndex = 13;
            this.p1_24.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_16
            // 
            this.p1_16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_16.Location = new System.Drawing.Point(447, 120);
            this.p1_16.Name = "p1_16";
            this.p1_16.Size = new System.Drawing.Size(55, 55);
            this.p1_16.TabIndex = 13;
            this.p1_16.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_23
            // 
            this.p1_23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_23.Location = new System.Drawing.Point(386, 181);
            this.p1_23.Name = "p1_23";
            this.p1_23.Size = new System.Drawing.Size(55, 55);
            this.p1_23.TabIndex = 14;
            this.p1_23.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_8
            // 
            this.p1_8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_8.Location = new System.Drawing.Point(447, 59);
            this.p1_8.Name = "p1_8";
            this.p1_8.Size = new System.Drawing.Size(55, 55);
            this.p1_8.TabIndex = 7;
            this.p1_8.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_22
            // 
            this.p1_22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_22.Location = new System.Drawing.Point(325, 181);
            this.p1_22.Name = "p1_22";
            this.p1_22.Size = new System.Drawing.Size(55, 55);
            this.p1_22.TabIndex = 15;
            this.p1_22.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_15
            // 
            this.p1_15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_15.Location = new System.Drawing.Point(386, 120);
            this.p1_15.Name = "p1_15";
            this.p1_15.Size = new System.Drawing.Size(55, 55);
            this.p1_15.TabIndex = 14;
            this.p1_15.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_21
            // 
            this.p1_21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_21.Location = new System.Drawing.Point(264, 181);
            this.p1_21.Name = "p1_21";
            this.p1_21.Size = new System.Drawing.Size(55, 55);
            this.p1_21.TabIndex = 11;
            this.p1_21.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_20
            // 
            this.p1_20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_20.Location = new System.Drawing.Point(203, 181);
            this.p1_20.Name = "p1_20";
            this.p1_20.Size = new System.Drawing.Size(55, 55);
            this.p1_20.TabIndex = 12;
            this.p1_20.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_7
            // 
            this.p1_7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_7.Location = new System.Drawing.Point(386, 59);
            this.p1_7.Name = "p1_7";
            this.p1_7.Size = new System.Drawing.Size(55, 55);
            this.p1_7.TabIndex = 7;
            this.p1_7.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_19
            // 
            this.p1_19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_19.Location = new System.Drawing.Point(142, 181);
            this.p1_19.Name = "p1_19";
            this.p1_19.Size = new System.Drawing.Size(55, 55);
            this.p1_19.TabIndex = 10;
            this.p1_19.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_14
            // 
            this.p1_14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_14.Location = new System.Drawing.Point(325, 120);
            this.p1_14.Name = "p1_14";
            this.p1_14.Size = new System.Drawing.Size(55, 55);
            this.p1_14.TabIndex = 15;
            this.p1_14.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_18
            // 
            this.p1_18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_18.Location = new System.Drawing.Point(81, 181);
            this.p1_18.Name = "p1_18";
            this.p1_18.Size = new System.Drawing.Size(55, 55);
            this.p1_18.TabIndex = 9;
            this.p1_18.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_13
            // 
            this.p1_13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_13.Location = new System.Drawing.Point(264, 120);
            this.p1_13.Name = "p1_13";
            this.p1_13.Size = new System.Drawing.Size(55, 55);
            this.p1_13.TabIndex = 11;
            this.p1_13.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_17
            // 
            this.p1_17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_17.Location = new System.Drawing.Point(20, 181);
            this.p1_17.Name = "p1_17";
            this.p1_17.Size = new System.Drawing.Size(55, 55);
            this.p1_17.TabIndex = 8;
            this.p1_17.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_6
            // 
            this.p1_6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_6.Location = new System.Drawing.Point(325, 59);
            this.p1_6.Name = "p1_6";
            this.p1_6.Size = new System.Drawing.Size(55, 55);
            this.p1_6.TabIndex = 7;
            this.p1_6.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_12
            // 
            this.p1_12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_12.Location = new System.Drawing.Point(203, 120);
            this.p1_12.Name = "p1_12";
            this.p1_12.Size = new System.Drawing.Size(55, 55);
            this.p1_12.TabIndex = 12;
            this.p1_12.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_5
            // 
            this.p1_5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_5.Location = new System.Drawing.Point(264, 59);
            this.p1_5.Name = "p1_5";
            this.p1_5.Size = new System.Drawing.Size(55, 55);
            this.p1_5.TabIndex = 6;
            this.p1_5.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_11
            // 
            this.p1_11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_11.Location = new System.Drawing.Point(142, 120);
            this.p1_11.Name = "p1_11";
            this.p1_11.Size = new System.Drawing.Size(55, 55);
            this.p1_11.TabIndex = 10;
            this.p1_11.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_4
            // 
            this.p1_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_4.Location = new System.Drawing.Point(203, 59);
            this.p1_4.Name = "p1_4";
            this.p1_4.Size = new System.Drawing.Size(55, 55);
            this.p1_4.TabIndex = 6;
            this.p1_4.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_10
            // 
            this.p1_10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_10.Location = new System.Drawing.Point(81, 120);
            this.p1_10.Name = "p1_10";
            this.p1_10.Size = new System.Drawing.Size(55, 55);
            this.p1_10.TabIndex = 9;
            this.p1_10.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_3
            // 
            this.p1_3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_3.Location = new System.Drawing.Point(142, 59);
            this.p1_3.Name = "p1_3";
            this.p1_3.Size = new System.Drawing.Size(55, 55);
            this.p1_3.TabIndex = 5;
            this.p1_3.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_9
            // 
            this.p1_9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_9.Location = new System.Drawing.Point(20, 120);
            this.p1_9.Name = "p1_9";
            this.p1_9.Size = new System.Drawing.Size(55, 55);
            this.p1_9.TabIndex = 8;
            this.p1_9.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // p1_2
            // 
            this.p1_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.p1_2.Location = new System.Drawing.Point(81, 59);
            this.p1_2.Name = "p1_2";
            this.p1_2.Size = new System.Drawing.Size(55, 55);
            this.p1_2.TabIndex = 4;
            this.p1_2.Click += new System.EventHandler(this.panelEquipment_content_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(729, 237);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 16);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "遺産である場合チェック";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "PPsave\\Player\\****.dbとなっているファイルです";
            // 
            // 属性名
            // 
            this.属性名.HeaderText = "属性名";
            this.属性名.Name = "属性名";
            // 
            // 項目名
            // 
            this.項目名.HeaderText = "項目名";
            this.項目名.Name = "項目名";
            // 
            // 項目名all
            // 
            this.項目名all.HeaderText = "項目名all";
            this.項目名all.Name = "項目名all";
            this.項目名all.Width = 300;
            // 
            // 日本語説明
            // 
            this.日本語説明.HeaderText = "日本語説明";
            this.日本語説明.Name = "日本語説明";
            this.日本語説明.Width = 300;
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.Width = 60;
            // 
            // 値
            // 
            this.値.HeaderText = "値";
            this.値.Name = "値";
            // 
            // index
            // 
            this.index.HeaderText = "index";
            this.index.Name = "index";
            this.index.Width = 40;
            // 
            // indexh
            // 
            this.indexh.HeaderText = "index(hex)";
            this.indexh.Name = "indexh";
            this.indexh.Width = 80;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 833);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panelEquipment);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.p1_1.ResumeLayout(false);
            this.p1_1.PerformLayout();
            this.panelEquipment.ResumeLayout(false);
            this.panelEquipment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ConvertOcs2Json_Label;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.Panel p1_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label 装備;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelEquipment;
        private System.Windows.Forms.Panel p1_24;
        private System.Windows.Forms.Panel p1_16;
        private System.Windows.Forms.Panel p1_23;
        private System.Windows.Forms.Panel p1_8;
        private System.Windows.Forms.Panel p1_22;
        private System.Windows.Forms.Panel p1_15;
        private System.Windows.Forms.Panel p1_21;
        private System.Windows.Forms.Panel p1_20;
        private System.Windows.Forms.Panel p1_7;
        private System.Windows.Forms.Panel p1_19;
        private System.Windows.Forms.Panel p1_14;
        private System.Windows.Forms.Panel p1_18;
        private System.Windows.Forms.Panel p1_13;
        private System.Windows.Forms.Panel p1_17;
        private System.Windows.Forms.Panel p1_6;
        private System.Windows.Forms.Panel p1_12;
        private System.Windows.Forms.Panel p1_5;
        private System.Windows.Forms.Panel p1_11;
        private System.Windows.Forms.Panel p1_4;
        private System.Windows.Forms.Panel p1_10;
        private System.Windows.Forms.Panel p1_3;
        private System.Windows.Forms.Panel p1_9;
        private System.Windows.Forms.Panel p1_2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 属性名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 項目名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 項目名all;
        private System.Windows.Forms.DataGridViewTextBoxColumn 日本語説明;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn 値;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexh;
    }
}

