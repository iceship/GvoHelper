using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GvoHelper
{
    public partial class Form6 : Form
    {
        private GVOCall Call = new GVOCall();

        public Form6()
        {
            InitializeComponent();
        }

        private int SelectCityNo;

        private int indexOfItemUnderMouseToDrag;
        private int indexOfItemUnderMouseToDrop;

        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;

        private Cursor MyNoDropCursor;
        private Cursor MyNormalCursor;

        //取得所有遊戲視窗hWnd
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    if (StartCheck.Checked)
                    {
                        StartCheck.Text = "停止";
                        Form1.UsingProcess[User].Sequence = true;
                    }
                    else
                    {
                        StartCheck.Text = "開始";
                        Form1.UsingProcess[User].Sequence = false;
                    }

                    if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Call.GetConnectState(Form1.UsingProcess[User].hWnd) && Call.GetUserId(Form1.UsingProcess[User].hWnd) != 0)
                    {
                        timer1.Stop();

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

                        #region 交易清單
                        if (SelectCityNo != -1 && GVOCall.City[SelectCityNo].Done)
                        {
                            textBox1.Clear();
                            string Buy_Name = "購買：";
                            for (int i = 0; i < GVOCall.City[SelectCityNo].CheckedBuy.Length; i++)
                            {
                                if (GVOCall.City[SelectCityNo].CheckedBuy[i] != 0)
                                {
                                    for (int j = 0; j < GVOCall.City[SelectCityNo]._BuyTradeInfo.Length; j++)
                                    {
                                        if (GVOCall.City[SelectCityNo]._BuyTradeInfo[j].ID == GVOCall.City[SelectCityNo].CheckedBuy[i])
                                            Buy_Name += GVOCall.City[SelectCityNo]._BuyTradeInfo[j].Name + "、";
                                    }
                                }
                            }
                            if (Buy_Name != "購買：")
                                Buy_Name = Buy_Name.Substring(0, Buy_Name.Length - 1);
                            textBox1.AppendText(Buy_Name);
                        }
                        #endregion

                        #region 動作序列
                        for (int Sequence_Index = 0; Sequence_Index < Sequence_listBox.Items.Count; Sequence_Index++)
                        {
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Action = Sequence_listBox.Items[Sequence_Index].ToString().Substring(0, Sequence_listBox.Items[Sequence_Index].ToString().IndexOf("]") + 1);

                            if (Form1.UsingProcess[User].Sequence)
                            {
                                if (!Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete)
                                {
                                    Sequence_listBox.SelectedIndex = Sequence_Index;
                                    //動作判斷
                                    Action(User, Sequence_Index);
                                    break;
                                }
                                else
                                {
                                    //loop
                                    if (Sequence_Index + 1 == Sequence_listBox.Items.Count)
                                    {
                                        if (Loop_checkBox.Checked)
                                        {
                                            for (int i = 0; i < Sequence_listBox.Items.Count; i++)
                                                Form1.UsingProcess[User]._Action_Sequence[i].Complete = false;

                                            if (Form1.UsingProcess[User].User_Type == "ELSE")
                                                StartCheck.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        timer1.Start();
                    }
                    else
                        Form1.UsingProcess[User].Sequence = false;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    timer2.Stop();
                    bool Init = true;
                    //檢查未初始化的配方
                    for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
                    {
                        //配方未初始化
                        if (!Form1.UsingProcess[User]._Item._BookInfo[i].Formula_Init)
                            Init = false;
                    }

                    if (!Init)
                    {
                        Form1.uInfo.Action_Sequence Produce_Sequence = new Form1.uInfo.Action_Sequence();
                        Produce_Sequence.Formula_Name = "初始化";
                        new CityCall().Produce(User, ref  Produce_Sequence);
                        timer2.Start();
                    }
                    else
                    {
                        Init_Button.Text = "初始化完成";
                        for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
                        {
                            //配方未初始化
                            if (Form1.UsingProcess[User]._Item._BookInfo[i].Formula_Init)
                            {
                                Console.WriteLine("[" + Form1.UsingProcess[User]._Item._BookInfo[i].Name + "]");
                                for (int j = 0; j < Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo.Length; j++)
                                {
                                    Console.WriteLine(Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo[j].ID + "=" + Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo[j].Formula_Name);
                                }

                            }
                        }
                    }
                }
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Call.GetConnectState(Form1.UsingProcess[User].hWnd) && Call.GetUserId(Form1.UsingProcess[User].hWnd) != 0)
                    {
                        timer1.Stop();
                        if (Form1.UsingProcess[User].User_Type == "猜猜看")
                        {
                            //if (building.Checked)
                            {
                                //building.Text = "停止造船";

                                if (Form1.UsingProcess[User].Place == 0x4)
                                {
                                    //if (進度progressBar.Maximum != BoatbuildingTime[種類comboBox.SelectedIndex])
                                    //    進度progressBar.Maximum = BoatbuildingTime[種類comboBox.SelectedIndex];

                                    if (進度progressBar.Value != Form1.UsingProcess[User].MaritimeDays && Form1.UsingProcess[User].MaritimeDays != -1)
                                        if (Form1.UsingProcess[User].MaritimeDays <= 進度progressBar.Maximum)
                                            進度progressBar.Value = Form1.UsingProcess[User].MaritimeDays;

                                    if (Form1.UsingProcess[User].MaritimeDays >= 進度progressBar.Maximum)
                                    {
                                        for (int i = 0; i < Form1.UsingProcess[User]._Regular.Length; i++)
                                        {
                                            if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[i].Coordinate) < 25)
                                            {
                                                Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[i].ID);
                                                Form1.UsingProcess[User].BuildShipDone = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        #region 飄船
                                        if (飄船checkBox.Checked)
                                        {
                                            for (int i = 0; i < Form1.UsingProcess[User]._Regular.Length; i++)
                                            {
                                                if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[i].Coordinate) < 25)
                                                {
                                                    if (Form1.UsingProcess[User].TimeCount > 3)
                                                    {
                                                        Call.Turn(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[i].Coordinate.X, Form1.UsingProcess[User]._Regular[i].Coordinate.Y);
                                                        Form1.UsingProcess[User].TimeCount = 0;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else if (Form1.UsingProcess[User].Place == 0x1C)//若在碼頭時進入城市
                                {
                                    if (Form1.UsingProcess[User].TimeCount > 3)
                                    {
                                        if (!Form1.UsingProcess[User].BuildShipDone)
                                            Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, 0x8F);
                                        else
                                            Call.Sailing(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID);
                                        Form1.UsingProcess[User].TimeCount = 0;
                                    }
                                }
                                else
                                {
                                    if (!Form1.UsingProcess[User].BuildShipDone)
                                    {
                                        if (Form1.UsingProcess[User].Place == 0x8 && new CityCall().FindNPC(User, "造船廠老闆", ""))
                                        {
                                            //取船
                                            if (!Form1.UsingProcess[User].GetShip)
                                            {
                                                //Call.Talk(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo].ShipyardID);
                                                Form1.UsingProcess[User].GetShip = true;
                                            }
                                            else
                                            {
                                                if (Form1.UsingProcess[User].ShipCode == -1)
                                                {
                                                    #region 造船
                                                    //Warehouse = BoatWarehouse[種類comboBox.SelectedIndex] + Convert.ToInt32(BoatWarehouse[種類comboBox.SelectedIndex] * (改倉numericUpDown.Value / 100));
                                                    //Call.BuildShip(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo].ShipyardID, Warehouse, 材質comboBox.SelectedIndex + 1, BoatID[種類comboBox.SelectedIndex]);//造船
                                                    //造船明細textBox.Text = "";
                                                    //造船明細textBox.AppendText("造『" + 種類comboBox.Text + "』次數：" + (++Form1.UsingProcess[User].BuildShipCount) + "\r\n");
                                                    Form1.UsingProcess[User].BuildShipDone = true;
                                                    Form1.UsingProcess[User].GetShip = false;
                                                    Form1.UsingProcess[User].ShipCode = 0;
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 賣船
                                                    if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) != "出售船隻")
                                                    {
                                                        if (Form1.UsingProcess[User].TimeCount > 3)
                                                        {
                                                            //Call.InfoButton(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo].ShipyardID, "賣船");
                                                            Form1.UsingProcess[User].TimeCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Call.GetBoatNumber(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User].ShipCode, 編號comboBox.SelectedIndex))
                                                        {
                                                            //賣船
                                                            //if (Form1.UsingProcess[User].ShipCode > 0 && Form1.UsingProcess[User].ShipCode < 0xFFFF)
                                                                //Call.SellBoat(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo].ShipyardID, Form1.UsingProcess[User].ShipCode);

                                                            Form1.UsingProcess[User].ShipCode = -1;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                    else
                                        new CityCall().ToPier(User);
                                }
                            }
                            //else
                                //building.Text = "開始造船";
                        }
                        else
                            Call.PostMessage(Form1.UsingProcess[User].hWnd, "﹝" + Form1.UsingProcess[User].Name + "﹞無使用權限。");

                        timer1.Start();
                    }
                }
            }
        }
        //動作判斷
        private void Action(int User, int Sequence_Index)
        {
            switch (Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Action)
            {
                case "[生產]":
                    Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Formula_Name = Sequence_listBox.Items[Sequence_Index].ToString().Replace("[生產]", "");

                    if (Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Recovery_Power)
                    {
                        if (new CityCall().ToPub(User))
                        {
                            if (Form1.UsingProcess[User].Power < Form1.UsingProcess[User].MaxPower / 2)
                                new CityCall().Ordering(User);
                            else
                                Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Recovery_Power = false;
                        }
                    }
                    else
                    {
                        if (new CityCall().Produce(User, ref Form1.UsingProcess[User]._Action_Sequence[Sequence_Index]))
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    }
                    break;
                case "[購買]":
                    if (new CityCall().ToTrader(User))
                        if (new CityCall().BuyTrade(User, Sequence_listBox.Items[Sequence_Index].ToString().Replace("[購買]", "")))
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    break;
                case "[出售]":
                    if (new CityCall().ToTrader(User))
                        if (new CityCall().SellTrade(User, Sequence_listBox.Items[Sequence_Index].ToString().Replace("[出售]", "")))
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    break;
                case "[使用]":
                    for (int TradeBook_Index = 0; TradeBook_Index < Form1.UsingProcess[User]._Item.LandItemNum; TradeBook_Index++)
                    {
                        if (Form1.UsingProcess[User]._Item._LandItemInfo[TradeBook_Index].Name == Sequence_listBox.Items[Sequence_Index].ToString().Replace("[使用]", ""))
                        {
                            if (Form1.UsingProcess[User]._Item._LandItemInfo[TradeBook_Index].Num > 0)
                            {
                                Call.UseItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._LandItemInfo[TradeBook_Index].Code);
                                --Form1.UsingProcess[User]._Item._LandItemInfo[TradeBook_Index].Num;
                            }
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                            break;
                        }
                    }
                    break;
                case "[航行]":
                    if (Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Recovery_Power)
                    {
                        if (new CityCall().ToPub(User))
                        {
                            if (Form1.UsingProcess[User].Power < Form1.UsingProcess[User].MaxPower / 2)
                                new CityCall().Ordering(User);
                            else
                            {
                                if (Form1.UsingProcess[User].Fatigue > 0)
                                {
                                    if (Form1.UsingProcess[User].Place == 0x8)
                                        Call.Entertain(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "休息站老闆", "餐廳老闆", "", "").ID);
                                    else
                                        Call.Entertain(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "酒館老闆", "", "", "").ID);
                                }
                                Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Recovery_Power = false;
                            }
                        }
                    }
                    else
                    {
                        if (!Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Navigator)
                        {
                            if (Form1.UsingProcess[User].Power < Form1.UsingProcess[User].MaxPower / 2)
                            {
                                if (Form1.UsingProcess[User].Place != 0x4)
                                    Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Recovery_Power = true;
                            }

                            string FileName = Sequence_listBox.Items[Sequence_Index].ToString().Replace("[航行]", "");
                            string Route = FileName, MapRoute = null;

                            if (FileName.Contains("﹝") && FileName.Contains("﹞") && Form1.UsingProcess[User].GetMap)
                            {
                                MapRoute = FileName.Substring(FileName.IndexOf("﹝")).Replace("﹝", "").Replace("﹞", "");
                                FileName = Application.StartupPath + "\\route\\" + MapRoute + ".route";
                            }
                            else if (!string.IsNullOrWhiteSpace(Route))
                                FileName = Application.StartupPath + "\\route\\" + Route + ".route";

                            Form1.UsingProcess[User]._navigation = new Navigator.Navigation_Info();
                            Form1.UsingProcess[User]._navigation.Route = new Navigator.Navigation_Info.Route_Info[512];

                            if (File.Exists(FileName))
                            {
                                using (StreamReader sr = new StreamReader(FileName))
                                {
                                    int count = 0;
                                    while (!sr.EndOfStream)
                                    {
                                        string line = sr.ReadLine();
                                        Form1.UsingProcess[User]._navigation.Route[count].X = Convert.ToInt32(line.Substring(0, line.IndexOf(","))) / 4;
                                        Form1.UsingProcess[User]._navigation.Route[count].Y = Convert.ToInt32(line.Substring(line.IndexOf(",") + 1, 5)) / 4;
                                        Form1.UsingProcess[User]._navigation.Route[count].CityName = line.Substring(line.LastIndexOf(",") + 1);
                                        ++count;
                                    }
                                    Form1.UsingProcess[User]._navigation.Point = count;
                                    Form1.UsingProcess[User]._navigation.Move = 0;
                                }
                            }
                            Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Navigator = true;
                        }
                        else
                        {
                            if (new Navigator().Navigate(User, ref Form1.UsingProcess[User]._navigation, true))
                            {
                                Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Navigator = false;
                                Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                            }
                            else
                            {
                                if (Sea_checkBox.Checked)
                                {
                                    //海上生產
                                }
                            }
                        }
                    }
                    break;
                case "[閱讀]":
                    string[] BookArray = new string[6] { "地理學", "考古學", "宗教學", "生物學", "財寶鑑定", "美術" };
                    for (int ReadBook_Type = 0; ReadBook_Type < BookArray.Length; ReadBook_Type++)
                    {
                        if (Sequence_listBox.Items[Sequence_Index].ToString().Replace("[閱讀]", "") == BookArray[ReadBook_Type])
                            if (new CityCall().ReadBook(User, ReadBook_Type))
                                Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    }
                    break;
                case "[開圖]":
                    if (!Form1.UsingProcess[User].GetMap || new CityCall().FindTreasure(User, Sequence_listBox.Items[Sequence_Index].ToString().Replace("[開圖]", "")))
                        Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    break;
                case "[報告]":
                    if (!Form1.UsingProcess[User].Report || new CityCall().Report(User, Sequence_listBox.Items[Sequence_Index].ToString().Replace("[報告]", "")))
                        Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    break;
                case "[專攻]":
                    if (new CityCall().Research(User, Sequence_listBox.Items[Sequence_Index].ToString().Replace("[專攻]", "")))
                        Form1.UsingProcess[User]._Action_Sequence[Sequence_Index].Complete = true;
                    break;
                default:
                    break;
            }
        }

        #region 序列拖曳
        private void Sequence_listBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                Object item = (object)e.Data.GetData(typeof(System.String));

                // Perform drag-and-drop, depending upon the effect.
                if (e.Effect == DragDropEffects.Move)
                {
                    Sequence_listBox.Items.RemoveAt(indexOfItemUnderMouseToDrag);

                    // Insert the item.
                    if (indexOfItemUnderMouseToDrop != ListBox.NoMatches)
                    {
                        Sequence_listBox.Items.Insert(indexOfItemUnderMouseToDrop, item);
                        Sequence_listBox.SelectedIndex = indexOfItemUnderMouseToDrop;
                    }
                    else
                    {
                        Sequence_listBox.Items.Add(item);
                        Sequence_listBox.SelectedIndex = Sequence_listBox.Items.Count - 1;
                    }
                }
            }
        }

        private void Sequence_listBox_DragOver(object sender, DragEventArgs e)
        {
            // Determine whether string data exists in the drop data. If not, then
            // the drop effect reflects that the drop cannot occur.
            if (!e.Data.GetDataPresent(typeof(System.String)))
            {

                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Move;

            // Get the index of the item the mouse is below. 

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.

            indexOfItemUnderMouseToDrop = Sequence_listBox.IndexFromPoint(Sequence_listBox.PointToClient(new Point(e.X, e.Y)));
        }

        private void Sequence_listBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            indexOfItemUnderMouseToDrag = Sequence_listBox.IndexFromPoint(e.X, e.Y);

            if (indexOfItemUnderMouseToDrag != ListBox.NoMatches)
            {
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        private void Sequence_listBox_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset the drag rectangle when the mouse button is raised.
            dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void Sequence_listBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {

                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Create custom cursors for the drag-and-drop operation.
                    try
                    {
                        MyNormalCursor = new Cursor("3dwarro.cur");
                        MyNoDropCursor = new Cursor("3dwno.cur");

                    }
                    catch
                    {
                        // An error occurred while attempting to load the cursors, so use
                        // standard cursors.
                        //UseCustomCursorsCheck.Checked = false;
                    }
                    finally
                    {
                        // The screenOffset is used to account for any desktop bands 
                        // that may be at the top or left side of the screen when 
                        // determining when to cancel the drag drop operation.
                        screenOffset = SystemInformation.WorkingArea.Location;

                        // Proceed with the drag-and-drop, passing in the list item.                    
                        DragDropEffects dropEffect = Sequence_listBox.DoDragDrop(Sequence_listBox.Items[indexOfItemUnderMouseToDrag], DragDropEffects.All | DragDropEffects.Link);
                    }
                }
            }

        }
        #endregion

        private void Sequence_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    if (Sequence_listBox.Items.Count >= Form1.UsingProcess[User]._Action_Sequence.Length)
                    {
                        MsgBox.Show(this, "動作序列已滿", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (Form1.UsingProcess[User].Sequence)
                        return;
                }
            }

            switch (((Button)sender).Text)
            {
                case "生產":
                    if (!string.IsNullOrWhiteSpace(配方comboBox.Text))
                        Sequence_listBox.Items.Add("[生產]" + 配方comboBox.Text);
                    break;
                case "購買":
                    string BuyList = null;

                    if (SelectCityNo != -1)
                    {
                        for (int i = 0; i < GVOCall.City[SelectCityNo].BuyMenuNum; i++)
                        {
                            for (int j = 0; j < GVOCall.City[SelectCityNo]._BuyTradeInfo.Length; j++)
                            {
                                if (GVOCall.City[SelectCityNo]._BuyTradeInfo[j].ID != 0)
                                {
                                    if (GVOCall.City[SelectCityNo]._BuyTradeInfo[j].ID == GVOCall.City[SelectCityNo].CheckedBuy[i])
                                        BuyList += GVOCall.City[SelectCityNo]._BuyTradeInfo[j].Name + "、";
                                }
                            }

                        }
                    }

                    if (!string.IsNullOrWhiteSpace(BuyList))
                    {
                        BuyList = BuyList.Substring(0, BuyList.Length - 1);
                        Sequence_listBox.Items.Add("[購買]" + BuyList);
                    }
                    else
                        Sequence_listBox.Items.Add("[購買]全部");
                    break;
                case "出售":
                    string SellList = null;
                    //for (int i = 0; SelectCityNo != -1 && i < GVOCall.City[SelectCityNo].SellMenuNum; i++)
                    //if (SellTradeList.Items.Count > 0 && SellTradeList.Items[i].Checked)
                    //SellList += SellTradeList.Items[i].Text + ",";
                    if (!string.IsNullOrWhiteSpace(SellList))
                    {
                        SellList = SellList.Substring(0, SellList.Length - 1);
                        Sequence_listBox.Items.Add("[出售]" + SellList);
                    }
                    else
                        Sequence_listBox.Items.Add("[出售]全部");
                    break;
                case "星書":
                    if (!string.IsNullOrWhiteSpace(採買書comboBox.Text))
                        Sequence_listBox.Items.Add("[使用]" + 採買書comboBox.Text);
                    break;
                case "航行":
                    if (!string.IsNullOrWhiteSpace(開圖航線comboBox.Text))
                        Sequence_listBox.Items.Add("[航行]" + 航線comboBox.Text + "﹝" + 開圖航線comboBox.Text + "﹞");
                    else if (!string.IsNullOrWhiteSpace(航線comboBox.Text))
                        Sequence_listBox.Items.Add("[航行]" + 航線comboBox.Text);
                    break;
                case "閱讀":
                    if (!string.IsNullOrWhiteSpace(領域comboBox.Text))
                        Sequence_listBox.Items.Add("[閱讀]" + 領域comboBox.Text);
                    break;
                case "開圖":
                    if (!string.IsNullOrWhiteSpace(開圖地點textBox.Text) && !string.IsNullOrWhiteSpace(開圖座標textBox.Text) && !string.IsNullOrWhiteSpace(開圖技能comboBox.Text))
                        if (開圖座標textBox.Text.Contains(","))
                            Sequence_listBox.Items.Add("[開圖]" + 開圖地點textBox.Text + "﹝" + 開圖座標textBox.Text + "﹞" + 開圖技能comboBox.Text);
                    break;
                case "報告":
                    if (!string.IsNullOrWhiteSpace(報告comboBox.Text))
                        Sequence_listBox.Items.Add("[報告]" + 報告comboBox.Text);
                    break;
                case "專攻":
                    if (!string.IsNullOrWhiteSpace(專攻comboBox.Text))
                        Sequence_listBox.Items.Add("[專攻]" + 專攻comboBox.Text);
                    break;
                case "刪除動作":
                    if (Sequence_listBox.SelectedIndex != -1)
                        Sequence_listBox.Items.RemoveAt(Sequence_listBox.SelectedIndex);
                    break;
            }
        }

        private void 書籍comboBox_DropDown(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    書籍comboBox.Items.Clear();

                    for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
                    {
                        書籍comboBox.Items.Add(Form1.UsingProcess[User]._Item._BookInfo[i].Name);
                    }
                    break;
                }
            }
        }

        private void 配方comboBox_DropDown(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    配方comboBox.Items.Clear();

                    for (int Book_Index = 0; Book_Index < Form1.UsingProcess[User]._Item.BookNum; Book_Index++)
                    {
                        if (Form1.UsingProcess[User]._Item._BookInfo[Book_Index].Name == 書籍comboBox.Text)
                        {
                            if (Form1.UsingProcess[User]._Item._BookInfo[Book_Index].Formula_Init)
                            {
                                for (int Formula_Index = 0; Formula_Index < Form1.UsingProcess[User]._Item._BookInfo[Book_Index]._FormulaInfo.Length; Formula_Index++)
                                {
                                    if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User]._Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].Formula_Name))
                                        配方comboBox.Items.Add(Form1.UsingProcess[User]._Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].Formula_Name);
                                }
                            }
                            else
                            {
                                配方comboBox.Items.Add("配方未讀取");
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void 採買書comboBox_DropDown(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    採買書comboBox.Items.Clear();

                    for (int i = 0; i < Form1.UsingProcess[User]._Item._LandItemInfo.Length; i++)
                    {
                        if (Form1.UsingProcess[User]._Item._LandItemInfo[i].Num > 0)
                            採買書comboBox.Items.Add(Form1.UsingProcess[User]._Item._LandItemInfo[i].Name);
                    }
                    break;
                }
            }
        }

        private void 航線comboBox_DropDown(object sender, EventArgs e)
        {
            航線comboBox.Items.Clear();
            開圖航線comboBox.Items.Clear();

            if (!Directory.Exists(Application.StartupPath + "\\route"))
                Directory.CreateDirectory(Application.StartupPath + "\\route");

            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "\\route");
            FileInfo[] file = dir.GetFiles("*.route");

            foreach (FileInfo _file in file)
            {
                航線comboBox.Items.Add(_file.Name.Replace(".route", ""));
                開圖航線comboBox.Items.Add(_file.Name.Replace(".route", ""));
            }

        }

        private void 開圖技能comboBox_DropDown(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                {
                    開圖技能comboBox.Items.Clear();

                    for (int i = 0; i < Form1.UsingProcess[User]._SkillInfo.Length; i++)
                    {
                        if (Form1.UsingProcess[User]._SkillInfo[i].Adventure_Active)
                            開圖技能comboBox.Items.Add(Form1.UsingProcess[User]._SkillInfo[i].Name);
                    }
                    break;
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
        }

        private void Init_Button_Click(object sender, EventArgs e)
        {
            timer2.Start();
            Init_Button.Text = "初始化中...";
            Init_Button.Enabled = false;
        }
    }

}