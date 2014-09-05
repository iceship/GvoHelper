namespace GvoHelper
{
    partial class Form3
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.building = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.編號comboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.改倉numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.材質comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.種類comboBox = new System.Windows.Forms.ComboBox();
            this.進度progressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.飄船checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.research_comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.other_msg = new System.Windows.Forms.CheckBox();
            this.npc_msg = new System.Windows.Forms.CheckBox();
            this.skillexp_msg = new System.Windows.Forms.CheckBox();
            this.exp_msg = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.頻道ToolStripDropDownButton = new System.Windows.Forms.ToolStripSplitButton();
            this.TelltoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TargetToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PartyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CompanyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NshoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChatToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.shout_msg = new System.Windows.Forms.CheckBox();
            this.company_msg = new System.Windows.Forms.CheckBox();
            this.party_msg = new System.Windows.Forms.CheckBox();
            this.tell_msg = new System.Windows.Forms.CheckBox();
            this.say_msg = new System.Windows.Forms.CheckBox();
            this.link_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.改倉numericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserComboBox
            // 
            this.UserComboBox.FormattingEnabled = true;
            this.UserComboBox.Location = new System.Drawing.Point(12, 10);
            this.UserComboBox.Name = "UserComboBox";
            this.UserComboBox.Size = new System.Drawing.Size(225, 20);
            this.UserComboBox.TabIndex = 0;
            this.UserComboBox.DropDown += new System.EventHandler(this.GethWndMatrix);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // building
            // 
            this.building.Appearance = System.Windows.Forms.Appearance.Button;
            this.building.AutoSize = true;
            this.building.Location = new System.Drawing.Point(811, 10);
            this.building.Name = "building";
            this.building.Size = new System.Drawing.Size(63, 22);
            this.building.TabIndex = 66;
            this.building.Text = "開始造船";
            this.building.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.編號comboBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.改倉numericUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.材質comboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.種類comboBox);
            this.groupBox1.Location = new System.Drawing.Point(580, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 122);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "造船設定";
            // 
            // 編號comboBox
            // 
            this.編號comboBox.FormattingEnabled = true;
            this.編號comboBox.Items.AddRange(new object[] {
            "0(使用中無法賣)",
            "1",
            "2",
            "3",
            "4"});
            this.編號comboBox.Location = new System.Drawing.Point(105, 93);
            this.編號comboBox.Name = "編號comboBox";
            this.編號comboBox.Size = new System.Drawing.Size(113, 20);
            this.編號comboBox.TabIndex = 11;
            this.編號comboBox.Text = "0(使用中無法賣)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "賣出持有船隻：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "改倉：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 改倉numericUpDown
            // 
            this.改倉numericUpDown.Enabled = false;
            this.改倉numericUpDown.Location = new System.Drawing.Point(55, 70);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "材質：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "種類：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            this.材質comboBox.Location = new System.Drawing.Point(55, 45);
            this.材質comboBox.Name = "材質comboBox";
            this.材質comboBox.Size = new System.Drawing.Size(154, 20);
            this.材質comboBox.TabIndex = 2;
            this.材質comboBox.Text = "桃木";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(105, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "%";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 種類comboBox
            // 
            this.種類comboBox.FormattingEnabled = true;
            this.種類comboBox.ItemHeight = 12;
            this.種類comboBox.Location = new System.Drawing.Point(55, 21);
            this.種類comboBox.Name = "種類comboBox";
            this.種類comboBox.Size = new System.Drawing.Size(154, 20);
            this.種類comboBox.TabIndex = 0;
            // 
            // 進度progressBar
            // 
            this.進度progressBar.Location = new System.Drawing.Point(649, 223);
            this.進度progressBar.Name = "進度progressBar";
            this.進度progressBar.Size = new System.Drawing.Size(217, 21);
            this.進度progressBar.TabIndex = 70;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(578, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 72;
            this.label6.Text = "造船進度：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // 飄船checkBox
            // 
            this.飄船checkBox.AutoSize = true;
            this.飄船checkBox.Location = new System.Drawing.Point(826, 58);
            this.飄船checkBox.Name = "飄船checkBox";
            this.飄船checkBox.Size = new System.Drawing.Size(48, 16);
            this.飄船checkBox.TabIndex = 73;
            this.飄船checkBox.Text = "飄船";
            this.飄船checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.research_comboBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(580, 166);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(225, 51);
            this.groupBox3.TabIndex = 88;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "大學專攻設定";
            // 
            // research_comboBox
            // 
            this.research_comboBox.FormattingEnabled = true;
            this.research_comboBox.ItemHeight = 12;
            this.research_comboBox.Items.AddRange(new object[] {
            "獎學制度說明會*",
            "調查技術1*",
            "調查技術1**",
            "自然學・人文科學1*",
            "自然學・人文科學1**",
            "陸上戰鬥技術1*",
            "陸上戰鬥技術1**",
            "船舶修理技術1*"});
            this.research_comboBox.Location = new System.Drawing.Point(53, 20);
            this.research_comboBox.Name = "research_comboBox";
            this.research_comboBox.Size = new System.Drawing.Size(154, 20);
            this.research_comboBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "類別：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBox1
            // 
            this.textBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.textBox1.Location = new System.Drawing.Point(12, 36);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(440, 230);
            this.textBox1.TabIndex = 89;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.other_msg);
            this.groupBox2.Controls.Add(this.npc_msg);
            this.groupBox2.Controls.Add(this.skillexp_msg);
            this.groupBox2.Controls.Add(this.exp_msg);
            this.groupBox2.Location = new System.Drawing.Point(458, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(73, 256);
            this.groupBox2.TabIndex = 90;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "允許訊息";
            // 
            // other_msg
            // 
            this.other_msg.AutoSize = true;
            this.other_msg.Location = new System.Drawing.Point(6, 87);
            this.other_msg.Name = "other_msg";
            this.other_msg.Size = new System.Drawing.Size(48, 16);
            this.other_msg.TabIndex = 5;
            this.other_msg.Text = "其它";
            this.other_msg.UseVisualStyleBackColor = true;
            // 
            // npc_msg
            // 
            this.npc_msg.AutoSize = true;
            this.npc_msg.Location = new System.Drawing.Point(6, 65);
            this.npc_msg.Name = "npc_msg";
            this.npc_msg.Size = new System.Drawing.Size(46, 16);
            this.npc_msg.TabIndex = 4;
            this.npc_msg.Text = "NPC";
            this.npc_msg.UseVisualStyleBackColor = true;
            // 
            // skillexp_msg
            // 
            this.skillexp_msg.AutoSize = true;
            this.skillexp_msg.Location = new System.Drawing.Point(6, 43);
            this.skillexp_msg.Name = "skillexp_msg";
            this.skillexp_msg.Size = new System.Drawing.Size(60, 16);
            this.skillexp_msg.TabIndex = 3;
            this.skillexp_msg.Text = "熟練度";
            this.skillexp_msg.UseVisualStyleBackColor = true;
            // 
            // exp_msg
            // 
            this.exp_msg.AutoSize = true;
            this.exp_msg.Location = new System.Drawing.Point(6, 21);
            this.exp_msg.Name = "exp_msg";
            this.exp_msg.Size = new System.Drawing.Size(48, 16);
            this.exp_msg.TabIndex = 2;
            this.exp_msg.Text = "經驗";
            this.exp_msg.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.頻道ToolStripDropDownButton,
            this.ChatToolStripTextBox});
            this.statusStrip1.Location = new System.Drawing.Point(0, 275);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(542, 23);
            this.statusStrip1.TabIndex = 91;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // 頻道ToolStripDropDownButton
            // 
            this.頻道ToolStripDropDownButton.BackColor = System.Drawing.Color.Transparent;
            this.頻道ToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.頻道ToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TelltoolStripMenuItem,
            this.SayToolStripMenuItem,
            this.ShoutToolStripMenuItem,
            this.PartyToolStripMenuItem,
            this.CompanyToolStripMenuItem,
            this.NshoutToolStripMenuItem,
            this.ServerToolStripMenuItem});
            this.頻道ToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.頻道ToolStripDropDownButton.Name = "頻道ToolStripDropDownButton";
            this.頻道ToolStripDropDownButton.Size = new System.Drawing.Size(84, 21);
            this.頻道ToolStripDropDownButton.Text = "頻道：公開";
            this.頻道ToolStripDropDownButton.ToolTipText = " ";
            // 
            // TelltoolStripMenuItem
            // 
            this.TelltoolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.TelltoolStripMenuItem.CheckOnClick = true;
            this.TelltoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TargetToolStripComboBox});
            this.TelltoolStripMenuItem.Name = "TelltoolStripMenuItem";
            this.TelltoolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.TelltoolStripMenuItem.Text = "Tell";
            this.TelltoolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // TargetToolStripComboBox
            // 
            this.TargetToolStripComboBox.Name = "TargetToolStripComboBox";
            this.TargetToolStripComboBox.Size = new System.Drawing.Size(121, 24);
            this.TargetToolStripComboBox.TextChanged += new System.EventHandler(this.TargetToolStripComboBox_TextChanged);
            // 
            // SayToolStripMenuItem
            // 
            this.SayToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.SayToolStripMenuItem.Checked = true;
            this.SayToolStripMenuItem.CheckOnClick = true;
            this.SayToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SayToolStripMenuItem.Name = "SayToolStripMenuItem";
            this.SayToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SayToolStripMenuItem.Text = "Say";
            this.SayToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ShoutToolStripMenuItem
            // 
            this.ShoutToolStripMenuItem.Name = "ShoutToolStripMenuItem";
            this.ShoutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ShoutToolStripMenuItem.Text = "Shout";
            this.ShoutToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // PartyToolStripMenuItem
            // 
            this.PartyToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.PartyToolStripMenuItem.CheckOnClick = true;
            this.PartyToolStripMenuItem.Name = "PartyToolStripMenuItem";
            this.PartyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.PartyToolStripMenuItem.Text = "Party";
            this.PartyToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // CompanyToolStripMenuItem
            // 
            this.CompanyToolStripMenuItem.Name = "CompanyToolStripMenuItem";
            this.CompanyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.CompanyToolStripMenuItem.Text = "Company";
            this.CompanyToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // NshoutToolStripMenuItem
            // 
            this.NshoutToolStripMenuItem.Name = "NshoutToolStripMenuItem";
            this.NshoutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.NshoutToolStripMenuItem.Text = "NShout";
            this.NshoutToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ServerToolStripMenuItem
            // 
            this.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
            this.ServerToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ServerToolStripMenuItem.Text = "聊天伺服器";
            this.ServerToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ChatToolStripTextBox
            // 
            this.ChatToolStripTextBox.Name = "ChatToolStripTextBox";
            this.ChatToolStripTextBox.Size = new System.Drawing.Size(400, 23);
            this.ChatToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatToolStripTextBox_KeyDown);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.shout_msg);
            this.groupBox4.Controls.Add(this.company_msg);
            this.groupBox4.Controls.Add(this.party_msg);
            this.groupBox4.Controls.Add(this.tell_msg);
            this.groupBox4.Controls.Add(this.say_msg);
            this.groupBox4.Location = new System.Drawing.Point(458, 119);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(73, 147);
            this.groupBox4.TabIndex = 91;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "允許頻道";
            // 
            // shout_msg
            // 
            this.shout_msg.AutoSize = true;
            this.shout_msg.Checked = true;
            this.shout_msg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shout_msg.Location = new System.Drawing.Point(6, 106);
            this.shout_msg.Name = "shout_msg";
            this.shout_msg.Size = new System.Drawing.Size(51, 16);
            this.shout_msg.TabIndex = 6;
            this.shout_msg.Text = "Shout";
            this.shout_msg.UseVisualStyleBackColor = true;
            // 
            // company_msg
            // 
            this.company_msg.AutoSize = true;
            this.company_msg.Checked = true;
            this.company_msg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.company_msg.Location = new System.Drawing.Point(6, 84);
            this.company_msg.Name = "company_msg";
            this.company_msg.Size = new System.Drawing.Size(48, 16);
            this.company_msg.TabIndex = 5;
            this.company_msg.Text = "商會";
            this.company_msg.UseVisualStyleBackColor = true;
            // 
            // party_msg
            // 
            this.party_msg.AutoSize = true;
            this.party_msg.Checked = true;
            this.party_msg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.party_msg.Location = new System.Drawing.Point(6, 63);
            this.party_msg.Name = "party_msg";
            this.party_msg.Size = new System.Drawing.Size(48, 16);
            this.party_msg.TabIndex = 4;
            this.party_msg.Text = "艦隊";
            this.party_msg.UseVisualStyleBackColor = true;
            // 
            // tell_msg
            // 
            this.tell_msg.AutoSize = true;
            this.tell_msg.Checked = true;
            this.tell_msg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tell_msg.Location = new System.Drawing.Point(6, 21);
            this.tell_msg.Name = "tell_msg";
            this.tell_msg.Size = new System.Drawing.Size(60, 16);
            this.tell_msg.TabIndex = 3;
            this.tell_msg.Text = "悄悄話";
            this.tell_msg.UseVisualStyleBackColor = true;
            // 
            // say_msg
            // 
            this.say_msg.AutoSize = true;
            this.say_msg.Checked = true;
            this.say_msg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.say_msg.Location = new System.Drawing.Point(6, 42);
            this.say_msg.Name = "say_msg";
            this.say_msg.Size = new System.Drawing.Size(48, 16);
            this.say_msg.TabIndex = 2;
            this.say_msg.Text = "公開";
            this.say_msg.UseVisualStyleBackColor = true;
            // 
            // link_button
            // 
            this.link_button.Location = new System.Drawing.Point(362, 8);
            this.link_button.Name = "link_button";
            this.link_button.Size = new System.Drawing.Size(90, 23);
            this.link_button.TabIndex = 92;
            this.link_button.Text = "伺服器連線";
            this.link_button.UseVisualStyleBackColor = true;
            this.link_button.Click += new System.EventHandler(this.link_button_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 298);
            this.Controls.Add(this.link_button);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.進度progressBar);
            this.Controls.Add(this.飄船checkBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.building);
            this.Controls.Add(this.UserComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chat Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.改倉numericUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox UserComboBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox building;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 種類comboBox;
        private System.Windows.Forms.NumericUpDown 改倉numericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 材質comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar 進度progressBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox 編號comboBox;
        private System.Windows.Forms.CheckBox 飄船checkBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox research_comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox exp_msg;
        private System.Windows.Forms.CheckBox skillexp_msg;
        private System.Windows.Forms.CheckBox npc_msg;
        private System.Windows.Forms.CheckBox other_msg;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSplitButton 頻道ToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem SayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PartyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CompanyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox ChatToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem TelltoolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox TargetToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem ShoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NshoutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox shout_msg;
        private System.Windows.Forms.CheckBox company_msg;
        private System.Windows.Forms.CheckBox party_msg;
        private System.Windows.Forms.CheckBox tell_msg;
        private System.Windows.Forms.CheckBox say_msg;
        private System.Windows.Forms.Button link_button;
    }
}