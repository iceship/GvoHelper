using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace GvoHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class uInfo
        {
            public uInfo()
            {
                hWnd = IntPtr.Zero;
                Window = null;
                ServerName = null;
                Name = null;
                ID = 0;

                Power = 0;
                MaxPower = 0;
                Money = 0;
                Fatigue = 0;

                CityNo = -1;
                PlaceNo = 0;
                Place = 0;
                PlaceName = null;
                Weather = 0;
                MaritimeDays = 0;
                TempMaritimeDays = 0;

                PartyID = new int[5];
                UsingSkill = new int[3];
                UsingSkillImage = new PictureBox[3];

                BoatException = new bool[5][];
                for (int i = 0; i < 5; i++)
                    BoatException[i] = new bool[32];

                Exchange_Position = false;

                IsHasten = false;
                CancelHasten = false;
                Navalbattle = false;

                ItemBoxOpen = false;
                InitItem = false;
                UpdateItem = true;

                SkillInfoBoxOpen = false;
                InitSkill = false;

                AdjutantInfoBoxOpen = false;
                InitAdjutant = false;
                UpdateAdjutant = true;

                _Adjutant = new AdjutantInfo[2];
                for (int i = 0; i < 2; i++)
                {
                    _Adjutant[i].LV = new int[3];
                    _Adjutant[i].Property = new int[6];
                    _Adjutant[i].AsDay = new int[6];
                    _Adjutant[i].DoneAsDay = new int[6];
                }

                _Item.CuisineNum = 0;
                _Item._CuisineInfo = new Item.ItemInfo[80];
                _Item.CheckedCuisine = new bool[200];

                _Item.SeaItemNum = 0;
                _Item._SeaItemInfo = new Item.ItemInfo[80];

                _Item.LandItemNum = 0;
                _Item._LandItemInfo = new Item.ItemInfo[4];

                _Item._HaveItemInfo = new Item.ItemInfo[100];

                _Item.BookNum = 0;
                _Item._BookInfo = new Item.ItemInfo[80];

                SkillNum = 0;
                _SkillInfo = new SkillInfo[50];

                IsLandBattle = false;
                _LandShortcuts = new LandShortcuts[7];

                Cos = 0;
                Sin = 0;
                _Destinations = new Destinations();
                LandExit = new LandInfo[5];

                Old_Cos = 0;
                Old_Sin = 0;

                TurnCount = 0;

                SBCount = 0;
                SBItemCount = 0;

                ExceptionCount = 0;
                UseSkillCount = 0;
                FollowCount = 0;
                HastenCount = 0;
                UseCuisineCount = 0;
                CheckItemCount = 0;

                CountChangeCoordinate = 0;
            }

            public Thread stateThread;
            public Mutex stateMutex = new Mutex();
            public Thread targetThread;
            public Mutex targetMutex = new Mutex();

            public IntPtr hWnd;
            public string Window, ServerName, Name;
            public int ID;

            public string User_Type;

            public int Power, MaxPower, Money, Fatigue;//行動力
            public int[] Materials = new int[4];//物資
            public int[] SetMaterials = new int[4];//物資(保存的數量)
            public string PlaceName, DungeonName;//地點名稱
            public int CityNo, Place, PlaceNo;
            public int Weather, MaritimeDays, TempMaritimeDays;
            public TargetInfo target = new TargetInfo();

            public int[] PartyID, UsingSkill;
            public int UsingSkillNum;
            public bool[][] BoatException;
            public PictureBox[] UsingSkillImage;

            //目標資料(活動、場景目標)
            public struct TargetInfo
            {
                public string Name;
                public int ID;
                public PointF Coordinate;
                public bool Saved;
            }
            public TargetInfo[] _People, _Regular;

            //副官資料
            public struct AdjutantInfo
            {
                public int ID, As, ChangeAs, TotalAsDay, ChangeAsTime;
                public string Name;
                public int[] LV, Property, AsDay, DoneAsDay;
                public bool CheckChange;
            }
            //public enum AdjutantPost { 航海長, 警戒員, 主計長, 倉管, 兵長, 船醫 };
            public AdjutantInfo[] _Adjutant;

            //道具資料
            public struct Item
            {
                public struct FormulaInfo
                {
                    public int ID;
                    public string Formula_Name;
                }

                public struct ItemInfo
                {
                    public int ID, Code, Num, Equip_Type;
                    public string Name;
                    public bool CheckUse, Formula_Init, Equip;
                    public FormulaInfo[] _FormulaInfo;
                    public int Attack, Defense, Durability;
                }
                //道具
                public int SeaItemNum, LandItemNum, BookNum;
                public ItemInfo[] _SeaItemInfo, _LandItemInfo, _HaveItemInfo, _BookInfo;
                //料理
                public int CuisineNum;
                public ItemInfo[] _CuisineInfo;
                public bool[] CheckedCuisine;
            }
            public Item _Item;
            //技能資料
            public struct SkillInfo
            {
                public int ID, Cost, Rank;
                public string Name;
                public bool CheckUse, NoSkillImage;
                public bool Active, Adventure_Active, Navalbattle;
            }
            public int SkillNum;
            public SkillInfo[] _SkillInfo;
            //座標&角度
            public PointF Coordinate, Old_Coordinate, Rand_Coordinate;
            public float Cos, Sin, Old_Cos, Old_Sin;
            //急加速用-檢查是否轉向
            public struct Destinations
            {
                public PointF Coordinate;
                public int PlaceNo;
            }
            public Destinations _Destinations;
            //陸地出口
            public struct LandInfo
            {
                public string Name;
                public PointF Exit;
            }
            public LandInfo[] LandExit;
            //儲存已勾選資料
            public string Save_CuisineName = null;
            public string Save_SkillName = null;

            public bool IsHasten, CancelHasten, Navalbattle;
            public bool InitItem, UpdateItem,ItemBoxOpen;
            public bool InitSkill, SkillInfoBoxOpen;
            public bool InitAdjutant, UpdateAdjutant, AdjutantInfoBoxOpen;
            public bool Exchange_Position;

            public string TradeMenu;

            public int TimeCount;

            public int SupplyCount;
            public int TurnCount, IntoPierCount;
            public int UseSkillCount, HastenCount;
            public int ExceptionCount, FollowCount, UseCuisineCount, CheckItemCount;
            public int CountChangeCoordinate;
            //海戰
            public int SBCount, SBItemCount;
            //陸戰
            public struct LandbattleInfo
            {
                public int ID, HP, MaxHP, Attack, Defense, LV, State, SkillState, Motion, MaxMotion;
                public string Name;
                public LandShortcuts[] Shortcuts;
            }
            public struct LandShortcuts
            {
                public int ID, Num, NeedMotion, UseTarget;
                public bool Use;
            }
            public LandbattleInfo[] Friend = new LandbattleInfo[5];
            public LandbattleInfo[] Enemy = new LandbattleInfo[10];
            public LandShortcuts[] _LandShortcuts;
            public bool IsLandBattle;
            public int LBCureCount, LBAttackCount, LBAttackIdleCount, LBAttackTargetID, LBSkillCount;
            //競技場
            public int ArenaIdleCount, ArenaPassCount;
            public bool ArenaGetMission, ArenaReadyMission;
            //地下城
            public int DungeonIdleCount, DungeonIntoCount, DungeonClearCount, DungeonSellIdleCount;
            public bool DungeonSell;
            //導航
            public Navigator.Navigation_Info _navigation;
            //造船
            public bool BuildShipDone, GetShip;
            public int BuildShipCount, ShipCode;
            //大學
            public bool Talk;
            //閱讀
            public bool ReadBook, GetMap, Report;
            //序列動作
            public struct Action_Sequence
            {
                //閱讀
                //public int ReadBook_Type;

                public bool Complete, Recovery_Power, Navigator;
                //動作
                public string Action;
                //生產書籍&配方名稱
                public string Book_Name, Formula_Name;
            }
            public Action_Sequence[] _Action_Sequence = new Action_Sequence[20];
            public bool Sequence;
            //
            public bool Change_Angle;
            //LOG
            public int LogStart, LogEnd;
        }

        public struct TradeInfo
        {
            public int Code, ID, Multiple, Num, MultiplePrice, Price;
        }

        public static uInfo[] UsingProcess = new uInfo[5];

        private int TargetID, Mission;
        
        private ResourceManager skill = new ResourceManager("GvoHelper.Properties.Resources", Assembly.GetExecutingAssembly());
        private GVOCall Call = new GVOCall();
        private WinAPIHook hook = new WinAPIHook();
        private PictureBox picTemp;

        private PointF temp;

        public static Mutex mut = new Mutex();

        private string game_ver = "7.6.0.0";

        private string FilePath, FileName;

        //限制只能輸入數字&空格則為0
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }
        //取得所有遊戲視窗hWnd
        private void GetAllProcess(object sender, EventArgs e)
        {
            comboBox0.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            Call.SearchAllWindow();

            for (int i = 0; GVOCall.AllProcess[i] != IntPtr.Zero; i++)
            {
                string SN = Call.GetServerName(GVOCall.AllProcess[i]);
                string UN = Call.GetUserName(GVOCall.AllProcess[i]);

                if (SN != null && UN != null)
                {
                    bool used = false;
                    for (int User = 0; User < 5; User++)
                        if (UsingProcess[User].Window == "﹝" + SN + "﹞" + UN)
                            used = true;
                    
                    comboBox0.Items.Add("﹝" + SN + "﹞" + UN);
                    if (!used)
                    {
                        comboBox1.Items.Add("﹝" + SN + "﹞" + UN);
                        comboBox2.Items.Add("﹝" + SN + "﹞" + UN);
                        comboBox3.Items.Add("﹝" + SN + "﹞" + UN);
                        comboBox4.Items.Add("﹝" + SN + "﹞" + UN);
                        if (Call.GethWnd("﹝" + SN + "﹞" + UN) != GVOCall.AllProcess[i])
                            WinAPI.SetWindowText(GVOCall.AllProcess[i], "﹝" + SN + "﹞" + UN);
                    }

                    switch (Call.CheckUser(SN, UN))
                    {
                        case "Pirate":
                            this.Close();
                            break;
                        case "猜猜看":
                            當前目標ToolStripMenuItem.Enabled = true;
                            回到出口Button.Visible = true;
                            break;
                    }
                }
            }
        }
        //讀取快捷鍵
        private void LoadShortcuts()
        {
            using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath, "GvoHelper.ini")))
            {
                foreach (Control ctrl in 快捷鍵panel.Controls)
                {
                    string ShortcutSkillId = myCINI.getKeyValue("Shortcuts", ctrl.Name);
                    ShortcutSkillId = ShortcutSkillId.Substring(ShortcutSkillId.IndexOf("_") + 1);
                    ((Button)ctrl).Image = (Image)skill.GetObject("_" + ShortcutSkillId.ToString().PadLeft(8, '0'));
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            快捷鍵panel.Visible = false;
            技能列panel.Visible = false;
            補給ToolStrip.Visible = false;
            金錢ToolStrip.Visible = false;
            TargetIDBox.Visible = false;

            this.Width -= 200;
            this.Height -= 45;

            LoadShortcuts();//得取快捷鍵
            Call.LoadCityInfo(ref GVOCall.City);//讀取城市資料

            for (int User = 0; User < 5; User++)
                UsingProcess[User] = new uInfo();//角色資料初始化

            if (!Directory.Exists(Application.StartupPath + "\\set"))
                Directory.CreateDirectory(Application.StartupPath + "\\set");
        }

        #region 檢視
        private void 最上層顯示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (最上層顯示ToolStripMenuItem.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }

        private void 技能監控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (技能監控ToolStripMenuItem.Checked)
            {
                panel1.Visible = true;
                panel2.Visible = true;
                panel3.Visible = true;
                panel4.Visible = true;
                panel5.Visible = true;
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
            }
        }

        private void 技能列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (快捷鍵panel.Visible)
            {
                快捷鍵panel.Visible = false;
                this.Width -= 35;
                if (技能列panel.Visible)
                {
                    exbutton.Text = "→";
                    技能列panel.Visible = false;
                    this.Width -= 165;
                }
            }
            else
            {
                快捷鍵panel.Visible = true;
                this.Width += 35;
            }
        }

        private void 補給管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (補給ToolStrip.Visible)
            {
                補給ToolStrip.Visible = false;
                if (!金錢ToolStrip.Visible)
                    this.Height -= 25;
            }
            else
            {
                if (!金錢ToolStrip.Visible)
                    this.Height += 25;
                補給ToolStrip.Visible = true;
            }
        }

        private void 金錢管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (金錢ToolStrip.Visible)
            {
                金錢ToolStrip.Visible = false;
                if (!補給ToolStrip.Visible)
                    this.Height -= 25;
            }
            else
            {
                if (!補給ToolStrip.Visible)
                    this.Height += 25;
                金錢ToolStrip.Visible = true;
            }
        }

        private void 當前目標ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetIDBox.Visible)
            {
                TargetIDBox.Visible = false;
                this.Height -= 20;
            }
            else
            {
                TargetIDBox.Visible = true;
                this.Height += 20;
            }
        }
        #endregion

        #region 工具
        private void GVONavi_Click(object sender, EventArgs e)
        {
            int User = 0;
            IntPtr hWnd = WinAPI.FindWindow(null, "GVONavi(Beta) - " + UsingProcess[User].Name);
            if (hWnd != IntPtr.Zero)
                WinAPI.SetForegroundWindow(hWnd);
            else
            {
                Form2 myForm = new Form2();
                string path = Path.Combine(Application.StartupPath, "map.png");
                myForm.SeaMapPath = path;

                if (!System.IO.File.Exists(Path.Combine(Application.StartupPath, "map.png")))
                    MessageBox.Show("找不到map.png地圖(需放置於GVOHelper相同目錄)");
                else
                    myForm.Show();
            }
        }

        private void ChatLog_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 myForm = new Form3();
            myForm.Show();
        }

        private void 交易幫手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 myForm = new Form4();
            myForm.Show();
        }

        private void 陸戰幫手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 myForm = new Form5();
            myForm.Show();
        }

        private void 序列幫手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 myForm = new Form6();
            myForm.Show();
        }
        #endregion

        private void 遊戲多開ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (get_game_ver() != game_ver)
            {
                MessageBox.Show("遊戲版本檢查錯誤！！", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IntPtr ret = IntPtr.Zero;
            WinAPI.STARTUPINFO sInfo = new WinAPI.STARTUPINFO();
            WinAPI.PROCESS_INFORMATION pInfo = new WinAPI.PROCESS_INFORMATION();

            if (WinAPI.CreateProcess(null, new StringBuilder(FilePath + @"\" + FileName), null, null, false, WinAPI.ProcessCreationFlags.CREATE_SUSPENDED, null, null, ref sInfo, ref pInfo))
            {
                long endAddr = 0xF00000;
                long baseAddr = 0x401000;
                WinAPI.MEMORY_BASIC_INFORMATION inf = new WinAPI.MEMORY_BASIC_INFORMATION();
                while (WinAPI.VirtualQueryEx(pInfo.hProcess, (IntPtr)baseAddr, out inf, (uint)Marshal.SizeOf(inf)))
                {
                    //Console.WriteLine(Convert.ToString((int)inf.BaseAddress, 16) + " " + Convert.ToString(inf.RegionSize, 16) + " " + inf.Protect);
                    baseAddr = (long)inf.BaseAddress + (long)inf.RegionSize;
                    if (baseAddr > endAddr)
                        break;
                    if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                    {
                        byte[] buf = new byte[inf.RegionSize];
                        int readBytes = 0;
                        WinAPI.ReadProcessMemory(pInfo.hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);
                        for (int j = 0; j < buf.Length; j++)
                        {
                            uint sByte = 0;
                            sByte = (buf[j]);//0-255
                            
                            if (sByte == 0x47)
                            {
                                if ((uint)(buf[j + 1] | buf[j + 2] << 8) == 0x3E89)
                                {
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + j, 16));
                                    WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)inf.BaseAddress + j, BitConverter.GetBytes(0x90), 1, out ret);
                                }

                            }
                            else if (sByte == 0x84)
                            {
                                if ((uint)(buf[j - 1]) == 0x0F && (uint)(buf[j + 1] | buf[j + 2] << 8 | buf[j + 3] << 16 | buf[j + 4] << 24) == 0x00000235 && (uint)(buf[j + 5]) == 0xE8)
                                {
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + j, 16));
                                    WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)inf.BaseAddress + j, BitConverter.GetBytes(0x85), 1, out ret);
                                }
                            }
                            else if (sByte == 0x68)
                            {
                                if ((uint)(buf[j - 1]) == 0xCC && (uint)(buf[j + 5]) == 0xE8 && (uint)(buf[j + 0xA]) == 0x68 && (uint)(buf[j + 0xF]) == 0xE8)
                                {
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + j, 16));
                                    WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)inf.BaseAddress + j, BitConverter.GetBytes(0xC3), 4, out ret);
                                }
                            }

                        }
                    }
                }
                //多開
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x496844, BitConverter.GetBytes(0x90), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x496914, BitConverter.GetBytes(0x90), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x49F71C, BitConverter.GetBytes(0x90), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x4A0F1C, BitConverter.GetBytes(0x90), 1, out ret);
                //繞過NP
                //0F84 1A020000
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x497EA7, BitConverter.GetBytes(0x85), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x497F77, BitConverter.GetBytes(0x85), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x4A0DD7, BitConverter.GetBytes(0x85), 1, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x4A25D7, BitConverter.GetBytes(0x85), 1, out ret);
                //#ASCII"GvoTW"
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0xB090E0, BitConverter.GetBytes(0xC3), 4, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0xB09C50, BitConverter.GetBytes(0xC3), 4, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0xB96B40, BitConverter.GetBytes(0xC3), 4, out ret);
                //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0xBA73F0, BitConverter.GetBytes(0xC3), 4, out ret);
                WinAPI.ResumeThread(pInfo.hThread);
            }
            WinAPI.CloseHandle(pInfo.hProcess);
            WinAPI.CloseHandle(pInfo.hThread);
        }

        private void 視窗最小化Button_Click(object sender, EventArgs e)
        {
            int Admiral = 0;
            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    if (UsingProcess[User].ID == UsingProcess[User].PartyID[0])
                        Admiral = User;
                }
            }

            for (int User = 0; User < 5; User++)
                if (User != Admiral)
                    WinAPI.ShowWindow(UsingProcess[User].hWnd, (int)WinAPI.CommandShow.SW_MINIMIZE);
        }

        #region 艦隊控制
        private void 組隊Button_Click(object sender, EventArgs e)
        {
            int Admiral = 0;
            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    if (UsingProcess[User].ID == UsingProcess[User].PartyID[0])
                        Admiral = User;
                }
            }

            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero && User != Admiral)
                {
                    if (!Call.CheckPartyID(UsingProcess[Admiral].hWnd, Call.GetUserId(UsingProcess[User].hWnd)))
                    {
                        Call.Invite(UsingProcess[Admiral].hWnd, Call.GetUserId(UsingProcess[User].hWnd));
                        Call.Delay(500);
                        Call.Agree(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[Admiral].hWnd));
                        Call.Delay(500);
                    }
                }
            }
        }

        private void 跟隨Button_Click(object sender, EventArgs e)
        {
            if (!自動跟隨ToolStripMenuItem.Checked)
                自動跟隨ToolStripMenuItem.Checked = true;
            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero && Call.GetPartyStatus(UsingProcess[User].hWnd))
                {
                    if (!Call.GetFollowStatus(UsingProcess[User].hWnd))
                    {
                        Call.Follow(UsingProcess[User].hWnd, UsingProcess[User].Place);//跟隨
                    }
                }
            }
        }

        private void 取消跟隨Button_Click(object sender, EventArgs e)
        {
            if (自動跟隨ToolStripMenuItem.Checked)
                自動跟隨ToolStripMenuItem.Checked = false;
            for (int User = 0; User <= 4; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero && Call.GetFollowStatus(UsingProcess[User].hWnd))
                    Call.CancelFollow(UsingProcess[User].hWnd, UsingProcess[User].Place);
        }

        private void 停船Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero && Call.GetSailStatus(UsingProcess[User].hWnd) != 0)
                    Call.ChangeSailStatus(UsingProcess[User].hWnd, 0);
        }

        private void 回到出口Button_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 視窗置前
        private void Foreground_Click(object sender, EventArgs e)
        {
            Button _button = (Button)sender;
            int User;
            for (User = 0; User < 5; User++)
                if (_button.Name.Contains(User.ToString()))
                    break;
            WinAPI.ShowWindow(UsingProcess[User].hWnd, (int)WinAPI.CommandShow.SW_SHOWDEFAULT);
            WinAPI.SetForegroundWindow(UsingProcess[User].hWnd); //置前
        }
        #endregion

        //多執行緒補給(沒用到) 
        private void Supply(object mode)
        {
            //Call.Supply(UseAllProcess, Convert.ToInt32(Thread.CurrentThread.Name), (int)mode);
        }

        private void 套餐Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero && UsingProcess[User].CityNo != -1 && UsingProcess[User].Power < UsingProcess[User].MaxPower)
                {
                    Call.OrderingMeal(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "酒館老闆", "休息站老闆", "餐廳老闆", "").ID, GVOCall.City[UsingProcess[User].CityNo].Meal);
                    //if (Form1.UsingProcess[User].Place == 0x8)
                        //Call.OrderingMeal(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "休息站老闆", "餐廳老闆", "", "").ID, GVOCall.City[UsingProcess[User].CityNo].Meal);
                    //else
                        //Call.OrderingMeal(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "酒館老闆", "", "", "").ID, GVOCall.City[UsingProcess[User].CityNo].Meal);
                }
        }

        private void 點餐Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero && UsingProcess[User].CityNo != -1 && UsingProcess[User].Power < UsingProcess[User].MaxPower)
                {
                    Call.Ordering(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "酒館老闆", "休息站老闆", "餐廳老闆", "").ID, GVOCall.City[UsingProcess[User].CityNo].Food, GVOCall.City[UsingProcess[User].CityNo].Drink);
                    //if (Form1.UsingProcess[User].Place == 0x8)
                        //Call.Ordering(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "休息站老闆", "餐廳老闆", "", "").ID, GVOCall.City[UsingProcess[User].CityNo].Food, GVOCall.City[UsingProcess[User].CityNo].Drink);
                    //else
                        //Call.Ordering(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "酒館老闆", "", "", "").ID, GVOCall.City[UsingProcess[User].CityNo].Food, GVOCall.City[UsingProcess[User].CityNo].Drink);
                }
            /*
            for (int i = 0; i <= 4; i++)//建立多執行緒
            {
                Thread subThread = new Thread(new ThreadStart(Ordering));
                if (UseAllProcess[i] != IntPtr.Zero)
                {
                    subThread.Name = i.ToString();
                    subThread.Start();
                    if (i < 4 && UseAllProcess[i + 1] != IntPtr.Zero)
                        continue;
                }
                while (subThread.IsAlive == true)
                {
                    Call.Delay(100);
                }
            }
            Call.Delay(500);
             */
        }

        private void 款待Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Entertain(UsingProcess[User].hWnd, TargetID);
        }

        private void 閱讀書籍Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Readbook(UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "學者", "", "", "").ID, GVOCall.City[UsingProcess[User].CityNo].Subject);
        }

        private void 攀談Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Talk(UsingProcess[User].hWnd, TargetID);
        }

        private void 接受委託Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User <= 4; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.AgreeMission(UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "委託介紹人", "商人委託介紹人", "冒險家委託介紹人", "海事委託介紹人").ID, GVOCall.City[UsingProcess[User].CityNo].Mission);
        }

        private void 放棄委託Button_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.AbandonMission(UsingProcess[User].hWnd, Mission);
        }

        private void 存取金錢ToolStripButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    if (金錢的數量ToolStripTextBox.Text != "")
                        Call.MoneyManagement(UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, "銀行職員", "", "", "").ID, UsingProcess[User].Money, Convert.ToInt32(金錢的數量ToolStripTextBox.Text));
        }

        #region 募集船員
        private void 募集ToolStripDropDownButton_ButtonClick(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    if (最大船員數ToolStripMenuItem.Checked)
                        Call.Raise(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), true);
                    else
                        Call.Raise(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), false);
                }
            }
        }

        private void 最大船員數ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            必要船員數ToolStripMenuItem.Checked = false;
            最大船員數ToolStripMenuItem.Checked = true;
            募集ToolStripDropDownButton.Text = "募集：最大數";
        }

        private void 必要船員數ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            必要船員數ToolStripMenuItem.Checked = true;
            最大船員數ToolStripMenuItem.Checked = false;
            募集ToolStripDropDownButton.Text = "募集：必要數";
        }
        #endregion

        #region 補給物資
        private void 補給ToolStripDropDownButton_ButtonClick(object sender, EventArgs e)
        {
            bool set = false;
            int[] UserN = new int[4];
            if (自訂的數量ToolStripMenuItem.Checked)
            {
                set = true;
                if (水的數量ToolStripTextBox.Text != "")
                    UserN[0] = Convert.ToInt32(水的數量ToolStripTextBox.Text);
                if (糧食的數量ToolStripTextBox.Text != "")
                    UserN[1] = Convert.ToInt32(糧食的數量ToolStripTextBox.Text);
                if (資材的數量ToolStripTextBox.Text != "")
                    UserN[2] = Convert.ToInt32(資材的數量ToolStripTextBox.Text);
                if (彈藥的數量ToolStripTextBox.Text != "")
                    UserN[3] = Convert.ToInt32(彈藥的數量ToolStripTextBox.Text);
            }
            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Supply(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), UserN, set);
            }
        }

        private void 自動補給ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自動補給ToolStripMenuItem.Checked = true;
            自訂的數量ToolStripMenuItem.Checked = false;
            保存的數量ToolStripMenuItem.Checked = false;
            補給ToolStripDropDownButton.Text = "補給：自動";
            if (補給ToolStrip.Visible)
                if (!金錢ToolStrip.Visible)
                    this.Height -= 25;
            補給ToolStrip.Visible = false;

            補給管理ToolStripMenuItem.Checked = false;
            水的數量ToolStripTextBox.Enabled = false;
            糧食的數量ToolStripTextBox.Enabled = false;
            資材的數量ToolStripTextBox.Enabled = false;
            彈藥的數量ToolStripTextBox.Enabled = false;
        }

        private void 自訂的數量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自動補給ToolStripMenuItem.Checked = false;
            自訂的數量ToolStripMenuItem.Checked = true;
            保存的數量ToolStripMenuItem.Checked = false;
            補給ToolStripDropDownButton.Text = "補給：自訂數";
            if (!補給ToolStrip.Visible)
                if (!金錢ToolStrip.Visible)
                    this.Height += 25;
            補給ToolStrip.Visible = true;

            補給管理ToolStripMenuItem.Checked = true;
            水的數量ToolStripTextBox.Enabled = true;
            糧食的數量ToolStripTextBox.Enabled = true;
            資材的數量ToolStripTextBox.Enabled = true;
            彈藥的數量ToolStripTextBox.Enabled = true;
        }

        private void 保存的數量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自動補給ToolStripMenuItem.Checked = false;
            自訂的數量ToolStripMenuItem.Checked = false;
            保存的數量ToolStripMenuItem.Checked = true;
            補給ToolStripDropDownButton.Text = "補給：保存數";
            if (補給ToolStrip.Visible)
                if (!金錢ToolStrip.Visible)
                    this.Height -= 25;
            補給ToolStrip.Visible = false;

            補給管理ToolStripMenuItem.Checked = false;
            水的數量ToolStripTextBox.Enabled = false;
            糧食的數量ToolStripTextBox.Enabled = false;
            資材的數量ToolStripTextBox.Enabled = false;
            彈藥的數量ToolStripTextBox.Enabled = false;
        }
        #endregion

        #region 進入城市地點
        private void 進城ToolStripDropDownButton_ButtonClick(object sender, EventArgs e)
        {
            int mode;
            if (港口廣場ToolStripMenuItem.Checked)
                mode = 0x8F;
            else if (廣場ToolStripMenuItem.Checked)
                mode = 0x90;
            else if (商業地區ToolStripMenuItem.Checked)
                mode = 0x91;
            else if (商務會館ToolStripMenuItem.Checked)
                mode = 0x92;
            else if (陸地探險ToolStripMenuItem.Checked)
                mode = 0x93;
            else if (靠岸ToolStripMenuItem.Checked)
                mode = 0xCD;
            else
                mode = 0x8F;

            for (int User = 0; User < 5; User++)
            {
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    if (!自動進城ToolStripMenuItem.Checked)
                        if (UsingProcess[User].CityNo != -1)
                            GVOCall.City[UsingProcess[User].CityNo].EnterMode = mode;
                    Call.IntoCity(UsingProcess[User].hWnd, UsingProcess[User].ID, mode);
                }
            }
        }

        private void 自動進城ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：自動進城";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = true;
        }

        private void 靠岸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：靠岸";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = true;
            自動進城ToolStripMenuItem.Checked = false;
        }

        private void 陸地探險ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：陸地探險";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = true;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = false;
        }

        private void 商務會館ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：商務會館";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = true;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = false;
        }

        private void 商業地區ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：商業地區";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = true;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = false;
        }

        private void 廣場ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：廣場";
            港口廣場ToolStripMenuItem.Checked = false;
            廣場ToolStripMenuItem.Checked = true;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = false;
        }

        private void 港口廣場ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            進城ToolStripDropDownButton.Text = "進城：港口廣場";
            港口廣場ToolStripMenuItem.Checked = true;
            廣場ToolStripMenuItem.Checked = false;
            商業地區ToolStripMenuItem.Checked = false;
            商務會館ToolStripMenuItem.Checked = false;
            陸地探險ToolStripMenuItem.Checked = false;
            靠岸ToolStripMenuItem.Checked = false;
            自動進城ToolStripMenuItem.Checked = false;
        }
        #endregion

        private void 出航ToolStripButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Sailing(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd));
        }

        #region 角色欄(委任提督)
        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            #region comboBox初始化
            int User;

            ComboBox _comboBox = (ComboBox)sender;
            System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
            TabPage _tabpage = new TabPage();

            for (User = 0; User < 5; User++)
            {
                if (_comboBox.Name.Contains(User.ToString()))
                {
                    switch (User)
                    {
                        case 0:
                            _timer = timer0;
                            _tabpage = tabPage0;
                            break;
                        case 1:
                            _timer = timer1;
                            _tabpage = tabPage1;
                            break;
                        case 2:
                            _timer = timer2;
                            _tabpage = tabPage2;
                            break;
                        case 3:
                            _timer = timer3;
                            _tabpage = tabPage3;
                            break;
                        case 4:
                            _timer = timer4;
                            _tabpage = tabPage4;
                            break;
                    }
                    break;
                }
            }
            #endregion

            _timer.Stop();
            if (Call.CheckPartyID(UsingProcess[User].hWnd, Call.GetUserId(Call.GethWnd(comboBox0.Text))))
                Call.ChangeAdmiral(UsingProcess[User].hWnd, Call.GetUserId(Call.GethWnd(comboBox0.Text)));

            bool IsChange = false;

            #region 交換位置
            if (User == 0)
            {
                for (int i = 1; i < 5; i++)
                {
                    if (UsingProcess[i].Window == comboBox0.Text)
                    {
                        IsChange = true;
                        uInfo TempProcess = UsingProcess[i];
                        switch (i)
                        {
                            case 1:
                                UsingProcess[1] = UsingProcess[0];
                                comboBox1.Text = UsingProcess[0].Window;
                                UsingProcess[0] = TempProcess;
                                break;
                            case 2:
                                UsingProcess[2] = UsingProcess[1];
                                comboBox2.Text = UsingProcess[1].Window;
                                UsingProcess[1] = UsingProcess[0];
                                comboBox1.Text = UsingProcess[0].Window;
                                UsingProcess[0] = TempProcess;
                                break;
                            case 3:
                                UsingProcess[3] = UsingProcess[2];
                                comboBox3.Text = UsingProcess[2].Window;
                                UsingProcess[2] = UsingProcess[1];
                                comboBox2.Text = UsingProcess[1].Window;
                                UsingProcess[1] = UsingProcess[0];
                                comboBox1.Text = UsingProcess[0].Window;
                                UsingProcess[0] = TempProcess;
                                break;
                            case 4:
                                UsingProcess[4] = UsingProcess[3];
                                comboBox4.Text = UsingProcess[3].Window;
                                UsingProcess[3] = UsingProcess[2];
                                comboBox3.Text = UsingProcess[2].Window;
                                UsingProcess[2] = UsingProcess[1];
                                comboBox2.Text = UsingProcess[1].Window;
                                UsingProcess[1] = UsingProcess[0];
                                comboBox1.Text = UsingProcess[0].Window;
                                UsingProcess[0] = TempProcess;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            #endregion

            UsingProcess[User].Exchange_Position = true;
           
            if (!IsChange && UsingProcess[User].Window != _comboBox.Text)
                UsingProcess[User] = new uInfo();

            _tabpage.Text = Call.GetUserName(Call.GethWnd(_comboBox.Text));

            UsingProcess[User].Window = _comboBox.Text;

            Call.SetItemBox(UsingProcess[User].hWnd, true);
            Call.SetInfoBox(UsingProcess[User].hWnd, true);

            _timer.Start();
        }
        #endregion

        #region 角色監控
        private void timer(int User)
        {
            #region timer初始化
            System.Windows.Forms.Timer timer;
            ComboBox comboBox;
            VistaStyleProgressBar.ProgressBar fatigueBar, powerBar;
            ToolStripLabel water, grain, stuff, ammunition;
            ListView CuisineList, SkillList, Navalbattle_SkillList;
            TextBox Cuisine, Fatigue, AdjutantChange;
            ListView AdjutantList1, AdjutantList2;
            GroupBox _gropBox1, _gropBox2;
            switch (User)
            {
                case 0:
                    timer = timer0;
                    comboBox = comboBox0;
                    UsingProcess[User].UsingSkillImage[0] = SkillImage0_0;
                    UsingProcess[User].UsingSkillImage[1] = SkillImage0_1;
                    UsingProcess[User].UsingSkillImage[2] = SkillImage0_2;
                    fatigueBar = FatigueBar0;
                    powerBar = PowerBar0;
                    water = Water0;
                    grain = Grain0;
                    stuff = Stuff0;
                    ammunition = Ammunition0;
                    CuisineList = CuisineList0;

                    SkillList = SkillList0;
                    Navalbattle_SkillList = Navalbattle_SkillList0;

                    Cuisine = Cuisine0;
                    Fatigue = Fatigue0;

                    AdjutantChange = AdjutantChange0;
                    AdjutantList1 = AdjutantList0_1;
                    AdjutantList2 = AdjutantList0_2;
                    _gropBox1 = Adjutant0_1;
                    _gropBox2 = Adjutant0_2;
                    break;
                case 1:
                    timer = timer1;
                    comboBox = comboBox1;
                    UsingProcess[User].UsingSkillImage[0] = SkillImage1_0;
                    UsingProcess[User].UsingSkillImage[1] = SkillImage1_1;
                    UsingProcess[User].UsingSkillImage[2] = SkillImage1_2;
                    fatigueBar = FatigueBar1;
                    powerBar = PowerBar1;
                    water = Water1;
                    grain = Grain1;
                    stuff = Stuff1;
                    ammunition = Ammunition1;
                    CuisineList = CuisineList1;

                    SkillList = SkillList1;
                    Navalbattle_SkillList = Navalbattle_SkillList1;

                    Cuisine = Cuisine1;
                    Fatigue = Fatigue1;

                    AdjutantChange = AdjutantChange1;
                    AdjutantList1 = AdjutantList1_1;
                    AdjutantList2 = AdjutantList1_2;
                    _gropBox1 = Adjutant1_1;
                    _gropBox2 = Adjutant1_2;
                    break;
                case 2:
                    timer = timer2;
                    comboBox = comboBox2;
                    UsingProcess[User].UsingSkillImage[0] = SkillImage2_0;
                    UsingProcess[User].UsingSkillImage[1] = SkillImage2_1;
                    UsingProcess[User].UsingSkillImage[2] = SkillImage2_2;
                    fatigueBar = FatigueBar2;
                    powerBar = PowerBar2;
                    water = Water2;
                    grain = Grain2;
                    stuff = Stuff2;
                    ammunition = Ammunition2;
                    CuisineList = CuisineList2;

                    SkillList = SkillList2;
                    Navalbattle_SkillList = Navalbattle_SkillList2;

                    Cuisine = Cuisine2;
                    Fatigue = Fatigue2;

                    AdjutantChange = AdjutantChange2;
                    AdjutantList1 = AdjutantList2_1;
                    AdjutantList2 = AdjutantList2_2;
                    _gropBox1 = Adjutant2_1;
                    _gropBox2 = Adjutant2_2;
                    break;
                case 3:
                    timer = timer3;
                    comboBox = comboBox3;
                    UsingProcess[User].UsingSkillImage[0] = SkillImage3_0;
                    UsingProcess[User].UsingSkillImage[1] = SkillImage3_1;
                    UsingProcess[User].UsingSkillImage[2] = SkillImage3_2;
                    fatigueBar = FatigueBar3;
                    powerBar = PowerBar3;
                    water = Water3;
                    grain = Grain3;
                    stuff = Stuff3;
                    ammunition = Ammunition3;
                    CuisineList = CuisineList3;

                    SkillList = SkillList3;
                    Navalbattle_SkillList = Navalbattle_SkillList3;

                    Cuisine = Cuisine3;
                    Fatigue = Fatigue3;

                    AdjutantChange = AdjutantChange3;
                    AdjutantList1 = AdjutantList3_1;
                    AdjutantList2 = AdjutantList3_2;
                    _gropBox1 = Adjutant3_1;
                    _gropBox2 = Adjutant3_2;
                    break;
                case 4:
                    timer = timer4;
                    comboBox = comboBox4;
                    UsingProcess[User].UsingSkillImage[0] = SkillImage4_0;
                    UsingProcess[User].UsingSkillImage[1] = SkillImage4_1;
                    UsingProcess[User].UsingSkillImage[2] = SkillImage4_2;
                    fatigueBar = FatigueBar4;
                    powerBar = PowerBar4;
                    water = Water4;
                    grain = Grain4;
                    stuff = Stuff4;
                    ammunition = Ammunition4;
                    CuisineList = CuisineList4;

                    SkillList = SkillList4;
                    Navalbattle_SkillList = Navalbattle_SkillList4;

                    Cuisine = Cuisine4;
                    Fatigue = Fatigue4;

                    AdjutantChange = AdjutantChange4;
                    AdjutantList1 = AdjutantList4_1;
                    AdjutantList2 = AdjutantList4_2;
                    _gropBox1 = Adjutant4_1;
                    _gropBox2 = Adjutant4_2;
                    break;
                default:
                    return;
            }
            #endregion

            if ((UsingProcess[User].hWnd = Call.GethWnd(comboBox.Text)) != IntPtr.Zero && Call.GetConnectState(UsingProcess[User].hWnd) && Call.GetUserId(UsingProcess[User].hWnd) != 0)
            {
                timer.Stop();

                if (comboBox.BackColor == Color.Red)
                    comboBox.BackColor = Color.White;

                #region 檢查Server&Name&&ID
                UsingProcess[User].ServerName = Call.GetServerName(UsingProcess[User].hWnd);
                UsingProcess[User].Name = Call.GetUserName(UsingProcess[User].hWnd);
                UsingProcess[User].ID = Call.GetUserId(UsingProcess[User].hWnd);

                //帳號權限
                if (string.IsNullOrWhiteSpace(Form1.UsingProcess[User].User_Type))
                    Form1.UsingProcess[User].User_Type = Call.CheckUser(UsingProcess[User].ServerName, UsingProcess[User].Name);

                if (UsingProcess[User].ServerName != null && UsingProcess[User].Name != null && Form1.UsingProcess[User].User_Type == "Pirate")
                    this.Close();

                if (comboBox.Text != "﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name)
                {
                    UsingProcess[User] = new uInfo();
                    WinAPI.SetWindowText(UsingProcess[User].hWnd, "﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name);
                    comboBox.Text = "﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name;
                    timer.Start();
                    return;
                }

                if (!Call.CheckUserName(UsingProcess[User].hWnd, UsingProcess[User].ID, UsingProcess[User].Name))
                {
                    Call.PostMessage(UsingProcess[User].hWnd, "<<請勿使用不正常方式啟動程式>>");
                    this.Close();
                    return;
                }
                #endregion

                if (Call.SceneChange(UsingProcess[User].hWnd))
                    UsingProcess[User].CountChangeCoordinate = 0;

                if (Call.SceneChange(UsingProcess[User].hWnd) || Call.Busy(UsingProcess[User].hWnd))
                    UsingProcess[User].TimeCount = 0;
                else
                {
                    if (UsingProcess[User].TimeCount < 1000)
                    {
                        ++UsingProcess[User].TimeCount;
                        if (UsingProcess[User].TimeCount % 30 == 0)
                            Call.Responding(UsingProcess[User].hWnd);
                    }
                    else
                    {
                        if (Form1.UsingProcess[User].User_Type == "猜猜看")
                        {
                            if (Form1.UsingProcess[User].Place != 0x8)
                            {
                                Call.UseSkill(UsingProcess[User].hWnd, 0);
                                UsingProcess[User].TimeCount = 0;
                            }
                        }
                    }
                }
                //byte[] bytes = Encoding.Unicode.GetBytes("使用物品");
                //for (int i = 0; i < bytes.Length; i++)
                //Console.Write(Convert.ToString(bytes[i], 16) + " ");
                //Console.WriteLine();

                //Call.PostMessage(Form1.UsingProcess[User].hWnd, "測試用");

                #region 技能監控
                Call.GetUsingSkills(UsingProcess[User].hWnd, UsingProcess[User].UsingSkill, ref UsingProcess[User].UsingSkillNum);//取得使用中的技能

                if (技能監控ToolStripMenuItem.Checked)
                {
                    UsingProcess[User].UsingSkillImage[0].Image = skill.GetObject("_" + UsingProcess[User].UsingSkill[0].ToString("D8")) as Image;
                    UsingProcess[User].UsingSkillImage[1].Image = skill.GetObject("_" + UsingProcess[User].UsingSkill[1].ToString("D8")) as Image;
                    UsingProcess[User].UsingSkillImage[2].Image = skill.GetObject("_" + UsingProcess[User].UsingSkill[2].ToString("D8")) as Image;
                }
                #endregion

                #region 行動力、疲倦度
                if (Call.GetUserStatus(UsingProcess[User].hWnd, ref UsingProcess[User].Power, ref UsingProcess[User].MaxPower, ref UsingProcess[User].Money, ref UsingProcess[User].Fatigue))
                {
                    if (fatigueBar.Value != UsingProcess[User].Fatigue)
                        fatigueBar.Value = UsingProcess[User].Fatigue;
                    if (powerBar.MaxValue != UsingProcess[User].MaxPower && UsingProcess[User].MaxPower != 0)
                        powerBar.MaxValue = UsingProcess[User].MaxPower;
                    if (powerBar.Value != UsingProcess[User].Power)
                        powerBar.Value = UsingProcess[User].Power;
                }
                #endregion

                #region 物資數量
                if (Call.GetMaterialsStatus(UsingProcess[User].hWnd, UsingProcess[User].Materials, UsingProcess[User].SetMaterials))
                {
                    if (water.Text != UsingProcess[User].Materials[0].ToString())
                        water.Text = UsingProcess[User].Materials[0].ToString();
                    if (grain.Text != UsingProcess[User].Materials[1].ToString())
                        grain.Text = UsingProcess[User].Materials[1].ToString();
                    if (stuff.Text != UsingProcess[User].Materials[2].ToString())
                        stuff.Text = UsingProcess[User].Materials[2].ToString();
                    if (ammunition.Text != UsingProcess[User].Materials[3].ToString())
                        ammunition.Text = UsingProcess[User].Materials[3].ToString();
                }
                #endregion

                #region 取得角度、座標
                UsingProcess[User].Old_Coordinate = UsingProcess[User].Coordinate;
                UsingProcess[User].Old_Cos = UsingProcess[User].Cos;
                UsingProcess[User].Old_Sin = UsingProcess[User].Old_Sin;

                Call.GetAngle(UsingProcess[User].hWnd, ref UsingProcess[User].Cos, ref UsingProcess[User].Sin);
                UsingProcess[User].Coordinate = Call.GetCoordinate(UsingProcess[User].hWnd);
                #endregion

                #region 取得地點、名稱、場景切換
                int TempPlace = UsingProcess[User].Place;
                UsingProcess[User].PlaceName = Call.GetPlace(UsingProcess[User].hWnd, ref UsingProcess[User].PlaceNo, ref UsingProcess[User].Place);

                #region 取得城市編號
                if (UsingProcess[User].Place == 0x8 || UsingProcess[User].Place == 0xC || UsingProcess[User].Place == 0x1C)
                {
                    UsingProcess[User].CityNo = UsingProcess[User].PlaceNo - 1;
                    if (!GVOCall.City[UsingProcess[User].CityNo].Done)
                    {
                        GVOCall.City[UsingProcess[User].CityNo].Done = true;
                        GVOCall.City[UsingProcess[User].CityNo]._BuyTradeInfo = new GVOCall.CityInfo.BuyTradeInfo[15];//購買交易品資料
                        GVOCall.City[UsingProcess[User].CityNo].CheckedBuy = new int[15];
                        GVOCall.City[UsingProcess[User].CityNo]._SellTradeInfo = new GVOCall.CityInfo.SellTradeInfo[256];//出售交易品資料
                        GVOCall.City[UsingProcess[User].CityNo].CheckedSell = new bool[1536];

                        GVOCall.City[UsingProcess[User].CityNo]._SceneInfo = new GVOCall.CityInfo.TargetInfo[256];//場景資料
                        GVOCall.City[UsingProcess[User].CityNo]._NPCInfo = new GVOCall.CityInfo.TargetInfo[100];//NPC資料

                        GVOCall.City[UsingProcess[User].CityNo].Meal = new int[5];//套餐ID
                        GVOCall.City[UsingProcess[User].CityNo].MealName = new string[5];//套餐名字
                    }
                }
                else
                    UsingProcess[User].CityNo = -1;
                #endregion

                //取得所有活動目標
                if (Call.InDungeon(UsingProcess[User].hWnd, UsingProcess[User].Place))
                    UsingProcess[User]._People = Call.GetAllPeople(UsingProcess[User].hWnd);

                //取得所有固定目標
                UsingProcess[User]._Regular = Call.GetAllRegular(UsingProcess[User].hWnd);
                if (Form1.UsingProcess[User].CityNo != -1 && UsingProcess[User].Place == 0x8)
                {
                    for (int i = 0; i < Form1.UsingProcess[User]._Regular.Length; i++)
                    {
                        if (!Form1.UsingProcess[User]._Regular[i].Coordinate.IsEmpty)
                        {
                            for (int j = 0; j < GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo.Length; j++)
                            {
                                if (GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[j].Coordinate.IsEmpty)
                                {
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[j].Coordinate = Form1.UsingProcess[User]._Regular[i].Coordinate;
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[j].ID = Form1.UsingProcess[User]._Regular[i].ID;
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[j].Name = Form1.UsingProcess[User]._Regular[i].Name;
                                    Console.WriteLine(Form1.UsingProcess[User]._Regular[i].Name + " " + Form1.UsingProcess[User]._Regular[i].Coordinate + "座標儲存中...");
                                    break;
                                }
                                else if (Form1.UsingProcess[User]._Regular[i].ID == GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[j].ID)
                                    break;
                            }
                        }
                    }
                }

                if (UsingProcess[User].Place != TempPlace)
                {
                    //所在場景(0x4海上 0x8陸地 0xC室內 0x1C港口 >0x1C 登陸點
                    if (UsingProcess[User].Place == 0x4 && TempPlace >= 0x1C)//港口到海上
                    {
                        UsingProcess[User].UpdateItem = false;
                        UsingProcess[User].LandExit = new uInfo.LandInfo[5];
                    }

                    if (TempPlace == 0xC || TempPlace == 0x1C)
                    {
                        if (Call.InDungeon(UsingProcess[User].hWnd, UsingProcess[User].Place))
                            ++UsingProcess[User].DungeonIntoCount;
                    }

                    if (UsingProcess[User].Place == 0x8 && TempPlace == 0xC)//室內到室外
                    {
                        int distance = 700;
                        if (Call.GetLastPlace(Form1.UsingProcess[User].hWnd).Contains("教會"))
                            distance = 1200;

                        for (int i = 0; i < GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo.Length; i++)
                        {
                            if (Call.Distance(UsingProcess[User].Coordinate, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate) <= distance)
                            {
                                if (GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name != Call.GetLastPlace(Form1.UsingProcess[User].hWnd))
                                {
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name = Call.GetLastPlace(Form1.UsingProcess[User].hWnd);
                                    Console.WriteLine(GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate + "儲存座標名稱：" + GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name);
                                }
                            }
                        }
                    }

                    #region 記錄陸地出口
                    if (TempPlace != 0x4 && TempPlace != 0xC)
                    {
                        //0x8, 0x10, 0x13, 0x18, 0x1B
                        if (UsingProcess[User].Place != 0xC && UsingProcess[User].Place > 0x8 && UsingProcess[User].Place < 0x1C)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (UsingProcess[User].LandExit[i].Name == UsingProcess[User].PlaceName)
                                    break;
                                if (UsingProcess[User].LandExit[i].Exit.IsEmpty)
                                {
                                    UsingProcess[User].LandExit[i].Name = UsingProcess[User].PlaceName;
                                    UsingProcess[User].LandExit[i].Exit = UsingProcess[User].Coordinate;
                                    Call.PostMessage(UsingProcess[User].hWnd, "<<記錄出口：" + UsingProcess[User].LandExit[i].Name + ">>");
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                    {
                        if (UsingProcess[User].Place >= 0x1C && UsingProcess[User].Place < 0x30)
                        {

                            if (TempPlace == 0x4)//海上到港口
                            {
                                #region 自動進城
                                if (自動進城ToolStripMenuItem.Checked)
                                {
                                    if (UsingProcess[User].Place == 0x1C)
                                    {
                                        if (GVOCall.City[UsingProcess[User].CityNo].EnterMode != 0)
                                            Call.IntoCity(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), GVOCall.City[UsingProcess[User].CityNo].EnterMode);
                                        else
                                            Call.IntoCity(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), 0x8F);//港口廣場                            
                                    }
                                    else
                                        Call.IntoCity(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), 0x93);//陸地探險
                                    //if (UsingProcess[User].Place != 0x1C && UsingProcess[User].PlaceName.Contains("靠岸地點"))//0x20, 0x23
                                }
                                #endregion
                                Form1.UsingProcess[User].ReadBook = false;
                            }

                            if (TempPlace == 0x8)//室外到港口
                            {
                                Form1.UsingProcess[User].Talk = false;
                                //Form1.UsingProcess[User].Research = false;
                            }
                        }
                    }
                }
                #endregion

                if (Form1.UsingProcess[User].User_Type == "猜猜看")
                {
                    #region 瞬移
                    if (瞬移ToolStripMenuItem.Checked)
                    {
                        if (Call.CoordinateChange(UsingProcess[User].hWnd) && UsingProcess[User].Place != 0x4 && UsingProcess[User].Place != 0xFF)
                        {
                            bool InLand = false;
                            int LandNo = -1, Dis = 0;
                            for (int i = 0; i < 5; i++)
                                if (UsingProcess[User].LandExit[i].Name == UsingProcess[User].PlaceName)
                                {
                                    InLand = true;
                                    LandNo = i;
                                    Dis = (int)Call.Distance(UsingProcess[User].LandExit[i].Exit, UsingProcess[User].Coordinate);
                                    break;
                                }

                            if (!InLand || Dis > 2500)
                                UsingProcess[User].CountChangeCoordinate++;

                            if (UsingProcess[User].CountChangeCoordinate > 3 || UsingProcess[User].Place >= 0x30)
                            {
                                Call.MoveCoordinate(UsingProcess[User].hWnd);
                                //Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + " 瞬移");
                                UsingProcess[User].CountChangeCoordinate = 0;
                            }
                        }
                        else
                            UsingProcess[User].CountChangeCoordinate = 0;
                    }
                    #endregion

                    if (劇情模式ToolStripMenuItem.Checked)
                        Call.BusyMode(UsingProcess[User].hWnd);
                }

                Form1.UsingProcess[User].targetThread = new Thread(new ThreadStart(thread_getTarget));
                Form1.UsingProcess[User].targetThread.Name = Form1.UsingProcess[User].Name;
                if ((Form1.UsingProcess[User].targetThread.ThreadState & (ThreadState.Stopped | ThreadState.Unstarted)) != 0)
                    Form1.UsingProcess[User].targetThread.Start();

                #region 當前目標
                if (User == 0)
                {
                    string TargetName = Form1.UsingProcess[User].target.Name;
                    //string TargetName = Call.GetTargetName(UsingProcess[User].hWnd, ref TempTID, ref TX, ref TY);
                    if (Form1.UsingProcess[User].target.ID != 0 && TargetID != Form1.UsingProcess[User].target.ID)
                    {
                        TargetID = Form1.UsingProcess[User].target.ID;
                        TargetIDBox.Text = TargetName + "『" + TargetID + "』座標：" + Form1.UsingProcess[User].target.Coordinate;
                        float cos = 0, sin = 0;
                        Call.GetPeopleAngle(Form1.UsingProcess[User].hWnd, TargetID,ref cos, ref sin);
                    }
                    //Console.WriteLine(" 距離：" + Call.Distance(UsingProcess[User].Coordinate, Call.GetTargetCoordinate(UsingProcess[User].hWnd, TargetID, 0)));

                    if (Mission != 0)
                        放棄委託Button.Enabled = true;
                    else
                        放棄委託Button.Enabled = false;

                    if (UsingProcess[User].CityNo != -1)
                    {
                        switch (TargetName)
                        {
                            //酒館
                            case "休息站老闆":
                            case "餐廳老闆":
                            case "酒館老闆":
                                if (new CityCall().GetClosestTarget(User, TargetName, "", "", "").ID != TargetID)
                                    new CityCall().GetNpcInfo(User, TargetName, "");
                                Call.GetPubMenu(UsingProcess[User].hWnd, ref GVOCall.City[UsingProcess[User].CityNo].Drink, ref GVOCall.City[UsingProcess[User].CityNo].Food, ref GVOCall.City[UsingProcess[User].CityNo].DrinkName, ref GVOCall.City[UsingProcess[User].CityNo].FoodName);
                                Call.GetPubMeal(UsingProcess[User].hWnd, ref GVOCall.City[UsingProcess[User].CityNo].Meal, ref GVOCall.City[UsingProcess[User].CityNo].MealName);
                                break;
                            //委托
                            case "委託介紹人":
                            case "商人委託介紹人":
                            case "冒險家委託介紹人":
                            case "海事委託介紹人":
                                if (new CityCall().GetClosestTarget(User, TargetName, "", "", "").ID != TargetID)
                                    new CityCall().GetNpcInfo(User, TargetName, "");
                                Call.GetMission(UsingProcess[User].hWnd, ref GVOCall.City[UsingProcess[User].CityNo].Mission, ref GVOCall.City[UsingProcess[User].CityNo].MissionName);
                                break;
                            //銀行
                            case "銀行職員":
                                if (new CityCall().GetClosestTarget(User, TargetName, "", "", "").ID != TargetID)
                                    new CityCall().GetNpcInfo(User, TargetName, "");
                                break;
                            //書庫
                            case "學者":
                                if (new CityCall().GetClosestTarget(User, TargetName, "", "", "").ID != TargetID)
                                    new CityCall().GetNpcInfo(User, TargetName, "");
                                Call.GetSubject(UsingProcess[User].hWnd, ref  GVOCall.City[UsingProcess[User].CityNo].Subject);
                                break;
                            //交易所
                            case "交易所店主":
                            case "交易所學徒":
                                if (new CityCall().GetClosestTarget(User, TargetName, "", "", "").ID != TargetID)
                                    new CityCall().GetNpcInfo(User, TargetName, "");
                                if (Call.GetBuyTradeMenu(Form1.UsingProcess[User].hWnd, ref GVOCall.City[Form1.UsingProcess[User].CityNo].BuyMenuNum, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._BuyTradeInfo))
                                    Form1.UsingProcess[User].TradeMenu = "BuyPage";
                                else if (Call.GetSellTradeMenu(Form1.UsingProcess[User].hWnd, ref GVOCall.City[Form1.UsingProcess[User].CityNo].SellMenuNum, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._SellTradeInfo))
                                    Form1.UsingProcess[User].TradeMenu = "SellPage";
                                else
                                    Form1.UsingProcess[User].TradeMenu = null;
                                break;
                            default:
                                break;
                        }

                        #region 點餐
                        if (GVOCall.City[UsingProcess[User].CityNo].Drink != 0 || GVOCall.City[UsingProcess[User].CityNo].Food != 0)
                            點餐Button.Enabled = true;
                        else
                            點餐Button.Enabled = false;
                        #endregion

                        #region 套餐
                        bool CheckMealMenu = true;
                        for (int i = 0; i < 5; i++)
                            if (GVOCall.City[UsingProcess[User].CityNo].Meal[i] == 0)
                            {
                                CheckMealMenu = false;
                                break;
                            }
                        if (CheckMealMenu)
                            套餐Button.Enabled = true;
                        else
                            套餐Button.Enabled = false;
                        #endregion

                        #region 任務
                        Mission = GVOCall.City[UsingProcess[User].CityNo].Mission;
                        string MissionText = "任務：" + GVOCall.City[UsingProcess[User].CityNo].MissionName;
                        if (label3.Text != MissionText)
                            label3.Text = MissionText;

                        if (GVOCall.City[UsingProcess[User].CityNo].Mission != 0)//City[UsingProcess[User].CityNo].MissionName != null
                            接受委託Button.Enabled = true;
                        else
                            接受委託Button.Enabled = false;

                        if (Mission != 0)
                            放棄委託Button.Enabled = true;
                        else
                            放棄委託Button.Enabled = false;
                        #endregion

                        #region 學科

                        switch (GVOCall.City[UsingProcess[User].CityNo].Subject)
                        {
                            case 0x6:
                                閱讀書籍Button.Text = "閱讀書籍：地理";
                                閱讀書籍Button.Enabled = true;
                                break;
                            case 0x19:
                                閱讀書籍Button.Text = "閱讀書籍：考古";
                                閱讀書籍Button.Enabled = true;
                                break;
                            case 0x1A:
                                閱讀書籍Button.Text = "閱讀書籍：宗教";
                                閱讀書籍Button.Enabled = true;
                                break;
                            case 0x1B:
                                閱讀書籍Button.Text = "閱讀書籍：生物";
                                閱讀書籍Button.Enabled = true;
                                break;
                            case 0x1C:
                                閱讀書籍Button.Text = "閱讀書籍：美術";
                                閱讀書籍Button.Enabled = true;
                                break;
                            case 0x1D:
                                閱讀書籍Button.Text = "閱讀書籍：財寶";
                                閱讀書籍Button.Enabled = true;
                                break;
                            default:
                                閱讀書籍Button.Text = "閱讀書籍：　　";
                                閱讀書籍Button.Enabled = false;
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        點餐Button.Enabled = false;
                        套餐Button.Enabled = false;
                        接受委託Button.Enabled = false;
                        閱讀書籍Button.Enabled = false;
                    }
                }
                #endregion

                #region 自動跟隨
                //已組隊 && 無跟隨 && 非提督 && 非暴風雨/雪 && 非戰鬥
                if (Call.GetPartyID(UsingProcess[User].hWnd, UsingProcess[User].PartyID) && !Call.GetFollowStatus(UsingProcess[User].hWnd) && UsingProcess[User].ID != UsingProcess[User].PartyID[0] && !Call.GetBadWeather(Form1.UsingProcess[User].hWnd) && !Call.GetNavalbattle(UsingProcess[User].hWnd))
                {
                    if (UsingProcess[User].Place < 0x1C || UsingProcess[User].Place >= 0x30)
                    {
                        comboBox.ForeColor = Color.Lime;
                        if (自動跟隨ToolStripMenuItem.Checked && UsingProcess[User].PlaceNo == UsingProcess[0].PlaceNo && UsingProcess[User].Place == UsingProcess[0].Place)
                        {
                            ++UsingProcess[User].FollowCount;
                            int Distance = (int)Call.Distance(UsingProcess[User].Coordinate, Call.GetTargetCoordinate(UsingProcess[User].hWnd, UsingProcess[User].PartyID[0], 0));
                            if (UsingProcess[User].PartyID[0] == UsingProcess[0].ID)
                                Distance = (int)Call.Distance(UsingProcess[User].Coordinate, UsingProcess[0].Coordinate);
                            if (UsingProcess[User].FollowCount > 2)
                            {
                                if (Distance > 950)
                                {
                                    if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                                    {
                                        if (UsingProcess[User].Place != 0x4)
                                        {
                                            if (UsingProcess[User].PartyID[0] == UsingProcess[0].ID)
                                                Call.MoveCoordinate(UsingProcess[User].hWnd, new PointF(UsingProcess[0].Coordinate.X, UsingProcess[0].Coordinate.Y), true);
                                            else
                                                Call.MoveCoordinate(UsingProcess[User].hWnd, Call.GetTargetCoordinate(UsingProcess[User].hWnd, UsingProcess[User].PartyID[0], 0), true);
                                            Call.PostMessage(UsingProcess[User].hWnd, "<<提督 - 距離 " + Distance + ">>");
                                            UsingProcess[User].FollowCount = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    Call.Follow(UsingProcess[User].hWnd, UsingProcess[User].Place);//跟隨
                                    Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + " 跟隨");
                                    UsingProcess[User].FollowCount = 0;
                                }
                            }
                        }
                        else
                            UsingProcess[User].FollowCount = 0;
                    }
                }
                else
                    if (comboBox.ForeColor == Color.Lime)
                        comboBox.ForeColor = Color.Black;
                #endregion

                if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                {
                    bool cuisinelist_change = false, skilllist_change = false;
                    string Save_FileName = "﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + ".ini";
                    if (!File.Exists(Application.StartupPath + "\\set\\" + Save_FileName))
                        File.Create(Application.StartupPath + "\\set\\" + Save_FileName);

                    #region 讀取資料
                    switch (Call.CheckWindows(UsingProcess[User].hWnd))
                    {
                        case "物品欄":
                            #region 讀取物品資料
                            if (Call.GetFightID(UsingProcess[User].hWnd))
                                break;

                            if (Call.GetItemAddr(UsingProcess[User].hWnd, ref UsingProcess[User]._Item))
                            {
                                if (File.Exists(Application.StartupPath + "\\set\\" + Save_FileName))
                                {
                                    using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath + "\\set\\", Save_FileName)))
                                    {
                                        UsingProcess[User].Save_CuisineName = myCINI.getKeyValue("已勾選", "料理");
                                    }
                                }

                                if (!UsingProcess[User].InitItem || !UsingProcess[User].UpdateItem)
                                {
                                    if (!UsingProcess[User].InitItem)
                                        Call.PostMessage(UsingProcess[User].hWnd, "<<初始化物品資料>>");
                                    else if (!UsingProcess[User].UpdateItem)
                                        Call.PostMessage(UsingProcess[User].hWnd, "<<更新物品資料>>");

                                    UsingProcess[User].InitItem = true;
                                    UsingProcess[User].UpdateItem = true;

                                    if (UsingProcess[User].ItemBoxOpen)
                                        UsingProcess[User].ItemBoxOpen = false;
                                }

                                if (Call.Check_Display_ItemBox(UsingProcess[User].hWnd) != "顯示")
                                {
                                    Call.Sound(UsingProcess[User].hWnd, false);
                                    Call.CloseWindows(UsingProcess[User].hWnd);
                                }

                                Call.DisplayItemBox(UsingProcess[User].hWnd, true);
                                Call.SetItemBox(UsingProcess[User].hWnd, true);
                            }
                            #endregion
                            break;
                        case "技能":
                            #region 讀取技能資料
                            if (Call.GetSkillInfo(UsingProcess[User].hWnd, ref UsingProcess[User].SkillNum, UsingProcess[User]._SkillInfo))
                            {
                                if (File.Exists(Application.StartupPath + "\\set\\" + Save_FileName))
                                {
                                    using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath + "\\set\\", Save_FileName)))
                                    {
                                        UsingProcess[User].Save_SkillName = myCINI.getKeyValue("已勾選", "技能");
                                    }
                                }

                                if (!UsingProcess[User].InitSkill)
                                {
                                    Call.PostMessage(UsingProcess[User].hWnd, "<<初始化技能資料>>");

                                    UsingProcess[User].InitSkill = true;

                                    if (UsingProcess[User].SkillInfoBoxOpen)
                                        UsingProcess[User].SkillInfoBoxOpen = false;
                                }
                                else
                                {
                                    if (Call.Check_Display_SkillInfo(UsingProcess[User].hWnd) != "顯示")
                                    {
                                        Call.Sound(UsingProcess[User].hWnd, false);
                                        Call.CloseWindows(UsingProcess[User].hWnd);
                                        Call.DisplaySkillInfo(UsingProcess[User].hWnd, true);
                                    }
                                    Call.SetInfoBox(UsingProcess[User].hWnd, true);
                                }
                            }
                            #endregion
                            break;
                        case "副官情報":
                            #region 讀取副官資料
                            if (Call.GetAdjutantInfo(UsingProcess[User].hWnd, UsingProcess[User]._Adjutant))
                            {
                                if (!UsingProcess[User].InitAdjutant || !UsingProcess[User].UpdateAdjutant)
                                {
                                    UsingProcess[User].TempMaritimeDays = UsingProcess[User].MaritimeDays;

                                    if (!UsingProcess[User].UpdateAdjutant)
                                        Call.PostMessage(UsingProcess[User].hWnd, "<<更新副官資料>>");
                                    else
                                        Call.PostMessage(UsingProcess[User].hWnd, "<<初始化副官資料>>");

                                    UsingProcess[User].InitAdjutant = true;
                                    UsingProcess[User].UpdateAdjutant = true;

                                    if (UsingProcess[User].AdjutantInfoBoxOpen)
                                        UsingProcess[User].AdjutantInfoBoxOpen = false;
                                }
                                else
                                {
                                    if (Call.Check_Display_AdjutantInfo(UsingProcess[User].hWnd) != "顯示")
                                    {
                                        Call.Sound(UsingProcess[User].hWnd, false);
                                        Call.CloseWindows(UsingProcess[User].hWnd);
                                        Call.DisplayAdjutantInfo(UsingProcess[User].hWnd, true);
                                    }
                                    Call.SetInfoBox(UsingProcess[User].hWnd, true);
                                }
                            }
                            #endregion
                            break;
                        case "選擇配方":
                            Call.GetFormula(UsingProcess[User].hWnd, ref UsingProcess[User]._Item);
                            break;
                        case null:
                            Call.Sound(UsingProcess[User].hWnd, true);
                            if (Call.GetBattleStatus(UsingProcess[User].hWnd) < 5 && get_game_ver() == game_ver)
                            {
                                if (!UsingProcess[User].InitSkill)
                                {
                                    #region 打開技能欄
                                    if (Call.Check_Display_SkillInfo(UsingProcess[User].hWnd) != "隱藏")
                                    {
                                        Call.DisplaySkillInfo(UsingProcess[User].hWnd, false);
                                        Call.SetInfoBox(UsingProcess[User].hWnd, false);
                                    }
                                    else
                                    {
                                        if (!UsingProcess[User].SkillInfoBoxOpen)
                                        {
                                            Call.InfoButton(UsingProcess[User].hWnd, UsingProcess[User].ID, "技能");
                                            UsingProcess[User].SkillInfoBoxOpen = true;
                                        }
                                    }
                                    #endregion
                                }
                                
                                else if ((!UsingProcess[User].InitItem || !UsingProcess[User].UpdateItem) && UsingProcess[User].Place < 0x1C)
                                {
                                    #region 打開物品欄
                                    if (Call.Check_Display_ItemBox(UsingProcess[User].hWnd) != "隱藏")
                                    {
                                        Call.DisplayItemBox(UsingProcess[User].hWnd, false);
                                        Call.SetItemBox(UsingProcess[User].hWnd, false);
                                    }
                                    else
                                    {
                                        if (!UsingProcess[User].ItemBoxOpen)
                                        {
                                            //打開物品欄
                                            Call.OpenItemBox(UsingProcess[User].hWnd);
                                            //Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + " 打開物品欄 ");
                                            UsingProcess[User].ItemBoxOpen = true;
                                            UsingProcess[User].CheckItemCount = 0;
                                        }
                                        else
                                        {
                                            ++UsingProcess[User].CheckItemCount;
                                            //初始化失敗
                                            if (UsingProcess[User].CheckItemCount > 10)
                                            {
                                                UsingProcess[User].ItemBoxOpen = false;
                                                UsingProcess[User].InitItem = true;
                                                UsingProcess[User].UpdateItem = true;
                                                Call.PostMessage(UsingProcess[User].hWnd, "<<讀取物品資料失敗>>");
                                                Call.SetItemBox(UsingProcess[User].hWnd, true);
                                                if (Call.Check_Display_ItemBox(UsingProcess[User].hWnd) == "隱藏")
                                                    Call.DisplayItemBox(UsingProcess[User].hWnd, true);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (!UsingProcess[User].InitAdjutant || !UsingProcess[User].UpdateAdjutant)
                                {
                                    #region 打開副官欄
                                    if (Call.Check_Display_AdjutantInfo(UsingProcess[User].hWnd) != "隱藏")
                                    {
                                        Call.DisplayAdjutantInfo(UsingProcess[User].hWnd, false);
                                        Call.SetInfoBox(UsingProcess[User].hWnd, false);
                                    }
                                    else
                                    {
                                        if (!UsingProcess[User].AdjutantInfoBoxOpen)
                                        {
                                            Call.InfoButton(UsingProcess[User].hWnd, UsingProcess[User].ID, "副官情報");
                                            UsingProcess[User].AdjutantInfoBoxOpen = true;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            break;
                        default:
                            Call.Sound(UsingProcess[User].hWnd, true);
                            break;
                    }
                    #endregion

                    #region 料理清單
                    if (CuisineList.Items.Count > 0)
                    {
                        if (CuisineList.Items.Count != UsingProcess[User]._Item.CuisineNum)//種類數量錯誤 清除料理list
                            CuisineList.Items.Clear();
                        else
                        {
                            UsingProcess[User].Save_CuisineName = null;
                            for (int i = 0; i < CuisineList.Items.Count; i++)
                            {
                                int CuisineNo = UsingProcess[User]._Item._CuisineInfo[i].ID % 1505000;
                                if (CuisineList.Items[i].Text != UsingProcess[User]._Item._CuisineInfo[i].Name)
                                    CuisineList.Items.Clear();
                                else if (CuisineList.Items[i].SubItems[1].Text != UsingProcess[User]._Item._CuisineInfo[i].Num.ToString())
                                    CuisineList.Items[i].SubItems[1].Text = UsingProcess[User]._Item._CuisineInfo[i].Num.ToString();
                                else
                                {
                                    //記憶體 記錄已勾選使用的料理
                                    if (CuisineList.Items[i].Checked)
                                    {
                                        if (!UsingProcess[User]._Item.CheckedCuisine[CuisineNo])
                                            cuisinelist_change = true;
                                        UsingProcess[User]._Item.CheckedCuisine[CuisineNo] = true;
                                        UsingProcess[User].Save_CuisineName += UsingProcess[User]._Item._CuisineInfo[i].Name + ",";
                                    }
                                    else
                                    {
                                        if (UsingProcess[User]._Item.CheckedCuisine[CuisineNo])
                                            cuisinelist_change = true;
                                        UsingProcess[User]._Item.CheckedCuisine[CuisineNo] = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (CuisineList.Items.Count != UsingProcess[User]._Item.CuisineNum)
                    {
                        for (int i = 0; i < UsingProcess[User]._Item.CuisineNum; i++)
                        {
                            CuisineList.Items.Add(UsingProcess[User]._Item._CuisineInfo[i].Name);
                            CuisineList.Items[i].SubItems.Add(UsingProcess[User]._Item._CuisineInfo[i].Num.ToString());

                            int CuisineNo = UsingProcess[User]._Item._CuisineInfo[i].ID % 1505000;
                            if (UsingProcess[User]._Item.CheckedCuisine[CuisineNo] || (UsingProcess[User].Save_CuisineName != null && UsingProcess[User].Save_CuisineName.Contains(UsingProcess[User]._Item._CuisineInfo[i].Name)))
                                CuisineList.Items[i].Checked = true;
                        }
                    }
                    #endregion

                    #region 技能清單
                    int CanUse_SkillNum = 0, CanUse_Navalbattle_SkillNum = 0;
                    for (int i = 0; i < UsingProcess[User].SkillNum; i++)
                    {
                        if (UsingProcess[User]._SkillInfo[i].Active)
                        {
                            if (!UsingProcess[User]._SkillInfo[i].Navalbattle)
                                ++CanUse_SkillNum;
                            else
                                ++CanUse_Navalbattle_SkillNum;
                        }
                    }

                    if (SkillList.Items.Count > 0)
                    {
                        if (SkillList.Items.Count != CanUse_SkillNum)//清除list
                            SkillList.Items.Clear();
                        else
                        {
                            UsingProcess[User].Save_SkillName = null;
                            for (int i = 0; i < SkillList.Items.Count; i++)
                            {
                                if (SkillList.Items[i].Checked)
                                {
                                    if (!UsingProcess[User]._SkillInfo[Convert.ToInt32(SkillList.Items[i].SubItems[2].Text)].CheckUse)
                                        skilllist_change = true;
                                    UsingProcess[User]._SkillInfo[Convert.ToInt32(SkillList.Items[i].SubItems[2].Text)].CheckUse = true;
                                    UsingProcess[User].Save_SkillName += UsingProcess[User]._SkillInfo[Convert.ToInt32(SkillList.Items[i].SubItems[2].Text)].Name + ",";
                                }
                                else
                                {
                                    if (UsingProcess[User]._SkillInfo[Convert.ToInt32(SkillList.Items[i].SubItems[2].Text)].CheckUse)
                                        skilllist_change = true;
                                    UsingProcess[User]._SkillInfo[Convert.ToInt32(SkillList.Items[i].SubItems[2].Text)].CheckUse = false;
                                }
                            }
                        }
                    }
                    else if (SkillList.Items.Count != CanUse_SkillNum)
                    {
                        int index = 0;
                        for (int i = 0; i < UsingProcess[User].SkillNum; i++)
                        {
                            if (UsingProcess[User]._SkillInfo[i].Active && !UsingProcess[User]._SkillInfo[i].Navalbattle)
                            {
                                SkillList.Items.Add(UsingProcess[User]._SkillInfo[i].Name);

                                SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].Rank.ToString());//技能等級
                                SkillList.Items[index].SubItems.Add(i.ToString());//技能序號(非技能ID)
                                //SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].ID.ToString());//技能ID
                                //SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].Cost.ToString());//技能所需行動力
                                if (UsingProcess[User]._SkillInfo[i].CheckUse || (UsingProcess[User].Save_SkillName != null && UsingProcess[User].Save_SkillName.Contains(UsingProcess[User]._SkillInfo[i].Name)))
                                    SkillList.Items[index].Checked = true;
                                ++index;
                            }
                        }
                    }

                    if (Navalbattle_SkillList.Items.Count > 0)
                    {
                        if (Navalbattle_SkillList.Items.Count != CanUse_Navalbattle_SkillNum)//清除list
                            Navalbattle_SkillList.Items.Clear();
                        else
                        {
                            for (int i = 0; i < Navalbattle_SkillList.Items.Count; i++)
                            {
                                if (Navalbattle_SkillList.Items[i].Checked)
                                {
                                    UsingProcess[User]._SkillInfo[Convert.ToInt32(Navalbattle_SkillList.Items[i].SubItems[2].Text)].CheckUse = true;
                                    UsingProcess[User].Save_SkillName += UsingProcess[User]._SkillInfo[Convert.ToInt32(Navalbattle_SkillList.Items[i].SubItems[2].Text)].Name + ",";
                                }
                                else
                                    UsingProcess[User]._SkillInfo[Convert.ToInt32(Navalbattle_SkillList.Items[i].SubItems[2].Text)].CheckUse = false;
                            }
                        }
                    }
                    else if (Navalbattle_SkillList.Items.Count != CanUse_Navalbattle_SkillNum)
                    {
                        int index = 0;
                        for (int i = 0; i < UsingProcess[User].SkillNum; i++)
                        {
                            if (UsingProcess[User]._SkillInfo[i].Active && UsingProcess[User]._SkillInfo[i].Navalbattle)
                            {
                                Navalbattle_SkillList.Items.Add(UsingProcess[User]._SkillInfo[i].Name);

                                Navalbattle_SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].Rank.ToString());//技能等級
                                Navalbattle_SkillList.Items[index].SubItems.Add(i.ToString());//技能序號(非技能ID)
                                //Navalbattle_SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].ID.ToString());//技能ID
                                //Navalbattle_SkillList.Items[index].SubItems.Add(UsingProcess[User]._SkillInfo[i].Cost.ToString());//技能所需行動力
                                if (UsingProcess[User]._SkillInfo[i].CheckUse || (UsingProcess[User].Save_SkillName != null && UsingProcess[User].Save_SkillName.Contains(UsingProcess[User]._SkillInfo[i].Name)))
                                    Navalbattle_SkillList.Items[index].Checked = true;
                                ++index;
                            }
                        }
                    }
                    #endregion

                    #region 副官資料
                    if (UsingProcess[User].InitAdjutant && UsingProcess[User].UpdateAdjutant)
                    {
                        ListView _listView = new ListView();
                        GroupBox _gropBox = new GroupBox();

                        int AdjutantChangeDay = 10;//更換天數
                        if (!string.IsNullOrWhiteSpace(AdjutantChange.Text) && Convert.ToInt32(AdjutantChange.Text) > 0)
                            AdjutantChangeDay = Convert.ToInt32(AdjutantChange.Text);

                        for (int i = 0; i < 2; i++)
                        {
                            UsingProcess[User]._Adjutant[i].CheckChange = false;
                            if (UsingProcess[User]._Adjutant[i].ID == -1)
                            {
                                UsingProcess[User]._Adjutant[i].As = -1;
                                continue;
                            }

                            #region 初始化LostView & GropBpx
                            switch (i)
                            {
                                case 0:
                                    _listView = AdjutantList1;
                                    _gropBox = _gropBox1;
                                    break;
                                case 1:
                                    _listView = AdjutantList2;
                                    _gropBox = _gropBox2;
                                    break;
                            }
                            #endregion

                            #region 擔任間隔
                            if (UsingProcess[User].MaritimeDays > UsingProcess[User].TempMaritimeDays)
                            {
                                ++UsingProcess[User]._Adjutant[i].ChangeAsTime;
                                ++UsingProcess[User]._Adjutant[i].TotalAsDay;
                                ++UsingProcess[User]._Adjutant[i].DoneAsDay[UsingProcess[User]._Adjutant[i].As];

                                if (!string.IsNullOrWhiteSpace(_listView.Items[UsingProcess[User]._Adjutant[i].As].SubItems[3].Text))
                                    if (Convert.ToInt32(_listView.Items[UsingProcess[User]._Adjutant[i].As].SubItems[3].Text) > 0)
                                        _listView.Items[UsingProcess[User]._Adjutant[i].As].SubItems[3].Text = (Convert.ToInt32(_listView.Items[UsingProcess[User]._Adjutant[i].As].SubItems[3].Text) - 1).ToString();
                            }
                            #endregion

                            #region 副官名稱、等級
                            if (UsingProcess[User]._Adjutant[i].Name != null)
                                if (_gropBox.Text != UsingProcess[User]._Adjutant[i].Name + " 冒：" + UsingProcess[User]._Adjutant[i].LV[0] + " 商：" + UsingProcess[User]._Adjutant[i].LV[1] + " 戰：" + UsingProcess[User]._Adjutant[i].LV[2])
                                    _gropBox.Text = UsingProcess[User]._Adjutant[i].Name + " 冒：" + UsingProcess[User]._Adjutant[i].LV[0] + " 商：" + UsingProcess[User]._Adjutant[i].LV[1] + " 戰：" + UsingProcess[User]._Adjutant[i].LV[2];
                            #endregion

                            for (int Post = 0; Post < UsingProcess[User]._Adjutant[i].AsDay.Length; Post++)
                            {
                                #region 副官特性
                                if (UsingProcess[User]._Adjutant[i].Property[Post] == 255)
                                {
                                    if (_listView.Items[Post].SubItems[1].Text != "??")
                                        _listView.Items[Post].SubItems[1].Text = "??";
                                }
                                else if (UsingProcess[User]._Adjutant[i].Property[Post] > 0)
                                {
                                    if (_listView.Items[Post].SubItems[1].Text != UsingProcess[User]._Adjutant[i].Property[Post].ToString())
                                        _listView.Items[Post].SubItems[1].Text = UsingProcess[User]._Adjutant[i].Property[Post].ToString();
                                }

                                if (_listView.Items[Post].SubItems[2].Text != UsingProcess[User]._Adjutant[i].DoneAsDay[Post].ToString())
                                    _listView.Items[Post].SubItems[2].Text = UsingProcess[User]._Adjutant[i].DoneAsDay[Post].ToString();

                                if (!UsingProcess[User].Exchange_Position)
                                {
                                    //記錄擔任天數
                                    if (!string.IsNullOrWhiteSpace(_listView.Items[Post].SubItems[3].Text))
                                        UsingProcess[User]._Adjutant[i].AsDay[Post] = Convert.ToInt32(_listView.Items[Post].SubItems[3].Text);
                                    else
                                        UsingProcess[User]._Adjutant[i].AsDay[Post] = 0;
                                }
                                else
                                {
                                    //交換位置時，檢查擔任天數
                                    if (UsingProcess[User]._Adjutant[i].AsDay[Post] != 0)
                                        _listView.Items[Post].SubItems[3].Text = UsingProcess[User]._Adjutant[i].AsDay[Post].ToString();
                                    else
                                        _listView.Items[Post].SubItems[3].Text = "";

                                }
                                #endregion

                                #region 擔任職位(顏色)
                                if (Post == UsingProcess[User]._Adjutant[i].As)
                                {
                                    if (_listView.Items[Post].BackColor != Color.GreenYellow)
                                        _listView.Items[Post].BackColor = Color.GreenYellow;
                                }
                                else
                                    if (_listView.Items[Post].BackColor != Color.White)
                                        _listView.Items[Post].BackColor = Color.White;
                                #endregion

                                if (UsingProcess[User]._Adjutant[i].ChangeAsTime >= AdjutantChangeDay)
                                {
                                    if (UsingProcess[User]._Adjutant[i].ChangeAs == 5)
                                        UsingProcess[User]._Adjutant[i].ChangeAs = 0;

                                    if (UsingProcess[User]._Adjutant[i].AsDay[Post] > 0 && UsingProcess[User]._Adjutant[i].As != Post)
                                    {
                                        //擔任職位相同 或者 另一副官擔任職位相同
                                        if (UsingProcess[User]._Adjutant[i].ChangeAs > Post || (UsingProcess[User]._Adjutant[~i + 2].CheckChange && UsingProcess[User]._Adjutant[~i + 2].ChangeAs == Post))
                                            continue;

                                        UsingProcess[User]._Adjutant[i].ChangeAs = Post;
                                        UsingProcess[User]._Adjutant[i].CheckChange = true;
                                        UsingProcess[User]._Adjutant[i].ChangeAsTime = 0;
                                    }
                                }
                            }

                        }
                        //變更副官擔任
                        Call.ChangeAdjutant(UsingProcess[User].hWnd, UsingProcess[User]._Adjutant);

                        if (UsingProcess[User].TempMaritimeDays != UsingProcess[User].MaritimeDays && UsingProcess[User].MaritimeDays != -1)
                            UsingProcess[User].TempMaritimeDays = UsingProcess[User].MaritimeDays;

                        UsingProcess[User].Exchange_Position = false;
                    }
                    #endregion

                    if (cuisinelist_change || skilllist_change)
                    {
                        using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath + "\\set\\", Save_FileName)))
                        {
                            Console.WriteLine("寫入檔案");
                            if (UsingProcess[User].Save_CuisineName != null && UsingProcess[User].Save_CuisineName.Length > 0)
                            {
                                UsingProcess[User].Save_CuisineName = UsingProcess[User].Save_CuisineName.Substring(0, UsingProcess[User].Save_CuisineName.Length - 1);
                                myCINI.setKeyValue("已勾選", "料理", UsingProcess[User].Save_CuisineName);
                            }

                            if (UsingProcess[User].Save_SkillName != null && UsingProcess[User].Save_SkillName.Length > 0)
                            {
                                UsingProcess[User].Save_SkillName = UsingProcess[User].Save_SkillName.Substring(0, UsingProcess[User].Save_SkillName.Length - 1);
                                myCINI.setKeyValue("已勾選", "技能", UsingProcess[User].Save_SkillName);
                            }
                        }
                    }
                }

                //在海上 
                if (UsingProcess[User].Place == 0x4)
                {
                    UsingProcess[User].MaritimeDays = Call.GetMaritimeDays(UsingProcess[User].hWnd);
                    //00-0F晴天 10-1F晴天 20-2F下雨 30-3F下雪 40-4F暴风 50-5F暴雪
                    int WeatherStatus = Call.GetWeatherStatus(UsingProcess[User].hWnd);

                    if (Call.GetNavalbattle(UsingProcess[User].hWnd))
                    {
                        //戰鬥
                        if (!UsingProcess[User].Navalbattle)
                        {
                            UsingProcess[User].UpdateItem = false;
                            //Console.WriteLine("敵方提督：『0x" + Convert.ToString(Call.GetEnemyAdmiralID(UsingProcess[User].hWnd), 16) + "』");
                            UsingProcess[User].Navalbattle = true;
                        }

                        if (自動停戰ToolStripMenuItem.Checked)
                        {
                            if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                            {
                                #region 自動停戰

                                string itemname = "";
                                if (Call.GetEnemyAdmiralID(UsingProcess[User].hWnd) >= 0x1800000)
                                {
                                    if (UsingProcess[User].SBItemCount >= 5)
                                        itemname = "獻給地方海盜的上納品";
                                    else
                                        itemname = "停戰協定書";
                                }
                                else
                                    itemname = "高級上納品（大型船用）";

                                ++UsingProcess[User].SBCount;
                                if (UsingProcess[User].SBCount > 2)
                                {
                                    if (itemname == "停戰協定書")
                                        ++UsingProcess[User].SBItemCount;

                                    for (int i = 0; i < UsingProcess[User]._Item.SeaItemNum; i++)
                                    {
                                        if (UsingProcess[User]._Item._SeaItemInfo[i].Name == itemname && UsingProcess[User]._Item._SeaItemInfo[i].Num != 0)
                                        {
                                            Call.UseItem(UsingProcess[User].hWnd, UsingProcess[User]._Item._SeaItemInfo[i].Code);
                                            UsingProcess[User].SBCount = 0;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        //非戰鬥
                        UsingProcess[User].Navalbattle = false;
                        UsingProcess[User].SBCount = 0;
                        UsingProcess[User].SBItemCount = 0;
                    }

                    #region 使用技能
                    for (int skillIndex = 0; skillIndex < UsingProcess[User].SkillNum; skillIndex++)
                    {
                        //檢查是否使用中
                        bool Useskill = true;

                        //行動力低於Cost || 無勾選 || 肉搏戰
                        if (UsingProcess[User].Power < UsingProcess[User]._SkillInfo[skillIndex].Cost || !UsingProcess[User]._SkillInfo[skillIndex].CheckUse || Call.GetFightID(UsingProcess[User].hWnd))
                            continue;

                        if (UsingProcess[User].Navalbattle)
                        {
                            if (自動停戰ToolStripMenuItem.Checked)
                                break;

                            switch (UsingProcess[User]._SkillInfo[skillIndex].Name)
                            {
                                case "救助":
                                    Call.PostMessage(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].Name + "尚未加入判斷式");
                                    Useskill = false;
                                    break;
                                case "修理":
                                    Call.PostMessage(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].Name + "尚未加入判斷式");
                                    Useskill = false;
                                    break;
                                case "外科醫術":
                                    Call.PostMessage(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].Name + "尚未加入判斷式");
                                    Useskill = false;
                                    break;
                                case "佈設水雷":
                                    Call.PostMessage(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].Name + "尚未加入判斷式");
                                    Useskill = false;
                                    break;
                                default:
                                    if (!UsingProcess[User]._SkillInfo[skillIndex].Navalbattle)
                                        Useskill = false;
                                    break;
                            }
                        }
                        else
                        {
                            switch (UsingProcess[User]._SkillInfo[skillIndex].Name)
                            {
                                case "操帆":
                                    //自動操帆 && 跟隨 && 暴風
                                    if (Call.GetAutoSailState(UsingProcess[User].hWnd) || Call.GetFollowStatus(UsingProcess[User].hWnd) || Call.GetBadWeather(Form1.UsingProcess[User].hWnd))
                                        Useskill = false;
                                    break;
                                case "運用":
                                    //物資>0
                                    if ((UsingProcess[User].Materials[0] == 0 && UsingProcess[User].Materials[1] == 0))
                                        Useskill = false;
                                    break;
                                case "調度":
                                    //00-0F晴天 10-1F晴天 20-2F下雨 30-3F下雪 40-4F暴风 50-5F暴雪
                                    if (WeatherStatus < 0x20 || (WeatherStatus > 0x2F && WeatherStatus < 0x40) || WeatherStatus > 0x4F)
                                        Useskill = false;
                                    break;
                                case "救助":
                                    if (!Call.GetBoatException3(UsingProcess[User].hWnd))
                                        Useskill = false;
                                    break;
                                case "修理":
                                    Call.PostMessage(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].Name + "尚未加入判斷式");
                                    Useskill = false;
                                    break;
                                case "划船":
                                    break;
                                default:
                                    if (UsingProcess[User]._SkillInfo[skillIndex].Navalbattle)
                                        Useskill = false;
                                    break;
                            }
                        }

                        //有技能圖
                        if (!UsingProcess[User]._SkillInfo[skillIndex].NoSkillImage)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (UsingProcess[User].UsingSkillNum == 3 || UsingProcess[User]._SkillInfo[skillIndex].ID == UsingProcess[User].UsingSkill[i])
                                {
                                    Useskill = false;
                                    break;
                                }
                            }
                        }

                        if (Useskill)
                        {
                            if (UsingProcess[User].UseSkillCount < 1)
                                ++UsingProcess[User].UseSkillCount;
                            else
                            {
                                if (UsingProcess[User]._SkillInfo[skillIndex].Name == "操帆" && UsingProcess[User].UseSkillCount < 5)
                                    ++UsingProcess[User].UseSkillCount;
                                else
                                {
                                    Call.UseSkill(UsingProcess[User].hWnd, UsingProcess[User]._SkillInfo[skillIndex].ID);
                                    UsingProcess[User].UseSkillCount = 0;
                                }
                            }
                            break;
                        }
                    }
                    #endregion

                    if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                    {
                        #region 海上災難
                        if (自動消災ToolStripMenuItem.Checked)
                        {
                            ++UsingProcess[User].ExceptionCount;
                            Call.GetPartyException(UsingProcess[User].hWnd, UsingProcess[User].BoatException);

                            if (UsingProcess[User].ExceptionCount > 3)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (!Call.GetPartyStatus(UsingProcess[User].hWnd) && i > 0)
                                        break;
                                    for (int except = 0; except < 32; except++)
                                    {
                                        if (UsingProcess[User].BoatException[i][except] && !Call.GetFightID(UsingProcess[User].hWnd))
                                        {
                                            int skillId = 0;
                                            string itemName = Call.EliminateStatus(UsingProcess[User].hWnd, except, ref skillId);

                                            if (skillId != 0)
                                            {
                                                for (int skill_index = 0; skill_index < UsingProcess[User].SkillNum; skill_index++)
                                                {
                                                    if (UsingProcess[User]._SkillInfo[skill_index].ID == skillId)
                                                    {
                                                        Call.UseSkill(UsingProcess[User].hWnd, skillId);
                                                        UsingProcess[User].ExceptionCount = 0;
                                                        //Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + "使用技能" + skillId);
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (UsingProcess[User].PartyID[i] == UsingProcess[User].ID || !Call.GetPartyStatus(UsingProcess[User].hWnd))
                                                {
                                                    if (itemName == "料理")
                                                    {
                                                        UsingProcess[User].UseCuisineCount = 5;
                                                        UsingProcess[User].ExceptionCount = 0;
                                                    }
                                                    else
                                                    {
                                                        if (itemName != null)
                                                        {
                                                            for (int j = 0; j < UsingProcess[User]._Item.SeaItemNum; j++)
                                                            {
                                                                if (itemName == UsingProcess[User]._Item._SeaItemInfo[j].Name && UsingProcess[User]._Item._SeaItemInfo[j].Num > 0)
                                                                {
                                                                    Call.UseItem(UsingProcess[User].hWnd, UsingProcess[User]._Item._SeaItemInfo[j].Code);
                                                                    Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + " 使用物品 " + UsingProcess[User]._Item._SeaItemInfo[j].Name);
                                                                    --UsingProcess[User]._Item._SeaItemInfo[j].Num;
                                                                    UsingProcess[User].ExceptionCount = 0;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    #region 急加速
                    if (自動急加速ToolStripMenuItem.Checked && !Call.GetFollowStatus(UsingProcess[User].hWnd))
                    {
                        double Degree = Math.Sqrt(Math.Pow(Math.Acos(UsingProcess[User].Cos) * (180D / Math.PI) - Math.Acos(UsingProcess[User].Old_Cos) * (180D / Math.PI), 2));

                        UsingProcess[User].IsHasten = false;
                        for (int i = 0; i < 3; i++)
                            if (UsingProcess[User].UsingSkill[i] == 2001)
                                UsingProcess[User].IsHasten = true;

                        if (Call.Destinations(UsingProcess[User].hWnd, ref UsingProcess[User]._Destinations) && UsingProcess[User].IsHasten)
                        {
                            Call.StopSkill(UsingProcess[User].hWnd, 2001);
                            UsingProcess[User].HastenCount = 0;
                        }
                        else if (!UsingProcess[User].IsHasten)
                        {
                            //角度小於0.003 && 無轉向 && 有開帆 && 無跟隨 && 非暴風 && 行動力>=30
                            if (Degree <= 0.003 && UsingProcess[User].TurnCount == 0 && Call.GetSailStatus(UsingProcess[User].hWnd) > 0 && !Call.GetFollowStatus(UsingProcess[User].hWnd) && !Call.GetBadWeather(Form1.UsingProcess[User].hWnd) && UsingProcess[User].Power >= 30 && UsingProcess[User].UsingSkillNum != 3)
                            {
                                ++UsingProcess[User].HastenCount;
                                if (UsingProcess[User].HastenCount > 3)
                                {
                                    Call.UseSkill(UsingProcess[User].hWnd, 2001);
                                    UsingProcess[User].HastenCount = 0;
                                }
                            }
                            else
                                UsingProcess[User].HastenCount = 0;
                        }
                    }
                    #endregion
                }
                else
                {
                    UsingProcess[User].UseSkillCount = 0;
                    UsingProcess[User].HastenCount = 0;
                }


                if (UsingProcess[User].SupplyCount < 10)
                    ++UsingProcess[User].SupplyCount;

                if (Form1.UsingProcess[User].User_Type == "猜猜看" || Form1.UsingProcess[User].User_Type == "TEST")
                {
                    #region 自動補給
                    if (自動補給ToolStripMenuItem.Checked)
                    {
                        if (UsingProcess[User].Place >= 0x1C && UsingProcess[User].Place < 0x30)//碼頭
                        {
                            if (UsingProcess[User].SupplyCount > 5)
                            {
                                Call.Supply(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd), new int[4], false);
                                UsingProcess[User].SupplyCount = -10;
                            }
                        }
                    }
                    #endregion
                }

                #region 使用料理
                int[] Set = new int[2] { 101, 101 };
                if (Cuisine.Text != "")
                    Set[0] = UsingProcess[User].MaxPower * Convert.ToInt32(Cuisine.Text) / 100;
                if (Fatigue.Text != "")
                    Set[1] = Convert.ToInt32(Fatigue.Text);
                if (UsingProcess[User].Power < Set[0] || UsingProcess[User].Fatigue >= Set[1] || UsingProcess[User].UseCuisineCount > 4)
                {
                    //!港口&&!肉搏戰
                    if (UsingProcess[User].Place < 0x1C && !Call.GetFightID(UsingProcess[User].hWnd))
                    {
                        ++UsingProcess[User].UseCuisineCount;
                        if (UsingProcess[User].UseCuisineCount > 4)
                        {
                            for (int i = 0; i < UsingProcess[User]._Item.CuisineNum; i++)
                            {
                                if (CuisineList.Items[i].Checked)
                                {
                                    if (UsingProcess[User]._Item._CuisineInfo[i].Num == 0)
                                        continue;
                                    Call.UseItem(UsingProcess[User].hWnd, UsingProcess[User]._Item._CuisineInfo[i].Code);
                                    Console.WriteLine("﹝" + UsingProcess[User].ServerName + "﹞" + UsingProcess[User].Name + " 使用料理 " + UsingProcess[User]._Item._CuisineInfo[i].Name);
                                    --UsingProcess[User]._Item._CuisineInfo[i].Num;
                                    UsingProcess[User].UseCuisineCount = 0;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        UsingProcess[User].UseCuisineCount = 0;
                }
                #endregion

                timer.Start();
            }
            else
            {
                if (UsingProcess[User].hWnd == IntPtr.Zero)
                {
                    #region 遊戲視窗不存在
                    if (comboBox.BackColor == Color.Red)
                        comboBox.BackColor = Color.White;
                    timer.Stop();
                    comboBox.Text = " ";
                    water.Text = " ";
                    grain.Text = " ";
                    stuff.Text = " ";
                    ammunition.Text = " ";
                    fatigueBar.Value = 0;
                    powerBar.Value = 0;

                    UsingProcess[User] = new uInfo();
                    CuisineList.Items.Clear();
                    SkillList.Items.Clear();
                    #endregion
                }
                else if (!Call.GetConnectState(UsingProcess[User].hWnd))
                {
                    #region 斷線
                    if (comboBox.BackColor != Color.Red)
                        comboBox.BackColor = Color.Red;
                    #endregion
                }

                if (Form1.UsingProcess[User].User_Type == "猜猜看")
                {
                    if (自動登錄ToolStripMenuItem.Checked && UsingProcess[User].hWnd != IntPtr.Zero)
                    {
                        if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                        {
                            if (System.DateTime.Now.Hour < 8 || System.DateTime.Now.Hour > 12)
                                Call.Login(UsingProcess[User].hWnd);
                        }
                        else
                            Call.Login(UsingProcess[User].hWnd);
                    }
                }
            }
        }

        private void timer0_Tick(object sender, EventArgs e)
        {
            int User = 0;
            timer(User);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int User = 1;
            timer(User);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int User = 2;
            timer(User);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            int User = 3;
            timer(User);
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            int User = 4;
            timer(User);
        }

        private void thread_state()
        {
            for (int User = 0; User < 5; User++)
            {
                if (Thread.CurrentThread.Name == UsingProcess[User].Name && UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    Call.GetTargetInfo(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User].target);
                }
            }
        }

        private void thread_getTarget()
        {
            for (int User = 0; User < 5; User++)
            {
                if (Thread.CurrentThread.Name == UsingProcess[User].Name && UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    Form1.UsingProcess[User].targetMutex.WaitOne();
                    Call.GetTargetInfo(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User].target);
                    Form1.UsingProcess[User].targetMutex.ReleaseMutex();
                }
            }
        }
        #endregion

        #region 技能列
        private void exbutton_Click(object sender, EventArgs e)
        {
            if (技能列panel.Visible)
            {
                exbutton.Text = "→";
                技能列panel.Visible = false;
                this.Width -= 165;
            }
            else
            {
                exbutton.Text = "←";
                技能列panel.Visible = true;
                this.Width += 165;
            }
        }

        private void ShortcutButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Image != null)
            {
                using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath, "GvoHelper.ini")))
                {
                    int ShortcutSkillId = Convert.ToInt32(myCINI.getKeyValue("Shortcuts", ((Button)sender).Name));
                    for (int User = 0; User < 5; User++)
                        if (UsingProcess[User].hWnd != IntPtr.Zero)
                            Call.UseSkill(UsingProcess[User].hWnd, ShortcutSkillId);
                }
            }
        }
        
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            picTemp = (PictureBox)sender;
            if (picTemp.Image != null)
                picTemp.DoDragDrop(picTemp.Image, DragDropEffects.All);
        }

        private void skillbutton_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void skillbutton_DragDrop(object sender, DragEventArgs e)
        {
            if (picTemp.Image != null)
            {
                ((Button)sender).Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath, "GvoHelper.ini")))
                    myCINI.setKeyValue("Shortcuts", ((Button)sender).Name, picTemp.Name.Substring(picTemp.Name.IndexOf("_") + 1));
            }
        }
        #endregion

        #region 中斷技能
        private void stopSkill_Click(object sender, EventArgs e)
        {
            PictureBox s = (PictureBox)sender;

            for (int User = 0; User < 5; User++)
                for (int Skill = 0; Skill < 3; Skill++)
                    if (s == UsingProcess[User].UsingSkillImage[Skill])
                        Call.StopSkill(UsingProcess[User].hWnd, UsingProcess[User].UsingSkill[Skill]);
        }
        #endregion

        private void test(object sender, EventArgs e)
        {
            long endAddr = 0xF00000;
            long baseAddr = 0x000000;
            WinAPI.MEMORY_BASIC_INFORMATION inf = new WinAPI.MEMORY_BASIC_INFORMATION();
            int pid = Call.GetPid(UsingProcess[0].hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            while (WinAPI.VirtualQueryEx((IntPtr)hProcess, (IntPtr)baseAddr, out inf, (uint)Marshal.SizeOf(inf)))
            {
                Console.WriteLine(Convert.ToString((int)inf.BaseAddress, 16) + " " + Convert.ToString(inf.RegionSize, 16) + " " +  inf.Protect);

                baseAddr = (long)inf.BaseAddress + (long)inf.RegionSize;
                if (baseAddr > endAddr)
                    break;
                if ((inf.Protect == WinAPI.MEMMessage.PAGE_READWRITE || inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READWRITE || inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_WRITECOPY) || inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);
                    for (int j = 0; j < buf.Length; j++)
                    {
                        var sByte = (uint)0;
                        var sByte2 = (uint)0;
                        var sByte4 = (uint)0;
                        sByte = (buf[j]);//0-255
                        if (j + 1 < buf.Length)
                        {
                            sByte2 = (uint)(buf[j] | buf[j + 1] << 8);//0-65535
                        }
                        if (j + 3 < buf.Length)
                        {
                            sByte4 = (uint)(buf[j] | buf[j + 1] << 8 | buf[j + 2] << 16 | buf[j + 3] << 24);//0->
                        }

                        if (sByte == 0xE9)
                        {
                            if ((uint)(buf[j + 1] | buf[j + 2] << 8 | buf[j + 3] << 16 | buf[j + 4] << 24) == 0x21B)
                                Console.WriteLine(Convert.ToString((long)inf.BaseAddress + j + 1, 16));
                        }
                        //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x491EE6, BitConverter.GetBytes(0xE9), 1, out ret);
                        //WinAPI.WriteProcessMemory(pInfo.hProcess, (IntPtr)0x491EE7, BitConverter.GetBytes(0x0000021B), 4, out ret);
                    }
                    Console.WriteLine(" ");
                }
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            int User = 0;

            if (Call.GetBoatException3(UsingProcess[User].hWnd))
            {
                //Call.UseSkill(UsingProcess[User].hWnd, 18);
                //Call.Delay(2000);
                //Call.UseSkill(UsingProcess[User].hWnd, 102);
                for (int i = 0; i < UsingProcess[User]._Regular.Length; i++)
                {
                    if (Call.Distance(UsingProcess[User].Coordinate, UsingProcess[User]._Regular[i].Coordinate) < 25)
                    {
                        Call.IntoScene(UsingProcess[User].hWnd, UsingProcess[User]._Regular[i].ID);
                        break;
                    }
                }
                Call.Delay(2000);
            }
            else
            {
                if (UsingProcess[User].Place == 0x4)
                    Call.Turn(UsingProcess[User].hWnd, temp.X, temp.Y);
                else
                {
                    Call.Sailing(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd));
                    Call.Delay(2000);
                }
            }
            //Call.NpcButton(UsingProcess[User].hWnd, TargetID, 0xAD);
            //Console.WriteLine("通訊with 0x" + Convert.ToString(TargetID, 16));

            timer5.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //消黃名
            timer5.Stop();
            int User = 0;

            if (Call.GetBoatException3(UsingProcess[User].hWnd))
            {
                Call.IntoScene(UsingProcess[User].hWnd, 0x40100D0);
                Call.Delay(2000);
            }
            if (UsingProcess[User].Place == 0x4)
            {
                //Call.Turn(UsingProcess[User].hWnd, temp.X, temp.Y);
                if (UsingProcess[User].UsingSkill[0] != 102)
                    Call.UseSkill(UsingProcess[User].hWnd, 102);
            }
            else
            {
                Call.Sailing(UsingProcess[User].hWnd, Call.GetUserId(UsingProcess[User].hWnd));
                Call.Delay(2000);
            }
            timer5.Start();
        }

        private void 通訊Button_Click(object sender, EventArgs e)
        {
            //通訊
            if (!timer5.Enabled)
            {
                //int User = 0;
                //temp.X = UsingProcess[User].Coordinate.X;
                //temp.Y = UsingProcess[User].Coordinate.Y;
                timer5.Start();
            }
            else
                timer5.Stop();
        }

        private void SetASDay_Click(object sender, EventArgs e)
        {
            int User = -1;
            if (((Button)sender).Name.Contains("Set"))
            {
                User = Convert.ToInt32(((Button)sender).Name.Replace("SetASDay", ""));
            }
            else if (((Button)sender).Name.Contains("Clear"))
            {
                User = Convert.ToInt32(((Button)sender).Name.Replace("ClearASDay", ""));
            }

            ListView AdjutantList1, AdjutantList2;
            switch (User)
            {
                case 0:
                    AdjutantList1 = AdjutantList0_1;
                    AdjutantList2 = AdjutantList0_2;
                    AdjutantChange0.Text = "1";
                    break;
                case 1:
                    AdjutantList1 = AdjutantList1_1;
                    AdjutantList2 = AdjutantList1_2;
                    AdjutantChange1.Text = "1";
                    break;
                case 2:
                    AdjutantList1 = AdjutantList2_1;
                    AdjutantList2 = AdjutantList2_2;
                    AdjutantChange2.Text = "1";
                    break;
                case 3:
                    AdjutantList1 = AdjutantList3_1;
                    AdjutantList2 = AdjutantList3_2;
                    AdjutantChange3.Text = "1";
                    break;
                case 4:
                    AdjutantList1 = AdjutantList4_1;
                    AdjutantList2 = AdjutantList4_2;
                    AdjutantChange4.Text = "1";
                    break;
                default:
                    return;
            }

             ListView _listView = new ListView();

             for (int i = 0; i < 2; i++)
             {
                 #region 初始化LostView
                 switch (i)
                 {
                     case 0:
                         _listView = AdjutantList1;
                         break;
                     case 1:
                         _listView = AdjutantList2;
                         break;
                 }
                 #endregion

                 for (int Post = 0; Post < 6; Post++)
                 {
                     if (((Button)sender).Name.Contains("Set"))
                     {
                         if (UsingProcess[User]._Adjutant[i].Property[Post] == 100)
                             _listView.Items[Post].SubItems[3].Text = "";
                         else if (UsingProcess[User]._Adjutant[i].Property[Post] == 255)
                             _listView.Items[Post].SubItems[3].Text = "100";
                         else
                             _listView.Items[Post].SubItems[3].Text = UsingProcess[User]._Adjutant[i].Property[Post].ToString();
                     }
                     else if (((Button)sender).Name.Contains("Clear"))
                     {
                         _listView.Items[Post].SubItems[3].Text = "";
                     }
                 }
             }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Call.TEST(UsingProcess[0].hWnd);
            //Call.SelectFormula(UsingProcess[0].hWnd, 0);
            //Call.GetAllFormula(UsingProcess[User].hWnd);
            //Call.Login(UsingProcess[0].hWnd);
            //if (Call.GetFormulaIndex(UsingProcess[0].hWnd, "製粉法") != -1)
            Call.SelectFormula(UsingProcess[0].hWnd, 2);
            //Call.test(UsingProcess[0].hWnd, GVOCall.City[UsingProcess[0].CityNo].TraderID);
            //if (Call.GetItemsInfo(UsingProcess[0].hWnd, ref UsingProcess[0]._Item._HaveItemInfo))
            //{
            //for (int i = 0; i < UsingProcess[0]._Item._HaveItemInfo.Length; i++)
            //{
            //if (UsingProcess[0]._Item._HaveItemInfo[i].ID != 0)
            //Console.WriteLine(UsingProcess[0]._Item._HaveItemInfo[i].ID + "=" + UsingProcess[0]._Item._HaveItemInfo[i].Name);
            //}
            //Call.Equip(Form1.UsingProcess[0].hWnd, Form1.UsingProcess[0]._Item._HaveItemInfo);
            //Call.Equip(Form1.UsingProcess[0].hWnd, Form1.UsingProcess[0]._Item._HaveItemInfo);
            //Call.DropItem(Form1.UsingProcess[0].hWnd, Form1.UsingProcess[0]._Item._HaveItemInfo);
            //}
            //Call.DropItem(UsingProcess[0].hWnd, UsingProcess[0]._Item._HaveItemInfo, "砒石之毒、解毒藥");
            //Call.Equip(UsingProcess[0].hWnd, UsingProcess[0]._Item._HaveItemInfo);
            //for (int NPC_Index = 0; Form1.UsingProcess[User].CityNo != -1 && NPC_Index < GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo.Length; NPC_Index++)
            //{
            //    if (GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID != 0)
            //        Console.WriteLine(NPC_Index + " " + GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name + " " + GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID + " " + GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Coordinate);
            //}
        }

        private string get_game_ver()
        {
            InitFilePath();

            string path = FilePath + @"\" + FileName;
            //Console.WriteLine(System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion.ToString().Replace(", ", "."));
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion.ToString().Replace(", ", ".");
        }

        private void InitFilePath()
        {
            using (CINI myCINI = new CINI(Path.Combine(Application.StartupPath, "GvoHelper.ini")))
            {
                FilePath = myCINI.getKeyValue("Main", "遊戲路徑");
                FileName = "GVOnline.bin";

                while (!File.Exists(FilePath + @"\" + FileName))
                {
                    folderBrowserDialog1.Description = "請選擇GVOnline.bin所在目錄";
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        myCINI.setKeyValue("Main", "遊戲路徑", folderBrowserDialog1.SelectedPath);
                        FilePath = myCINI.getKeyValue("Main", "遊戲路徑");
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
    }
}
