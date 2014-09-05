namespace GvoHelper
{
    partial class Form6
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UserComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Sequence_Read = new System.Windows.Forms.Button();
            this.領域comboBox = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.生產 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Sea_checkBox = new System.Windows.Forms.CheckBox();
            this.Init_Button = new System.Windows.Forms.Button();
            this.Sequence_Produce = new System.Windows.Forms.Button();
            this.書籍comboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.配方comboBox = new System.Windows.Forms.ComboBox();
            this.Sequence_listBox = new System.Windows.Forms.ListBox();
            this.Sequence_Sell = new System.Windows.Forms.Button();
            this.Sequence_Delete = new System.Windows.Forms.Button();
            this.Sequence_Buy = new System.Windows.Forms.Button();
            this.Sequence_Navigator = new System.Windows.Forms.Button();
            this.Loop_checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.開圖航線comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.航線comboBox = new System.Windows.Forms.ComboBox();
            this.StartCheck = new System.Windows.Forms.CheckBox();
            this.交易 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CityComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.閱讀 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Sequence_Research = new System.Windows.Forms.Button();
            this.專攻comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.Sequence_Report = new System.Windows.Forms.Button();
            this.報告comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.開圖座標textBox = new System.Windows.Forms.TextBox();
            this.開圖地點textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Sequence_Skill = new System.Windows.Forms.Button();
            this.開圖技能comboBox = new System.Windows.Forms.ComboBox();
            this.使用 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Sequence_TradeBook = new System.Windows.Forms.Button();
            this.採買書comboBox = new System.Windows.Forms.ComboBox();
            this.移動 = new System.Windows.Forms.TabPage();
            this.造船 = new System.Windows.Forms.TabPage();
            this.進度progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.飄船checkBox = new System.Windows.Forms.CheckBox();
            this.編號comboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.改倉numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.材質comboBox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.船種comboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.生產.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.交易.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.閱讀.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.使用.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.移動.SuspendLayout();
            this.造船.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.改倉numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // UserComboBox
            // 
            this.UserComboBox.FormattingEnabled = true;
            this.UserComboBox.Location = new System.Drawing.Point(12, 12);
            this.UserComboBox.MaxDropDownItems = 5;
            this.UserComboBox.Name = "UserComboBox";
            this.UserComboBox.Size = new System.Drawing.Size(225, 20);
            this.UserComboBox.TabIndex = 84;
            this.UserComboBox.TabStop = false;
            this.UserComboBox.DropDown += new System.EventHandler(this.GetAllProcess);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Sequence_Read);
            this.groupBox1.Controls.Add(this.領域comboBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 50);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "讀書設定";
            // 
            // Sequence_Read
            // 
            this.Sequence_Read.Location = new System.Drawing.Point(166, 19);
            this.Sequence_Read.Name = "Sequence_Read";
            this.Sequence_Read.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Read.TabIndex = 89;
            this.Sequence_Read.Text = "閱讀";
            this.Sequence_Read.UseVisualStyleBackColor = true;
            this.Sequence_Read.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 領域comboBox
            // 
            this.領域comboBox.FormattingEnabled = true;
            this.領域comboBox.ItemHeight = 12;
            this.領域comboBox.Items.AddRange(new object[] {
            "地理學",
            "考古學",
            "宗教學",
            "生物學",
            "財寶鑑定",
            "美術"});
            this.領域comboBox.Location = new System.Drawing.Point(6, 21);
            this.領域comboBox.Name = "領域comboBox";
            this.領域comboBox.Size = new System.Drawing.Size(154, 20);
            this.領域comboBox.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // 生產
            // 
            this.生產.BackColor = System.Drawing.SystemColors.Control;
            this.生產.Controls.Add(this.groupBox7);
            this.生產.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.生產.Location = new System.Drawing.Point(4, 4);
            this.生產.Name = "生產";
            this.生產.Size = new System.Drawing.Size(320, 294);
            this.生產.TabIndex = 2;
            this.生產.Text = "生產";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Sea_checkBox);
            this.groupBox7.Controls.Add(this.Init_Button);
            this.groupBox7.Controls.Add(this.Sequence_Produce);
            this.groupBox7.Controls.Add(this.書籍comboBox);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.配方comboBox);
            this.groupBox7.Location = new System.Drawing.Point(3, 7);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(225, 150);
            this.groupBox7.TabIndex = 86;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "生產設定";
            // 
            // Sea_checkBox
            // 
            this.Sea_checkBox.AutoSize = true;
            this.Sea_checkBox.Enabled = false;
            this.Sea_checkBox.Location = new System.Drawing.Point(135, 69);
            this.Sea_checkBox.Name = "Sea_checkBox";
            this.Sea_checkBox.Size = new System.Drawing.Size(72, 16);
            this.Sea_checkBox.TabIndex = 95;
            this.Sea_checkBox.Text = "海上生產";
            this.Sea_checkBox.UseVisualStyleBackColor = true;
            // 
            // Init_Button
            // 
            this.Init_Button.Location = new System.Drawing.Point(6, 65);
            this.Init_Button.Name = "Init_Button";
            this.Init_Button.Size = new System.Drawing.Size(75, 23);
            this.Init_Button.TabIndex = 89;
            this.Init_Button.Text = "初始化";
            this.Init_Button.UseVisualStyleBackColor = true;
            this.Init_Button.Click += new System.EventHandler(this.Init_Button_Click);
            // 
            // Sequence_Produce
            // 
            this.Sequence_Produce.Location = new System.Drawing.Point(132, 121);
            this.Sequence_Produce.Name = "Sequence_Produce";
            this.Sequence_Produce.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Produce.TabIndex = 88;
            this.Sequence_Produce.Text = "生產";
            this.Sequence_Produce.UseVisualStyleBackColor = true;
            this.Sequence_Produce.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 書籍comboBox
            // 
            this.書籍comboBox.FormattingEnabled = true;
            this.書籍comboBox.Location = new System.Drawing.Point(53, 15);
            this.書籍comboBox.Name = "書籍comboBox";
            this.書籍comboBox.Size = new System.Drawing.Size(154, 20);
            this.書籍comboBox.TabIndex = 9;
            this.書籍comboBox.DropDown += new System.EventHandler(this.書籍comboBox_DropDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "書籍：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "配方：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 配方comboBox
            // 
            this.配方comboBox.FormattingEnabled = true;
            this.配方comboBox.ItemHeight = 12;
            this.配方comboBox.Location = new System.Drawing.Point(53, 39);
            this.配方comboBox.Name = "配方comboBox";
            this.配方comboBox.Size = new System.Drawing.Size(154, 20);
            this.配方comboBox.TabIndex = 0;
            this.配方comboBox.DropDown += new System.EventHandler(this.配方comboBox_DropDown);
            // 
            // Sequence_listBox
            // 
            this.Sequence_listBox.AllowDrop = true;
            this.Sequence_listBox.FormattingEnabled = true;
            this.Sequence_listBox.ItemHeight = 12;
            this.Sequence_listBox.Location = new System.Drawing.Point(12, 38);
            this.Sequence_listBox.Name = "Sequence_listBox";
            this.Sequence_listBox.Size = new System.Drawing.Size(225, 220);
            this.Sequence_listBox.TabIndex = 87;
            this.Sequence_listBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.Sequence_listBox_DragDrop);
            this.Sequence_listBox.DragOver += new System.Windows.Forms.DragEventHandler(this.Sequence_listBox_DragOver);
            this.Sequence_listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Sequence_listBox_MouseDown);
            this.Sequence_listBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Sequence_listBox_MouseMove);
            this.Sequence_listBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Sequence_listBox_MouseUp);
            // 
            // Sequence_Sell
            // 
            this.Sequence_Sell.Location = new System.Drawing.Point(86, 118);
            this.Sequence_Sell.Name = "Sequence_Sell";
            this.Sequence_Sell.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Sell.TabIndex = 90;
            this.Sequence_Sell.Text = "出售";
            this.Sequence_Sell.UseVisualStyleBackColor = true;
            this.Sequence_Sell.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // Sequence_Delete
            // 
            this.Sequence_Delete.Location = new System.Drawing.Point(162, 265);
            this.Sequence_Delete.Name = "Sequence_Delete";
            this.Sequence_Delete.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Delete.TabIndex = 92;
            this.Sequence_Delete.Text = "刪除動作";
            this.Sequence_Delete.UseVisualStyleBackColor = true;
            this.Sequence_Delete.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // Sequence_Buy
            // 
            this.Sequence_Buy.Location = new System.Drawing.Point(5, 118);
            this.Sequence_Buy.Name = "Sequence_Buy";
            this.Sequence_Buy.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Buy.TabIndex = 89;
            this.Sequence_Buy.Text = "購買";
            this.Sequence_Buy.UseVisualStyleBackColor = true;
            this.Sequence_Buy.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // Sequence_Navigator
            // 
            this.Sequence_Navigator.Location = new System.Drawing.Point(154, 73);
            this.Sequence_Navigator.Name = "Sequence_Navigator";
            this.Sequence_Navigator.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Navigator.TabIndex = 94;
            this.Sequence_Navigator.Text = "航行";
            this.Sequence_Navigator.UseVisualStyleBackColor = true;
            this.Sequence_Navigator.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // Loop_checkBox
            // 
            this.Loop_checkBox.AutoSize = true;
            this.Loop_checkBox.Checked = true;
            this.Loop_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Loop_checkBox.Location = new System.Drawing.Point(12, 269);
            this.Loop_checkBox.Name = "Loop_checkBox";
            this.Loop_checkBox.Size = new System.Drawing.Size(72, 16);
            this.Loop_checkBox.TabIndex = 93;
            this.Loop_checkBox.Text = "循環序列";
            this.Loop_checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.開圖航線comboBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.Sequence_Navigator);
            this.groupBox4.Controls.Add(this.航線comboBox);
            this.groupBox4.Location = new System.Drawing.Point(3, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(252, 102);
            this.groupBox4.TabIndex = 94;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "航線設定";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 97;
            this.label5.Text = "正常航線：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 開圖航線comboBox
            // 
            this.開圖航線comboBox.FormattingEnabled = true;
            this.開圖航線comboBox.Location = new System.Drawing.Point(75, 47);
            this.開圖航線comboBox.Name = "開圖航線comboBox";
            this.開圖航線comboBox.Size = new System.Drawing.Size(154, 20);
            this.開圖航線comboBox.TabIndex = 96;
            this.開圖航線comboBox.DropDown += new System.EventHandler(this.航線comboBox_DropDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 95;
            this.label4.Text = "開圖航線：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 航線comboBox
            // 
            this.航線comboBox.FormattingEnabled = true;
            this.航線comboBox.Location = new System.Drawing.Point(75, 21);
            this.航線comboBox.Name = "航線comboBox";
            this.航線comboBox.Size = new System.Drawing.Size(154, 20);
            this.航線comboBox.TabIndex = 9;
            this.航線comboBox.DropDown += new System.EventHandler(this.航線comboBox_DropDown);
            // 
            // StartCheck
            // 
            this.StartCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.StartCheck.Location = new System.Drawing.Point(12, 291);
            this.StartCheck.Name = "StartCheck";
            this.StartCheck.Size = new System.Drawing.Size(75, 23);
            this.StartCheck.TabIndex = 95;
            this.StartCheck.Text = "開始";
            this.StartCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartCheck.UseVisualStyleBackColor = true;
            // 
            // 交易
            // 
            this.交易.BackColor = System.Drawing.SystemColors.Control;
            this.交易.Controls.Add(this.groupBox8);
            this.交易.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.交易.Location = new System.Drawing.Point(4, 4);
            this.交易.Name = "交易";
            this.交易.Padding = new System.Windows.Forms.Padding(3);
            this.交易.Size = new System.Drawing.Size(320, 294);
            this.交易.TabIndex = 0;
            this.交易.Text = "交易";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.textBox1);
            this.groupBox8.Controls.Add(this.CityComboBox);
            this.groupBox8.Controls.Add(this.Sequence_Sell);
            this.groupBox8.Controls.Add(this.Sequence_Buy);
            this.groupBox8.Location = new System.Drawing.Point(3, 7);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(225, 147);
            this.groupBox8.TabIndex = 92;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "城市設定";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 47);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(212, 65);
            this.textBox1.TabIndex = 92;
            this.textBox1.Text = "交易清單";
            // 
            // CityComboBox
            // 
            this.CityComboBox.FormattingEnabled = true;
            this.CityComboBox.Location = new System.Drawing.Point(6, 21);
            this.CityComboBox.MaxDropDownItems = 5;
            this.CityComboBox.Name = "CityComboBox";
            this.CityComboBox.Size = new System.Drawing.Size(155, 20);
            this.CityComboBox.TabIndex = 91;
            this.CityComboBox.TabStop = false;
            this.CityComboBox.TextChanged += new System.EventHandler(this.CityComboBox_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Controls.Add(this.交易);
            this.tabControl1.Controls.Add(this.生產);
            this.tabControl1.Controls.Add(this.閱讀);
            this.tabControl1.Controls.Add(this.使用);
            this.tabControl1.Controls.Add(this.移動);
            this.tabControl1.Controls.Add(this.造船);
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)), true);
            this.tabControl1.Location = new System.Drawing.Point(243, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(346, 302);
            this.tabControl1.TabIndex = 92;
            // 
            // 閱讀
            // 
            this.閱讀.BackColor = System.Drawing.SystemColors.Control;
            this.閱讀.Controls.Add(this.groupBox3);
            this.閱讀.Controls.Add(this.groupBox9);
            this.閱讀.Controls.Add(this.groupBox2);
            this.閱讀.Controls.Add(this.groupBox1);
            this.閱讀.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.閱讀.Location = new System.Drawing.Point(4, 4);
            this.閱讀.Name = "閱讀";
            this.閱讀.Size = new System.Drawing.Size(320, 294);
            this.閱讀.TabIndex = 4;
            this.閱讀.Text = " 閱讀";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Sequence_Research);
            this.groupBox3.Controls.Add(this.專攻comboBox);
            this.groupBox3.Location = new System.Drawing.Point(3, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(250, 50);
            this.groupBox3.TabIndex = 97;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "大學設定";
            // 
            // Sequence_Research
            // 
            this.Sequence_Research.Location = new System.Drawing.Point(166, 19);
            this.Sequence_Research.Name = "Sequence_Research";
            this.Sequence_Research.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Research.TabIndex = 95;
            this.Sequence_Research.Text = "專攻";
            this.Sequence_Research.UseVisualStyleBackColor = true;
            this.Sequence_Research.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 專攻comboBox
            // 
            this.專攻comboBox.FormattingEnabled = true;
            this.專攻comboBox.ItemHeight = 12;
            this.專攻comboBox.Items.AddRange(new object[] {
            "獎學制度說明會*",
            "調查技術1*",
            "調查技術1**",
            "自然學・人文科學1*",
            "自然學・人文科學1**",
            "陸上戰鬥技術1*",
            "陸上戰鬥技術1**"});
            this.專攻comboBox.Location = new System.Drawing.Point(6, 21);
            this.專攻comboBox.Name = "專攻comboBox";
            this.專攻comboBox.Size = new System.Drawing.Size(154, 20);
            this.專攻comboBox.TabIndex = 10;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.Sequence_Report);
            this.groupBox9.Controls.Add(this.報告comboBox);
            this.groupBox9.Location = new System.Drawing.Point(3, 163);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(250, 50);
            this.groupBox9.TabIndex = 90;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "報告設定";
            // 
            // Sequence_Report
            // 
            this.Sequence_Report.Location = new System.Drawing.Point(166, 19);
            this.Sequence_Report.Name = "Sequence_Report";
            this.Sequence_Report.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Report.TabIndex = 89;
            this.Sequence_Report.Text = "報告";
            this.Sequence_Report.UseVisualStyleBackColor = true;
            this.Sequence_Report.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 報告comboBox
            // 
            this.報告comboBox.FormattingEnabled = true;
            this.報告comboBox.ItemHeight = 12;
            this.報告comboBox.Items.AddRange(new object[] {
            "美3(男性大理石像地圖)",
            "地3(未完成的地圖)"});
            this.報告comboBox.Location = new System.Drawing.Point(6, 21);
            this.報告comboBox.Name = "報告comboBox";
            this.報告comboBox.Size = new System.Drawing.Size(154, 20);
            this.報告comboBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.開圖座標textBox);
            this.groupBox2.Controls.Add(this.開圖地點textBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Sequence_Skill);
            this.groupBox2.Controls.Add(this.開圖技能comboBox);
            this.groupBox2.Location = new System.Drawing.Point(3, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 94);
            this.groupBox2.TabIndex = 96;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "開圖設定";
            // 
            // 開圖座標textBox
            // 
            this.開圖座標textBox.Location = new System.Drawing.Point(53, 67);
            this.開圖座標textBox.Name = "開圖座標textBox";
            this.開圖座標textBox.Size = new System.Drawing.Size(154, 22);
            this.開圖座標textBox.TabIndex = 104;
            this.開圖座標textBox.Text = "1532, 3376";
            // 
            // 開圖地點textBox
            // 
            this.開圖地點textBox.Location = new System.Drawing.Point(53, 41);
            this.開圖地點textBox.Name = "開圖地點textBox";
            this.開圖地點textBox.Size = new System.Drawing.Size(154, 22);
            this.開圖地點textBox.TabIndex = 103;
            this.開圖地點textBox.Text = "東地中海";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 102;
            this.label3.Text = "座標：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 100;
            this.label2.Text = "地點：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 98;
            this.label1.Text = "技能：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Sequence_Skill
            // 
            this.Sequence_Skill.Location = new System.Drawing.Point(213, 67);
            this.Sequence_Skill.Name = "Sequence_Skill";
            this.Sequence_Skill.Size = new System.Drawing.Size(75, 23);
            this.Sequence_Skill.TabIndex = 96;
            this.Sequence_Skill.Text = "開圖";
            this.Sequence_Skill.UseVisualStyleBackColor = true;
            this.Sequence_Skill.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 開圖技能comboBox
            // 
            this.開圖技能comboBox.FormattingEnabled = true;
            this.開圖技能comboBox.ItemHeight = 12;
            this.開圖技能comboBox.Location = new System.Drawing.Point(53, 15);
            this.開圖技能comboBox.Name = "開圖技能comboBox";
            this.開圖技能comboBox.Size = new System.Drawing.Size(154, 20);
            this.開圖技能comboBox.TabIndex = 10;
            this.開圖技能comboBox.DropDown += new System.EventHandler(this.開圖技能comboBox_DropDown);
            // 
            // 使用
            // 
            this.使用.BackColor = System.Drawing.SystemColors.Control;
            this.使用.Controls.Add(this.groupBox6);
            this.使用.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.使用.Location = new System.Drawing.Point(4, 4);
            this.使用.Name = "使用";
            this.使用.Size = new System.Drawing.Size(320, 294);
            this.使用.TabIndex = 7;
            this.使用.Text = "使用";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Sequence_TradeBook);
            this.groupBox6.Controls.Add(this.採買書comboBox);
            this.groupBox6.Location = new System.Drawing.Point(3, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(250, 50);
            this.groupBox6.TabIndex = 94;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "星書設定";
            // 
            // Sequence_TradeBook
            // 
            this.Sequence_TradeBook.Location = new System.Drawing.Point(166, 19);
            this.Sequence_TradeBook.Name = "Sequence_TradeBook";
            this.Sequence_TradeBook.Size = new System.Drawing.Size(75, 23);
            this.Sequence_TradeBook.TabIndex = 95;
            this.Sequence_TradeBook.Text = "星書";
            this.Sequence_TradeBook.UseVisualStyleBackColor = true;
            this.Sequence_TradeBook.Click += new System.EventHandler(this.Sequence_Click);
            // 
            // 採買書comboBox
            // 
            this.採買書comboBox.FormattingEnabled = true;
            this.採買書comboBox.Location = new System.Drawing.Point(6, 21);
            this.採買書comboBox.Name = "採買書comboBox";
            this.採買書comboBox.Size = new System.Drawing.Size(154, 20);
            this.採買書comboBox.TabIndex = 9;
            this.採買書comboBox.DropDown += new System.EventHandler(this.採買書comboBox_DropDown);
            // 
            // 移動
            // 
            this.移動.BackColor = System.Drawing.SystemColors.Control;
            this.移動.Controls.Add(this.groupBox4);
            this.移動.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.移動.Location = new System.Drawing.Point(4, 4);
            this.移動.Name = "移動";
            this.移動.Size = new System.Drawing.Size(320, 294);
            this.移動.TabIndex = 3;
            this.移動.Text = "移動";
            // 
            // 造船
            // 
            this.造船.BackColor = System.Drawing.SystemColors.Control;
            this.造船.Controls.Add(this.進度progressBar);
            this.造船.Controls.Add(this.groupBox5);
            this.造船.Controls.Add(this.label9);
            this.造船.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.造船.Location = new System.Drawing.Point(4, 4);
            this.造船.Name = "造船";
            this.造船.Size = new System.Drawing.Size(320, 294);
            this.造船.TabIndex = 6;
            this.造船.Text = "造船";
            // 
            // 進度progressBar
            // 
            this.進度progressBar.Location = new System.Drawing.Point(58, 131);
            this.進度progressBar.Name = "進度progressBar";
            this.進度progressBar.Size = new System.Drawing.Size(170, 21);
            this.進度progressBar.TabIndex = 97;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.飄船checkBox);
            this.groupBox5.Controls.Add(this.編號comboBox);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.改倉numericUpDown);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.材質comboBox);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.船種comboBox);
            this.groupBox5.Location = new System.Drawing.Point(3, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(225, 122);
            this.groupBox5.TabIndex = 96;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "造船設定";
            // 
            // 飄船checkBox
            // 
            this.飄船checkBox.AutoSize = true;
            this.飄船checkBox.Location = new System.Drawing.Point(159, 65);
            this.飄船checkBox.Name = "飄船checkBox";
            this.飄船checkBox.Size = new System.Drawing.Size(48, 16);
            this.飄船checkBox.TabIndex = 99;
            this.飄船checkBox.Text = "飄船";
            this.飄船checkBox.UseVisualStyleBackColor = true;
            // 
            // 編號comboBox
            // 
            this.編號comboBox.FormattingEnabled = true;
            this.編號comboBox.Items.AddRange(new object[] {
            "0(無法販賣)",
            "1",
            "2",
            "3",
            "4"});
            this.編號comboBox.Location = new System.Drawing.Point(101, 87);
            this.編號comboBox.Name = "編號comboBox";
            this.編號comboBox.Size = new System.Drawing.Size(106, 20);
            this.編號comboBox.TabIndex = 11;
            this.編號comboBox.Text = "0(無法販賣)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "賣出持有船隻：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 7;
            this.label11.Text = "改倉：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 改倉numericUpDown
            // 
            this.改倉numericUpDown.Enabled = false;
            this.改倉numericUpDown.Location = new System.Drawing.Point(53, 64);
            this.改倉numericUpDown.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.改倉numericUpDown.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            this.改倉numericUpDown.Name = "改倉numericUpDown";
            this.改倉numericUpDown.Size = new System.Drawing.Size(44, 22);
            this.改倉numericUpDown.TabIndex = 6;
            this.改倉numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 5;
            this.label12.Text = "材質：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "船種：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 材質comboBox
            // 
            this.材質comboBox.Enabled = false;
            this.材質comboBox.FormattingEnabled = true;
            this.材質comboBox.Items.AddRange(new object[] {
            "雪松木",
            "紅松木",
            "桃木",
            "榆木",
            "柚木",
            "橡木",
            "紅木",
            "包銅",
            "紫壇木",
            "包鐵"});
            this.材質comboBox.Location = new System.Drawing.Point(53, 39);
            this.材質comboBox.Name = "材質comboBox";
            this.材質comboBox.Size = new System.Drawing.Size(154, 20);
            this.材質comboBox.TabIndex = 2;
            this.材質comboBox.Text = "桃木";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(103, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "%";
            this.label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 船種comboBox
            // 
            this.船種comboBox.FormattingEnabled = true;
            this.船種comboBox.ItemHeight = 12;
            this.船種comboBox.Location = new System.Drawing.Point(53, 15);
            this.船種comboBox.Name = "船種comboBox";
            this.船種comboBox.Size = new System.Drawing.Size(154, 20);
            this.船種comboBox.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 98;
            this.label9.Text = "進度：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 321);
            this.Controls.Add(this.StartCheck);
            this.Controls.Add(this.Sequence_Delete);
            this.Controls.Add(this.Loop_checkBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Sequence_listBox);
            this.Controls.Add(this.UserComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form6";
            this.Text = "序列幫手";
            this.groupBox1.ResumeLayout(false);
            this.生產.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.交易.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.閱讀.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.使用.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.移動.ResumeLayout(false);
            this.造船.ResumeLayout(false);
            this.造船.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.改倉numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox UserComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox 領域comboBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage 生產;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox 航線comboBox;
        private System.Windows.Forms.Button Sequence_Navigator;
        private System.Windows.Forms.Button Sequence_Buy;
        private System.Windows.Forms.Button Sequence_Delete;
        private System.Windows.Forms.Button Sequence_Produce;
        private System.Windows.Forms.Button Sequence_Sell;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox Sea_checkBox;
        private System.Windows.Forms.Button Init_Button;
        private System.Windows.Forms.ComboBox 書籍comboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox 配方comboBox;
        private System.Windows.Forms.ListBox Sequence_listBox;
        private System.Windows.Forms.CheckBox Loop_checkBox;
        private System.Windows.Forms.CheckBox StartCheck;
        private System.Windows.Forms.TabPage 交易;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 移動;
        private System.Windows.Forms.TabPage 閱讀;
        private System.Windows.Forms.TabPage 造船;
        private System.Windows.Forms.TabPage 使用;
        private System.Windows.Forms.ProgressBar 進度progressBar;
        private System.Windows.Forms.CheckBox 飄船checkBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox 編號comboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown 改倉numericUpDown;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox 材質comboBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox 船種comboBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button Sequence_TradeBook;
        private System.Windows.Forms.ComboBox 採買書comboBox;
        internal System.Windows.Forms.ComboBox CityComboBox;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button Sequence_Read;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox 開圖座標textBox;
        private System.Windows.Forms.TextBox 開圖地點textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Sequence_Skill;
        private System.Windows.Forms.ComboBox 開圖技能comboBox;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button Sequence_Report;
        private System.Windows.Forms.ComboBox 報告comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 開圖航線comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Sequence_Research;
        private System.Windows.Forms.ComboBox 專攻comboBox;
    }
}