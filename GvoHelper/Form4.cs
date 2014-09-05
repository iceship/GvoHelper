using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GvoHelper
{
    public partial class Form4 : Form
    {
        private GVOCall Call = new GVOCall();

        private bool Editing;
        private int SelectCityNo;
        //private int TargetID;

        public Form4()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            #region 加入城市名稱選單
            for (int i = 0; i < GVOCall.City.Length; i++)
            {
                if (GVOCall.City[i].Done)
                {
                    bool Done = false;
                    for (int j = 0; j < CityComboBox.Items.Count; j++)
                    {
                        if (CityComboBox.Items[j].ToString() == GVOCall.City[i].Name)
                        {
                            Done = true;
                            break;
                        }
                    }
                    if (!Done)
                        CityComboBox.Items.Add(GVOCall.City[i].Name);
                }
            }
            #endregion


            #region 記錄數量
            if (SelectCityNo != -1 && !Editing)
            {
                if (GVOCall.City[SelectCityNo].BuyMenuNum == BuyTradeList.Items.Count)
                {
                    for (int i = 0; i < BuyTradeList.Items.Count; i++)
                    {
                        #region 最大購買數量

                        
                        if (MaxBuyCheck.Checked)
                        {
                            if (BuyTradeList.Items[i].Checked)
                            {
                                if (BuyTradeList.Items[i].SubItems[3].Text != GVOCall.City[SelectCityNo]._BuyTradeInfo[i].MaxNum.ToString())
                                    BuyTradeList.Items[i].SubItems[3].Text = GVOCall.City[SelectCityNo]._BuyTradeInfo[i].MaxNum.ToString();
                            }
                            else
                                if (BuyTradeList.Items[i].SubItems[3].Text != "0")
                                    BuyTradeList.Items[i].SubItems[3].Text = "0";
                        }
                        //else if (SaleRankCheck.Checked)
                        //{
                            //int TradeNum = 0;

                            //for (int j = 0; j < Form1.UsingProcess[User].SkillNum; j++)
                            //{
                                //if (Form1.UsingProcess[User]._SkillInfo[j].Name == GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Type + "買賣")
                                //{
                                    //Console.WriteLine(Form1.UsingProcess[User]._SkillInfo[j].Name + " " + Form1.UsingProcess[User]._SkillInfo[j].Rank);
                                    //if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 199)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 8;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 499)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 7;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 999)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 6;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 1999)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 5;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 2999)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 4;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 3999)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 3;
                                    //else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 4999)
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 2;
                                    //else
                                        //TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 1;
                                    //break;
                                //}
                            //}
                            //if (BuyTradeList.Items[i].Checked)
                            //{
                                //if (BuyTradeList.Items[i].SubItems[3].Text != TradeNum.ToString())
                                    //BuyTradeList.Items[i].SubItems[3].Text = TradeNum.ToString();
                            //}
                            //else
                                //if (BuyTradeList.Items[i].SubItems[3].Text != "0")
                                    //BuyTradeList.Items[i].SubItems[3].Text = "0";
                        //}
                        #endregion

                        if (!string.IsNullOrWhiteSpace(BuyTradeList.Items[i].SubItems[3].Text))
                            GVOCall.City[SelectCityNo]._BuyTradeInfo[i].BuyNum = Convert.ToInt32(BuyTradeList.Items[i].SubItems[3].Text);
                    }
                }

                if (GVOCall.City[SelectCityNo].SellMenuNum == SellTradeList.Items.Count)
                {
                    for (int i = 0; i < SellTradeList.Items.Count; i++)
                    {
                        #region 最大出售數量
                        if (MaxSellCheck.Checked)
                        {
                            if (SellTradeList.Items[i].Checked)
                            {
                                if (ProfitCheck.Checked && GVOCall.City[SelectCityNo]._SellTradeInfo[i].Profit < 0)
                                    SellTradeList.Items[i].Checked = false;
                                else
                                    if (SellTradeList.Items[i].SubItems[3].Text != GVOCall.City[SelectCityNo]._SellTradeInfo[i].MaxNum.ToString())
                                        SellTradeList.Items[i].SubItems[3].Text = GVOCall.City[SelectCityNo]._SellTradeInfo[i].MaxNum.ToString();
                            }
                            else
                                if (SellTradeList.Items[i].SubItems[3].Text != "0")
                                    SellTradeList.Items[i].SubItems[3].Text = "0";
                        }
                        #endregion

                        if (!string.IsNullOrWhiteSpace(SellTradeList.Items[i].SubItems[3].Text))
                            GVOCall.City[SelectCityNo]._SellTradeInfo[i].SellNum = Convert.ToInt32(SellTradeList.Items[i].SubItems[3].Text);

                        #region 穫利顏色
                        if (GVOCall.City[SelectCityNo]._SellTradeInfo[i].Profit < 0)
                        {
                            if (SellTradeList.Items[i].BackColor != Color.Red)
                                SellTradeList.Items[i].BackColor = Color.Red;
                        }
                        else
                        {
                            if (SellTradeList.Items[i].BackColor != Color.White)
                                SellTradeList.Items[i].BackColor = Color.White;
                        }
                        #endregion
                    }
                }
            }
            #endregion

            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Call.GetConnectState(Form1.UsingProcess[User].hWnd) && Call.GetUserId(Form1.UsingProcess[User].hWnd) != 0)
                    {
                        timer1.Stop();
                        
                        TradeBook1.Text = "1星書：" + Form1.UsingProcess[User]._Item._LandItemInfo[0].Num.ToString();
                        TradeBook2.Text = "2星書：" + Form1.UsingProcess[User]._Item._LandItemInfo[1].Num.ToString();
                        TradeBook3.Text = "3星書：" + Form1.UsingProcess[User]._Item._LandItemInfo[2].Num.ToString();
                        TradeBook4.Text = "4星書：" + Form1.UsingProcess[User]._Item._LandItemInfo[3].Num.ToString();

                        
                        #region 記錄數量
                        if (SelectCityNo != -1 && !Editing)
                        {
                            if (GVOCall.City[SelectCityNo].BuyMenuNum == BuyTradeList.Items.Count)
                            {
                                for (int i = 0; i < BuyTradeList.Items.Count; i++)
                                {
                                    #region 最大購買數量
                                    if (MaxBuyCheck.Checked)
                                    {
                                        if (BuyTradeList.Items[i].Checked)
                                        {
                                            if (BuyTradeList.Items[i].SubItems[3].Text != GVOCall.City[SelectCityNo]._BuyTradeInfo[i].MaxNum.ToString())
                                                BuyTradeList.Items[i].SubItems[3].Text = GVOCall.City[SelectCityNo]._BuyTradeInfo[i].MaxNum.ToString();
                                        }
                                        else
                                            if (BuyTradeList.Items[i].SubItems[3].Text != "0")
                                                BuyTradeList.Items[i].SubItems[3].Text = "0";
                                    }
                                    else if (SaleRankCheck.Checked)
                                    {
                                        int TradeNum = 0;
                                        for (int j = 0; j < Form1.UsingProcess[User].SkillNum; j++)
                                        {
                                            if (Form1.UsingProcess[User]._SkillInfo[j].Name == GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Type + "買賣")
                                            {
                                                //Console.WriteLine(Form1.UsingProcess[User]._SkillInfo[j].Name + " " + Form1.UsingProcess[User]._SkillInfo[j].Rank);
                                                if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 199)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 8;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 499)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 7;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 999)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 6;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 1999)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 5;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 2999)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 4;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 3999)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 3;
                                                else if (GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price < 4999)
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 2;
                                                else
                                                    TradeNum = Form1.UsingProcess[User]._SkillInfo[j].Rank * 1;
                                                break;
                                            }
                                        }
                                        if (BuyTradeList.Items[i].Checked)
                                        {
                                            if (BuyTradeList.Items[i].SubItems[3].Text != TradeNum.ToString())
                                                BuyTradeList.Items[i].SubItems[3].Text = TradeNum.ToString();
                                        }
                                        else
                                            if (BuyTradeList.Items[i].SubItems[3].Text != "0")
                                                BuyTradeList.Items[i].SubItems[3].Text = "0";
                                    }
                                    #endregion

                                    if (!string.IsNullOrWhiteSpace(BuyTradeList.Items[i].SubItems[3].Text))
                                        GVOCall.City[SelectCityNo]._BuyTradeInfo[i].BuyNum = Convert.ToInt32(BuyTradeList.Items[i].SubItems[3].Text);
                                }
                            }

                            if (GVOCall.City[SelectCityNo].SellMenuNum == SellTradeList.Items.Count)
                            {
                                for (int i = 0; i < SellTradeList.Items.Count; i++)
                                {
                                    #region 最大出售數量
                                    if (MaxSellCheck.Checked)
                                    {
                                        if (SellTradeList.Items[i].Checked)
                                        {
                                            if (ProfitCheck.Checked && GVOCall.City[SelectCityNo]._SellTradeInfo[i].Profit < 0)
                                                SellTradeList.Items[i].Checked = false;
                                            else
                                                if (SellTradeList.Items[i].SubItems[3].Text != GVOCall.City[SelectCityNo]._SellTradeInfo[i].MaxNum.ToString())
                                                    SellTradeList.Items[i].SubItems[3].Text = GVOCall.City[SelectCityNo]._SellTradeInfo[i].MaxNum.ToString();
                                        }
                                        else
                                            if (SellTradeList.Items[i].SubItems[3].Text != "0")
                                                SellTradeList.Items[i].SubItems[3].Text = "0";
                                    }
                                    #endregion

                                    if (!string.IsNullOrWhiteSpace(SellTradeList.Items[i].SubItems[3].Text))
                                        GVOCall.City[SelectCityNo]._SellTradeInfo[i].SellNum = Convert.ToInt32(SellTradeList.Items[i].SubItems[3].Text);

                                    #region 穫利顏色
                                    if (GVOCall.City[SelectCityNo]._SellTradeInfo[i].Profit < 0)
                                    {
                                        if (SellTradeList.Items[i].BackColor != Color.Red)
                                            SellTradeList.Items[i].BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        if (SellTradeList.Items[i].BackColor != Color.White)
                                            SellTradeList.Items[i].BackColor = Color.White;
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion

                        if (LockCity.Checked && Form1.UsingProcess[User].CityNo != -1)
                        {
                            switch (Form1.UsingProcess[User].TradeMenu)
                            {
                                case "BuyPage":
                                    tabControl1.SelectedTab = BuyPage;
                                    break;
                                case "SellPage":
                                    tabControl1.SelectedTab = SellPage;
                                    break;
                            }

                            if (CityComboBox.Text != GVOCall.City[Form1.UsingProcess[User].CityNo].Name)
                                CityComboBox.Text = GVOCall.City[Form1.UsingProcess[User].CityNo].Name;
                            else
                            {
                                #region 交易品列表錯誤
                                if (BuyTradeList.Items.Count != GVOCall.City[Form1.UsingProcess[User].CityNo].BuyMenuNum || SellTradeList.Items.Count != GVOCall.City[Form1.UsingProcess[User].CityNo].SellMenuNum)
                                    CityComboBox.Text = "";
                                else
                                {
                                    if (BuyTradeList.Items.Count == GVOCall.City[Form1.UsingProcess[User].CityNo].BuyMenuNum)
                                    {
                                        for (int i = 0; i < BuyTradeList.Items.Count; i++)
                                        {
                                            //名稱
                                            if (BuyTradeList.Items[i].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Name)
                                            {
                                                BuyTradeList.Items[i].Checked = false;
                                                BuyTradeList.Items[i].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Name;
                                            }

                                            //單價
                                            if (BuyTradeList.Items[i].SubItems[1].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Price.ToString())
                                                BuyTradeList.Items[i].SubItems[1].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Price.ToString();
                                            //最大數量
                                            if (BuyTradeList.Items[i].SubItems[2].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].MaxNum.ToString())
                                                BuyTradeList.Items[i].SubItems[2].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].MaxNum.ToString();

                                            for (int j = 0; j < GVOCall.City[Form1.UsingProcess[User].CityNo].CheckedBuy.Length; j++)
                                            {
                                                if (BuyTradeList.Items[i].SubItems[4].Text == GVOCall.City[Form1.UsingProcess[User].CityNo].CheckedBuy[j].ToString())
                                                {
                                                    if (GVOCall.City[Form1.UsingProcess[User].CityNo].CheckedBuy[j] != 0)
                                                        BuyTradeList.Items[i].Checked = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (SellTradeList.Items.Count == GVOCall.City[Form1.UsingProcess[User].CityNo].SellMenuNum)
                                    {
                                        for (int i = 0; i < SellTradeList.Items.Count; i++)
                                        {
                                            if (SellTradeList.Items[i].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].Name)
                                                SellTradeList.Items[i].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].Name;
                                            if (SellTradeList.Items[i].SubItems[1].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].SellPrice.ToString())
                                                SellTradeList.Items[i].SubItems[1].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].SellPrice.ToString();
                                            if (SellTradeList.Items[i].SubItems[2].Text != GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].MaxNum.ToString())
                                                SellTradeList.Items[i].SubItems[2].Text = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].MaxNum.ToString();

                                            int ItemNo = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].ID % 1600000;
                                            if (GVOCall.City[Form1.UsingProcess[User].CityNo].CheckedSell[ItemNo])
                                                SellTradeList.Items[i].Checked = true;
                                            else
                                                SellTradeList.Items[i].Checked = false;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        switch (Form1.UsingProcess[User].target.Name)
                        {
                            //交易所
                            case "交易所店主":
                            case "交易所學徒":
                                //GVOCall.City[Form1.UsingProcess[User].CityNo].TraderID = TargetID;
                                if (Form1.UsingProcess[User].CityNo != -1)
                                {
                                    if (Call.GetBuyTradeMenu(Form1.UsingProcess[User].hWnd, ref GVOCall.City[Form1.UsingProcess[User].CityNo].BuyMenuNum, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo))
                                        Form1.UsingProcess[User].TradeMenu = "BuyPage";
                                    else if (Call.GetSellTradeMenu(Form1.UsingProcess[User].hWnd, ref GVOCall.City[Form1.UsingProcess[User].CityNo].SellMenuNum, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo))
                                        Form1.UsingProcess[User].TradeMenu = "SellPage";
                                }
                                else
                                    Form1.UsingProcess[User].TradeMenu = null;
                                break;
                            default:
                                Form1.UsingProcess[User].TradeMenu = null;
                                break;
                        }

                        timer1.Start();
                    }
                }
            }
        }

        private void CityComboBox_TextChanged(object sender, EventArgs e)
        {
            SelectCityNo = -1;
            for (int i = 0; i < GVOCall.City.Length; i++)
                if (CityComboBox.Text == GVOCall.City[i].Name)
                {
                    SelectCityNo = i;
                    break;
                }

            BuyTradeList.Items.Clear();
            SellTradeList.Items.Clear();

            if (SelectCityNo != -1)
            {
                Editing = true;

                #region 購買交易品清單
                for (int i = 0; i < GVOCall.City[SelectCityNo].BuyMenuNum; i++)
                {
                    //類別分群
                    string Type = GVOCall.City[SelectCityNo]._BuyTradeInfo[i].UseBook.ToString();
                    int GroupNo = BuyTradeList.Groups[Type].Items.Count;
                    //交易品名稱
                    BuyTradeList.Groups[Type].Items.Add(GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Name);
                    //交易品價格
                    BuyTradeList.Groups[Type].Items[GroupNo].SubItems.Add(GVOCall.City[SelectCityNo]._BuyTradeInfo[i].Price.ToString());
                    //交易品最大購買數量
                    BuyTradeList.Groups[Type].Items[GroupNo].SubItems.Add(GVOCall.City[SelectCityNo]._BuyTradeInfo[i].MaxNum.ToString());
                    //交易品欲購買數量
                    BuyTradeList.Groups[Type].Items[GroupNo].SubItems.Add(GVOCall.City[SelectCityNo]._BuyTradeInfo[i].BuyNum.ToString());
                    //交易品ID(不顯示)
                    BuyTradeList.Groups[Type].Items[GroupNo].SubItems.Add(GVOCall.City[SelectCityNo]._BuyTradeInfo[i].ID.ToString());
                    //勾選
                    for (int j = 0; j < GVOCall.City[SelectCityNo].CheckedBuy.Length; j++)
                        if (BuyTradeList.Groups[Type].Items[GroupNo].SubItems[4].Text == GVOCall.City[SelectCityNo].CheckedBuy[j].ToString())
                        {
                            if (GVOCall.City[SelectCityNo].CheckedBuy[j] != 0)
                                BuyTradeList.Groups[Type].Items[GroupNo].Checked = true;
                            break;
                        }
                    BuyTradeList.Items.Add(BuyTradeList.Groups[Type].Items[GroupNo]);
                }
                #endregion

                #region 出售交易品清單
                for (int i = 0; i < GVOCall.City[SelectCityNo].SellMenuNum; i++)
                {
                    //交易品名稱
                    SellTradeList.Items.Add(GVOCall.City[SelectCityNo]._SellTradeInfo[i].Name);
                    //交易品價格
                    SellTradeList.Items[i].SubItems.Add(GVOCall.City[SelectCityNo]._SellTradeInfo[i].SellPrice.ToString());
                    //交易品最大購買數量
                    SellTradeList.Items[i].SubItems.Add(GVOCall.City[SelectCityNo]._SellTradeInfo[i].MaxNum.ToString());
                    //交易品欲購買數量
                    SellTradeList.Items[i].SubItems.Add(GVOCall.City[SelectCityNo]._SellTradeInfo[i].SellNum.ToString());

                    int ItemNo = GVOCall.City[SelectCityNo]._SellTradeInfo[i].ID % 1600000;
                    if (ItemNo < GVOCall.City[SelectCityNo].CheckedSell.Length)
                    {
                        if (GVOCall.City[SelectCityNo].CheckedSell[ItemNo])
                            SellTradeList.Items[i].Checked = true;
                    }
                }
                #endregion

                Editing = false;
            }
        }

        private void BuyTradeList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (SelectCityNo != -1 && !Editing)
                for (int i = 0; i < BuyTradeList.Items.Count; i++)//記錄已勾選購買的交易品
                {
                    if (BuyTradeList.Items[i].Checked)
                        GVOCall.City[SelectCityNo].CheckedBuy[i] = Convert.ToInt32(BuyTradeList.Items[i].SubItems[4].Text);
                    else
                        GVOCall.City[SelectCityNo].CheckedBuy[i] = 0;
                }
        }

        private void SellTradeList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (SelectCityNo != -1 && !Editing)
            {
                for (int i = 0; i < SellTradeList.Items.Count; i++)
                {
                    int ItemNo = GVOCall.City[SelectCityNo]._SellTradeInfo[i].ID % 1600000;

                    if (SellTradeList.Items[i].Checked)
                        GVOCall.City[SelectCityNo].CheckedSell[ItemNo] = true;
                    else
                        GVOCall.City[SelectCityNo].CheckedSell[ItemNo] = false;
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = SellPage;
            tabControl1.SelectedTab = BuyPage;
        }

        private void LockCity_CheckedChanged(object sender, EventArgs e)
        {
            if (LockCity.Checked)
                CityComboBox.Enabled = false;
            else
                CityComboBox.Enabled = true;
        }

        private void SelectBuyALL_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BuyTradeList.Items.Count; i++)//記錄已勾選購買的交易品
            {
                if (!BuyTradeList.Items[i].Checked)
                    BuyTradeList.Items[i].Checked = true;
            }
        }

        private void SelectSellALL_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SellTradeList.Items.Count; i++)//記錄已勾選購買的交易品
            {
                if (!SellTradeList.Items[i].Checked)
                    SellTradeList.Items[i].Checked = true;
            }
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    int CheckTradeNum = 0;
                    for (int i = 0; i < BuyTradeList.Items.Count; i++)//記錄已勾選的交易品
                        if (BuyTradeList.Items[i].Checked && !string.IsNullOrWhiteSpace(BuyTradeList.Items[i].SubItems[3].Text))
                            ++CheckTradeNum;

                    int[] Multiple = new int[] { 1, 2, 5, 10, 20, 50, 100 };

                    Form1.TradeInfo[] TradeInfo = new Form1.TradeInfo[CheckTradeNum * 3];

                    int index = 0;
                    for (int i = 0; i < GVOCall.City[Form1.UsingProcess[User].CityNo].BuyMenuNum; i++)
                    {
                        if (BuyTradeList.Items[i].Checked && !string.IsNullOrWhiteSpace(BuyTradeList.Items[i].SubItems[3].Text))
                        {
                            int MaxNum = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].MaxNum;//最大購買量
                            int TradeNum = Convert.ToInt32(BuyTradeList.Items[i].SubItems[3].Text);//購買量
                            if (TradeNum > 0)
                            {
                                TradeInfo[index].ID = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].ID;
                                TradeInfo[index].Num = TradeNum;
                                TradeInfo[index].Price = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Price;
                                ++index;
                            }

                            #region 倍數轉換
                            /*
                            for (int j = 2; j >= 0; j--)
                            {
                                if (TradeNum > 0)
                                {
                                    int Num = TradeNum / GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Multiple[j];
                                    if (Num > 0)
                                    {
                                        for (int k = 0; k < Multiple.Length; k++)
                                            if (GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Multiple[j] == Multiple[k])
                                            {
                                                TradeInfo[index].Multiple = k;
                                                break;
                                            }
                                        TradeInfo[index].ID = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].ID;
                                        TradeInfo[index].Num = Num;
                                        TradeInfo[index].MultiplePrice = GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].MultiplePrice[j];

                                        TradeNum -= Num * GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Multiple[j];
                                        MaxNum -= Num * GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo[i].Multiple[j];
                                        ++index;
                                    }
                                }
                            }
*/
                            #endregion

                            #region 批次購買
                            if (batchcheck.Checked && index > 0)
                            {
                                if (PartyCheckBox.Checked)
                                {
                                    for (int Party = 0; Party < 5; Party++)
                                        if (Form1.UsingProcess[Party].hWnd != IntPtr.Zero)
                                            Call.BuyTrade(Form1.UsingProcess[Party].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                                }
                                else
                                    Call.BuyTrade(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);

                                index = 0;
                                Call.Delay(100);
                            }
                            #endregion
                        }
                    }

                    if (!batchcheck.Checked && index > 0)
                    {
                        if (PartyCheckBox.Checked)
                        {
                            for (int Party = 0; Party < 5; Party++)
                                if (Form1.UsingProcess[Party].hWnd != IntPtr.Zero)
                                    Call.BuyTrade(Form1.UsingProcess[Party].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                        }
                        else
                            Call.BuyTrade(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                    }
                }
            }
        }

        private void SellButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    int CheckTradeNum = 0;
                    for (int i = 0; i < SellTradeList.Items.Count; i++)//記錄已勾選的交易品
                        if (SellTradeList.Items[i].Checked && !string.IsNullOrWhiteSpace(SellTradeList.Items[i].SubItems[3].Text))
                            ++CheckTradeNum;

                    Form1.TradeInfo[] TradeInfo = new Form1.TradeInfo[CheckTradeNum];

                    int index = 0;
                    for (int i = 0; i < GVOCall.City[Form1.UsingProcess[User].CityNo].SellMenuNum; i++)
                    {
                        if (SellTradeList.Items[i].Checked && !string.IsNullOrWhiteSpace(SellTradeList.Items[i].SubItems[3].Text))
                        {
                            int MaxNum = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].MaxNum;
                            int TradeNum = Convert.ToInt32(SellTradeList.Items[i].SubItems[3].Text);

                            //if (MaxSellCheck.Checked)
                            //TradeNum = City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].MaxNum;

                            TradeInfo[index].Code = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].Code;
                            TradeInfo[index].Num = TradeNum;
                            TradeInfo[index].Price = GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo[i].SellPrice;
                            ++index;
                        }
                    }
                    if (index > 0)
                    {
                        if (PartyCheckBox.Checked)
                        {
                            for (int Party = 0; Party < 5; Party++)
                                if (Form1.UsingProcess[Party].hWnd != IntPtr.Zero)
                                    Call.SellTrade(Form1.UsingProcess[Party].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                        }
                        else
                            Call.SellTrade(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                    }
                }
            }
        }

        private void TradeBook_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    string book = ((Button)sender).Name.Replace("TradeBook", "");
                    for (int i = 0; i < Form1.UsingProcess[User]._Item.LandItemNum; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User]._Item._LandItemInfo[i].Name) && Form1.UsingProcess[User]._Item._LandItemInfo[i].Name.Contains(book))
                        {
                            Call.UseItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._LandItemInfo[i].Code);
                            --Form1.UsingProcess[User]._Item._LandItemInfo[i].Num;
                            break;
                        }
                    }
                }
            }
        }

        private void GetAllProcess(object sender, EventArgs e)
        {
            UserComboBox.Items.Clear();
            Call.SearchAllWindow();

            try
            {
                for (int i = 0; GVOCall.AllProcess[i] != IntPtr.Zero; i++)
                {
                    string SN = Call.GetServerName(GVOCall.AllProcess[i]);
                    string UN = Call.GetUserName(GVOCall.AllProcess[i]);
                    for (int Users = 0; Users < 5; Users++)
                    {
                        if (Form1.UsingProcess[Users].hWnd != IntPtr.Zero)
                        {
                            if (SN == Form1.UsingProcess[Users].ServerName && UN == Form1.UsingProcess[Users].Name)
                                UserComboBox.Items.Add("﹝" + SN + "﹞" + UN);
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
