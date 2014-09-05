namespace GvoHelper
{
    partial class Form4
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("未辨識類別", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("採買書類別1", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("採買書類別2", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("採買書類別3", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("採買書類別4", System.Windows.Forms.HorizontalAlignment.Center);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BuyButton = new System.Windows.Forms.Button();
            this.CityComboBox = new System.Windows.Forms.ComboBox();
            this.SelectBuyALL = new System.Windows.Forms.Button();
            this.batchcheck = new System.Windows.Forms.CheckBox();
            this.LockCity = new System.Windows.Forms.CheckBox();
            this.MaxBuyCheck = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.BuyPage = new System.Windows.Forms.TabPage();
            this.SaleRankCheck = new System.Windows.Forms.CheckBox();
            this.TradeBook4 = new System.Windows.Forms.Button();
            this.TradeBook3 = new System.Windows.Forms.Button();
            this.TradeBook2 = new System.Windows.Forms.Button();
            this.TradeBook1 = new System.Windows.Forms.Button();
            this.BuyTradeList = new GvoHelper.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SellPage = new System.Windows.Forms.TabPage();
            this.MaxSellCheck = new System.Windows.Forms.CheckBox();
            this.SellButton = new System.Windows.Forms.Button();
            this.ProfitCheck = new System.Windows.Forms.CheckBox();
            this.SelectSellALL = new System.Windows.Forms.Button();
            this.SellTradeList = new GvoHelper.ListViewEx();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserComboBox = new System.Windows.Forms.ComboBox();
            this.PartyCheckBox = new System.Windows.Forms.CheckBox();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.BuyPage.SuspendLayout();
            this.SellPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BuyButton
            // 
            this.BuyButton.Location = new System.Drawing.Point(372, 116);
            this.BuyButton.Name = "BuyButton";
            this.BuyButton.Size = new System.Drawing.Size(77, 27);
            this.BuyButton.TabIndex = 3;
            this.BuyButton.Text = "購買";
            this.BuyButton.UseVisualStyleBackColor = true;
            this.BuyButton.Click += new System.EventHandler(this.BuyButton_Click);
            // 
            // CityComboBox
            // 
            this.CityComboBox.FormattingEnabled = true;
            this.CityComboBox.Location = new System.Drawing.Point(12, 10);
            this.CityComboBox.MaxDropDownItems = 5;
            this.CityComboBox.Name = "CityComboBox";
            this.CityComboBox.Size = new System.Drawing.Size(150, 20);
            this.CityComboBox.TabIndex = 84;
            this.CityComboBox.TabStop = false;
            this.CityComboBox.TextChanged += new System.EventHandler(this.CityComboBox_TextChanged);
            // 
            // SelectBuyALL
            // 
            this.SelectBuyALL.Location = new System.Drawing.Point(372, 83);
            this.SelectBuyALL.Name = "SelectBuyALL";
            this.SelectBuyALL.Size = new System.Drawing.Size(77, 27);
            this.SelectBuyALL.TabIndex = 85;
            this.SelectBuyALL.Text = "全選";
            this.SelectBuyALL.UseVisualStyleBackColor = true;
            this.SelectBuyALL.Click += new System.EventHandler(this.SelectBuyALL_Click);
            // 
            // batchcheck
            // 
            this.batchcheck.AutoSize = true;
            this.batchcheck.Location = new System.Drawing.Point(372, 50);
            this.batchcheck.Name = "batchcheck";
            this.batchcheck.Size = new System.Drawing.Size(72, 16);
            this.batchcheck.TabIndex = 88;
            this.batchcheck.Text = "批次購買";
            this.batchcheck.UseVisualStyleBackColor = true;
            // 
            // LockCity
            // 
            this.LockCity.AutoSize = true;
            this.LockCity.Location = new System.Drawing.Point(168, 12);
            this.LockCity.Name = "LockCity";
            this.LockCity.Size = new System.Drawing.Size(72, 16);
            this.LockCity.TabIndex = 89;
            this.LockCity.Text = "鎖定城市";
            this.LockCity.UseVisualStyleBackColor = true;
            this.LockCity.CheckedChanged += new System.EventHandler(this.LockCity_CheckedChanged);
            // 
            // MaxBuyCheck
            // 
            this.MaxBuyCheck.AutoSize = true;
            this.MaxBuyCheck.Location = new System.Drawing.Point(372, 6);
            this.MaxBuyCheck.Name = "MaxBuyCheck";
            this.MaxBuyCheck.Size = new System.Drawing.Size(84, 16);
            this.MaxBuyCheck.TabIndex = 90;
            this.MaxBuyCheck.Text = "最大購買量";
            this.MaxBuyCheck.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.BuyPage);
            this.tabControl1.Controls.Add(this.SellPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(470, 320);
            this.tabControl1.TabIndex = 91;
            // 
            // BuyPage
            // 
            this.BuyPage.BackColor = System.Drawing.Color.Transparent;
            this.BuyPage.Controls.Add(this.SaleRankCheck);
            this.BuyPage.Controls.Add(this.TradeBook4);
            this.BuyPage.Controls.Add(this.TradeBook3);
            this.BuyPage.Controls.Add(this.TradeBook2);
            this.BuyPage.Controls.Add(this.TradeBook1);
            this.BuyPage.Controls.Add(this.BuyTradeList);
            this.BuyPage.Controls.Add(this.MaxBuyCheck);
            this.BuyPage.Controls.Add(this.BuyButton);
            this.BuyPage.Controls.Add(this.batchcheck);
            this.BuyPage.Controls.Add(this.SelectBuyALL);
            this.BuyPage.Location = new System.Drawing.Point(4, 25);
            this.BuyPage.Name = "BuyPage";
            this.BuyPage.Padding = new System.Windows.Forms.Padding(3);
            this.BuyPage.Size = new System.Drawing.Size(462, 291);
            this.BuyPage.TabIndex = 0;
            this.BuyPage.Text = "購買";
            // 
            // SaleRankCheck
            // 
            this.SaleRankCheck.AutoSize = true;
            this.SaleRankCheck.Enabled = false;
            this.SaleRankCheck.Location = new System.Drawing.Point(372, 28);
            this.SaleRankCheck.Name = "SaleRankCheck";
            this.SaleRankCheck.Size = new System.Drawing.Size(72, 16);
            this.SaleRankCheck.TabIndex = 95;
            this.SaleRankCheck.Text = "取引等級";
            this.SaleRankCheck.UseVisualStyleBackColor = true;
            // 
            // TradeBook4
            // 
            this.TradeBook4.AutoSize = true;
            this.TradeBook4.Location = new System.Drawing.Point(372, 259);
            this.TradeBook4.Name = "TradeBook4";
            this.TradeBook4.Size = new System.Drawing.Size(77, 27);
            this.TradeBook4.TabIndex = 94;
            this.TradeBook4.Text = "4星書：";
            this.TradeBook4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TradeBook4.UseVisualStyleBackColor = true;
            this.TradeBook4.Click += new System.EventHandler(this.TradeBook_Click);
            // 
            // TradeBook3
            // 
            this.TradeBook3.AutoSize = true;
            this.TradeBook3.Location = new System.Drawing.Point(372, 226);
            this.TradeBook3.Name = "TradeBook3";
            this.TradeBook3.Size = new System.Drawing.Size(77, 27);
            this.TradeBook3.TabIndex = 93;
            this.TradeBook3.Text = "3星書：";
            this.TradeBook3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TradeBook3.UseVisualStyleBackColor = true;
            this.TradeBook3.Click += new System.EventHandler(this.TradeBook_Click);
            // 
            // TradeBook2
            // 
            this.TradeBook2.AutoSize = true;
            this.TradeBook2.Location = new System.Drawing.Point(372, 193);
            this.TradeBook2.Name = "TradeBook2";
            this.TradeBook2.Size = new System.Drawing.Size(77, 27);
            this.TradeBook2.TabIndex = 92;
            this.TradeBook2.Text = "2星書：";
            this.TradeBook2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TradeBook2.UseVisualStyleBackColor = true;
            this.TradeBook2.Click += new System.EventHandler(this.TradeBook_Click);
            // 
            // TradeBook1
            // 
            this.TradeBook1.AutoSize = true;
            this.TradeBook1.Location = new System.Drawing.Point(372, 160);
            this.TradeBook1.Name = "TradeBook1";
            this.TradeBook1.Size = new System.Drawing.Size(77, 27);
            this.TradeBook1.TabIndex = 91;
            this.TradeBook1.Text = "1星書：";
            this.TradeBook1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TradeBook1.UseVisualStyleBackColor = true;
            this.TradeBook1.Click += new System.EventHandler(this.TradeBook_Click);
            // 
            // BuyTradeList
            // 
            this.BuyTradeList.CheckBoxes = true;
            this.BuyTradeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.BuyTradeList.FullRowSelect = true;
            this.BuyTradeList.GridLines = true;
            listViewGroup1.Header = "未辨識類別";
            listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup1.Name = "0";
            listViewGroup2.Header = "採買書類別1";
            listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup2.Name = "1";
            listViewGroup3.Header = "採買書類別2";
            listViewGroup3.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup3.Name = "2";
            listViewGroup4.Header = "採買書類別3";
            listViewGroup4.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup4.Name = "3";
            listViewGroup5.Header = "採買書類別4";
            listViewGroup5.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup5.Name = "4";
            this.BuyTradeList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5});
            this.BuyTradeList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.BuyTradeList.Location = new System.Drawing.Point(6, 6);
            this.BuyTradeList.Name = "BuyTradeList";
            this.BuyTradeList.Size = new System.Drawing.Size(360, 280);
            this.BuyTradeList.TabIndex = 2;
            this.BuyTradeList.UseCompatibleStateImageBehavior = false;
            this.BuyTradeList.View = System.Windows.Forms.View.Details;
            this.BuyTradeList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.BuyTradeList_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "交易品名稱";
            this.columnHeader1.Width = 125;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "單價";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "最大購買量";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "欲購買數量";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 75;
            // 
            // SellPage
            // 
            this.SellPage.BackColor = System.Drawing.Color.Transparent;
            this.SellPage.Controls.Add(this.MaxSellCheck);
            this.SellPage.Controls.Add(this.SellButton);
            this.SellPage.Controls.Add(this.ProfitCheck);
            this.SellPage.Controls.Add(this.SelectSellALL);
            this.SellPage.Controls.Add(this.SellTradeList);
            this.SellPage.Location = new System.Drawing.Point(4, 25);
            this.SellPage.Name = "SellPage";
            this.SellPage.Padding = new System.Windows.Forms.Padding(3);
            this.SellPage.Size = new System.Drawing.Size(462, 291);
            this.SellPage.TabIndex = 1;
            this.SellPage.Text = "出售";
            // 
            // MaxSellCheck
            // 
            this.MaxSellCheck.AutoSize = true;
            this.MaxSellCheck.Location = new System.Drawing.Point(372, 6);
            this.MaxSellCheck.Name = "MaxSellCheck";
            this.MaxSellCheck.Size = new System.Drawing.Size(84, 16);
            this.MaxSellCheck.TabIndex = 94;
            this.MaxSellCheck.Text = "最大持有量";
            this.MaxSellCheck.UseVisualStyleBackColor = true;
            // 
            // SellButton
            // 
            this.SellButton.Location = new System.Drawing.Point(372, 83);
            this.SellButton.Name = "SellButton";
            this.SellButton.Size = new System.Drawing.Size(77, 27);
            this.SellButton.TabIndex = 91;
            this.SellButton.Text = "出售";
            this.SellButton.UseVisualStyleBackColor = true;
            this.SellButton.Click += new System.EventHandler(this.SellButton_Click);
            // 
            // ProfitCheck
            // 
            this.ProfitCheck.AutoSize = true;
            this.ProfitCheck.Location = new System.Drawing.Point(372, 28);
            this.ProfitCheck.Name = "ProfitCheck";
            this.ProfitCheck.Size = new System.Drawing.Size(48, 16);
            this.ProfitCheck.TabIndex = 93;
            this.ProfitCheck.Text = "穫利";
            this.ProfitCheck.UseVisualStyleBackColor = true;
            // 
            // SelectSellALL
            // 
            this.SelectSellALL.Location = new System.Drawing.Point(372, 50);
            this.SelectSellALL.Name = "SelectSellALL";
            this.SelectSellALL.Size = new System.Drawing.Size(77, 27);
            this.SelectSellALL.TabIndex = 92;
            this.SelectSellALL.Text = "全選";
            this.SelectSellALL.UseVisualStyleBackColor = true;
            this.SelectSellALL.Click += new System.EventHandler(this.SelectSellALL_Click);
            // 
            // SellTradeList
            // 
            this.SellTradeList.CheckBoxes = true;
            this.SellTradeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.SellTradeList.FullRowSelect = true;
            this.SellTradeList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SellTradeList.Location = new System.Drawing.Point(6, 6);
            this.SellTradeList.Name = "SellTradeList";
            this.SellTradeList.Size = new System.Drawing.Size(360, 280);
            this.SellTradeList.TabIndex = 3;
            this.SellTradeList.UseCompatibleStateImageBehavior = false;
            this.SellTradeList.View = System.Windows.Forms.View.Details;
            this.SellTradeList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.SellTradeList_ItemChecked);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "交易品名稱";
            this.columnHeader5.Width = 125;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "單價";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "持有數量";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 75;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "欲出售數量";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 75;
            // 
            // UserComboBox
            // 
            this.UserComboBox.FormattingEnabled = true;
            this.UserComboBox.Location = new System.Drawing.Point(257, 8);
            this.UserComboBox.MaxDropDownItems = 5;
            this.UserComboBox.Name = "UserComboBox";
            this.UserComboBox.Size = new System.Drawing.Size(225, 20);
            this.UserComboBox.TabIndex = 92;
            this.UserComboBox.TabStop = false;
            this.UserComboBox.DropDown += new System.EventHandler(this.GetAllProcess);
            // 
            // PartyCheckBox
            // 
            this.PartyCheckBox.AutoSize = true;
            this.PartyCheckBox.Location = new System.Drawing.Point(257, 31);
            this.PartyCheckBox.Name = "PartyCheckBox";
            this.PartyCheckBox.Size = new System.Drawing.Size(72, 16);
            this.PartyCheckBox.TabIndex = 93;
            this.PartyCheckBox.Text = "艦隊動作";
            this.PartyCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 374);
            this.Controls.Add(this.PartyCheckBox);
            this.Controls.Add(this.UserComboBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.LockCity);
            this.Controls.Add(this.CityComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form4";
            this.Text = "交易幫手";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.tabControl1.ResumeLayout(false);
            this.BuyPage.ResumeLayout(false);
            this.BuyPage.PerformLayout();
            this.SellPage.ResumeLayout(false);
            this.SellPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private ListViewEx BuyTradeList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button BuyButton;
        internal System.Windows.Forms.ComboBox CityComboBox;
        private System.Windows.Forms.Button SelectBuyALL;
        private System.Windows.Forms.CheckBox batchcheck;
        private System.Windows.Forms.CheckBox LockCity;
        private System.Windows.Forms.CheckBox MaxBuyCheck;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage BuyPage;
        private System.Windows.Forms.TabPage SellPage;
        private ListViewEx SellTradeList;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.CheckBox MaxSellCheck;
        private System.Windows.Forms.Button SellButton;
        private System.Windows.Forms.CheckBox ProfitCheck;
        private System.Windows.Forms.Button SelectSellALL;
        private System.Windows.Forms.Button TradeBook4;
        private System.Windows.Forms.Button TradeBook3;
        private System.Windows.Forms.Button TradeBook2;
        private System.Windows.Forms.Button TradeBook1;
        internal System.Windows.Forms.ComboBox UserComboBox;
        private System.Windows.Forms.CheckBox PartyCheckBox;
        private System.Windows.Forms.CheckBox SaleRankCheck;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
    }
}