using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace GvoHelper
{
    class GVOCall
    {
        //00 00 00 00 98 E5 CA 00 00 00 00 00 2E 50 41 56 43 45 78 63 65 70 74 69 6F 6E 40 40 00 00 00 00 98 E5 CA 00 00 00 00 00 2E 3F 41 56 43 43 72 63 33 32 66 6F 72 42 49 40 40 00 00 00 30 7C BC 00
        const int CALL_ECX = 0xE598E8; //一般call用ecx//

        const int PostMessageAddr = 0xE53FD4;//文字訊息//
        //================================================================
        const int BusyAddr = 0xE54330;//程式忙碌//
        const int BusyModeAddr = BusyAddr + 0x14;//1-對話 2-陸戰
        const int SeaCoordinatePtr = BusyModeAddr + 0x4;//海域座標
        const int HasWindowAddr = SeaCoordinatePtr + 0x8;//有視窗出現
        const int TargetTypeAddr = SeaCoordinatePtr + 0x18;//目標型態0-人物 1-場景 2-無目標
        const int TargetIDAddr = TargetTypeAddr + 0x4;//目標ID
        //================================================================
        const int SceneChangeAddr = 0xE54E50;//1-場景切換//
        //================================================================
        const int SetSupplyAddr = 0xE56510;//物資-保存的數量//
        //================================================================
        const int LoginAddrPtr = 0xCE2D9C + 0x200;//登入用???//
        //================================================================
        const int ConnectStateAddr = 0xE5991C;//連線狀態//
        const int SocketAddr = ConnectStateAddr + 0x8;//socket狀態
        //================================================================
        const int ServerNameAddrPtr = ConnectStateAddr + 0x98;//伺服器名字PTR
        const int UserNameAddrPtr = ConnectStateAddr + 0x9C;//選擇角色-角色名字PTR
        const int CheckUserNameAddrPtr = ConnectStateAddr + 0xD0;//角色名字PTR
        const int UserIDAddr = ConnectStateAddr + 0xA8;//角色ID
        const int BoatExceptionAddr = ConnectStateAddr + 0xB8;//海上狀況
        const int AutoSailStateAddr = ConnectStateAddr + 0xBD;//自動操帆狀態
        const int FightStatusAddr = ConnectStateAddr + 0xD8;//(1-攻方 2-守方 3- 4- 5-陸戰攻? 6-陸戰守?)
        //================================================================
        const int FightIDAddr = 0xE59A60;//肉搏戰目標ID//
        const int PowerAddr = FightIDAddr + 0x10;//行動力
        const int MaxPowerAddr = FightIDAddr + 0x14;//MAX行動力
        //================================================================
        const int MoneyAddr = MaxPowerAddr + 0x4;//持有金錢
        const int CrewAddr = MoneyAddr + 0x4;//船員數
        const int FatigueAddr = MoneyAddr + 0x8;//疲勞度
        const int WaterAddr = MoneyAddr + 0xA;//水
        const int GrainAddr = MoneyAddr + 0xC;//糧食
        const int AmmunitionAddr = MoneyAddr + 0xE;//彈藥
        const int StuffAddr = MoneyAddr + 0x10;//資材
        const int Durability = MoneyAddr + 0x12;//耐久度
        const int BoatExceptionAddr2 = MoneyAddr + 0x13;//船舵損壞128 船帆破損1~
        const int BoatExceptionAddr3 = MoneyAddr + 0x14;//0-正常 1~-無法航行
        //================================================================
        const int CoordinateAddr = 0xE59AC0;//座標XZY float 4bye //港口座標 37000 35000//
        const int CosAddr = CoordinateAddr + 0x10;//COS 
        const int SinAddr = CoordinateAddr + 0x18;//SIN
        //================================================================
        const int UserActionAddr = 0xE59D50;//人物動作//
        //================================================================
        const int MaxDurability = 0xE59DEC;//Max耐久度//
        const int MaxCrewAddr = MaxDurability + 0x2;//最大船員數
        const int NecessaryCrewAddr = MaxCrewAddr + 0x2;//必要船員數
        //================================================================
        const int LandFollowAddr = 0xE59E50;//陸地跟隨ID//
        const int SailStateAddr = LandFollowAddr + 0x4;//船帆狀態 0-停船 1 2 3 4-全帆
        const int SeaFollowAddr = LandFollowAddr + 0x70;//海上跟隨ID
        const int MoveAddrPtr = LandFollowAddr + 0xC4;//移動CALL用
        //================================================================
        const int PeopleBase = 0xE59F20;//活動對象Base
        const int RegularBase = PeopleBase + 0x20;//固定對象Base
        //================================================================
        const int PlaceNameAddrPtr = 0xE59F60;//城市海域名PTR//
        //================================================================
        const int PlaceNoAddr = 0xE5A00C;//地圖編號//
        //================================================================
        const int PartyAddrPtr = 0xE5A014;//艦隊PTR
        const int DungeonAdmiralAddr = PartyAddrPtr + 0x68;//探險領隊
        //================================================================
        const int SkillBase = 0xE47510;//技能名稱Base// 
        //================================================================
        const int ItemTypeBase1 = 0xE5A62C;//交易品類型Base1///////////
        const int ItemBase = 0xE5A62C;//道具、裝備、交易品名稱Base
        //================================================================
        const int FormulaBase = 0xE5A664;//配方Base//
        const int SceneBase = 0xE5A760;//場景名稱Base//
        //================================================================
        const int FightAddr = 0xE5AF94;//戰鬥標誌//
        const int FriendlyAdmiralAddr = FightAddr + 0x6C;//友方提督
        const int EnemyAdmiralAddr = FightAddr + 0x70;//敵方提督
        //================================================================
        const int FriendAddr = EnemyAdmiralAddr + 0x10;//陸戰友方ID
        const int EnemyAddr = FriendAddr + 0x3E8;//陸戰敵方ID
        const int LBTargetAddr = FriendAddr + 0x7D8;//陸戰-選定目標
        const int LBShortcutsIAddr = FriendAddr + 0x7E0;//陸戰物品快捷鍵 F5~F8
        const int LBAttackTargetAddr = FriendAddr + 0x8C0;//陸戰-攻擊目標
        const int LBShortcutsSAddr = FriendAddr + 0x8C4;//陸戰技巧快捷鍵 F1~F3

        const int PassPlayCodeAddr = 0xCE6418;//跳過甲板戰&競技場演出//
        //================================================================
        const int SkillBoxAddrPtr = 0xCE7368;//技能欄PTR//
        //================================================================
        const int WeatherAddr = 0xE5CD41;//天氣//
        const int MaritimeDaysAddr = WeatherAddr + 0x3;//航行天數
        //================================================================
        const int ExitDungeonCodeAddr = MaritimeDaysAddr + 0x248;//離開地下城所需Code
        //================================================================
        const int MsgBoxAddr1 = 0xCE9174;//角色身上跳出的訊息//
        const int LogAddr = 0xCE91FC;//Log//
        const int LogWordAddr = 0xCE9378;//Log內容//
        //================================================================
        const int MsgBoxAddr2 = 0xCED4FC;//跳出的訊息框//
        //================================================================
        const int WindowTitleAddrPtr = 0xEC4E80;//視窗標題PTR**
        //================================================================
        const int SkillAddrPtr = 0xEC50D0;//使用中技能數PTR**
        //================================================================
        const int BaseAddrPtr1 = 0xED1638;//基本位址1//
        const int BaseAddrPtr2 = BaseAddrPtr1 + 0x4;//基本位址2
        //滑鼠所指位址PTR
        const int WindowBaseAddrPtr1 = BaseAddrPtr1 + 0x80;//視窗底層欄位址1PTR
        //================================================================
        const int CuisineSPACE = 0xF00000;//SPACE
        const int BookSPACE = CuisineSPACE + 0x10000;//SPACE
        const int SeaItemSPACE = CuisineSPACE + 0x20000;//SPACE
        const int LandItemSPACE = CuisineSPACE + 0x30000;//SPACE
        const int TradeSPACE = CuisineSPACE + 0x40000;//SPACE
        const int TextSPACE = CuisineSPACE + 0x50000;//SPACE
        const int PostSPACE = CuisineSPACE + 0x60000;//SPACE
        const int OtherSPACE = CuisineSPACE + 0x70000;//SPACE

        WinAPI.MEMORY_BASIC_INFORMATION inf = new WinAPI.MEMORY_BASIC_INFORMATION();
        
        public struct CityInfo
        {
            public struct BuyTradeInfo
            {
                public int ID, Price, MaxNum, BuyNum, UseBook;
                public string Name, Type;
            }

            public struct SellTradeInfo
            {
                public int Code, ID, SellPrice, MaxNum, SellNum, Profit;
                public float BuyPrice;
                public string Name;
            }

            public struct TargetInfo
            {
                public int ID;
                public string Name;
                public PointF Coordinate;
                public float cos, sin;
            }

            public int ID;
            public string Name;
            public Point Coordinate;
            public int EnterMode;
            public bool Done;

            public int BuyMenuNum, SellMenuNum;
            public BuyTradeInfo[] _BuyTradeInfo;
            public SellTradeInfo[] _SellTradeInfo;
            public int[] CheckedBuy;
            public bool[] CheckedSell;

            public TargetInfo[] _SceneInfo, _NPCInfo;

            public int Drink, Food;//食物、飲料
            public string DrinkName, FoodName;
            public int[] Meal;
            public string[] MealName;
            public int Mission;//任務
            public string MissionName;
            public int Subject;//學科
        }

        public static IntPtr[] AllProcess;
        public static CityInfo[] City;

        public void SearchAllWindow()
        {
            AllProcess = new IntPtr[64];
            SearchForWindow("Greate Voyages Online Game MainFrame", null);
        }

        private void SearchForWindow(string wndclass, string title)
        {
            WinAPI.SearchData sd = new WinAPI.SearchData { Wndclass = wndclass, Title = title };
            WinAPI.EnumWindows(new WinAPI.EnumWindowsProc(EnumProc), ref sd);
        }

        private bool EnumProc(IntPtr hWnd, ref WinAPI.SearchData data)
        {
            StringBuilder buffer = new StringBuilder(1024);
            WinAPI.GetClassName(hWnd, buffer, buffer.Capacity);
            if (buffer.ToString().StartsWith(data.Wndclass))
            {
                for (int i = 0; hWnd != IntPtr.Zero; i++)
                {
                    if (AllProcess[i] == IntPtr.Zero && AllProcess[i] != hWnd)
                    {
                        AllProcess[i] = hWnd;
                        break;
                    }
                }
            }
            return true;
        }

        //延遲
        public void Delay(int Delaytime)
        {
            int SaveTime = WinAPI.timeGetTime();
            while (WinAPI.timeGetTime() < (SaveTime + Delaytime))
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
        }
        //虛擬碼
        private UIntPtr MakeLParam(uint VirtualKey, string flag)
        {
            string LParam, FirstByte, SecondByte, OtherByte = "0001";
            if (flag == "WM_KEYDOWN")
                FirstByte = "00";
            else
                FirstByte = "C0";
            SecondByte = Convert.ToString(WinAPI.MapVirtualKey(VirtualKey, 0), 16);
            LParam = FirstByte + SecondByte + OtherByte;
            return (UIntPtr)Convert.ToUInt32(LParam, 16);

            //WinAPI.PostMessage(hWnd, WinAPI.WM_KEYDOWN, (IntPtr)Keys.F8, MakeLParam((uint)Keys.F8, "VM_KEYDOWN"));
            //WinAPI.PostMessage(hWnd, WinAPI.WM_KEYUP, (IntPtr)Keys.F8, MakeLParam((uint)Keys.F8, "VM_KEYUP"));
            //WinAPI.PostMessage(hWnd, WinAPI.WM_KEYDOWN, (IntPtr)Keys.F1, MakeLParam((uint)Keys.F1, "VM_KEYDOWN"));
            //WinAPI.PostMessage(hWnd, WinAPI.WM_KEYUP, (IntPtr)Keys.F1, MakeLParam((uint)Keys.F1, "VM_KEYUP"));
        }

        //讀取城市資料
        public void LoadCityInfo(ref CityInfo[] City)
        {
            int n = 0;

            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Coordinate)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)//array城市數量
                    {
                        string line = sr.ReadLine();
                        if (!line.Contains("["))
                            ++n;
                    }
                }
            }
            City = new CityInfo[n];

            n = 0;
            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Coordinate)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (!line.Contains("["))
                        {
                            line = line.Replace(" ", "");
                            City[n].Name = line.Substring(0, line.IndexOf("="));
                            //Console.Write(line.Substring(0, line.IndexOf("=")) + ",");
                            line = line.Substring(line.IndexOf("=") + 1);
                            if (line.Length > 3)
                            {
                                City[n].Coordinate.X = Convert.ToInt32(line.Substring(0, line.IndexOf(",")));
                                City[n].Coordinate.Y = Convert.ToInt32(line.Substring(line.IndexOf(",") + 1));
                                City[n].ID = 0;
                            }
                            //Console.WriteLine(n + " " + City[n].Name + "=" + City[n].Coordinate.X + "," + City[n].Coordinate.Y + "," + City[n].ID);
                            ++n;
                        }
                    }
                }
            }
        }
        //讀取技能資料
        public string LoadSkillInfo(int SkillID)
        {
            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Skill)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string Type = "[unknown]";
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains("["))
                            Type = line;
                        if (line.Contains("=") && line.Substring(0, line.IndexOf("=")) == SkillID.ToString())
                            return Type + line.Substring(line.IndexOf("=") + 1);
                    }
                }
            }
            return null;
        }
        //讀取道具資料
        public string LoadItemInfo(int ItemID)
        {
            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Item)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string Type = "[unknown]";
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains("["))
                            Type = line;
                        if (line.Contains(ItemID.ToString()))
                            return Type + line.Substring(line.IndexOf("=") + 1);
                    }
                }
            }
            return null;
        }
        //讀取道具資料
        public void LoadDropItemInfo(ref string[] Item_Name, ref int[] Item_Num)
        {
            if (System.IO.File.Exists(Path.Combine(Application.StartupPath, "drop.txt")))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(Application.StartupPath, "drop.txt")))
                {
                    int Index = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (!line.Contains("#") && line.Contains(","))
                        {
                            Item_Name[Index] = line.Substring(0, line.IndexOf(","));
                            Item_Num[Index] = Convert.ToInt32(line.Replace(Item_Name[Index], "").Replace(",", "").Replace(" ", ""));
                            ++Index;
                        }
                    }
                }
            }
        }
        //讀取料理資料
        public string LoadCuisineInfo(int CuisineID)
        {
            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Cuisine)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains(CuisineID.ToString()))
                            return line.Substring(line.IndexOf("=") + 1);
                    }
                }
            }
            return null;
        }
        //讀取配方資料
        public string LoadFormulaInfo(int FormulaID)
        {
            using (MemoryStream path = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GvoHelper.Properties.Resources.Formula)))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.Substring(0, line.IndexOf("=")) == FormulaID.ToString())
                            return line.Substring(line.IndexOf("=") + 1);

                    }
                }
            }
            return null;
        }

        //取得視窗hWnd
        public IntPtr GethWnd(string WindowName)
        {
            return WinAPI.FindWindow("Greate Voyages Online Game MainFrame", WindowName);
        }
        //取得視窗ProcessID
        public int GetPid(IntPtr hWnd)
        {
            int pid = 0;
            if (hWnd != IntPtr.Zero)
                WinAPI.GetWindowThreadProcessId(hWnd, ref pid);
            return pid;
        }
        //視窗無回應則關掉
        public void Responding(IntPtr hWnd)
        {
            try
            {
                if (!Process.GetProcessById(GetPid(hWnd)).Responding)
                    Process.GetProcessById(GetPid(hWnd)).Kill();
            }
            catch { }
        }

        //取得連線狀態
        public bool GetConnectState(IntPtr hWnd)
        {
            int ConnectState;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, ConnectStateAddr, out ConnectState, 4, 0);//0-斷線 1-連線
            WinAPI.CloseHandle(hProcess);
            if (ConnectState != 0)
                return true;
            return false;
        }
        //取得伺服器名字
        public string GetServerName(IntPtr hWnd)
        {
            int ServerNameAddr, LenServerName;
            int ret = 0;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, ServerNameAddrPtr, out ServerNameAddr, 4, 0);
            WinAPI.ReadProcessMemory(hProcess, ServerNameAddr - 0xC, out LenServerName, 4, 0);

            if (LenServerName > 0)
            {
                byte[] ServerName = new byte[LenServerName *= 2];
                WinAPI.ReadProcessMemory(hProcess, ServerNameAddr, ServerName, LenServerName, ref ret);
                WinAPI.CloseHandle(hProcess);
                return System.Text.Encoding.Unicode.GetString(ServerName);
            }
            WinAPI.CloseHandle(hProcess);
            return null;
        }
        //取得使用者名字
        public string GetUserName(IntPtr hWnd)
        {
            int UserNameAddr, LenUserName;
            int ret = 0;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, UserNameAddrPtr, out UserNameAddr, 4, 0);
            WinAPI.ReadProcessMemory(hProcess, UserNameAddr - 0xC, out LenUserName, 4, 0);

            if (LenUserName > 0)
            {
                byte[] UserName = new byte[LenUserName *= 2];
                WinAPI.ReadProcessMemory(hProcess, UserNameAddr, UserName, LenUserName, ref ret);
                WinAPI.CloseHandle(hProcess);
                return System.Text.Encoding.Unicode.GetString(UserName);
            }
            WinAPI.CloseHandle(hProcess);
            return null;
        }
        //取得使用者ID
        public int GetUserId(IntPtr hWnd)
        {
            int UserID;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, UserIDAddr, out UserID, 4, 0);//角色ID
            WinAPI.CloseHandle(hProcess);
            return UserID;
        }
        //檢查使用者名字
        public bool CheckUserName(IntPtr hWnd, int UserID, string UserName)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int LenUserName, UserNameAddr;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, CheckUserNameAddrPtr, out UserNameAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, UserNameAddr - 0xC, out LenUserName, 4, 0);

                if (LenUserName > 0)
                {
                    byte[] Name = new byte[LenUserName *= 2];
                    WinAPI.ReadProcessMemory(hProcess, UserNameAddr, Name, LenUserName, ref ret);
                    if (UserName != System.Text.Encoding.Unicode.GetString(Name))
                    {
                        WinAPI.CloseHandle(hProcess);
                        return false;
                    }
                }
                WinAPI.CloseHandle(hProcess);

            }
            return true;
        }
        //場景切換
        public bool SceneChange(IntPtr hWnd)
        {
            int Change;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, SceneChangeAddr, out Change, 4, 0);//場景切換
            WinAPI.CloseHandle(hProcess);
            if (Change == 0)
                return false;
            return true;
        }
        //程式忙碌
        public bool Busy(IntPtr hWnd)
        {
            int Busy, socketBusy;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            WinAPI.ReadProcessMemory(hProcess, BusyAddr, out Busy, 1, 0);//忙碌中
            WinAPI.ReadProcessMemory(hProcess, SocketAddr, out socketBusy, 1, 0);
            WinAPI.CloseHandle(hProcess);
            if (Busy == 0 && socketBusy == 0)
                return false;
            return true;
        }
        //取得角色狀態
        public bool GetUserStatus(IntPtr hWnd, ref int Power, ref int MaxPower, ref int Money, ref int Fatigue)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, PowerAddr, out Power, 4, 0);//行動力
                WinAPI.ReadProcessMemory(hProcess, MaxPowerAddr, out MaxPower, 4, 0);//最大行動力
                WinAPI.ReadProcessMemory(hProcess, FatigueAddr, out Fatigue, 2, 0);//疲勞度
                WinAPI.ReadProcessMemory(hProcess, MoneyAddr, out Money, 4, 0);//持有金錢
                WinAPI.CloseHandle(hProcess);

                if ((Fatigue % 10) != 0)
                    Fatigue = Fatigue / 10 + 1;
                else
                    Fatigue = Fatigue / 10;
                return true;
            }
            return false;
        }
        //取得物資狀態
        public bool GetMaterialsStatus(IntPtr hWnd, int[] Materials, int[] SetMaterials)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //持有的數量
                WinAPI.ReadProcessMemory(hProcess, WaterAddr, out Materials[0], 2, 0);//水
                WinAPI.ReadProcessMemory(hProcess, GrainAddr, out Materials[1], 2, 0);//糧食
                WinAPI.ReadProcessMemory(hProcess, StuffAddr, out Materials[2], 2, 0);//資材
                WinAPI.ReadProcessMemory(hProcess, AmmunitionAddr, out Materials[3], 2, 0);//彈藥

                //保存的數量
                for (int i = 0; i < 4; i++)
                    WinAPI.ReadProcessMemory(hProcess, SetSupplyAddr + i * 0x4, out SetMaterials[i], 4, 0);

                WinAPI.CloseHandle(hProcess);
                return true;
            }
            return false;
        }
        //取得地點
        public string GetPlace(IntPtr hWnd, ref int PlaceNo, ref int Place)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, PlaceNoAddr + 0x2, out PlaceNo, 1, 0);//城市、海域編號
                WinAPI.ReadProcessMemory(hProcess, PlaceNoAddr + 0x3, out Place, 1, 0);//所在場景(0x4海上 0x8陸地 0xC室內 0x1C港口 >=0x1C 登陸點
                //所在地點名稱
                int PlaceNameAddr, LenPlaceName, ret = 0;
                int DungeonNameAddrPtr, LenDungeonName;
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddrPtr, out PlaceNameAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr + 0x80, out DungeonNameAddrPtr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr + 0x4, out PlaceNameAddr, 4, 0);

                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr - 0xC, out LenPlaceName, 4, 0);
                byte[] PlaceName = new byte[LenPlaceName *= 2];
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr, PlaceName, LenPlaceName, ref ret);

                WinAPI.ReadProcessMemory(hProcess, DungeonNameAddrPtr - 0xC, out LenDungeonName, 4, 0);
                byte[] DungeonName = new byte[LenDungeonName *= 2];
                WinAPI.ReadProcessMemory(hProcess, DungeonNameAddrPtr, DungeonName, LenDungeonName, ref ret);
                WinAPI.CloseHandle(hProcess);

                //Console.WriteLine(System.Text.Encoding.Unicode.GetString(PlaceName));
                //Console.WriteLine(System.Text.Encoding.Unicode.GetString(DungeonName));

                if (!string.IsNullOrWhiteSpace(System.Text.Encoding.Unicode.GetString(DungeonName)))
                    return System.Text.Encoding.Unicode.GetString(DungeonName);
                return System.Text.Encoding.Unicode.GetString(PlaceName);
            }
            return null;
        }
        //取得上一層地點
        public string GetLastPlace(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int PlaceNameAddr, LenPlaceName, ret = 0;
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddrPtr + 0x4, out PlaceNameAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr + 0x4, out PlaceNameAddr, 4, 0);

                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr - 0xC, out LenPlaceName, 4, 0);
                byte[] PlaceName = new byte[LenPlaceName *= 2];
                WinAPI.ReadProcessMemory(hProcess, PlaceNameAddr, PlaceName, LenPlaceName, ref ret);

                WinAPI.CloseHandle(hProcess);

                //Console.WriteLine(System.Text.Encoding.Unicode.GetString(PlaceName));
                return System.Text.Encoding.Unicode.GetString(PlaceName);
            }
            return null;
        }
        //取得座標(陸地、海上)
        public PointF GetCoordinate(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int SeaXOffset = 0, SeaYOffset = 0, Place = 0;
                float X = 0, Y = 0;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                //人物座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr, out X, 4, 0);//座標X
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x8, out Y, 4, 0);//座標Y
                WinAPI.ReadProcessMemory(hProcess, PlaceNoAddr + 0x3, out Place, 1, 0);//所在場景(0x4海上 0x8陸地 0xC室內 0x1C港口 >=0x1C 登陸點

                WinAPI.CloseHandle(hProcess);
                if (GetSeaOffset(hWnd, ref SeaXOffset, ref SeaYOffset) && Place == 0x4)
                {
                    X = X / 10000 + SeaXOffset * 256;
                    if (X >= 16384)
                        X -= 16384;
                    Y = Y / 10000 + SeaYOffset * 256;
                }
                if (X != 0 && Y != 0)
                    return new PointF(X, Y);
            }
            return PointF.Empty;
            //測量座標 = 人物座標 / 10000 + 海域座標 * 256
            //港口座標 37000 35000
        }
        //取得海域座標
        public bool GetSeaOffset(IntPtr hWnd, ref int SeaXOffset, ref int SeaYOffset)
        {
            if (!SceneChange(hWnd))
            {
                int Place;
                int SeaCoordinateAddr;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, PlaceNoAddr + 0x3, out Place, 1, 0);//所在場景(0x4海上 0x8陸地 0xC室內 0x1C港口 >=0x1C 登陸點
                if (Place == 0x4)
                {
                    WinAPI.ReadProcessMemory(hProcess, SeaCoordinatePtr, out SeaCoordinateAddr, 4, 0);
                    //Console.WriteLine(Convert.ToString(SeaCoordinateAddr, 16));
                    WinAPI.ReadProcessMemory(hProcess, SeaCoordinateAddr + 0x408, out SeaCoordinateAddr, 4, 0);//會更動
                    //Console.WriteLine(Convert.ToString(SeaCoordinateAddr, 16));
                    WinAPI.ReadProcessMemory(hProcess, SeaCoordinateAddr + 0x4, out SeaCoordinateAddr, 4, 0);
                    //Console.WriteLine(Convert.ToString(SeaCoordinateAddr, 16));
                    WinAPI.ReadProcessMemory(hProcess, SeaCoordinateAddr + 0x12C, out SeaXOffset, 4, 0);//海域座標X
                    WinAPI.ReadProcessMemory(hProcess, SeaCoordinateAddr + 0x130, out SeaYOffset, 4, 0);//海域座標Y
                    WinAPI.CloseHandle(hProcess);
                    if (SeaXOffset != 0 || SeaYOffset != 0)
                        return true;
                }
            }
            return false;
        }
        //取得角度
        public void GetAngle(IntPtr hWnd, ref float Cos, ref float Sin)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                float tempCos, tempSin;
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, CosAddr, out tempCos, 4, 0);//Cos
                WinAPI.ReadProcessMemory(hProcess, SinAddr, out tempSin, 4, 0);//Sin
                WinAPI.CloseHandle(hProcess);

                //if (tempCos <= 1 && tempCos >= -1 && tempSin <= 1 && tempSin >= -1)
                {
                    Cos = tempCos;
                    Sin = tempSin;
                }
                //Console.WriteLine(tempCos + " " + tempSin);
            }
        }
        //取得天氣狀態
        public int GetWeatherStatus(IntPtr hWnd)
        {
            int Weather = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, WeatherAddr, out Weather, 1, 0);//天氣
                WinAPI.CloseHandle(hProcess);
                if (Weather >= 0x80)
                    Weather -= 0x80;
                //00-0F晴天 10-1F晴天 20-2F下雨 30-3F下雪 40-4F暴风 50-5F暴雪
            }
            return Weather;
        }
        //取得暴風&雪狀態
        public bool GetBadWeather(IntPtr hWnd)
        {
            if (GetWeatherStatus(hWnd) >= 0x40 && GetWeatherStatus(hWnd) <= 0x5F)
                return true;
            else
                return false;
        }
        //取得海上災難狀態
        public void GetBoatException(IntPtr hWnd, bool[] BoatException)
        {
            if (!SceneChange(hWnd))
            {
                int _BoatException;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, BoatExceptionAddr, out _BoatException, 4, 0);//海上船隻狀態
                WinAPI.CloseHandle(hProcess);

                for (int i = 0; i < 32; i++)
                {
                    if (_BoatException % 2 == 1)
                        BoatException[i] = true;
                    else
                        BoatException[i] = false;
                    _BoatException >>= 1;
                }
            }
        }
        //舵&帆損壞
        public void GetBoatException2(IntPtr hWnd, ref bool rudder, ref bool sail)
        {
            if (!SceneChange(hWnd))
            {
                int BoatException = 0;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, BoatExceptionAddr2, out BoatException, 1, 0);//無法航行
                WinAPI.CloseHandle(hProcess);
                if (BoatException >= 128)
                {
                    rudder = true;
                    BoatException -= 128;
                }
                else
                    rudder = false;

                if (BoatException > 0)
                    sail = true;
                else
                    sail = false;
            }
        }
        //無法航行
        public bool GetBoatException3(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int BoatException;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, BoatExceptionAddr3, out BoatException, 1, 0);//無法航行
                WinAPI.CloseHandle(hProcess);

                if (BoatException > 0)
                    return true;
            }
            return false;
        }
        //取得隊友災難狀態
        public void GetPartyException(IntPtr hWnd, bool[][] BoatException)
        {
            if (!SceneChange(hWnd))
            {
                int TargetAddrPtr;
                int[] PartyID = new int[5];

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                for (int User = 0; User < 5; User++)
                {
                    if (!GetPartyID(hWnd, PartyID) && User == 0)
                    {
                        GetBoatException(hWnd, BoatException[User]);
                        break;
                    }
                    else
                    {
                        int BoatException2 = 0, BoatException3 = 0;
                        int _BoatException;

                        TargetAddrPtr = GetNameAddr(hWnd, PartyID[User], PeopleBase);
                        WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x14, out _BoatException, 4, 0);//目標海上狀態
                        WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0xD3, out BoatException2, 1, 0);//目標帆、舵狀態
                        WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0xD4, out BoatException3, 1, 0);//目標船狀態(沉船)

                        for (int i = 0; i < 32; i++)
                        {
                            if (_BoatException % 2 == 1)
                                BoatException[User][i] = true;
                            else
                                BoatException[User][i] = false;
                            _BoatException >>= 1;
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得消除災難所需
        public string EliminateStatus(IntPtr hWnd, int status, ref int skillID)
        {
            string itemName = null;
            switch (status)
            {
                case 0://壞血病
                    skillID = 128;//疾病學
                    itemName = "萊姆果汁";//0x1500012
                    break;
                case 1://疫病
                    skillID = 128;//疾病學
                    itemName = "疾病用特效藥";//0x1500036
                    break;
                case 2://判亂
                    skillID = 123;//統率
                    break;
                case 3://鼠患
                    skillID = 19;//驅除
                    itemName = "滅鼠藥";//0x1500020
                    break;
                case 4://火災
                    skillID = 131;//滅火
                    break;
                case 6://漏水
                    itemName = "水桶";//0x1500011
                    break;
                case 7://混亂
                    skillID = 123;//統率
                    itemName = "沉靜的旗幟";//0x1500033
                    break;
                case 8://煙霧
                    itemName = "神秘的羽扇";//0x1500035
                    break;
                case 9://海藻
                    skillID = 19;//驅除
                    break;
                case 11://海妖
                    itemName = "純綿的耳塞";//0x1500041
                    break;
                case 12://海魔
                    itemName = "防海魔的聖痕";//0x1500079
                    break;
                case 13://大章魚
                    itemName = "防大章魚的聖痕";//0x1500080
                    break;
                case 14://大鯊魚
                    itemName = "防大鯊魚的聖痕";//0x1500081
                    break;
                case 16://衛生不良
                    itemName = "清潔用甲板長刷";//0x1500108
                    break;
                case 17://營養不良
                    itemName = "料理";
                    break;
                case 18://失眠
                    itemName = "安眠吊床";//0x1500109
                    break;
                case 19://思鄉病
                    itemName = "望鄉排鐘";//0x1500151
                    break;
                case 20://精神不安
                    itemName = "船歌的樂譜";//0x1500152
                    break;
                case 21://欲求不滿
                    itemName = "消愁酒桶";//0x1500021
                    break;
                case 22://爭吵
                    skillID = 123;//統率
                    itemName = "懲罰的繩索";//0x1500072
                    break;
                case 32://船帆損壞
                    itemName = "備用船帆";//0x1500042
                    break;
                case 33://船帆損壞
                    itemName = "備用船舵";//0x1500043
                    break;
                case 34://無法航行
                    skillID = 18;//救助
                    itemName = "救生工具";//0x150002
                    break;
                default:
                    break;
            }
            return itemName;
        }
        //取得戰鬥狀態
        public int GetBattleStatus(IntPtr hWnd)
        {
            int Battle = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, FightStatusAddr, out Battle, 1, 0);//(1-攻方 2-守方 3- 4- 5-陸戰攻? 6-陸戰守?)
                WinAPI.CloseHandle(hProcess);
            }
            return Battle;
        }
        //取得隊友戰鬥狀態
        public int GetPartyBattleStatus(IntPtr hWnd, int PartyID)
        {
            int Battle = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int TargetAddrPtr = GetNameAddr(hWnd, PartyID, PeopleBase);
                WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x34, out Battle, 1, 0);//(1-攻方 2-守方 3- 4- 5-陸戰攻? 6-陸戰守?)
                WinAPI.CloseHandle(hProcess);
            }
            return Battle;
        }
        //取得海戰狀態
        public bool GetNavalbattle(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int Navalbattle;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, FightAddr, out Navalbattle, 4, 0);//戰鬥標誌
                WinAPI.CloseHandle(hProcess);
                if (Navalbattle != 0)
                    return true;
            }
            return false;
        }
        //取得肉搏目標ID
        public bool GetFightID(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int FightID;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, FightIDAddr, out FightID, 4, 0);//肉搏目標ID
                WinAPI.CloseHandle(hProcess);
                if (FightID == 0)
                    return false;
            }
            return true;
        }
        //取得敵方提督ID
        public int GetEnemyAdmiralID(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int EnemyAdmiralID;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, EnemyAdmiralAddr, out EnemyAdmiralID, 4, 0);//敵方提督ID
                WinAPI.CloseHandle(hProcess);
                return EnemyAdmiralID;
            }
            return 0;
        }
        //取得航海天數
        public int GetMaritimeDays(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int MaritimeDays;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, MaritimeDaysAddr, out MaritimeDays, 4, 0);
                WinAPI.CloseHandle(hProcess);
                return MaritimeDays;
            }
            return -1;
        }
        //取得隊長ID
        public int GetPartyLeaderId(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int[] partyId = new int[5];
                if (GetPartyID(hWnd, partyId))
                    return partyId[0];
            }
            return 0;
        }
        //取得組隊狀態&&隊伍ID
        public bool GetPartyID(IntPtr hWnd, int[] party)
        {
            if (!SceneChange(hWnd))
            {
                int PartyAddr;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, PartyAddrPtr, out PartyAddr, 4, 0);

                for (int User = 0; User < 5; User++)
                {
                    if (PartyAddr != 0)
                    {
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0xC, out party[User], 4, 0);//隊員[User] ID
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0x0, out PartyAddr, 4, 0);
                    }
                    else
                        party[User] = 0;
                }
                WinAPI.CloseHandle(hProcess);
            }
            return GetPartyStatus(hWnd);
        }
        //取得組隊狀態
        public bool GetPartyStatus(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int PartyAddr;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, PartyAddrPtr, out PartyAddr, 4, 0);
                WinAPI.CloseHandle(hProcess);

                if (PartyAddr != 0)
                    return true;
            }
            return false;
        }
        //檢查隊友ID
        public bool CheckPartyID(IntPtr hWnd, int TargetID)
        {
            if (!SceneChange(hWnd))
            {
                int PartyAddr, PartyID;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, PartyAddrPtr, out PartyAddr, 4, 0);
                if (PartyAddr != 0)
                {
                    for (int User = 0; User < 5; User++)
                    {
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0xC, out PartyID, 4, 0);//隊員[User] ID
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0x0, out PartyAddr, 4, 0);
                        if (TargetID == PartyID)
                        {
                            WinAPI.CloseHandle(hProcess);
                            return true;
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;
        }
        //檢查隊友Place
        public bool CheckPartyPlace(IntPtr hWnd, int Place)
        {
            if (!SceneChange(hWnd))
            {
                int PartyAddr, PartyID, PartyPlace;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, PartyAddrPtr, out PartyAddr, 4, 0);
                if (PartyAddr != 0)
                {
                    for (int User = 0; User < 5; User++)
                    {
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0xC, out PartyID, 4, 0);//隊員[User] ID
                        if (PartyID != GetUserId(hWnd))
                            WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0x23, out PartyPlace, 1, 0);//隊員[User] Place
                        else
                            PartyPlace = Place;
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0x0, out PartyAddr, 4, 0);
                        if (PartyPlace != Place)
                        {
                            WinAPI.CloseHandle(hProcess);
                            return false;
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
                return true;
            }
            return false;
        }
        //取得跟隨狀態
        public bool GetFollowStatus(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int LandFollow, SeaFollow;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, LandFollowAddr, out LandFollow, 4, 0);//陸地跟隨ID
                WinAPI.ReadProcessMemory(hProcess, SeaFollowAddr, out SeaFollow, 4, 0);//海上跟隨ID
                WinAPI.CloseHandle(hProcess);
                if (LandFollow == 0 && SeaFollow == 0)
                    return false;
            }
            return true;
        }
        //取得距離
        public float Distance(PointF Target, PointF Self)
        {
            if (!Target.IsEmpty && !Self.IsEmpty)
                return (float)Math.Sqrt(Math.Pow(Target.X - Self.X, 2) + Math.Pow(Target.Y - Self.Y, 2));
            return float.NaN;
        }
        //取得目標座標
        public PointF GetTargetCoordinate(IntPtr hWnd, int TargetID, int TargetType)
        {
            if (!SceneChange(hWnd))
            {
                PointF Coordinate = new PointF();
                float TX, TY;
                int TargetAddrPtr;
                int SeaXOffset = 0, SeaYOffset = 0;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (TargetType == 0)//活動對象
                {
                    TargetAddrPtr = GetNameAddr(hWnd, TargetID, PeopleBase);
                    WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x100, out TX, 4, 0);//目標X座標
                    WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x108, out TY, 4, 0);//目標Y座標
                    if (GetSeaOffset(hWnd, ref SeaXOffset, ref SeaYOffset))
                    {
                        TX = TX / 10000 + SeaXOffset * 256;
                        if (TX >= 16384)
                            TX -= 16384;
                        TY = TY / 10000 + SeaYOffset * 256;
                    }
                    Coordinate.X = TX;
                    Coordinate.Y = TY;
                }
                else if (TargetType == 1)//固定對象
                {
                    //場景名字
                    TargetAddrPtr = GetNameAddr(hWnd, TargetID, RegularBase);
                    WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x178, out TX, 4, 0);//目標X座標
                    WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x180, out TY, 4, 0);//目標Y座標
                    if (GetSeaOffset(hWnd, ref SeaXOffset, ref SeaYOffset))
                    {
                        TX = TX / 10000 + SeaXOffset * 256;
                        if (TX >= 16384)
                            TX -= 16384;
                        TY = TY / 10000 + SeaYOffset * 256;
                    }
                    Coordinate.X = TX;
                    Coordinate.Y = TY;
                }

                WinAPI.CloseHandle(hProcess);
                return Coordinate;
            }
            return PointF.Empty;
        }
        //取得目標ID
        public int GetTargetID(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int TargetID;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, TargetIDAddr, out TargetID, 4, 0);
                WinAPI.CloseHandle(hProcess);

                return TargetID;
            }
            return 0;
        }
        //取得目標名字
        public void GetTargetInfo(IntPtr hWnd, ref Form1.uInfo.TargetInfo target)
        {
            if (!SceneChange(hWnd))
            {
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));

                //WinAPI.ReadProcessMemory(hProcess, TargetIDAddr, out target.ID, 4, 0);
                target.ID = GetTargetID(hWnd);
                if (target.ID != GetUserId(hWnd) && target.ID != 0)
                {
                    int Type;
                    WinAPI.ReadProcessMemory(hProcess, TargetTypeAddr, out Type, 4, 0);
                    if (Type == 0)//活動對象
                    {
                        target.Name = GetPeopleName(hWnd, target.ID);
                        target.Coordinate = GetTargetCoordinate(hWnd, target.ID, Type);
                    }
                    else if (Type == 1)//固定對象
                    {
                        target.Name = GetSceneName(hWnd, target.ID);
                        target.Coordinate = GetTargetCoordinate(hWnd, target.ID, Type);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得名稱位址
        public int GetNameAddr(IntPtr hWnd, int ID, int Base)
        {
            if (!SceneChange(hWnd))
            {
                int A, B, temp, addrptr, addr;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, Base, out A, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, Base + 0x4, out B, 4, 0);
                if (B == 0)
                    return 0;
                addrptr = A + ((ID / 16) % B) * 4;
                WinAPI.ReadProcessMemory(hProcess, addrptr, out addr, 4, 0);

                do
                {
                    WinAPI.ReadProcessMemory(hProcess, addr, out temp, 4, 0);
                    if (temp == ID)
                        break;
                    WinAPI.ReadProcessMemory(hProcess, addr + 0x8, out addr, 4, 0);
                } while (addr != 0 && !SceneChange(hWnd));

                if (addr > 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, addr + 0x4, out temp, 4, 0);
                    WinAPI.CloseHandle(hProcess);
                    return temp;
                }
                WinAPI.CloseHandle(hProcess);
            }
            return 0;
        }
        //取得人物角度////////////
        public void GetPeopleAngle(IntPtr hWnd, int TargetID, ref float Cos, ref float Sin)
        {
            if (!SceneChange(hWnd))
            {
                int TargetAddrPtr;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                TargetAddrPtr = GetNameAddr(hWnd, TargetID, PeopleBase);
                WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x110, out Cos, 4, 0);//目標cos
                WinAPI.ReadProcessMemory(hProcess, TargetAddrPtr + 0x118, out Sin, 4, 0);//目標sin
                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得人物名稱
        public string GetPeopleName(IntPtr hWnd, int PeopleID)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int PeopleAddrPtr, PeopleNameAddr, LenPeopleName;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                PeopleAddrPtr = GetNameAddr(hWnd, PeopleID, PeopleBase);
                if (PeopleAddrPtr != 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, PeopleAddrPtr + 0x2C, out PeopleNameAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, PeopleNameAddr - 0xC, out LenPeopleName, 4, 0);
                    if (LenPeopleName > 0)
                    {
                        byte[] Name = new byte[LenPeopleName *= 2];
                        WinAPI.ReadProcessMemory(hProcess, PeopleNameAddr, Name, LenPeopleName, ref ret);
                        WinAPI.CloseHandle(hProcess);

                        return System.Text.Encoding.Unicode.GetString(Name);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;
        }
        //取得場景名稱
        public string GetSceneName(IntPtr hWnd, int RegularID)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int RegularAddrPtr, SceneNameAddrPtr, SceneNameAddr, LenSceneName;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //場景名字
                RegularAddrPtr = GetNameAddr(hWnd, RegularID, RegularBase);
                if (RegularAddrPtr != 0)
                {
                    int SceneID;
                    //取得場景目標ID
                    WinAPI.ReadProcessMemory(hProcess, RegularAddrPtr + 0x174, out SceneID, 4, 0);
                    //Console.WriteLine(Convert.ToString(SceneID, 16));

                    #region 測試用
                    /*
                    //測試 PartyAddrPtr WeatherAddr
                    int TempAddr = PartyAddrPtr, Temp = 0, LenName;
                    int NameAddrPtr, NameAddr;
                    do
                    {
                        WinAPI.ReadProcessMemory(hProcess, TempAddr + 0x4, out Temp, 4, 0);
                        if (Temp == 0x11)
                        {
                            if ((NameAddrPtr = GetNameAddr(hWnd, SceneID, TempAddr)) > 0)
                            {
                                WinAPI.ReadProcessMemory(hProcess, NameAddrPtr + 0xC, out NameAddr, 4, 0);
                                WinAPI.ReadProcessMemory(hProcess, NameAddr - 0xC, out LenName, 4, 0);
                                if (LenName > 0)
                                {
                                    Console.WriteLine("000 " + Convert.ToString(TempAddr, 16));
                                    byte[] ItemName = new byte[LenName *= 2];
                                    WinAPI.ReadProcessMemory(hProcess, NameAddr, ItemName, LenName, ref ret);
                                    Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                                }
                            }
                        }
                        TempAddr += 4;
                        //Console.WriteLine(Convert.ToString(TempAddr, 16));
                    } while (TempAddr < WeatherAddr);
                    */
                    #endregion

                    SceneNameAddrPtr = GetNameAddr(hWnd, SceneID, SceneBase);
                    if (SceneNameAddrPtr != 0)
                    {
                        //Console.WriteLine(Convert.ToString(TargetNameAddrPtr, 16));
                        WinAPI.ReadProcessMemory(hProcess, SceneNameAddrPtr + 0xC, out SceneNameAddr, 4, 0);
                        //Console.WriteLine(Convert.ToString(TargetNameAddr, 16));
                        WinAPI.ReadProcessMemory(hProcess, SceneNameAddr - 0xC, out LenSceneName, 4, 0);
                        //Console.WriteLine(LenTargetName);
                        if (LenSceneName > 0)
                        {
                            byte[] Name = new byte[LenSceneName *= 2];
                            WinAPI.ReadProcessMemory(hProcess, SceneNameAddr, Name, LenSceneName, ref ret);
                            //Console.WriteLine(System.Text.Encoding.Unicode.GetString(TargetName));
                            WinAPI.CloseHandle(hProcess);

                            return System.Text.Encoding.Unicode.GetString(Name);
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;
        }
        //取得技能名稱(目前沒用到)
        public string GetSkillName(IntPtr hWnd, int SkillID)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int ItemAddrPtr, ItemAddr, NameLen;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                ItemAddrPtr = GetNameAddr(hWnd, SkillID, SkillBase);
                WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr + 0xC, out ItemAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, ItemAddr - 0xC, out NameLen, 4, 0);
                if (NameLen > 0)
                {
                    byte[] ItemName = new byte[NameLen *= 2];
                    WinAPI.ReadProcessMemory(hProcess, ItemAddr, ItemName, NameLen, ref ret);
                    WinAPI.CloseHandle(hProcess);
                    //Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                    return System.Text.Encoding.Unicode.GetString(ItemName);
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;

        }
        //取得道具名稱
        public string GetItemName(IntPtr hWnd, int ItemID)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int ItemAddrPtr, ItemAddr, NameLen;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                ItemAddrPtr = GetNameAddr(hWnd, ItemID, ItemBase);

                //測試 PartyAddrPtr WeatherAddr
                /*
                int TempAddr = PartyAddrPtr, Temp = 0, LenName;
                int NameAddrPtr, NameAddr;
                do
                {
                    WinAPI.ReadProcessMemory(hProcess, TempAddr + 0x4, out Temp, 4, 0);
                    if (Temp == 0x11)
                    {
                        if ((NameAddrPtr = GetNameAddr(hWnd, ItemID, TempAddr)) > 0)
                        {
                            WinAPI.ReadProcessMemory(hProcess, NameAddrPtr + 0xC, out NameAddr, 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, NameAddr - 0xC, out LenName, 4, 0);
                            if (LenName > 0)
                            {
                                Console.WriteLine("000 " + Convert.ToString(TempAddr, 16));
                                byte[] ItemName = new byte[LenName *= 2];
                                WinAPI.ReadProcessMemory(hProcess, NameAddr, ItemName, LenName, ref ret);
                                Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                            }
                        }
                    }
                    TempAddr += 4;
                    Console.WriteLine(Convert.ToString(TempAddr, 16));
                } while (TempAddr < WeatherAddr);
                */

                if (ItemAddrPtr != 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr + 0xC, out ItemAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, ItemAddr - 0xC, out NameLen, 4, 0);
                    if (NameLen > 0)
                    {
                        byte[] ItemName = new byte[NameLen *= 2];
                        WinAPI.ReadProcessMemory(hProcess, ItemAddr, ItemName, NameLen, ref ret);
                        WinAPI.CloseHandle(hProcess);
                        //Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                        return System.Text.Encoding.Unicode.GetString(ItemName);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;

        }
        //取得道具類型
        public string GetItemType(IntPtr hWnd, int ItemID)
        {
            if (!SceneChange(hWnd))
            {
                int ret = 0;
                int ItemAddrPtr = 0, ItemAddr, NameLen;

                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                ItemAddrPtr = GetNameAddr(hWnd, ItemID, ItemTypeBase1);
                #region 測試用
                /*
                int TempAddr = PartyAddrPtr, TempAddr2 = 0, Temp = 0, LenName;
                int NameAddrPtr, NameAddr;
                do
                {
                    WinAPI.ReadProcessMemory(hProcess, TempAddr + 0x4, out Temp, 4, 0);
                    if (Temp == 0x11)
                    {
                        if ((TempAddr2 = GetNameAddr(hWnd, ItemID, TempAddr)) > 0)
                        {
                            int TypeID;
                            WinAPI.ReadProcessMemory(hProcess, TempAddr2 + 0x40, out TypeID, 1, 0);
                            NameAddrPtr = GetNameAddr(hWnd, TypeID, TempAddr - 0x1C);

                            WinAPI.ReadProcessMemory(hProcess, NameAddrPtr + 0xC, out NameAddr, 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, NameAddr - 0xC, out LenName, 4, 0);
                            if (LenName > 0)
                            {
                                Console.WriteLine("000 " + Convert.ToString(TempAddr, 16));
                                byte[] ItemName = new byte[LenName *= 2];
                                WinAPI.ReadProcessMemory(hProcess, NameAddr, ItemName, LenName, ref ret);
                                Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                            }
                        }
                    }
                    TempAddr += 4;
                    //Console.WriteLine(Convert.ToString(TempAddr, 16));
                } while (TempAddr < WeatherAddr);
                */
                #endregion

                if (ItemAddrPtr > 0)
                {
                    int TypeID;
                    WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr + 0x40, out TypeID, 1, 0);
                    ItemAddrPtr = GetNameAddr(hWnd, TypeID, ItemTypeBase1 - 0x1C);

                    WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr + 0xC, out ItemAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, ItemAddr - 0xC, out NameLen, 4, 0);
                    if (NameLen > 0)
                    {
                        byte[] Name = new byte[NameLen *= 2];
                        WinAPI.ReadProcessMemory(hProcess, ItemAddr, Name, NameLen, ref ret);
                        WinAPI.CloseHandle(hProcess);
                        //Console.WriteLine(System.Text.Encoding.Unicode.GetString(Name));
                        return System.Text.Encoding.Unicode.GetString(Name);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;
        }
        //物品型態轉換
        public int ItemTypeChange(string ItemType)
        {
            string[] ItemType1 = new string[] { "食品", "調味料", "家畜", "醫藥品", "雜貨" };
            string[] ItemType2 = new string[] { "酒類", "礦石", "工業製品", "嗜好品", "染料" };
            string[] ItemType3 = new string[] { "纖維", "紡織品", "武器", "火器", "工藝品", "美術品" };
            string[] ItemType4 = new string[] { "香辛料", "香料", "寶石", "貴金屬" };

            for (int i = 0; i < ItemType1.Length; i++)
                if (ItemType == ItemType1[i])
                    return 1;
            for (int i = 0; i < ItemType2.Length; i++)
                if (ItemType == ItemType2[i])
                    return 2;
            for (int i = 0; i < ItemType3.Length; i++)
                if (ItemType == ItemType3[i])
                    return 3;
            for (int i = 0; i < ItemType4.Length; i++)
                if (ItemType == ItemType4[i])
                    return 4;
            return 0;
        }
         
        //取得買入交易物品清單
        public bool GetBuyTradeMenu(IntPtr hWnd, ref int MenuNum, ref CityInfo.BuyTradeInfo[] Trade)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "買入交易物品")
            {
                int TradeAddrPtr, TradeAddr;
                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                //取得陳列品數
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x684, out MenuNum, 4, 0);
                //取得交易品位址
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x67C, out TradeAddrPtr, 4, 0);
                //選擇陳列品的項次
                //WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x74C, out Select, 4, 0);

                if (TradeAddrPtr == 0)
                    return false;

                for (int i = 0; i < MenuNum; i++)
                {
                    WinAPI.ReadProcessMemory(hProcess, TradeAddrPtr + 0x8, out TradeAddr, 4, 0);

                    //交易品ID
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x8, out Trade[i].ID, 4, 0);
                    //交易品類型
                    Trade[i].Type = GetItemType(hWnd, Trade[i].ID);
                    //使用星書類型
                    Trade[i].UseBook = ItemTypeChange(Trade[i].Type);
                    //交易品名稱
                    Trade[i].Name = GetItemName(hWnd, Trade[i].ID);
                    //最大購買量
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x20, out Trade[i].MaxNum, 4, 0);
                    //單價TradePrice
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x18, out Trade[i].Price, 4, 0);
                    /*
                        //倍數價格&倍數
                        WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x18, out TradePriceAddr, 4, 0);
                        for (int j = 0; j < 3; j++)
                        {
                            WinAPI.ReadProcessMemory(hProcess, TradePriceAddr + 0x8, out Trade[i].MultiplePrice[j], 4, 0);
                            Trade[i].Multiple[j] = (int)Math.Ceiling(((double)Trade[i].MultiplePrice[j] / (double)Trade[i].Price));
                            if (Trade[i].Multiple[j] / 10 > 0)
                                Trade[i].Multiple[j] = (int)Math.Ceiling((double)Trade[i].Multiple[j] / 10) * 10;
                            if (Trade[i].Multiple[j] / 100 > 0)
                                Trade[i].Multiple[j] = (int)Math.Ceiling((double)Trade[i].Multiple[j] / 100) * 100;
                            WinAPI.ReadProcessMemory(hProcess, TradePriceAddr, out TradePriceAddr, 4, 0);
                        }
                     */
                    WinAPI.ReadProcessMemory(hProcess, TradeAddrPtr, out TradeAddrPtr, 4, 0);//下一個出售交易品
                }
                WinAPI.CloseHandle(hProcess);
                return true;
            }
            return false;
        }
        //取得出售交易物品清單
        public bool GetSellTradeMenu(IntPtr hWnd, ref int MenuNum, ref CityInfo.SellTradeInfo[] Trade)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "出售交易物品")
            {
                int TradeAddrPtr, TradeAddr;

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                //取得陳列品數
                //WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x628, out MenuNum, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x664, out MenuNum, 4, 0);

                //取得交易品位址
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x65C, out TradeAddrPtr, 4, 0);
                //WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr + 0x624, out TradeAddr, 4, 0);

                for (int i = 0; i < MenuNum; i++)
                {
                    WinAPI.ReadProcessMemory(hProcess, TradeAddrPtr + 0x8, out TradeAddr, 4, 0);

                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x8, out Trade[i].Code, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x10, out Trade[i].ID, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x18, out Trade[i].MaxNum, 2, 0);
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x20, out Trade[i].SellPrice, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, TradeAddr + 0x28, out Trade[i].BuyPrice, 4, 0);
                    Trade[i].Profit = (int)(Trade[i].SellPrice - Trade[i].BuyPrice);
                    Trade[i].Name = GetItemName(hWnd, Trade[i].ID);

                    WinAPI.ReadProcessMemory(hProcess, TradeAddrPtr, out TradeAddrPtr, 4, 0);//下一個出售交易品
                }

                WinAPI.CloseHandle(hProcess);
                return true;
            }
            WinAPI.CloseHandle(hProcess);
            return false;
        }
        //取得菜單
        public void GetPubMenu(IntPtr hWnd, ref int Drink, ref int Food, ref string DN, ref string FN)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "點餐")
            {
                int ret = 0;
                int DrinkAddr, FoodAddr;
                int DrinkNameAddr, FoodNameAddr;
                int LenDrinkName, LenFoodName;
                int MenuNum, FirstAddr;

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                //取得菜單數
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5FC, out MenuNum, 4, 0);
                //取得菜單第一項位址
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0xC8, out FirstAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, FirstAddr + 0x8, out FirstAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, FirstAddr + 0xC, out FirstAddr, 4, 0);
                //取得飲料位址
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x9B8, out DrinkAddr, 4, 0);
                //取得食物位址
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x9BC, out FoodAddr, 4, 0);

                //取得飲料名
                if (DrinkAddr != 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, DrinkAddr + 0x4, out DrinkNameAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, DrinkNameAddr - 0xC, out LenDrinkName, 4, 0);
                    if (LenDrinkName > 0)
                    {
                        //取得飲料ID
                        WinAPI.ReadProcessMemory(hProcess, DrinkAddr, out Drink, 4, 0);
                        byte[] DrinkName = new byte[LenDrinkName *= 2];
                        WinAPI.ReadProcessMemory(hProcess, DrinkNameAddr, DrinkName, LenDrinkName, ref ret);
                        DN = System.Text.Encoding.Unicode.GetString(DrinkName);
                    }
                    else
                        DN = null;
                }
                else
                    DN = null;
                //取得食物名
                if (FoodAddr != 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, FoodAddr + 0x4, out FoodNameAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FoodNameAddr - 0xC, out LenFoodName, 4, 0);
                    if (LenFoodName > 0)
                    {
                        //取得食物ID
                        WinAPI.ReadProcessMemory(hProcess, FoodAddr, out Food, 4, 0);

                        byte[] FoodName = new byte[LenFoodName *= 2];
                        WinAPI.ReadProcessMemory(hProcess, FoodNameAddr, FoodName, LenFoodName, ref ret);
                        FN = System.Text.Encoding.Unicode.GetString(FoodName);
                    }
                    else
                        FN = null;
                }
                else
                    FN = null;
                //Console.WriteLine(Drink.ToString() + " " + Food.ToString());

                if (Drink == 0 && Food == 0)
                {
                    //自動選取菜單
                    int Min_Drink_Recovery = 999, Min_Food_Recovery = 999;
                    int Drink_Recovery = 0, Food_Recovery = 0;
                    int Check_Type, Check_Use;

                    for (int i = 0; i < MenuNum; i++)
                    {
                        WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20 + 0x11, out Check_Type, 1, 0);
                        WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20 + 0x14, out Check_Use, 1, 0);
                        if (Check_Use == 1)
                        {
                            if (Check_Type == 1)
                            {
                                WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20 + 0x10, out Food_Recovery, 1, 0);
                                if (Min_Food_Recovery > Food_Recovery)
                                {
                                    Min_Food_Recovery = Food_Recovery;
                                    WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20, out Food, 4, 0);
                                }

                            }
                            else
                            {
                                WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20 + 0x10, out Drink_Recovery, 1, 0);
                                if (Min_Drink_Recovery > Drink_Recovery)
                                {
                                    Min_Drink_Recovery = Drink_Recovery;
                                    WinAPI.ReadProcessMemory(hProcess, FirstAddr + i * 0x20, out Drink, 4, 0);
                                }
                            }
                        }
                    }
                }


            }
            WinAPI.CloseHandle(hProcess);
        }
        //取得套餐
        public void GetPubMeal(IntPtr hWnd, ref int[] Meal, ref string[] MN)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "套餐")
            {
                int ret = 0;
                int MealAddrPtr, MealAddr;
                int MealNameAddr, LenMealName;

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x618, out MealAddrPtr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, MealAddrPtr + 0x14, out MealAddrPtr, 4, 0);

                for (int i = 0; i < 5; i++)
                {
                    WinAPI.ReadProcessMemory(hProcess, MealAddrPtr + 0x308 + i * 0x4, out MealAddr, 4, 0);//
                    if (MealAddr != 0)
                    {
                        WinAPI.ReadProcessMemory(hProcess, MealAddr + 0x4, out MealNameAddr, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, MealNameAddr - 0xC, out LenMealName, 4, 0);
                        if (LenMealName > 0 && LenMealName <= 20)
                        {
                            WinAPI.ReadProcessMemory(hProcess, MealAddr, out Meal[i], 4, 0);
                            byte[] MealName = new byte[LenMealName *= 2];
                            WinAPI.ReadProcessMemory(hProcess, MealNameAddr, MealName, LenMealName, ref ret);
                            MN[i] = System.Text.Encoding.Unicode.GetString(MealName);
                            //Console.WriteLine(Meal[i] + " " + MN[i]);
                        }
                        else
                            MN[i] = null;
                    }
                    else
                        MN[i] = null;
                }
            }
            WinAPI.CloseHandle(hProcess);
        }
        //取得任務名
        public void GetMission(IntPtr hWnd, ref int Mission, ref string MN)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "接受委託")
            {
                int ret = 0;
                int MissionNameAddr, LenMissionName;
                int NumMission, SelectMission, FirstAddr;


                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5F0, out NumMission, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x864, out SelectMission, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x2f0, out FirstAddr, 4, 0);

                NumMission = SelectMission;

                for (int i = 0; i <= NumMission; i++)
                {
                    i = SelectMission;//留著可能有用
                    WinAPI.ReadProcessMemory(hProcess, FirstAddr + (i * 0x68) + 0x4, out MissionNameAddr, 4, 0);
                    //Console.WriteLine("第" + NumMission.ToString() + "項任務名字位址 = " + MissionNameAddr.ToString());
                    if (MissionNameAddr != 0)
                    {
                        WinAPI.ReadProcessMemory(hProcess, MissionNameAddr - 0xC, out LenMissionName, 4, 0);
                        //Console.WriteLine("第" + NumMission.ToString() + "項任務名字長度 = " + LenMissionName.ToString());
                        if (LenMissionName > 0)
                        {
                            WinAPI.ReadProcessMemory(hProcess, FirstAddr + (i * 0x68), out Mission, 4, 0);
                            //Console.WriteLine("第" + NumMission.ToString() + "項任務ID = " + Mission.ToString());
                            byte[] MissionName = new byte[LenMissionName *= 2];
                            WinAPI.ReadProcessMemory(hProcess, MissionNameAddr, MissionName, LenMissionName, ref ret);
                            MN = System.Text.Encoding.Unicode.GetString(MissionName);
                            //Console.WriteLine("第" + NumMission.ToString() + "項任務名 = " + MN.ToString());
                        }
                        else
                            MN = null;
                    }
                    else
                        MN = null;
                }
            }
            WinAPI.CloseHandle(hProcess);
        }
        //取得學科
        public void GetSubject(IntPtr hWnd, ref int Subject)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "閱覽書籍")
            {
                //int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x90, out Subject, 4, 0);

                if (Subject > 0)
                    Subject += 0x18;
                else
                    Subject += 0x6;

                if (Subject == 0x1C)
                    Subject = 0x1D;
                else if (Subject == 0x1D)
                    Subject = 0x1C;
            }
            WinAPI.CloseHandle(hProcess);
        }
        //取得自動操帆狀態
        public bool GetAutoSailState(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int SailState, AutoSailState;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, SailStateAddr, out SailState, 4, 0);//船帆 0-停船 1 2 3 4-全帆
                WinAPI.ReadProcessMemory(hProcess, AutoSailStateAddr, out AutoSailState, 2, 0);//自動操帆 0-無 1-有
                WinAPI.CloseHandle(hProcess);
                if (SailState > 0 && AutoSailState == 0)
                    return false;
            }
            return true;
        }
        //取得船帆狀態
        public int GetSailStatus(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int SailState;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                WinAPI.ReadProcessMemory(hProcess, SailStateAddr, out SailState, 4, 0);//船帆 0-停船 1 2 3 4-全帆
                WinAPI.CloseHandle(hProcess);
                return SailState;
            }
            return 0;
        }
        //取得已使用技能
        public void GetUsingSkills(IntPtr hWnd, int[] Usingskill, ref int UsingSkillNum)
        {
            if (!SceneChange(hWnd))
            {
                int SkillAddr;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, SkillAddrPtr, out SkillAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, SkillAddr + 0x2C, out SkillAddr, 4, 0);

                WinAPI.ReadProcessMemory(hProcess, SkillAddr + 0xC, out Usingskill[0], 4, 0);
                WinAPI.ReadProcessMemory(hProcess, SkillAddr + 0x14, out Usingskill[1], 4, 0);
                WinAPI.ReadProcessMemory(hProcess, SkillAddr + 0x1C, out Usingskill[2], 4, 0);
                WinAPI.ReadProcessMemory(hProcess, SkillAddr + 0x38, out UsingSkillNum, 4, 0);

                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得人物動作
        public int GetUserAction(IntPtr hWnd)
        {
            int Action = -1;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, UserActionAddr, out Action, 4, 0);
                WinAPI.CloseHandle(hProcess);
            }
            return Action;
        }
        //取得生產狀態
        public bool GetProduceState(IntPtr hWnd)
        {
            int Produce = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "生產")
                {
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x1094, out Produce, 4, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
            if (Produce == 1)
                return true;
            return false;
        }
        //取得生產Box狀態
        public bool GetProduceBoxState(IntPtr hWnd)
        {
            int Box = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "生產")
                {
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0xF76, out Box, 2, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
            if (Box == 3)
                return true;
            return false;
        }
        //取得配方
        public bool GetFormula(IntPtr hWnd, ref Form1.uInfo.Item _Item)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "選擇配方")
                {
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    int Formula_Num, Addr, Book_ID;
                    //配方書籍ID
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x688, out Book_ID, 4, 0);
                    //配方數量
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x76C, out Formula_Num, 4, 0);

                    /*
                    int TempAddr = PartyAddrPtr, Temp = 0, LenName, r = 0;
                    int NameAddrPtr, NameAddr;
                    do
                    {
                        WinAPI.ReadProcessMemory(hProcess, TempAddr + 0x4, out Temp, 4, 0);
                        if (Temp == 0x11)
                        {
                            if ((NameAddrPtr = GetNameAddr(hWnd, 171, TempAddr)) > 0)
                            {
                                WinAPI.ReadProcessMemory(hProcess, NameAddrPtr + 0xC, out NameAddr, 4, 0);
                                WinAPI.ReadProcessMemory(hProcess, NameAddr - 0xC, out LenName, 4, 0);
                                if (LenName > 0)
                                {
                                    Console.WriteLine("000 " + Convert.ToString(TempAddr, 16));
                                    byte[] ItemName = new byte[LenName *= 2];
                                    WinAPI.ReadProcessMemory(hProcess, NameAddr, ItemName, LenName, ref r);
                                    Console.WriteLine(System.Text.Encoding.Unicode.GetString(ItemName));
                                }
                            }
                        }
                        TempAddr += 4;
                        //Console.WriteLine(Convert.ToString(TempAddr, 16));
                    } while (TempAddr < WeatherAddr);
                    */
                    
                    for (int Book_Index = 0; Book_Index < _Item.BookNum; Book_Index++)
                    {
                        if (_Item._BookInfo[Book_Index].ID == Book_ID)
                        {
                            if (!_Item._BookInfo[Book_Index].Formula_Init)
                            {
                                _Item._BookInfo[Book_Index]._FormulaInfo = new Form1.uInfo.Item.FormulaInfo[Formula_Num];
                                _Item._BookInfo[Book_Index].Formula_Init = true;
                            }

                            WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0xA34, out Addr, 4, 0);
                            for (int Formula_Index = 0; Formula_Index < Formula_Num; Formula_Index++)
                            {
                                WinAPI.ReadProcessMemory(hProcess, Addr + Formula_Index * 0x3C, out _Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].ID, 4, 0);
                                _Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].Formula_Name = LoadFormulaInfo(_Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].ID);
                                Console.WriteLine(_Item._BookInfo[Book_Index].Name + " " + _Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].Formula_Name);

                                if (string.IsNullOrWhiteSpace(LoadFormulaInfo(_Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].ID)))
                                {
                                    int NAddrPtr, NAddr, Len, ret = 0;
                                    NAddrPtr = GetNameAddr(hWnd, _Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].ID, FormulaBase);
                                    if (NAddrPtr != 0)
                                    {
                                        WinAPI.ReadProcessMemory(hProcess, NAddrPtr + 0xC, out NAddr, 4, 0);
                                        WinAPI.ReadProcessMemory(hProcess, NAddr - 0xC, out Len, 4, 0);
                                        if (Len > 0)
                                        {
                                            byte[] Name = new byte[Len *= 2];
                                            WinAPI.ReadProcessMemory(hProcess, NAddr, Name, Len, ref ret);
                                            _Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].Formula_Name = System.Text.Encoding.Unicode.GetString(Name);
                                            Console.WriteLine(_Item._BookInfo[Book_Index]._FormulaInfo[Formula_Index].ID + "=" + System.Text.Encoding.Unicode.GetString(Name));
                                        }
                                    }
                                }
                            }
                            return true;
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;
        }
        //取得配方書籍
        public string GetBookName(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "選擇配方")
                {
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    int Book_ID = 0;
                    //配方書籍ID
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x688, out Book_ID, 4, 0);
                    WinAPI.CloseHandle(hProcess);

                    return LoadItemInfo(Book_ID).Replace("[配方本]", "");
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;
        }
        //取得配方名稱
        public string GetFormulaName(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "生產")
                {
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1

                    int Formula_ID = 0;
                    //配方書籍ID
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5E4, out Formula_ID, 4, 0);
                    WinAPI.CloseHandle(hProcess);

                    return LoadFormulaInfo(Formula_ID);
                }
                WinAPI.CloseHandle(hProcess);
            }
            return null;
        }

        //角色身上跳出的訊息
        public int DiscoverMsgBox(IntPtr hWnd)
        {
            int Msg = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, MsgBoxAddr1, out Msg, 4, 0);
                WinAPI.CloseHandle(hProcess);
            }
            return Msg;
        }
        //訊息框
        public int MsgBox(IntPtr hWnd)
        {
            int Msg = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, MsgBoxAddr2, out Msg, 4, 0);
                WinAPI.CloseHandle(hProcess);
            }
            return Msg;
        }

        public void initMemoryInfo(IntPtr hWnd)
        {
            if (inf.RegionSize != 0)
                return;

            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            long baseAddr = 0x401000;
            WinAPI.VirtualQueryEx((IntPtr)hProcess, (IntPtr)baseAddr, out inf, (uint)Marshal.SizeOf(inf));
            WinAPI.CloseHandle(hProcess);
        }

        //攀談
        public static int TALK_CALL_ADDR;
        public void Talk(IntPtr hWnd, int TargetID)
        {
            if (TALK_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x10;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)buf[i + 0x2] == 0x40)
                        {
                            //mov     byte ptr [esp+C], 0A
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0xA)
                            {
                                //mov     byte ptr [esp+D], 3E
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x3E)
                                {
                                    TALK_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (TALK_CALL_ADDR == 0)
                return;

            AsmClassLibrary asm = new AsmClassLibrary();

            if (!SceneChange(hWnd))
            {
                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8B6870);
                //asm.Mov_EAX(0x8C2B40);
                asm.Mov_EAX(TALK_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //邀請組隊
        public static int INVITE_CALL_ADDR;
        public void Invite(IntPtr hWnd, int TargetID)
        {
            if (INVITE_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x10;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 10
                        if (sByte2 == 0xEC83 && (uint)buf[i + 0x2] == 0x10)
                        {
                            //mov     byte ptr [esp+C], 0C
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0xC)
                            {
                                //mov     byte ptr [esp+D], 3
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x3)
                                {
                                    INVITE_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (INVITE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                //邀請組隊
                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8CF950);
                //asm.Mov_EAX(0x8DBDF0);
                asm.Mov_EAX(INVITE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //接受組隊
        public static int AGREE_CALL_ADDR;
        public void Agree(IntPtr hWnd, int TargetID)
        {
            if (AGREE_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x10;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 10
                        if (sByte2 == 0xEC83 && (uint)buf[i + 0x2] == 0x10)
                        {
                            //mov     byte ptr [esp+C], 0C
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0xC)
                            {
                                //mov     byte ptr [esp+D], 9
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x9)
                                {
                                    AGREE_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (AGREE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8CFA30);
                //asm.Mov_EAX(0x8DBEC0);
                asm.Mov_EAX(AGREE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //跟隨
        public void Follow(IntPtr hWnd, int Place)
        {
            if (!SceneChange(hWnd))//非切換場景
            {
                if (Place != 0x1C)
                {
                    int TargetID = GetPartyLeaderId(hWnd);

                    string Button_Name = "陸地跟隨";
                    if (Place == 0x4)//於海上時 
                        Button_Name = "海上跟隨";

                    if (GetUserId(hWnd) != TargetID && TargetID != 0)//非提督
                        InfoButton(hWnd, TargetID, Button_Name);
                }
            }
        }
        //取消跟隨
        private static int CANCEL_SEA_FOLLOW_CALL_ADDR;
        private static int CANCEL_LAND_FOLLOW_CALL_ADDR;
        private void GetCancelSeaFollowCallAddr(IntPtr hWnd)
        {
            if (CANCEL_SEA_FOLLOW_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0xF;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 20
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                    {
                        //mov     byte ptr [esp+8], 0A
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x8 && (uint)buf[offset + 0x4] == 0x0A)
                        {
                            //mov     byte ptr [esp+9], 48
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x9 && (uint)buf[offset + 0x9] == 0x48)
                            {
                                CANCEL_SEA_FOLLOW_CALL_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }

                }
            }
        }
        private void GetCancelLandFollowCallAddr(IntPtr hWnd)
        {
            if (CANCEL_LAND_FOLLOW_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x10;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 20
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                    {
                        //mov     byte ptr [esp+C], 0A
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x0A)
                        {
                            //mov     byte ptr [esp+D], 49
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x49)
                            {
                                CANCEL_LAND_FOLLOW_CALL_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }

                }
            }
        }
        public void CancelFollow(IntPtr hWnd, int Place)
        {
            GetCancelSeaFollowCallAddr(hWnd);
            GetCancelLandFollowCallAddr(hWnd);
            if (CANCEL_SEA_FOLLOW_CALL_ADDR == 0 || CANCEL_LAND_FOLLOW_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                if (Place != 0x1C)
                {
                    if (Place == 0x4)//於海上時
                    {
                        //Place = 0x8D8AF0;//取消海上跟隨
                        Place = CANCEL_SEA_FOLLOW_CALL_ADDR;
                    }
                    else
                    {
                        //Place = 0x8D8B40;//取消陸地跟隨 
                        Place = CANCEL_LAND_FOLLOW_CALL_ADDR;
                    }

                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Mov_ECX(CALL_ECX);
                    asm.Mov_EAX(Place);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(GetPid(hWnd));
                }
            }
        }
        //變更帆位
        public static int CHANGESAILSTATUS_CALL_ADDR;
        public void ChangeSailStatus(IntPtr hWnd, int status)
        {
            if (CHANGESAILSTATUS_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x4D;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //push    -1
                        if (sByte2 == 0xFF6A)
                        {
                            //mov     byte ptr [esp+30], 0A
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x30 && (uint)buf[offset + 0x4] == 0xA)
                            {
                                //mov     byte ptr [esp+31], 17
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x31 && (uint)buf[offset + 0x9] == 0x17)
                                {
                                    CHANGESAILSTATUS_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (CHANGESAILSTATUS_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(status);//帆位
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8B7200);
                //asm.Mov_EAX(0x8C34C0);
                asm.Mov_EAX(CHANGESAILSTATUS_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
                Console.WriteLine(GetUserName(hWnd) + " 調整船帆");
            }
        }
        //募集船員
        public static int RAISE_CALL_ADDR;
        public void Raise(IntPtr hWnd, int UserId, bool Set)
        {
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));

            if (RAISE_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x20;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 44
                        if (sByte2 == 0xEC83 && (uint)buf[i + 0x2] == 0x44)
                        {
                            //mov     byte ptr [esp+18], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x18 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+19], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x19 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //cmp     edi, 6
                                    if ((uint)(buf[offset + 0x73] | buf[offset + 0x74] << 8) == 0xFF83 && (uint)buf[offset + 0x75] == 0x6)
                                    {
                                        //retn    10
                                        if ((uint)buf[offset + 0xB5] == 0xC2 && (uint)(buf[offset + 0xB6] | buf[offset + 0xB7] << 8) == 0x0010)
                                        {
                                            RAISE_CALL_ADDR = (int)inf.BaseAddress + i;
                                            //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (RAISE_CALL_ADDR == 0)
                return;

            int Crew, NecessaryCrew, MaxCrew;
            WinAPI.ReadProcessMemory(hProcess, CrewAddr, out Crew, 2, 0);//船員數
            WinAPI.ReadProcessMemory(hProcess, MaxCrewAddr, out MaxCrew, 2, 0);//最大船員數
            WinAPI.ReadProcessMemory(hProcess, NecessaryCrewAddr, out NecessaryCrew, 2, 0);//必要船員數
            //需要3個 4Byte去判斷 (慕集的船員等級)
            WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x28, BitConverter.GetBytes(0), 4, 0);
            WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x2C, BitConverter.GetBytes(0), 4, 0);
            WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x30, BitConverter.GetBytes(0), 4, 0);

            if (Set)
                WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x28, BitConverter.GetBytes(MaxCrew - Crew), 4, 0);
            else
                WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x28, BitConverter.GetBytes(NecessaryCrew - Crew), 4, 0);

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(OtherSPACE + 0x28);
                asm.Push(UserId);
                asm.Push(0x0);
                asm.Push(0x22);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8FB8A0);
                //asm.Mov_EAX(0x909520);
                asm.Mov_EAX(RAISE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //補給
        public static int SUPPLY_CALL_ADDR;
        public void Supply(IntPtr hWnd, int UserId, int[] UserN, bool set)
        {
            int[] SetNum = new int[4];
            int[] PossessNum = new int[4];
            int[] SupplyNum = new int[4];

            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (SUPPLY_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x21;
                        if (offset + 0xB3 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 200
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x0200)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //retn    1C
                                    if ((uint)buf[offset + 0xB1] == 0xC2 && (uint)(buf[offset + 0xB2] | buf[offset + 0xB3] << 8) == 0x001C)
                                    {
                                        SUPPLY_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (SUPPLY_CALL_ADDR == 0)
                return;
            
            if (GetMaterialsStatus(hWnd, PossessNum, SetNum))
            {
                bool Check_Supply = false;
                for (int i = 0; i < 4; i++)
                {
                    if (set)//自訂的數量
                    {
                        if (PossessNum[i] >= UserN[i])
                            SupplyNum[i] = 0;
                        else
                            SupplyNum[i] = UserN[i] - PossessNum[i];
                    }
                    else//保存的數量
                    {
                        if (PossessNum[i] >= SetNum[i])
                            SupplyNum[i] = 0;
                        else
                            SupplyNum[i] = SetNum[i] - PossessNum[i];
                    }
                    if (SupplyNum[i] > 0)
                        Check_Supply = true;
                }

                if (!SceneChange(hWnd) && Check_Supply)
                {
                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(SupplyNum[3]);//彈藥
                    asm.Push(SupplyNum[2]);//資材
                    asm.Push(SupplyNum[1]);//糧食
                    asm.Push(SupplyNum[0]);//水
                    asm.Push(UserId);//人物ID
                    asm.Push(0x0);
                    asm.Push(0x13);
                    asm.Mov_ECX(CALL_ECX);
                    //asm.Mov_EAX(0x8FAD80);
                    //asm.Mov_EAX(0x908A00);
                    asm.Mov_EAX(SUPPLY_CALL_ADDR);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(pid);
                    Console.WriteLine(GetUserName(hWnd) + " 補給");
                }
            }
        }
        //進入場景
        public static int INTOSCENE_CALL_ADDR;
        public void IntoScene(IntPtr hWnd, int SceneID)
        {
            if (INTOSCENE_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1D;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 10
                        if (sByte2 == 0xEC83 && (uint)buf[i + 0x2] == 0x10)
                        {
                            //mov     byte ptr [esp+14], 0A
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0xA)
                            {
                                //mov     byte ptr [esp+15], 5B
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x5B)
                                {
                                    INTOSCENE_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (INTOSCENE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                //進入港口
                asm.Pushad();
                asm.Push(SceneID);//場景的ID
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8B6B80);
                //asm.Mov_EAX(0x8C2E40);
                asm.Mov_EAX(INTOSCENE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
                Console.WriteLine(GetUserName(hWnd) + " 切換場景");
            }
        }
        //進入城市
        public static int INTOCITY_CALL_ADDR;
        public void IntoCity(IntPtr hWnd, int PlaceID, int mode)
        {
            if (INTOCITY_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x38;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 100
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x100)
                        {
                            //mov     byte ptr [esp+14], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+15], 2
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x2)
                                {
                                    INTOCITY_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (INTOCITY_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(PlaceID);
                asm.Push(0x0);
                asm.Push(mode);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8F9BE0);
                //asm.Mov_EAX(0x907860);
                asm.Mov_EAX(INTOCITY_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));

                string button_name = null;
                switch (mode)
                {
                    case 0x43: button_name = "前往碼頭";
                        break;
                    case 0x88: button_name = "乘坐馬車";
                        break;
                    case 0x8F: button_name = "港口廣場";
                        break;
                    case 0x90: button_name = "廣場";
                        break;
                    case 0x91: button_name = "商業地區";
                        break;
                    case 0x92: button_name = "商務會館";
                        break;
                    case 0x93: button_name = "陸地探險";
                        break;
                    case 0xCD: button_name = "靠岸";
                        break;
                }
                Console.WriteLine(GetUserName(hWnd) + " " + button_name);
            }
        }
        //出航
        public static int SAILING_CALL_ADDR;
        public void Sailing(IntPtr hWnd, int UserId)
        {
            if (SAILING_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x21;
                        if (offset + 0x33 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 200
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x200)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //push    0
                                    if ((uint)(buf[offset + 0x32] | buf[offset + 0x33] << 8) == 0x006A)
                                    {
                                        SAILING_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (SAILING_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(UserId);
                asm.Push(0x0);
                asm.Push(0x10);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8FAAE0);
                //asm.Mov_EAX(0x908760);
                asm.Mov_EAX(SAILING_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
                Console.WriteLine(GetUserName(hWnd) + " 出航");
            }
        }
        //套餐
        public static int ORDERINGMEAL_CALL_ADDR;
        public void OrderingMeal(IntPtr hWnd, int TargetID, int[] Meal)
        {
            if (ORDERINGMEAL_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x20;
                        if (offset + 0x79 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 44
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x44)
                        {
                            //mov     byte ptr [esp+18], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x18 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+19], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x19 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //cmp     edi, 5
                                    if ((uint)(buf[offset + 0x73] | buf[offset + 0x74] << 8 | buf[offset + 0x75] << 16) == 0x05FF83)
                                    {
                                        //push    13
                                        if ((uint)(buf[offset + 0x78] | buf[offset + 0x79] << 8) == 0x006A)
                                        {
                                            ORDERINGMEAL_CALL_ADDR = (int)inf.BaseAddress + i;
                                            //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ORDERINGMEAL_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));

                //需要5個 2Byte去點餐 (共3個4Byte)
                for (int i = 0; i < 5; i++)
                    WinAPI.WriteProcessMemory(hProcess, OtherSPACE + 0x28 + i * 0x2, BitConverter.GetBytes(Meal[i]), 2, 0);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(OtherSPACE + 0x28);
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x5);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8FA030);
                //asm.Mov_EAX(0x907cb0);
                asm.Mov_EAX(ORDERINGMEAL_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //點餐
        public static int ORDERING_CALL_ADDR;
        public void Ordering(IntPtr hWnd, int TargetID, int Food, int Drink)
        {
            if (ORDERING_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1C;
                        if (offset + 0x65 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+14], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+15], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //push    13
                                    if ((uint)(buf[offset + 0x64] | buf[offset + 0x65] << 8) == 0x136A)
                                    {
                                        ORDERING_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ORDERING_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(new Random().Next(1024));
                asm.Push(Food);
                asm.Push(Drink);
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x1);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8F9D40);
                //asm.Mov_EAX(0x9079C0);
                asm.Mov_EAX(ORDERING_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //款待
        public static int ENTERTAIN_CALL_ADDR;
        public void Entertain(IntPtr hWnd, int TargetID)
        {
            if (ENTERTAIN_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1B;
                        if (offset + 0x53 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //push    0
                                    if ((uint)(buf[offset + 0x39] | buf[offset + 0x3A] << 8) == 0x006A)
                                    {
                                        //push    0F
                                        if ((uint)(buf[offset + 0x52] | buf[offset + 0x53] << 8) == 0x0F6A)
                                        {
                                            ENTERTAIN_CALL_ADDR = (int)inf.BaseAddress + i;
                                            //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ENTERTAIN_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x2);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(ENTERTAIN_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //閱讀書籍
        public static int READBOOK_CALL_ADDR;
        public void Readbook(IntPtr hWnd, int TargetID, int Subject)
        {
            if (READBOOK_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1C;
                        if (offset + 0x86 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+14], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+15], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x04)
                                {
                                    //retn    14
                                    if ((uint)buf[offset + 0x84] == 0xC2 && (uint)(buf[offset + 0x85] | buf[offset + 0x86] << 8) == 0x0014)
                                    {
                                        READBOOK_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (READBOOK_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0);
                asm.Push(new Random().Next(1024));
                asm.Push(Subject);
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x2F);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x89B850);
                //asm.Mov_EAX(0x909C60);
                asm.Mov_EAX(READBOOK_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
            }
        }
        //閱讀書籍(連續)
        public void Readbook_Continuous(IntPtr hWnd, int SubjectIndex)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2
                //學科Index
                WinAPI.WriteProcessMemory(hProcess, WindowBaseAddr2 + 0x90, BitConverter.GetBytes(SubjectIndex), 4, 0);
                //閱讀連續
                WinAPI.WriteProcessMemory(hProcess, WindowBaseAddr1 + 0x914, BitConverter.GetBytes(1), 1, 0);

                WinAPI.CloseHandle(hProcess);
            }
        }
        //生產(連續)
        public void Produce_Continuous(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                if (WindowBaseAddr2 != 0)
                {
                    //連續生產
                    WinAPI.WriteProcessMemory(hProcess, WindowBaseAddr1 + 0x1094, BitConverter.GetBytes(1), 1, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //使用技能
        public static int USESKILL_CALL_ADDR;
        public void UseSkill(IntPtr hWnd, int skill)
        {
            if (USESKILL_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x17;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 20
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                        {
                            //mov     byte ptr [esp+10], 0A
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x0A)
                            {
                                //mov     byte ptr [esp+11], 40
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x40)
                                {
                                    USESKILL_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (USESKILL_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(skill);//技能編號
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8B68E0);
                //asm.Mov_EAX(0x8C2BA0);
                asm.Mov_EAX(USESKILL_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);

                Console.WriteLine(GetUserName(hWnd) + " 使用技能 " + LoadSkillInfo(skill));
            }
        }
        //中斷技能
        private static int STOPSKILL_CALL_ADDR;
        private void GetStopSkillCallAddr(IntPtr hWnd)
        {
            if (STOPSKILL_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x17;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 20
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                    {
                        //mov     byte ptr [esp+10], 0A
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x0A)
                        {
                            //mov     byte ptr [esp+11], 41
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x41)
                            {
                                STOPSKILL_CALL_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void StopSkill(IntPtr hWnd, int Skill)
        {
            GetStopSkillCallAddr(hWnd);
            if (STOPSKILL_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(Skill);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8C2C30); 
                asm.Mov_EAX(STOPSKILL_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //接受委託
        private static int AGREE_MISSION_CALL_ADDR;
        private void GetAgreeMissionCallAddr(IntPtr hWnd)
        {
            if (AGREE_MISSION_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1B;
                        if (offset + 0x77 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x04)
                                {
                                    //retn    10
                                    if ((uint)buf[offset + 0x75] == 0xC2 && (uint)(buf[offset + 0x76] | buf[offset + 0x77] << 8) == 0x0010)
                                    {
                                        AGREE_MISSION_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void AgreeMission(IntPtr hWnd, int TargetID, int Mission)
        {
            getButtonCallAddr(hWnd);
            if (BUTTON_CALL_ADDR == 0)
                return;

            GetAgreeMissionCallAddr(hWnd);
            if (AGREE_MISSION_CALL_ADDR == 0)
                return;
            
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();

                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x48);//接受委托
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x91f110);
                asm.Mov_EAX(BUTTON_CALL_ADDR);
                asm.Call_EAX();

                asm.Push(Mission);
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x1B);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x9209f0);
                asm.Mov_EAX(AGREE_MISSION_CALL_ADDR);
                asm.Call_EAX();

                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //放棄委托
        private static int ABANDON_MISSION_CALL_ADDR;
        private void GetAbandonMissionCallAddr(IntPtr hWnd) {
            if (ABANDON_MISSION_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x10;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+C], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+D], 15
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x15)
                                {
                                    ABANDON_MISSION_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void AbandonMission(IntPtr hWnd, int Mission)
        {
            GetAbandonMissionCallAddr(hWnd);

            if (ABANDON_MISSION_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(Mission);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x929b20);
                asm.Mov_EAX(ABANDON_MISSION_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //委任提督
        private static int CHANGE_ADMIRAL_CALL_ADDR;
        private void GetChangeAdmiralCallAddr(IntPtr hWnd)
        {
            if (CHANGE_ADMIRAL_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x10;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 10
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x10)
                    {
                        //mov     byte ptr [esp+C], C
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x0C)
                        {
                            //mov     byte ptr [esp+D], E
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x0E)
                            {
                                CHANGE_ADMIRAL_CALL_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void ChangeAdmiral(IntPtr hWnd, int TargetID)
        {
            GetChangeAdmiralCallAddr(hWnd);
            if (CHANGE_ADMIRAL_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8f22d0);
                asm.Mov_EAX(CHANGE_ADMIRAL_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //金錢管理
        public static int MONEYMANAGEMENT_CALL_ADDR;
        public void MoneyManagement(IntPtr hWnd, int TargetID, int Money, int SetMoney)
        {
            if (MONEYMANAGEMENT_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1B;
                        if (offset + 0x78 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 40
                        if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //retn    10
                                    if ((uint)buf[offset + 0x76] == 0xC2 && (uint)(buf[offset + 0x77] | buf[offset + 0x78] << 8) == 0x0010)
                                    {
                                        MONEYMANAGEMENT_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (MONEYMANAGEMENT_CALL_ADDR == 0)
                return;

            int mode = 0;
            Money /= 1000;
            if (Money > SetMoney)
            {
                Money = Money - SetMoney;
                mode = 0x2B;//存錢
            }
            else if (Money < SetMoney)
            {
                Money = SetMoney - Money;
                mode = 0x2C;//領錢
            }
            if (mode == 0)
                return;

            if (!SceneChange(hWnd))
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(Money *= 1000);//$$
                asm.Push(TargetID);//銀行NPC ID
                asm.Push(0x0);
                asm.Push(mode);//領錢或存錢
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8FBA20);
                //asm.Mov_EAX(0x907E40);
                asm.Mov_EAX(MONEYMANAGEMENT_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
                Console.WriteLine(GetUserName(hWnd) + " 存取金錢");
            }
        }
        //使用道具
        public static int USEITEM_CALL_ADDR;
        public void UseItem(IntPtr hWnd, int ItemAddr)
        {
            if (USEITEM_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x15;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 400
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x0400)
                        {
                            //mov     byte ptr [esp+14], 0A
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0x0A)
                            {
                                //mov     byte ptr [esp+15], 42
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x42)
                                {
                                    USEITEM_CALL_ADDR = (int)inf.BaseAddress + i;
                                    //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }

            if (USEITEM_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(ItemAddr);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8B69E0);
                //asm.Mov_EAX(0x8C2CA0);
                asm.Mov_EAX(USEITEM_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);

                Console.WriteLine("使用道具");
            }
        }

        //買商品
        private static int BUY_TRADE_CALL_ADDR;
        private void getBuyTradeCallAddr(IntPtr hWnd)
        {
            if (BUY_TRADE_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x2A;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 40C
                    if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x040C)
                    {
                        //mov     byte ptr [esp+20], 11
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x20 && (uint)buf[offset + 0x4] == 0x11)
                        {
                            //mov     byte ptr [esp+21], 4
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x21 && (uint)buf[offset + 0x9] == 0x04)
                            {
                                //retn    14
                                if ((uint)buf[offset + 0xFE] == 0xC2 && (uint)(buf[offset + 0xFF] | buf[offset + 0x100] << 8) == 0x0014)
                                {
                                    BUY_TRADE_CALL_ADDR = (int)inf.BaseAddress + i;
                                    Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void BuyTrade(IntPtr hWnd, int TargetID, Form1.TradeInfo[] Trade, int index)
        {
            getBuyTradeCallAddr(hWnd);
            if (BUY_TRADE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                for (int i = 0; i < index; i++)
                {
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0xC, BitConverter.GetBytes(Trade[i].ID), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0xC + 0x4, BitConverter.GetBytes(Trade[i].Num), 2, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0xC + 0x6, BitConverter.GetBytes(1101), 2, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0xC + 0x8, BitConverter.GetBytes(Trade[i].Price), 4, 0);
                }
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0x4, BitConverter.GetBytes(index), 4, 0);
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0x8, BitConverter.GetBytes(index), 4, 0);
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0xC, BitConverter.GetBytes(TradeSPACE), 4, 0);
                WinAPI.CloseHandle(hProcess);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TradeSPACE - 0x30);
                asm.Push(TradeSPACE - 0x10);
                asm.Push(TargetID);
                asm.Push(0);
                asm.Push(0x16);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(BUY_TRADE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 買商品");
            }
        }
        //賣商品
        private static int SELL_TRADE_CALL_ADDR;
        private void GetSellTradeCallAddr(IntPtr hWnd)
        {
            if (SELL_TRADE_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x2A;
                    if (offset + 0x169 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 40C
                    if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x040C)
                    {
                        //mov     byte ptr [esp+20], 11
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x20 && (uint)buf[offset + 0x4] == 0x11)
                        {
                            //mov     byte ptr [esp+21], 4
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x21 && (uint)buf[offset + 0x9] == 0x04)
                            {
                                //retn    10
                                if ((uint)buf[offset + 0x13D] == 0xC2 && (uint)(buf[offset + 0x13E] | buf[offset + 0x13F] << 8) == 0x0010)
                                {
                                    SELL_TRADE_CALL_ADDR = (int)inf.BaseAddress + i;
                                    Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SellTrade(IntPtr hWnd, int TargetID, Form1.TradeInfo[] Trade, int index)
        {
            GetSellTradeCallAddr(hWnd);
            if (SELL_TRADE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                for (int i = 0; i < index; i++)
                {
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0x10, BitConverter.GetBytes(Trade[i].Code), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0x10 + 0x4, BitConverter.GetBytes(0x10000), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0x10 + 0x8, BitConverter.GetBytes(Trade[i].Num), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TradeSPACE + i * 0x10 + 0xC, BitConverter.GetBytes(Trade[i].Price), 8, 0);
                }
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0x4, BitConverter.GetBytes(index), 4, 0);
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0x8, BitConverter.GetBytes(index), 4, 0);
                WinAPI.WriteProcessMemory(hProcess, TradeSPACE - 0xC, BitConverter.GetBytes(TradeSPACE), 4, 0);
                WinAPI.CloseHandle(hProcess);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TradeSPACE - 0x10);
                asm.Push(TargetID);
                asm.Push(0);
                asm.Push(0x17);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x920630);
                asm.Mov_EAX(SELL_TRADE_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 賣商品");
            }
        }
        //殺價&討價
        private static int HAGGLE_BUY_CALL_ADDR;
        private static int HAGGLE_SELL_CALL_ADDR;
        private void GetHaggleCallAddr(IntPtr hWnd)
        {
            if (HAGGLE_BUY_CALL_ADDR != 0 && HAGGLE_SELL_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x10;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 40
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x40)
                    {
                        //mov     byte ptr [esp+C], 11
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x11)
                        {
                            //mov     byte ptr [esp+D], 1F
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x1F)
                            {
                                HAGGLE_BUY_CALL_ADDR = (int)inf.BaseAddress + i;
                                Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }

                        //mov     byte ptr [esp+C], 11
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x11)
                        {
                            //mov     byte ptr [esp+D], 20
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x20)
                            {
                                HAGGLE_SELL_CALL_ADDR = (int)inf.BaseAddress + i;
                                Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void Haggle(IntPtr hWnd, int TargetID)
        {
            GetHaggleCallAddr(hWnd);
            if (HAGGLE_BUY_CALL_ADDR == 0 && HAGGLE_SELL_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(TargetID);//交易所老闆
                asm.Mov_ECX(CALL_ECX);
                if (CheckWindows(hWnd) == "買入交易物品")
                {
                    //asm.Mov_EAX(0x912050);//殺價
                    asm.Mov_EAX(HAGGLE_BUY_CALL_ADDR);
                }
                else if (CheckWindows(hWnd) == "出售交易物品")
                {
                    //asm.Mov_EAX(0x912150);//要價
                    asm.Mov_EAX(HAGGLE_SELL_CALL_ADDR);
                }
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }

        //選擇配方
        private static int SELECT_FORMULA_CALL_ADDR;
        private void GetSelectFormulaCallAddr(IntPtr hWnd)
        {
            if (SELECT_FORMULA_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0xF;
                    if (offset + 0xC > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //mov     eax, dword ptr [esp+4]
                    if (sByte2 == 0x448B && (uint)(buf[i + 0x2]) == 0x24 && (uint)(buf[i + 0x3]) == 0x04)
                    {
                        //cmp     eax, dword ptr [esi+678]
                        if ((uint)(buf[offset] | buf[offset + 0x1] << 8) == 0x863B && (uint)(buf[offset + 0x2] | buf[offset + 0x3] << 8 | buf[offset + 0x4] << 16 | buf[offset + 0x5] << 24) == 0x00000678)
                        {
                            //jge     006B62F8
                            //mov     edx, dword ptr [esi+674]
                            if ((uint)(buf[offset + 0xC] | buf[offset + 0xD] << 8) == 0x968B && (uint)(buf[offset + 0xE] | buf[offset + 0xF] << 8 | buf[offset + 0x10] << 16 | buf[offset + 0x11] << 24) == 0x00000674)
                            {
                                SELECT_FORMULA_CALL_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void SelectFormula(IntPtr hWnd, int FormulaIndex)
        {
            GetSelectFormulaCallAddr(hWnd);
            if (SELECT_FORMULA_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(FormulaIndex);
                asm.Mov_ECX(GetWindowBaseAddr1(hWnd));
                //asm.Mov_EAX(0x6B6210);
                asm.Mov_EAX(SELECT_FORMULA_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetWindowBaseAddr1(hWnd));
                Console.WriteLine("使用配方");
            }
        }

        public PointF GetTCoordinate(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                float TX, TY;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                //目地座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x20, out TX, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x28, out TY, 4, 0);
                WinAPI.CloseHandle(hProcess);
                return new PointF(TX, TY);
            }
            return PointF.Empty;
            //測量座標 = 人物座標 / 10000 + 海域座標 * 256
            //港口座標 37000 35000
        }
        //座標是否變更
        public bool CoordinateChange(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                float X, Y, TX, TY;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                //當前座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr, out X, 4, 0);//座標X
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x8, out Y, 4, 0);//座標Y
                //目地座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x20, out TX, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x28, out TY, 4, 0);
                WinAPI.CloseHandle(hProcess);
                if (X != TX || Y != TY)
                    return true;
            }
            return false;
            //測量座標 = 人物座標 / 10000 + 海域座標 * 256
            //港口座標 37000 35000
        }

        //瞬移(改記憶體 無CALL)
        public void MoveCoordinate(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                float X, Y, TX, TY;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //當前座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr, out X, 4, 0);//座標X
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x8, out Y, 4, 0);//座標Y
                //目地座標
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x20, out TX, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x28, out TY, 4, 0);

                if (X != TX || Y != TY)
                {
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr, BitConverter.GetBytes(TX), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x8, BitConverter.GetBytes(TY), 4, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //移動(瞬移)
        private static int MOVE_CALL_ADDR;
        private void GetMoveCallAddr(IntPtr hWnd)
        {
            if (MOVE_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x19;
                    if (offset + 0xA > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 20
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                    {
                        //mov     dword ptr [esp+10], ecx
                        if ((uint)buf[offset] == 0x89 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x244C && (uint)buf[offset + 0x3] == 0x10)
                        {
                            //mov     dword ptr [esp+14], edx
                            if ((uint)buf[offset + 0x7] == 0x89 && (uint)(buf[offset + 0x8] | buf[offset + 0x9] << 8) == 0x2454 && (uint)buf[offset + 0xA] == 0x14)
                            {
                                MOVE_CALL_ADDR = (int)inf.BaseAddress + i;
                                Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void MoveCoordinate(IntPtr hWnd, PointF Coordinate, bool IsWalk)
        {
            GetMoveCallAddr(hWnd);
            if (MOVE_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                int Move = 0;
                float X, Y, Z;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, MoveAddrPtr, out Move, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr, out X, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x4, out Z, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CoordinateAddr + 0x8, out Y, 4, 0);

                if ((Coordinate.X != X || Coordinate.Y != Y) && Move != 0)
                {
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x100, BitConverter.GetBytes(Coordinate.X - X), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x104, BitConverter.GetBytes(0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x108, BitConverter.GetBytes(Coordinate.Y - Y), 4, 0);
                    //WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x10C, BitConverter.GetBytes(0), 4, 0);

                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x110, BitConverter.GetBytes(Coordinate.X), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x114, BitConverter.GetBytes(0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x118, BitConverter.GetBytes(Coordinate.Y), 4, 0);
                    //WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x11C, BitConverter.GetBytes(0), 4, 0);

                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(0x0);
                    asm.Push(0x0);
                    asm.Push(CoordinateAddr + 0x100);//目的地與人物座標的距離XYZ
                    asm.Push(CoordinateAddr + 0x110);//目的座標XYZ
                    asm.Mov_ECX(Move);
                    //asm.Mov_EAX(0x809410);
                    asm.Mov_EAX(MOVE_CALL_ADDR);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(pid);
                    Console.WriteLine(GetUserName(hWnd) + " 移動座標" + Coordinate);

                    if (!IsWalk)
                    {
                        WinAPI.WriteProcessMemory(hProcess, CoordinateAddr, BitConverter.GetBytes(Coordinate.X), 4, 0);
                        WinAPI.WriteProcessMemory(hProcess, CoordinateAddr + 0x8, BitConverter.GetBytes(Coordinate.Y), 4, 0);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //移動(瞬移)到附近座標
        public void MoveNearbyCoordinate(IntPtr hWnd, PointF Coordinate, bool IsWalk)
        {
            if (!SceneChange(hWnd))
            {
                PointF Target_Coordinate = GetCoordinate(hWnd);
                do
                {
                    Target_Coordinate = new PointF((Target_Coordinate.X + Coordinate.X) / 2, (Target_Coordinate.Y + Coordinate.Y) / 2);
                    Console.WriteLine("距離：" + Distance(Target_Coordinate, Coordinate));
                } while (Distance(Target_Coordinate, Coordinate) > 450);

                MoveCoordinate(hWnd, Target_Coordinate, IsWalk);
            }
        }

        //檢查是否轉向
        public bool Destinations(IntPtr hWnd, ref Form1.uInfo.Destinations _Destinations)
        {
            if (!SceneChange(hWnd))
            {
                int MoveAddr;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                float X, Y;
                int PlaceNo;
                WinAPI.ReadProcessMemory(hProcess, MoveAddrPtr, out MoveAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, MoveAddr + 0xF4, out X, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, MoveAddr + 0xFC, out Y, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, PlaceNoAddr + 0x2, out PlaceNo, 1, 0);//城市、海域編號

                WinAPI.CloseHandle(hProcess);

                if (_Destinations.Coordinate != new PointF(X, Y))
                {
                    _Destinations.Coordinate = new PointF(X, Y);
                    if (_Destinations.PlaceNo != PlaceNo)
                        _Destinations.PlaceNo = PlaceNo;
                    else
                        return true;
                }
            }
            return false;
        }
        //船隻轉向
        public void Turn(IntPtr hWnd, float TX, float TY)
        {
            if (!SceneChange(hWnd))
            {
                int MoveAddr;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                PointF Coordinate = GetCoordinate(hWnd);
                float TempX = TX - Coordinate.X, TempY = TY - Coordinate.Y;
                float R = (float)Math.Sqrt(TempX * TempX + TempY * TempY);
                float Cos = TempX / R;
                float Sin = TempY / R;

                WinAPI.ReadProcessMemory(hProcess, MoveAddrPtr, out MoveAddr, 4, 0);
                WinAPI.WriteProcessMemory(hProcess, MoveAddr + 0x104, BitConverter.GetBytes(Cos), 4, 0);//轉向Cos
                WinAPI.WriteProcessMemory(hProcess, MoveAddr + 0x10C, BitConverter.GetBytes(Sin), 4, 0);//轉向Sin
                WinAPI.WriteProcessMemory(hProcess, MoveAddr + 0x114, BitConverter.GetBytes(1), 4, 0);//轉向
                WinAPI.CloseHandle(hProcess);
            }
        }

        //視窗文字
        public void PostMessage(IntPtr hWnd, string Text)
        {
            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //int Post = 1;
                //WinAPI.ReadProcessMemory(hProcess, PostMessageAddr + 0x8, out Post, 4, 0);
                //if (Post == 0)
                {
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x4, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x8, BitConverter.GetBytes(TextSPACE + 0x10), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0xC, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x10, BitConverter.GetBytes(0x14C0000), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x14, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x18, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x1C, BitConverter.GetBytes(TextSPACE + 0x20), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x20, BitConverter.GetBytes(0xFFFF), 4, 0);//音效
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x24, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x28, BitConverter.GetBytes(0x7), 4, 0);//顏色
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x2C, BitConverter.GetBytes(0xFFFF8001), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x30, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x34, BitConverter.GetBytes(0x0), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x39, Encoding.Unicode.GetBytes(Text), Encoding.Unicode.GetBytes(Text).Length, 0);
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x39 + Encoding.Unicode.GetBytes(Text).Length, BitConverter.GetBytes(0x0), 4, 0);

                    WinAPI.WriteProcessMemory(hProcess, PostMessageAddr, BitConverter.GetBytes(TextSPACE), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, PostMessageAddr + 0x4, BitConverter.GetBytes(TextSPACE), 4, 0);
                    WinAPI.WriteProcessMemory(hProcess, PostMessageAddr + 0x8, BitConverter.GetBytes(1), 4, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
        }


        //BaseAddr1
        public int GetWindowBaseAddr1(IntPtr hWnd)
        {
            int WindowBaseAddr1 = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int tier;
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddrPtr1 + 0x8, out tier, 4, 0);
                if (tier == 2)
                {
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddrPtr1, out WindowBaseAddr1, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1, out WindowBaseAddr1, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x8, out WindowBaseAddr1, 4, 0);
                }
                else
                {
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddrPtr1 + 0x4, out WindowBaseAddr1, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x4, out WindowBaseAddr1, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x8, out WindowBaseAddr1, 4, 0);
                }
                WinAPI.CloseHandle(hProcess);
            }
            //WindowBaseAddr1 = BaseAddr1
            return WindowBaseAddr1;
        }
        //BaseAddr2
        public int GetWindowBaseAddr2(IntPtr hWnd)
        {
            int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);
            int WindowBaseAddr2 = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x20, out WindowBaseAddr2, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x20, out WindowBaseAddr2, 4, 0);
                WinAPI.CloseHandle(hProcess);
            }
            //WindowBaseAddr2 = BaseAddr2
            return WindowBaseAddr2;
        }

        //NPC對話(劇情)
        public void BusyMode(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int BusyMode = 0;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, BusyModeAddr, out BusyMode, 4, 0);
                if (BusyMode == 1)//NPC對話(劇情)
                {
                    WinAPI.PostMessage(hWnd, WinAPI.WM_KEYDOWN, (IntPtr)Keys.Escape, MakeLParam((uint)Keys.Escape, "VM_KEYDOWN"));
                    WinAPI.PostMessage(hWnd, WinAPI.WM_KEYUP, (IntPtr)Keys.Escape, MakeLParam((uint)Keys.Escape, "VM_KEYUP"));
                }
                WinAPI.CloseHandle(hProcess);
            }
        }

        //檢查是否有視窗打開
        public string CheckWindows(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1

                int WindowCode;
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x4, out WindowCode, 4, 0);
                switch (WindowCode)
                {
                    case 0:
                        return null;
                    case 0x75C6:
                        return "物品欄";
                    case 0x75EF:
                        return "副官情報";
                    case 0x75D9:
                        return "技能";
                }

                int WindowTitleAddr = 0, TitleLen = 0, ret = 0, HasWindow;
                /*
                int HasHideWindowAddr = 0xDD75F4, HasHideWindow;
                WinAPI.ReadProcessMemory(hProcess, HasHideWindowAddr, out HasHideWindow, 4, 0);
                 */

                /*
                byte[] bytes = Encoding.Unicode.GetBytes("您的個人資訊。");
                for (int i = 0; i < bytes.Length; i++)
                Console.Write(Convert.ToString(bytes[i], 16) + " ");
                Console.WriteLine();
                */

                WinAPI.ReadProcessMemory(hProcess, HasWindowAddr, out HasWindow, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowTitleAddrPtr, out WindowTitleAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, WindowTitleAddr - 0xC, out TitleLen, 4, 0);

                if (HasWindow != 0 && TitleLen > 0)
                {
                    byte[] Title = new byte[TitleLen *= 2];
                    WinAPI.ReadProcessMemory(hProcess, WindowTitleAddr, Title, TitleLen, ref ret);
                    //Console.WriteLine(System.Text.Encoding.Unicode.GetString(Title));

                    switch (System.Text.Encoding.Unicode.GetString(Title))
                    {
                        case "您的個人資訊。":
                            return "人物情報";
                        case "僱用的副官情報。":
                            return "副官情報";
                        case "技能情報。":
                            return "技能";
                        case "所持物品情報。":
                            return "持有物品";
                        case "請選擇要使用的配方。":
                            return "選擇配方";
                        case "根據所選擇的配方進行生產。如果能製作的物品是材料的話，任何顏色的東西都能當做材料。":
                            return "生產";
                        case "請選擇要購買的交易品。按住Ctrl鍵點擊可一併購買。如已使用進貨採買書時，將在交易時一併消費":
                            return "買入交易物品";
                        case "請選擇要賣出的交易物品。":
                            return "出售交易物品";
                        case "請從菜單中選擇飲食品。會因飲食品的不同來恢復行動力。":
                            return "點餐";
                        case "請依序選點套餐":
                            return "套餐";
                        case "選擇要接受的任務":
                            return "接受委託";
                        /*
                        case 0xC4D564:
                            return "閱覽書籍";
                        case 0xB87CA4:
                            return "出售船隻";
                        case 0xBA0FEC:
                            return "選擇遊戲世界";//
                        case 0xB9DBA4:
                            return "選擇角色";//
                        case 0xB9DCFC:
                            return "開頭畫面";//
                        */
                    }
                }
            }
            return "???";
        }

        //關閉視窗欄
        public void CloseWindows(IntPtr hWnd)
        {
            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                WinAPI.PostMessage(hWnd, WinAPI.WM_KEYDOWN, (IntPtr)Keys.Escape, MakeLParam((uint)Keys.Escape, "VM_KEYDOWN"));
                WinAPI.PostMessage(hWnd, WinAPI.WM_KEYUP, (IntPtr)Keys.Escape, MakeLParam((uint)Keys.Escape, "VM_KEYUP"));
                Console.WriteLine(GetUserName(hWnd) + " 關閉視窗欄");
            }
        }
        //關閉音效
        public static int SOUND_ADDR;//0x4C0860;// 0x4BE8F0;
        private void GetSoundAddr(IntPtr hWnd)
        {
            if (SOUND_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0xD;
                    if (offset + 0x7 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //call    00xxxxxx
                    if (sByte == 0xE8)
                    {
                        //push    0
                        if ((uint)buf[offset] == 0x6A && (uint)buf[offset + 0x1] == 0x00)
                        {
                            //push    1000
                            if ((uint)buf[offset + 0x2] == 0x68 && (uint)(buf[offset + 0x3] | buf[offset + 0x4] << 8 | buf[offset + 0x5] << 16 | buf[offset + 0x6] << 24) == 0x00001000)
                            {
                                //push    ecx
                                if ((uint)buf[offset + 0x7] == 0x51)
                                {
                                    //push    0
                                    if ((uint)buf[offset + 0x8] == 0x6A && (uint)buf[offset + 0x9] == 0x00)
                                    {
                                        //push    edx
                                        if ((uint)buf[offset + 0xA] == 0x52)
                                        {
                                            SOUND_ADDR = (int)inf.BaseAddress + i;
                                            //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
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
        public void Sound(IntPtr hWnd, bool Set)
        {
            GetSoundAddr(hWnd);
            if (SOUND_ADDR == 0)
                return;

            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            if (Set)
                WinAPI.WriteProcessMemory(hProcess, SOUND_ADDR, BitConverter.GetBytes(0xE8), 1, 0);
            else
                WinAPI.WriteProcessMemory(hProcess, SOUND_ADDR, BitConverter.GetBytes(0xC3), 1, 0);
            WinAPI.CloseHandle(hProcess);
        }

        //打開物品欄
        private static int OPEN_ITEMBOX_ADDR;
        private void GetOpenItemBoxCallAddr(IntPtr hWnd)
        {
            if (OPEN_ITEMBOX_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x10;
                    if (offset + 0xA > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 20
                    if (sByte2 == 0xEC83 && (uint)(buf[i + 0x2]) == 0x20)
                    {
                        //mov     byte ptr [esp+C], 0A
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0xC && (uint)buf[offset + 0x4] == 0x0A)
                        {
                            //mov     byte ptr [esp+D], 30
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0xD && (uint)buf[offset + 0x9] == 0x30)
                            {
                                OPEN_ITEMBOX_ADDR = (int)inf.BaseAddress + i;
                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                            }
                        }
                    }
                }
            }
        }
        public void OpenItemBox(IntPtr hWnd)
        {
            GetOpenItemBoxCallAddr(hWnd);
            if (OPEN_ITEMBOX_ADDR == 0)
                return;

            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                int pid = GetPid(hWnd); 

                AsmClassLibrary asm = new AsmClassLibrary();

                //開物品欄
                asm.Pushad();
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8D8710);
                asm.Mov_EAX(OPEN_ITEMBOX_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 打開物品欄");
            }
        }

        //打開資訊欄
        private static int BUTTON_CALL_ADDR;
        private void getButtonCallAddr(IntPtr hWnd)
        {
            if (BUTTON_CALL_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x37;
                    if (offset + 0x9 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //sub     esp, 40C
                    if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x0100)
                    {
                        //mov     byte ptr [esp+14], 11
                        if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x14 && (uint)buf[offset + 0x4] == 0x11)
                        {
                            //mov     byte ptr [esp+15], 1
                            if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x15 && (uint)buf[offset + 0x9] == 0x01)
                            {
                                //retn    10
                                //if ((uint)buf[offset + 0x13D] == 0xC2 && (uint)(buf[offset + 0x13E] | buf[offset + 0x13F] << 8) == 0x0010)
                                {
                                    BUTTON_CALL_ADDR = (int)inf.BaseAddress + i;
                                    Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void InfoButton(IntPtr hWnd, int ID, string Button_Name)
        {
            getButtonCallAddr(hWnd);
            if (BUTTON_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                int Mode = 0;
                switch (Button_Name)
                {
                    case "技能":
                        Mode = 0x10;
                        break;
                    case "持有物品":
                        Mode = 0x12;
                        break;
                    case "副官情報":
                        Mode = 0x13;
                        break;
                    case "海上跟隨":
                        Mode = 0x3C;
                        break;
                    case "陸地跟隨":
                        Mode = 0x3D;
                        break;
                    case "接受委托":
                        Mode = 0x48;
                        break;
                    case "購買商品":
                        Mode = 0x4D;
                        break;
                    case "賣出商品":
                        Mode = 0x4E;
                        break;
                    case "賣船":
                        Mode = 0x54;
                        break;
                    case "點餐":
                        Mode = 0x71;
                        break;
                    case "閱覽書籍":
                        Mode = 0x84;
                        break;
                }

                if (Mode != 0)
                {
                    int pid = GetPid(hWnd);
                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(ID);
                    asm.Push(0x0);
                    asm.Push(Mode);
                    asm.Mov_ECX(CALL_ECX);
                    //asm.Mov_EAX(0x91f110);
                    asm.Mov_EAX(BUTTON_CALL_ADDR);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(pid);
                }
                Console.WriteLine(GetUserName(hWnd) + " " + Button_Name);
            }
        }

        //設定物品欄是否可打開
        public void SetItemBox(IntPtr hWnd, bool Set)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            int SetAddr = 0x435E85; // 0x43591A; //0x434F0A;

            if (Set)
                WinAPI.WriteProcessMemory(hProcess, SetAddr, BitConverter.GetBytes(0x75), 1, 0);
            else
                WinAPI.WriteProcessMemory(hProcess, SetAddr, BitConverter.GetBytes(0xEB), 1, 0);
            WinAPI.CloseHandle(hProcess);
        }

        //物品欄顯示模式
        private static int ITEM_BOX_STATE1_ADDR;//0x43D8E0 ADDR2-0x59 call ADDR1;//
        private static int ITEM_BOX_STATE2_ADDR;//0x457367//
        private void GetItemBoxStateAddr(IntPtr hWnd)
        {
            if (ITEM_BOX_STATE1_ADDR != 0 && ITEM_BOX_STATE2_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x87;
                    if (offset + 0x125 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //mov     eax, dword ptr fs:[0]
                    if (sByte2 == 0xFF6A)
                    {
                        //lea     ecx, dword ptr [eax+1AC0]
                        if ((uint)(buf[offset] | buf[offset + 0x1] << 8) == 0x888D && (uint)(buf[offset + 0x2] | buf[offset + 0x3] << 8 | buf[offset + 0x4] << 16 | buf[offset + 0x5] << 24) == 0x00001AC0)
                        {
                            //mov     eax, dword ptr [ecx]
                            if ((uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x018B)
                            {
                                //mov     edx, dword ptr [eax+C]
                                if ((uint)(buf[offset + 0x8] | buf[offset + 0x9] << 8) == 0x508B && (uint)buf[offset + 0xA] == 0x0C)
                                {
                                    //push    0
                                    if ((uint)buf[offset + 0xB] == 0x6A && (uint)buf[offset + 0xC] == 0x00)
                                    {
                                        //push    0
                                        if ((uint)buf[offset + 0xD] == 0x6A && (uint)buf[offset + 0xE] == 0x00)
                                        {
                                            //push    13D
                                            if ((uint)buf[offset + 0xF] == 0x68 && (uint)(buf[offset + 0x10] | buf[offset + 0x11] << 8 | buf[offset + 0x12] << 16 | buf[offset + 0x13] << 24) == 0x0000013D)
                                            {
                                                int Addr;
                                                ITEM_BOX_STATE2_ADDR = (int)inf.BaseAddress + i + 0x121;
                                                WinAPI.ReadProcessMemory(hProcess, ITEM_BOX_STATE2_ADDR - 0x59 + 0x1, out Addr, 4, 0);
                                                ITEM_BOX_STATE1_ADDR = ITEM_BOX_STATE2_ADDR - 0x59 - (int)(0xFFFFFFFF - (uint)Addr) + 0x4;
                                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
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
        public string Check_Display_ItemBox(IntPtr hWnd)
        {
            GetItemBoxStateAddr(hWnd);
            if (ITEM_BOX_STATE1_ADDR == 0 || ITEM_BOX_STATE2_ADDR == 0)
                return "顯示";

            int Main, ItemBox;
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            WinAPI.ReadProcessMemory(hProcess, ITEM_BOX_STATE1_ADDR, out Main, 1, 0);
            WinAPI.ReadProcessMemory(hProcess, ITEM_BOX_STATE2_ADDR + 0x1, out ItemBox, 1, 0);
            WinAPI.CloseHandle(hProcess);

            if (Main == 0x56 && ItemBox == 0x0)
                return "顯示";
            if (Main == 0xC3 && ItemBox == 0x1)
                return "隱藏";
            return null;
        }
        //顯示&隱藏物品欄
        public void DisplayItemBox(IntPtr hWnd, bool set)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (set)
            {
                //還原主畫面消失
                WinAPI.WriteProcessMemory(hProcess, ITEM_BOX_STATE1_ADDR, BitConverter.GetBytes(0x56), 1, 0);
                //還原物品欄顯示
                WinAPI.WriteProcessMemory(hProcess, ITEM_BOX_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x0), 1, 0);
            }
            else
            {
                //不讓主畫面消失
                WinAPI.WriteProcessMemory(hProcess, ITEM_BOX_STATE1_ADDR, BitConverter.GetBytes(0xC3), 1, 0);
                //不讓物品欄顯示 
                WinAPI.WriteProcessMemory(hProcess, ITEM_BOX_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x1), 1, 0);
            }
            WinAPI.CloseHandle(hProcess);
        }
        //取得物品資料
        public bool GetItemAddr(IntPtr hWnd, ref Form1.uInfo.Item _Item)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "物品欄")
                {
                    int[] AllItemAddr = new int[255];
                    int Select, Total, ItemAddrPtr1, ItemAddrPtr2;

                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    //第一項物品PTR
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x6B8, out Select, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x8FC, out ItemAddrPtr1, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x904, out Total, 4, 0);

                    _Item.CuisineNum = 0;
                    _Item.SeaItemNum = 0;
                    _Item.LandItemNum = 0;
                    _Item.BookNum = 0;

                    for (int i = 0; i < Total; i++)
                    {
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr1 + 0x8, out ItemAddrPtr2, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr2 + 0xC, out AllItemAddr[i], 4, 0);

                        int Item1, Item2, Item4, Item6, ItemID;
                        WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x8, out ItemID, 4, 0);

                        string CN = LoadCuisineInfo(ItemID);
                        string IN = LoadItemInfo(ItemID);

                        if (CN != null)
                        {
                            //Console.WriteLine(Convert.ToString(AllItemAddr[i], 16) + "=>" + CN);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x0, out Item1, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum, BitConverter.GetBytes(Item1), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x4, out Item2, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum + 0x4, BitConverter.GetBytes(Item2), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x8, out ItemID, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum + 0x8, BitConverter.GetBytes(ItemID), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0xC, out Item4, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum + 0xC, BitConverter.GetBytes(Item4), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x10, out _Item._CuisineInfo[_Item.CuisineNum].Num, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum + 0x10, BitConverter.GetBytes(_Item._CuisineInfo[_Item.CuisineNum].Num), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x14, out Item6, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, CuisineSPACE + 0x28 * _Item.CuisineNum + 0x14, BitConverter.GetBytes(Item6), 4, 0);

                            _Item._CuisineInfo[_Item.CuisineNum].ID = ItemID;
                            _Item._CuisineInfo[_Item.CuisineNum].Code = CuisineSPACE + 0x28 * _Item.CuisineNum;
                            _Item._CuisineInfo[_Item.CuisineNum].Name = CN;
                            Console.WriteLine(_Item._CuisineInfo[_Item.CuisineNum].Name);
                            _Item.CuisineNum++;
                        }
                        else if (IN != null && IN.Contains("[採買書]"))
                        {
                            if (IN.Contains("1"))
                                _Item.LandItemNum = 0;
                            if (IN.Contains("2"))
                                _Item.LandItemNum = 1;
                            if (IN.Contains("3"))
                                _Item.LandItemNum = 2;
                            if (IN.Contains("4"))
                                _Item.LandItemNum = 3;

                            //Console.WriteLine(Convert.ToString(AllItemAddr[i], 16) + "=>" + IN);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i], out Item1, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum, BitConverter.GetBytes(Item1), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x4, out Item2, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum + 0x4, BitConverter.GetBytes(Item2), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x8, out ItemID, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum + 0x8, BitConverter.GetBytes(ItemID), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0xC, out Item4, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum + 0xC, BitConverter.GetBytes(Item4), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x10, out _Item._LandItemInfo[_Item.LandItemNum].Num, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum + 0x10, BitConverter.GetBytes(_Item._LandItemInfo[_Item.LandItemNum].Num), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x14, out Item6, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, LandItemSPACE + 0x28 * _Item.LandItemNum + 0x14, BitConverter.GetBytes(Item6), 4, 0);

                            _Item._LandItemInfo[_Item.LandItemNum].ID = ItemID;
                            _Item._LandItemInfo[_Item.LandItemNum].Code = LandItemSPACE + 0x28 * _Item.LandItemNum;
                            _Item._LandItemInfo[_Item.LandItemNum].Name = IN.Replace("[採買書]", "");
                            //Console.WriteLine(_LandItemInfo[LandItemNum].Name);
                            _Item.LandItemNum = 4;
                        }
                        else if (IN != null && IN.Contains("[消災]"))
                        {
                            //Console.WriteLine(Convert.ToString(AllItemAddr[i], 16) + "=>" + IN);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i], out Item1, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum, BitConverter.GetBytes(Item1), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x4, out Item2, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum + 0x4, BitConverter.GetBytes(Item2), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x8, out ItemID, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum + 0x8, BitConverter.GetBytes(ItemID), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0xC, out Item4, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum + 0xC, BitConverter.GetBytes(Item4), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x10, out _Item._SeaItemInfo[_Item.SeaItemNum].Num, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum + 0x10, BitConverter.GetBytes(_Item._SeaItemInfo[_Item.SeaItemNum].Num), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x14, out Item6, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, SeaItemSPACE + 0x28 * _Item.SeaItemNum + 0x14, BitConverter.GetBytes(Item6), 4, 0);

                            _Item._SeaItemInfo[_Item.SeaItemNum].ID = ItemID;
                            _Item._SeaItemInfo[_Item.SeaItemNum].Code = SeaItemSPACE + 0x28 * _Item.SeaItemNum;
                            _Item._SeaItemInfo[_Item.SeaItemNum].Name = IN.Replace("[消災]", "");
                            //Console.WriteLine(_SeaItemInfo[SeaItemNum].Name);
                            ++_Item.SeaItemNum;
                        }
                        else if (IN != null && IN.Contains("[配方本]"))
                        {
                            //Console.WriteLine(Convert.ToString(AllItemAddr[i], 16) + "=>" + IN);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i], out Item1, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum, BitConverter.GetBytes(Item1), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x4, out Item2, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum + 0x4, BitConverter.GetBytes(Item2), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x8, out ItemID, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum + 0x8, BitConverter.GetBytes(ItemID), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0xC, out Item4, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum + 0xC, BitConverter.GetBytes(Item4), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x10, out _Item._BookInfo[_Item.BookNum].Num, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum + 0x10, BitConverter.GetBytes(_Item._BookInfo[_Item.BookNum].Num), 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AllItemAddr[i] + 0x14, out Item6, 4, 0);
                            WinAPI.WriteProcessMemory(hProcess, BookSPACE + 0x28 * _Item.BookNum + 0x14, BitConverter.GetBytes(Item6), 4, 0);

                            _Item._BookInfo[_Item.BookNum].ID = ItemID;
                            _Item._BookInfo[_Item.BookNum].Code = BookSPACE + 0x28 * _Item.BookNum;
                            _Item._BookInfo[_Item.BookNum].Name = IN.Replace("[配方本]", "");
                            //Console.WriteLine(_Item._BookInfo[_Item.BookNum].Name);
                            ++_Item.BookNum;
                        }
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr1, out ItemAddrPtr1, 4, 0);
                    }
                    WinAPI.CloseHandle(hProcess);
                    return true;
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;

        }
        //取得配方本Addr
        public int GetBookAddr(IntPtr hWnd, string Book_Name)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "物品欄")
                {
                    int AllItemAddr; ;
                    int Total, ItemAddrPtr1, ItemAddrPtr2;

                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    //第一項物品PTR
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x8D8, out Total, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x8D0, out ItemAddrPtr1, 4, 0);

                    for (int i = 0; i < Total; i++)
                    {
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr1 + 0x8, out ItemAddrPtr2, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr2 + 0xC, out AllItemAddr, 4, 0);

                        int ItemID;
                        WinAPI.ReadProcessMemory(hProcess, AllItemAddr + 0x8, out ItemID, 4, 0);

                        if (!string.IsNullOrWhiteSpace(LoadItemInfo(ItemID)) && LoadItemInfo(ItemID).Replace("[配方本]", "") == Book_Name)
                        {
                            WinAPI.CloseHandle(hProcess);
                            return AllItemAddr;
                        }
                        WinAPI.ReadProcessMemory(hProcess, ItemAddrPtr1, out ItemAddrPtr1, 4, 0);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return 0;
        }

        //設定資訊欄是否可打開
        public void SetInfoBox(IntPtr hWnd, bool Set)
        {
            if (!SceneChange(hWnd) && !Busy(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                int ADDR = 0x4373D0; // 0x436F90; // 0x436580;

                if (Set)
                    WinAPI.WriteProcessMemory(hProcess, ADDR, BitConverter.GetBytes(0xCE8B57), 3, 0);
                else
                    WinAPI.WriteProcessMemory(hProcess, ADDR, BitConverter.GetBytes(0x9006EB), 3, 0);
                WinAPI.CloseHandle(hProcess);
            }
        }

        //技能資訊欄顯示模式
        private static int SKILL_INFO_STATE1_ADDR;//0x43CB10 ADDR2-0x65 call ADDR1;
        private static int SKILL_INFO_STATE2_ADDR;//0x443165
        private void GetSkillInfoStateAddr(IntPtr hWnd)
        {
            if (SKILL_INFO_STATE1_ADDR != 0 && SKILL_INFO_STATE2_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x15;
                    if (offset + 0x85 > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //push    -1
                    if (sByte2 == 0xFF6A)
                    {
                        //push    ecx
                        if ((uint)buf[offset] == 0x51 && (uint)buf[offset + 0x1] == 0x57)
                        {
                            //push    1774
                            if ((uint)buf[offset + 0x2] == 0x68 && (uint)(buf[offset + 0x3] | buf[offset + 0x4] << 8 | buf[offset + 0x5] << 16 | buf[offset + 0x6] << 24) == 0x00001774)
                            {
                                //push    10
                                if ((uint)(buf[offset + 0x7] | buf[offset + 0x8] << 8) == 0x106A)
                                {
                                    int Addr;
                                    SKILL_INFO_STATE2_ADDR = (int)inf.BaseAddress + i + 0x85;
                                    WinAPI.ReadProcessMemory(hProcess, SKILL_INFO_STATE2_ADDR - 0x65 + 0x1, out Addr, 4, 0);
                                    SKILL_INFO_STATE1_ADDR = SKILL_INFO_STATE2_ADDR - 0x65 - (int)(0xFFFFFFFF - (uint)Addr) + 0x4;
                                    Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                }
                            }
                        }
                    }
                }
            }
        }
        public string Check_Display_SkillInfo(IntPtr hWnd)
        {
            GetSkillInfoStateAddr(hWnd);
            if (SKILL_INFO_STATE1_ADDR == 0 || SKILL_INFO_STATE2_ADDR == 0)
                return "顯示";

            if (!SceneChange(hWnd))
            {
                int state1, state2;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, SKILL_INFO_STATE1_ADDR, out state1, 3, 0);
                WinAPI.ReadProcessMemory(hProcess, SKILL_INFO_STATE2_ADDR + 0x1, out state2, 1, 0);
                WinAPI.CloseHandle(hProcess);

                if (state1 == 0xF18B56 && state2 == 0x0)
                    return "顯示";
                if (state1 == 0x0008C2 && state2 == 0x1)
                    return "隱藏";
            }
            return null;
        }
        //顯示&隱藏技能資訊欄
        public void DisplaySkillInfo(IntPtr hWnd, bool set)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (set)
            {
                //還原主畫面消失
                /*
                 * 56       push esi
                 * 8BF1     mov esi, ecx
                 */
                WinAPI.WriteProcessMemory(hProcess, SKILL_INFO_STATE1_ADDR, BitConverter.GetBytes(0xF18B56), 3, 0);
                //還原資訊欄
                WinAPI.WriteProcessMemory(hProcess, SKILL_INFO_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x0), 1, 0);
            }
            else
            {
                /*
                 * retn 8
                 */
                //不讓主畫面消失
                WinAPI.WriteProcessMemory(hProcess, SKILL_INFO_STATE1_ADDR, BitConverter.GetBytes(0x0008C2), 3, 0);
                //不讓資訊欄顯示
                WinAPI.WriteProcessMemory(hProcess, SKILL_INFO_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x1), 1, 0);
            }

            WinAPI.CloseHandle(hProcess);
        }
        //取得技能資料
        public bool GetSkillInfo(IntPtr hWnd, ref int Num, Form1.uInfo.SkillInfo[] skill)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "技能")
                {
                    int Index, SkillAddr, SkillNum, AllNum;
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    //技能情報 - 選擇頁面
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x63C, out Index, 4, 0);

                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x398, out SkillAddr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x39C, out SkillNum, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr2 + 0x3A0, out AllNum, 4, 0);
                    if (SkillAddr > 0)
                    {
                        Num = SkillNum;
                        int offset = 0x5C;
                        for (int i = 0; i < AllNum; i++)
                        {
                            WinAPI.ReadProcessMemory(hProcess, SkillAddr + i * offset, out skill[i].ID, 4, 0);
                            skill[i].Name = LoadSkillInfo(skill[i].ID);
                            if (skill[i].Name == null)
                                continue;

                            WinAPI.ReadProcessMemory(hProcess, SkillAddr + i * offset + 0x4, out skill[i].Rank, 1, 0);
                            int ExSkill_Rank = 0;
                            WinAPI.ReadProcessMemory(hProcess, SkillAddr + i * offset + 0x5, out ExSkill_Rank, 1, 0);//Rank額外加成
                            skill[i].Rank += ExSkill_Rank;
                            WinAPI.ReadProcessMemory(hProcess, SkillAddr + i * offset + 0xB, out skill[i].Cost, 1, 0);

                            if (skill[i].Name.Contains("脫逃") || (skill[i].Name.Contains("[戰鬥]") && !skill[i].Name.Contains("望風")))
                                skill[i].Navalbattle = true;//戰鬥用技能

                            //去除技能[Type]
                            skill[i].Name = skill[i].Name.Replace("[冒險]", "");
                            skill[i].Name = skill[i].Name.Replace("[交易]", "");
                            skill[i].Name = skill[i].Name.Replace("[戰鬥]", "");

                            if (skill[i].Cost > 0)
                            {
                                skill[i].Active = true;//可使用技能

                                string[] Skills = new string[] { "保管", "烹飪", "鑄造", "工藝", "縫紉", "煉金術", "會計", "身體語言", "地理學", "考古學", "宗教學", "生物學", "財寶鑑定", "美術", "觀察", "驅除", "統率", "疾病學", "滅火" };
                                string[] Adventure = new string[] { "搜索", "視認", "生態調查" };
                                string[] Fight_Skills = new string[] { "防禦", "突擊", "射擊", "戰術" };
                                string[] NoImage_Skills = new string[] { "操帆", "救助", "驅除", "修理", "統率", "疾病學", "外科醫術", "滅火", "佈設水雷" };

                                //排除生產技 && 被動技
                                for (int j = 0; j < Skills.Length; j++)
                                {
                                    if (skill[i].Name == Skills[j])
                                    {
                                        skill[i].Active = false;
                                        skill[i].Navalbattle = false;
                                    }
                                }

                                for (int j = 0; j < Adventure.Length; j++)
                                {
                                    if (skill[i].Name == Adventure[j])
                                    {
                                        skill[i].Adventure_Active = true;
                                        skill[i].Active = false;
                                    }
                                }

                                //排除肉搏戰技能
                                for (int j = 0; j < Fight_Skills.Length; j++)
                                {
                                    if (skill[i].Name == Fight_Skills[j])
                                    {
                                        skill[i].Active = false;
                                        skill[i].Navalbattle = false;
                                    }
                                }

                                //無技能圖案技能
                                for (int j = 0; j < NoImage_Skills.Length; j++)
                                {
                                    if (skill[i].Name == NoImage_Skills[j])
                                        skill[i].NoSkillImage = true;
                                }
                            }
                            Console.WriteLine(skill[i].Name + " " + skill[i].Rank + " " + ExSkill_Rank);
                        }
                        WinAPI.CloseHandle(hProcess);
                        return true;
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;
        }

        //副官資訊欄顯示模式
        private static int ADJUTANT_INFO_STATE1_ADDR;//0x43CA30 ADDR2-0x65 call ADDR1;
        private static int ADJUTANT_INFO_STATE2_ADDR;//0x43FB8C
        private void GetAdjutantInfoStateAddr(IntPtr hWnd)
        {
            if (ADJUTANT_INFO_STATE1_ADDR != 0 && ADJUTANT_INFO_STATE2_ADDR != 0)
                return;

            initMemoryInfo(hWnd);

            if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
            {
                byte[] buf = new byte[inf.RegionSize];
                int readBytes = 0;

                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                for (int i = 0; i < buf.Length; i++)
                {
                    int offset = i + 0x42;
                    if (offset + 0xC > buf.Length)
                        break;

                    uint sByte = (buf[i]);//0-255
                    uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                    uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                    //mov     eax, dword ptr fs:[0]
                    if (sByte2 == 0xA164)
                    {
                        //mov     eax, dword ptr [esp+24]
                        if ((uint)(buf[offset] | buf[offset + 0x1] << 8 | buf[offset + 0x2] << 16) == 0x24448B && (uint)buf[offset + 0x3] == 0x24)
                        {
                            //mov     ecx, dword ptr [esp+20]
                            if ((uint)(buf[offset + 0x4] | buf[offset + 0x5] << 8 | buf[offset + 0x6] << 16) == 0x244C8B && (uint)buf[offset + 0x7] == 0x20)
                            {
                                //push    edi
                                if ((uint)buf[offset + 0x8] == 0x57)
                                {
                                    //push    64
                                    if ((uint)buf[offset + 0x9] == 0x6A && (uint)buf[offset + 0xA] == 0x64)
                                    {
                                        //push    eax
                                        if ((uint)buf[offset + 0xB] == 0x50)
                                        {
                                            //push    ecx
                                            if ((uint)buf[offset + 0xC] == 0x51)
                                            {
                                                int Addr;
                                                ADJUTANT_INFO_STATE2_ADDR = (int)inf.BaseAddress + i + 0xAC;
                                                WinAPI.ReadProcessMemory(hProcess, ADJUTANT_INFO_STATE2_ADDR - 0x5B + 0x1, out Addr, 4, 0);
                                                ADJUTANT_INFO_STATE1_ADDR = ADJUTANT_INFO_STATE2_ADDR - 0x5B - (int)(0xFFFFFFFF - (uint)Addr) + 0x4;
                                                //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
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
        public string Check_Display_AdjutantInfo(IntPtr hWnd)
        {
            GetAdjutantInfoStateAddr(hWnd);
            if (ADJUTANT_INFO_STATE1_ADDR == 0 || ADJUTANT_INFO_STATE2_ADDR == 0)
                return "顯示";

            if (!SceneChange(hWnd))
            {
                int state1, state2;
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, ADJUTANT_INFO_STATE1_ADDR, out state1, 3, 0);
                WinAPI.ReadProcessMemory(hProcess, ADJUTANT_INFO_STATE2_ADDR + 0x1, out state2, 1, 0);
                WinAPI.CloseHandle(hProcess);

                if (state1 == 0xF18B56 && state2 == 0x0)
                    return "顯示";
                if (state1 == 0x0010C2 && state2 == 0x1)
                    return "隱藏";
            }
            return null;
        }
        //顯示&隱藏副官資訊欄
        public void DisplayAdjutantInfo(IntPtr hWnd, bool set)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (set)
            {
                //還原主畫面消失
                /* 
                 * 57       push    edi
                 * 8BF9     mov      edi, ecx
                 */
                WinAPI.WriteProcessMemory(hProcess, ADJUTANT_INFO_STATE1_ADDR, BitConverter.GetBytes(0xF98B57), 3, 0);
                //還原資訊欄
                WinAPI.WriteProcessMemory(hProcess, ADJUTANT_INFO_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x00), 1, 0);
            }
            else
            {
                //不讓主畫面消失
                /*
                 * C2 1000  retn 10
                 */
                WinAPI.WriteProcessMemory(hProcess, ADJUTANT_INFO_STATE1_ADDR, BitConverter.GetBytes(0x0010C2), 3, 0);
                //不讓資訊欄顯示
                WinAPI.WriteProcessMemory(hProcess, ADJUTANT_INFO_STATE2_ADDR + 0x1, BitConverter.GetBytes(0x01), 1, 0);
            }

            WinAPI.CloseHandle(hProcess);
        }
        //取得副官資料
        public bool GetAdjutantInfo(IntPtr hWnd, Form1.uInfo.AdjutantInfo[] Adjustant)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "副官情報")
                {
                    if (GetTargetID(hWnd) == GetUserId(hWnd) || GetTargetID(hWnd) == 0)
                    {
                        int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                        int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                        for (int i = 0; i < 2; i++)
                        {
                            int AdjustantNameAddr, AdjustantNameLen, ret = 0;

                            WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5F0 + i * 0x33C, out Adjustant[i].ID, 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5F4 + i * 0x33C, out AdjustantNameAddr, 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, AdjustantNameAddr - 0xC, out AdjustantNameLen, 4, 0);
                            byte[] AdjustantName = new byte[AdjustantNameLen *= 2];
                            WinAPI.ReadProcessMemory(hProcess, AdjustantNameAddr, AdjustantName, AdjustantNameLen, ref ret);
                            Adjustant[i].Name = System.Text.Encoding.Unicode.GetString(AdjustantName);

                            WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x626 + i * 0x33C, out Adjustant[i].As, 1, 0);
                            for (int j = 0; j < 3; j++)
                                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x62C + i * 0x33C + j * 0xC, out Adjustant[i].LV[j], 4, 0);
                            for (int j = 0; j < 6; j++)
                                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x910 + i * 0x33C + j, out Adjustant[i].Property[j], 1, 0);
                            //Console.WriteLine(Adjustant[i].ID + " " + Adjustant[i].Name + " " + Enum.Parse(typeof(Form1.Post), Adjustant[i].As.ToString()));
                        }
                    }
                    WinAPI.CloseHandle(hProcess);
                    return true;
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;
        }

        //變更副官擔任
        public static int CHANGEADJUTANT_CALL_ADDR;
        public void ChangeAdjutant(IntPtr hWnd, Form1.uInfo.AdjutantInfo[] adjutant)
        {
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));

            if (CHANGEADJUTANT_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x36;
                        if (offset + 0x9 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 208
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x0208)
                        {
                            //mov     byte ptr [esp+1C], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x1C && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+1D], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x1D && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //retn    0C
                                    if ((uint)buf[offset + 0xD8] == 0xC2 && (uint)(buf[offset + 0xD9] | buf[offset + 0xDA] << 8) == 0x000C)
                                    {
                                        CHANGEADJUTANT_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (CHANGEADJUTANT_CALL_ADDR == 0)
                return;

            if (!SceneChange(hWnd))
            {
                int count = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (adjutant[i].CheckChange)
                    {
                        ++count;
                        WinAPI.WriteProcessMemory(hProcess, PostSPACE + i, BitConverter.GetBytes(adjutant[i].ChangeAs), 4, 0);//副官i擔任位置
                    }
                    else
                        WinAPI.WriteProcessMemory(hProcess, PostSPACE + i, BitConverter.GetBytes(adjutant[i].As), 4, 0);//副官i擔任位置
                    WinAPI.WriteProcessMemory(hProcess, PostSPACE + 0x8 + i * 0x4, BitConverter.GetBytes(adjutant[i].ID), 4, 0);//副官iID

                    if (adjutant[i].CheckChange)
                        adjutant[i].As = adjutant[i].ChangeAs;
                }

                if (count > 0)
                {
                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(PostSPACE);
                    asm.Push(PostSPACE + 0x8);
                    asm.Push(count);
                    asm.Mov_ECX(CALL_ECX);
                    //asm.Mov_EAX(0x8FE550);
                    //asm.Mov_EAX(0x90C1D0);
                    asm.Mov_EAX(CHANGEADJUTANT_CALL_ADDR);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(GetPid(hWnd));
                }
            }
        }

        //取得持有物品內容
        public bool GetItemsInfo(IntPtr hWnd, ref Form1.uInfo.Item.ItemInfo[] _Item)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                if (CheckWindows(hWnd) == "持有物品")
                {
                    _Item = new Form1.uInfo.Item.ItemInfo[100];
                    int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                    int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                    //WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x620, out Item_Num, 4, 0);

                    int Index = 0, Item_AddrPtr, Item_Addr, Check_Equip;
                    WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x6F4, out Item_AddrPtr, 4, 0);
                    while (Item_AddrPtr != 0)
                    {
                        WinAPI.ReadProcessMemory(hProcess, Item_AddrPtr + 0x8, out Item_Addr, 4, 0);

                        WinAPI.ReadProcessMemory(hProcess, Item_Addr, out _Item[Index].Code, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x8, out _Item[Index].ID, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x34, out _Item[Index].Num, 4, 0);
                        if (_Item[Index].ID != 0)
                        {
                            _Item[Index].Name = LoadItemInfo(_Item[Index].ID);
                            if (string.IsNullOrWhiteSpace(_Item[Index].Name))
                                _Item[Index].Name = GetItemName(hWnd, _Item[Index].ID);
                            else
                            {
                                if (_Item[Index].Name.Contains("]"))
                                    _Item[Index].Name = _Item[Index].Name.Substring(_Item[Index].Name.IndexOf("]") + 1, _Item[Index].Name.Length - _Item[Index].Name.IndexOf("]") - 1);
                            }

                        }

                        WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x138, out Check_Equip, 4, 0);
                        if (Check_Equip == 1)
                            _Item[Index].Equip = true;

                        switch (_Item[Index].ID / 65536)
                        {
                            case 0:
                                _Item[Index].Equip_Type = 0;//"身體";
                                break;
                            case 1:
                            case 2:
                                _Item[Index].Equip_Type = 1;//"頭部";
                                break;
                            case 3:
                                _Item[Index].Equip_Type = 2;//"足部";
                                break;
                            case 4:
                            case 5:
                                _Item[Index].Equip_Type = 3;//"手部";
                                break;
                            case 6:
                                _Item[Index].Equip_Type = 4;//"武器";
                                break;
                            case 7:
                            case 8:
                                _Item[Index].Equip_Type = 5;//"飾品";
                                break;
                            default:
                                _Item[Index].Equip_Type = -1;
                                break;
                        }
                        if (_Item[Index].Equip_Type != -1)
                        {
                            WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x3E, out _Item[Index].Durability, 1, 0);
                            WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x4C, out _Item[Index].Attack, 4, 0);
                            WinAPI.ReadProcessMemory(hProcess, Item_Addr + 0x50, out _Item[Index].Defense, 4, 0);
                        }


                        //下一個物品
                        WinAPI.ReadProcessMemory(hProcess, Item_AddrPtr, out Item_AddrPtr, 4, 0);
                        ++Index;
                    }
                    WinAPI.CloseHandle(hProcess);
                    return true;
                }
                WinAPI.CloseHandle(hProcess);
            }
            return false;
        }

        public int GetDungeonAdmiral(IntPtr hWnd)
        {
            int AdmiralID = 0;
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.ReadProcessMemory(hProcess, DungeonAdmiralAddr, out AdmiralID, 4, 0);
                WinAPI.CloseHandle(hProcess);
            }
            return AdmiralID;
        }
        //進入地下城..
        public void IntoDungeon(IntPtr hWnd, int Code)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(Code);//地下城編號? 1byte編碼
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x8A5170);
                asm.Mov_EAX(0x913980);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 進入地下城");
            }
        }
        //是否在地下城
        public bool InDungeon(IntPtr hWnd, int Place)
        {
            if (Place >= 0x30 && Place <= 0x33)
                return true;
            return false;
        }
        //取得離開地下城所需Code
        public int GetExitDungeonCode(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int CodePtr, Code;
                WinAPI.ReadProcessMemory(hProcess, ExitDungeonCodeAddr, out CodePtr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, CodePtr + 0x30, out Code, 4, 0);
                WinAPI.CloseHandle(hProcess);
                return Code;
            }
            return 0;
        }
        //離開地下城..
        public void ExitDungeon(IntPtr hWnd)
        {
            if (!SceneChange(hWnd) && GetExitDungeonCode(hWnd) != 0)
            {
                int pid = GetPid(hWnd);
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(GetExitDungeonCode(hWnd));//地下城編號? 1byte編碼
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x864680);
                asm.Mov_EAX(0x8D07B0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 離開地下城");
            }
        }

        //接受競技場使命//
        public void GetArenaMission(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //獎勵動作
                WinAPI.WriteProcessMemory(hProcess, PostSPACE + 0x1000, BitConverter.GetBytes(9), 1, 0);
                WinAPI.WriteProcessMemory(hProcess, PostSPACE + 0x1001, BitConverter.GetBytes(13), 1, 0);
                WinAPI.WriteProcessMemory(hProcess, PostSPACE + 0x1002, BitConverter.GetBytes(19), 1, 0);
                //特殊勝利條件
                WinAPI.WriteProcessMemory(hProcess, PostSPACE + 0x1003, BitConverter.GetBytes(0), 1, 0);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(PostSPACE + 0x1000);
                asm.Push(PostSPACE + 0x1002);
                asm.Push(0x2);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x8A73C0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //完成戰鬥準備//
        public void Ready(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x8A7500);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //檢查跳過演出
        public bool CheckPassPlay(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int PartyAddr;
                int Pass = 1, Code = 0;
                WinAPI.ReadProcessMemory(hProcess, PassPlayCodeAddr, out Code, 4, 0);

                WinAPI.ReadProcessMemory(hProcess, PartyAddrPtr, out PartyAddr, 4, 0);
                if (PartyAddr != 0)
                {
                    for (int User = 0; User < 5; User++)
                    {
                        int PartyID = 0;
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0xC, out PartyID, 4, 0);//隊員[User] ID
                        WinAPI.ReadProcessMemory(hProcess, PartyAddr + 0x0, out PartyAddr, 4, 0);
                        if (GetUserId(hWnd) == PartyID)
                        {
                            WinAPI.ReadProcessMemory(hProcess, PassPlayCodeAddr + 0x6C + 0x8 * User, out Pass, 4, 0);
                            if (Pass == 0 && Code != 0)
                            {
                                WinAPI.CloseHandle(hProcess);
                                return false;
                            }
                        }
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return true;
        }
        //跳過演出
        public void PassPlay(IntPtr hWnd)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int Code = 0;
                WinAPI.ReadProcessMemory(hProcess, PassPlayCodeAddr, out Code, 4, 0);
                WinAPI.CloseHandle(hProcess);

                if (Code != 0)
                {
                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(GetUserId(hWnd));
                    asm.Push(Code);
                    asm.Mov_ECX(CALL_ECX);
                    asm.Mov_EAX(0x85D5A0);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(pid);
                }
            }
        }

        //取得陸戰資訊
        public void GetLBInfo(IntPtr hWnd, Form1.uInfo.LandbattleInfo[] Enemy, Form1.uInfo.LandbattleInfo[] Friend)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
                for (int i = 0; i < 5; i++)
                {
                    //友方
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + i * 0x64, out Friend[i].ID, 4, 0);
                    Friend[i].Name = GetPeopleName(hWnd, Friend[i].ID);
                    if (Friend[i].Name != null && Friend[i].Name.IndexOf("Lv") > 0)
                        Friend[i].Name = Friend[i].Name.Substring(0, Friend[i].Name.IndexOf("Lv"));
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x4 + i * 0x64, out Friend[i].HP, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x8 + i * 0x64, out Friend[i].MaxHP, 4, 0);
                    //WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x18 + i * 0x64, out Friend[i].Attack, 4, 0);
                    //WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x1C + i * 0x64, out Friend[i].Defense, 4, 0);
                    //WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x28 + i * 0x64, out Friend[i].LV, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x2C + i * 0x64, out Friend[i].State, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x34 + i * 0x64, out Friend[i].SkillState, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x4C + i * 0x64, out Friend[i].MaxMotion, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, FriendAddr + 0x50 + i * 0x64, out Friend[i].Motion, 4, 0);

                }
                for (int i = 0; i < 10; i++)
                {
                    //敵方
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + i * 0x64, out Enemy[i].ID, 4, 0);
                    Enemy[i].Name = GetPeopleName(hWnd, Enemy[i].ID);
                    if (Enemy[i].Name != null && Enemy[i].Name.IndexOf("Lv") > 0)
                        Enemy[i].Name = Enemy[i].Name.Substring(0, Enemy[i].Name.IndexOf("Lv"));
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x4 + i * 0x64, out Enemy[i].HP, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x8 + i * 0x64, out Enemy[i].MaxHP, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x18 + i * 0x64, out Enemy[i].Attack, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x1C + i * 0x64, out Enemy[i].Defense, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x28 + i * 0x64, out Enemy[i].LV, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, EnemyAddr + 0x30 + i * 0x64, out Enemy[i].State, 4, 0);//狀態小圖案
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得陸戰快捷鍵
        public void GetLBShortcuts(IntPtr hWnd, Form1.uInfo.LandShortcuts[] _LandShortcuts)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                for (int i = 0; i < 7; i++)
                {
                    if (i < 3)
                    {
                        int use = 0;
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsSAddr + i * 0xC, out _LandShortcuts[i].ID, 4, 0);
                        if (_LandShortcuts[i].ID > 0)
                        {
                            if (_LandShortcuts[i].ID < 5 || (_LandShortcuts[i].ID > 18 && _LandShortcuts[i].ID < 23) || (_LandShortcuts[i].ID > 30 && _LandShortcuts[i].ID < 35) || (_LandShortcuts[i].ID > 42 && _LandShortcuts[i].ID < 47) || (_LandShortcuts[i].ID > 54 && _LandShortcuts[i].ID < 59) || (_LandShortcuts[i].ID > 66 && _LandShortcuts[i].ID < 71) || (_LandShortcuts[i].ID > 78 && _LandShortcuts[i].ID < 83))
                                _LandShortcuts[i].ID = 1;//力量
                            else if (_LandShortcuts[i].ID < 9 || (_LandShortcuts[i].ID > 22 && _LandShortcuts[i].ID < 27) || (_LandShortcuts[i].ID > 34 && _LandShortcuts[i].ID < 39) || (_LandShortcuts[i].ID > 46 && _LandShortcuts[i].ID < 51) || (_LandShortcuts[i].ID > 58 && _LandShortcuts[i].ID < 63) || (_LandShortcuts[i].ID > 70 && _LandShortcuts[i].ID < 75) || (_LandShortcuts[i].ID > 82 && _LandShortcuts[i].ID < 87))
                                _LandShortcuts[i].ID = 2;//快速
                            else if (_LandShortcuts[i].ID < 13 || (_LandShortcuts[i].ID > 26 && _LandShortcuts[i].ID < 31) || (_LandShortcuts[i].ID > 38 && _LandShortcuts[i].ID < 43) || (_LandShortcuts[i].ID > 50 && _LandShortcuts[i].ID < 55) || (_LandShortcuts[i].ID > 62 && _LandShortcuts[i].ID < 67) || (_LandShortcuts[i].ID > 74 && _LandShortcuts[i].ID < 79) || (_LandShortcuts[i].ID > 86 && _LandShortcuts[i].ID < 91))
                                _LandShortcuts[i].ID = 3;//佯攻

                        }
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsSAddr + 0x4 + i * 0xC, out _LandShortcuts[i].NeedMotion, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsSAddr + 0x8 + i * 0xC, out use, 4, 0);
                        if (use == 1)
                            _LandShortcuts[i].Use = true;
                        else
                            _LandShortcuts[i].Use = false;
                    }
                    else
                    {
                        int use = 0;
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsIAddr + (i - 3) * 0x1C, out _LandShortcuts[i].ID, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsIAddr + 0x4 + (i - 3) * 0x1C, out _LandShortcuts[i].Num, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsIAddr + 0x8 + (i - 3) * 0x1C, out _LandShortcuts[i].NeedMotion, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsIAddr + 0xC + (i - 3) * 0x1C, out _LandShortcuts[i].UseTarget, 4, 0);//使用目標 3-友方 4-敵方
                        WinAPI.ReadProcessMemory(hProcess, LBShortcutsIAddr + 0x10 + (i - 3) * 0x1C, out use, 4, 0);
                        if (use == 1)
                            _LandShortcuts[i].Use = true;
                        else
                            _LandShortcuts[i].Use = false;

                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
        }
        //取得陸戰-選定目標
        public int GetLBTarget(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            int TargetID;

            WinAPI.ReadProcessMemory(hProcess, LBTargetAddr, out TargetID, 4, 0);
            WinAPI.CloseHandle(hProcess);

            return TargetID;
        }
        //取得陸戰-攻擊目標
        public int GetLBAttackTarget(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            int TargetID;

            WinAPI.ReadProcessMemory(hProcess, LBAttackTargetAddr, out TargetID, 4, 0);
            WinAPI.CloseHandle(hProcess);

            return TargetID;
        }

        //陸戰-開戰..
        public void StartLandBattle(IntPtr hWnd, int TargetID)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x86D330);
                asm.Mov_EAX(0x8D8FB0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 陸戰-開戰");
            }
        }
        //陸戰-援軍..
        public void JoinLandBattle(IntPtr hWnd, int TargetID)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x86D390);
                asm.Mov_EAX(0x8D9010);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 陸戰-援軍");
            }
        }
        //陸戰-攻擊..
        public void LBAttack(IntPtr hWnd, int TargetID)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x86D440);
                asm.Mov_EAX(0x8D90C0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                //Console.WriteLine(GetUserName(hWnd) + " 陸戰-攻擊");
            }
        }
        //使用陸戰道具..
        public void UseLBItem(IntPtr hWnd, int TargetID, int Shortcuts)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(Shortcuts);
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x86D4A0);
                asm.Mov_EAX(0x8D9120);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                //Console.WriteLine(GetUserName(hWnd) + " 使用陸戰道具");
            }
        }
        //使用陸戰技巧..
        public void UseLBSkill(IntPtr hWnd, int TargetID, int Shortcuts)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(-1);
                asm.Push(Shortcuts);
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x86D500);
                asm.Mov_EAX(0x8D9190);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                //Console.WriteLine(GetUserName(hWnd) + " 使用陸戰技巧");
            }
        }

        //取得所有活動目標數量
        public int GetAllPeopleNum(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            int Num;

            WinAPI.ReadProcessMemory(hProcess, PeopleBase + 0x8, out Num, 4, 0);
            WinAPI.CloseHandle(hProcess);
            return Num;
        }
        //取得所有活動目標
        public Form1.uInfo.TargetInfo[] GetAllPeople(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            Form1.uInfo.TargetInfo[] _Target = new Form1.uInfo.TargetInfo[GetAllPeopleNum(hWnd)];
            if (!SceneChange(hWnd))
            {
                int count = 0;
                int TargetAddrPTR, TargetAddr;

                WinAPI.ReadProcessMemory(hProcess, PeopleBase, out TargetAddrPTR, 4, 0);
                for (int i = 0; i < 0x11; i++)
                {
                    WinAPI.ReadProcessMemory(hProcess, TargetAddrPTR + 0x4 * i, out TargetAddr, 4, 0);
                    while (TargetAddr != 0 && !SceneChange(hWnd))
                    {
                        if (count >= _Target.Length)
                            break;
                        WinAPI.ReadProcessMemory(hProcess, TargetAddr, out _Target[count].ID, 4, 0);
                        if (_Target[count].ID != 0)
                        {
                            if (_Target[count].ID >= 0x800000)
                                _Target[count].Name = GetPeopleName(hWnd, _Target[count].ID);
                            _Target[count].Coordinate = GetTargetCoordinate(hWnd, _Target[count].ID, 0);
                            ++count;
                        }
                        WinAPI.ReadProcessMemory(hProcess, TargetAddr + 0x8, out TargetAddr, 4, 0);
                    }
                }
                WinAPI.CloseHandle(hProcess);
            }
            return _Target;
        }
        //取得所有固定目標數量
        public int GetAllRegularNum(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);
            int Num;

            WinAPI.ReadProcessMemory(hProcess, RegularBase + 0x8, out Num, 4, 0);
            WinAPI.CloseHandle(hProcess);
            return Num;
        }
        //取得所有固定目標
        public Form1.uInfo.TargetInfo[] GetAllRegular(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            int count = 0;
            int TargetAddrPTR, TargetAddr;
            Form1.uInfo.TargetInfo[] _Target = new Form1.uInfo.TargetInfo[GetAllRegularNum(hWnd)];

            WinAPI.ReadProcessMemory(hProcess, RegularBase, out TargetAddrPTR, 4, 0);

            for (int i = 0; i < 0x11; i++)
            {
                WinAPI.ReadProcessMemory(hProcess, TargetAddrPTR + 0x4 * i, out TargetAddr, 4, 0);
                while (TargetAddr != 0 && !SceneChange(hWnd))
                {
                    if (count >= _Target.Length)
                        break;
                    WinAPI.ReadProcessMemory(hProcess, TargetAddr, out _Target[count].ID, 4, 0);
                    if (_Target[count].ID != 0)
                    {
                        _Target[count].Name = GetSceneName(hWnd, _Target[count].ID);
                        _Target[count].Coordinate = GetTargetCoordinate(hWnd, _Target[count].ID, 1);
                        //Console.WriteLine(_Target[count].Name + " " + _Target[count].Coordinate);
                        ++count;
                    }
                    WinAPI.ReadProcessMemory(hProcess, TargetAddr + 0x8, out TargetAddr, 4, 0);
                }
            }
            WinAPI.CloseHandle(hProcess);
            return _Target;
        }

        //取得所有配方1~2000
        public void GetAllFormula(IntPtr hWnd)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            int AddrPtr = 0, Addr, Len, ret = 0;


            #region 測試用
            int TempAddr = 0xDB82CC, Temp = 0;
            do
            {
                WinAPI.ReadProcessMemory(hProcess, TempAddr + 0x4, out Temp, 4, 0);
                if (Temp == 0x11)
                {
                    if ((AddrPtr = GetNameAddr(hWnd, 232, TempAddr)) > 0)
                    {
                        WinAPI.ReadProcessMemory(hProcess, AddrPtr + 0xC, out Addr, 4, 0);
                        WinAPI.ReadProcessMemory(hProcess, Addr - 0xC, out Len, 4, 0);
                        if (Len > 0)
                        {
                            //Console.WriteLine(Convert.ToString(TempAddr, 16));
                        }
                    }
                }
                TempAddr += 8;
                //Console.WriteLine(Convert.ToString(TempAddr, 16));
            } while (TempAddr < ItemBase);
            #endregion

            for (int i = 0; i < 2000; i++)
            {
                AddrPtr = GetNameAddr(hWnd, i, 0xdb83ac);
                if (AddrPtr != 0)
                {
                    WinAPI.ReadProcessMemory(hProcess, AddrPtr + 0xC, out Addr, 4, 0);
                    WinAPI.ReadProcessMemory(hProcess, Addr - 0xC, out Len, 4, 0);
                    if (Len > 0)
                    {
                        byte[] Name = new byte[Len *= 2];
                        WinAPI.ReadProcessMemory(hProcess, Addr, Name, Len, ref ret);
                        Console.WriteLine(i + "=" + System.Text.Encoding.Unicode.GetString(Name));
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(LoadFormulaInfo(i)))
                            Console.WriteLine(i + "=" + LoadFormulaInfo(i));
                    }
                }

            }
            WinAPI.CloseHandle(hProcess);
        }
        //自動登入
        public void Login(IntPtr hWnd)
        {
            if (CheckWindows(hWnd) == "開頭畫面" && !GetConnectState(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                int LoginAddr;
                WinAPI.ReadProcessMemory(hProcess, LoginAddrPtr, out LoginAddr, 4, 0);
                WinAPI.WriteProcessMemory(hProcess, LoginAddr + 0xD4, BitConverter.GetBytes(11), 4, 0);
                Console.WriteLine("登入");
                WinAPI.CloseHandle(hProcess);
            }
            else
            {
                WinAPI.PostMessage(hWnd, WinAPI.WM_KEYDOWN, (IntPtr)WinAPI.VK_ENTER, MakeLParam((uint)WinAPI.VK_ENTER, "WM_KEYDOWN"));
                WinAPI.PostMessage(hWnd, WinAPI.WM_KEYUP, (IntPtr)WinAPI.VK_ENTER, MakeLParam((uint)WinAPI.VK_ENTER, "VM_KEYUP"));
            }
        }

        //取得船隻編號
        public bool GetBoatNumber(IntPtr hWnd, ref int ShipCode, int Select)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            if (CheckWindows(hWnd) == "出售船隻")
            {
                int WindowBaseAddr1 = GetWindowBaseAddr1(hWnd);//WindowBaseAddr1 = BaseAddr1
                int WindowBaseAddr2 = GetWindowBaseAddr2(hWnd);//WindowBaseAddr2 = BaseAddr2

                int Index, ShipCodeAddr;
                //int BoatNum;
                //目前選擇船 Index
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5E0, out Index, 4, 0);
                //WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5F0, out BoatNum, 4, 0);持有船數量
                WinAPI.ReadProcessMemory(hProcess, WindowBaseAddr1 + 0x5E8, out ShipCodeAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, ShipCodeAddr + (Select * 0x100), out ShipCode, 4, 0);
                WinAPI.CloseHandle(hProcess);
                return true;
            }
            WinAPI.CloseHandle(hProcess);
            return false;
        }
        //造船/
        public void BuildShip(IntPtr hWnd, int TargetID, int Warehouse, int Material, int ShipID)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);

                AsmClassLibrary asm = new AsmClassLibrary();

                //造船
                asm.Pushad();
                asm.Push(0x0);//船名字
                asm.Push(Warehouse);//船總倉
                asm.Push(Material);//船材質
                asm.Push(ShipID);//船ID
                asm.Push(TargetID);//造船廠老闆
                asm.Push(0x0);
                asm.Push(0x6);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x89EBB0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }

        }
        //賣船/
        public void SellBoat(IntPtr hWnd, int TargetID, int ShipCode)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(ShipCode);
                asm.Push(TargetID);//造船廠老闆
                asm.Push(0x0);
                asm.Push(0x8);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x899960);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //大學專攻
        public static int RESEARCH_CALL_ADDR;
        public void Research(IntPtr hWnd, int TargetID, string Researchs)
        {
            if (RESEARCH_CALL_ADDR == 0)
            {
                initMemoryInfo(hWnd);

                if (inf.Protect == WinAPI.MEMMessage.PAGE_EXECUTE_READ)
                {
                    byte[] buf = new byte[inf.RegionSize];
                    int readBytes = 0;

                    int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, GetPid(hWnd));
                    WinAPI.ReadProcessMemory((IntPtr)hProcess, (IntPtr)inf.BaseAddress, buf, inf.RegionSize, out readBytes);

                    for (int i = 0; i < buf.Length; i++)
                    {
                        int offset = i + 0x1E;
                        if (offset + 0x87 > buf.Length)
                            break;

                        uint sByte = (buf[i]);//0-255
                        uint sByte2 = (i + 1 > buf.Length) ? 0 : (uint)(buf[i] | buf[i + 1] << 8);//0-65535
                        uint sByte4 = (i + 3 > buf.Length) ? 0 : sByte4 = (uint)(buf[i] | buf[i + 1] << 8 | buf[i + 2] << 16 | buf[i + 3] << 24);//0->

                        //sub     esp, 100
                        if (sByte2 == 0xEC81 && (uint)(buf[i + 0x2] | buf[i + 0x3] << 8) == 0x0100)
                        {
                            //mov     byte ptr [esp+10], 11
                            if ((uint)buf[offset] == 0xC6 && (uint)(buf[offset + 0x1] | buf[offset + 0x2] << 8) == 0x2444 && (uint)buf[offset + 0x3] == 0x10 && (uint)buf[offset + 0x4] == 0x11)
                            {
                                //mov     byte ptr [esp+11], 4
                                if ((uint)buf[offset + 0x5] == 0xC6 && (uint)(buf[offset + 0x6] | buf[offset + 0x7] << 8) == 0x2444 && (uint)buf[offset + 0x8] == 0x11 && (uint)buf[offset + 0x9] == 0x4)
                                {
                                    //retn    0C
                                    if ((uint)buf[offset + 0x85] == 0xC2 && (uint)(buf[offset + 0x86] | buf[offset + 0x87] << 8) == 0x000C)
                                    {
                                        RESEARCH_CALL_ADDR = (int)inf.BaseAddress + i;
                                        //Console.WriteLine(Convert.ToString((long)inf.BaseAddress + i, 16));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (RESEARCH_CALL_ADDR == 0)
                return;

            int index = 0, star = 0, mode = 0;
            while (index != -1)
            {
                index = Researchs.IndexOf("*", index);
                if (index != -1)
                {
                    index = index + "*".Length;
                    ++star;
                }
            }

            Researchs = Researchs.Replace("*", "");
            switch (Researchs)
            {
                case "危機管理法1":
                    mode = 6;
                    break;
                case "自然學・人文科學1":
                    mode = 7;
                    break;
                case "調查技術1":
                    mode = 8;
                    break;
                case "陸上戰鬥技術1":
                    mode = 11;
                    break;
                case "船舶修理技術1":
                    mode = 23;
                    break;
                case "獎學制度說明會":
                    mode = 42;
                    break;
            }
            if (!SceneChange(hWnd) && mode != 0 && star != 0)
            {
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(star);
                asm.Push(mode);
                asm.Push(star);
                asm.Push(TargetID);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x908640);
                //asm.Mov_EAX(0x9162C0);
                asm.Mov_EAX(RESEARCH_CALL_ADDR);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(GetPid(hWnd));
                Console.WriteLine(GetUserName(hWnd) + " 大學專攻");
            }
        }
        //發現物報告//
        public void Report(IntPtr hWnd, int TargetID, int index)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(index);//發現物
                asm.Push(TargetID);
                asm.Push(0x0);
                asm.Push(0x26);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x89B7C0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
                Console.WriteLine(GetUserName(hWnd) + " 發現物報告");
            }
        }

        //名單
        public string CheckUser(string ServerName, string Name)
        {
            string[] VIP = new string[] { "脫拉庫", "逼恩答布游", "御用小當家", "兆豐企業", "兆豐金庫A", "兆豐金庫B", "兆豐金庫C", "兆豐金庫D", "不堪一擊", "蠁比哇卡粗勇", "歐都麥", "卡打掐", "滄海狂風", "发福的英格兰", "易麟风", "米粒陽光", "尼德兰的城墙", "萬里河山從頭越", "兆豐香辛買賣", "紅茶去冰", "綠茶去冰", "奶茶去冰", "青茶去冰", "花茶去冰", "札達爾a報馬仔", "卡利卡特a報馬仔", "威尼斯帝國戰隊A", "威尼斯帝國戰隊B", "威尼斯帝國戰隊C", "威尼斯帝國戰隊D", "威尼斯帝國戰隊E", "法蘭西帝國艦隊A", "法蘭西帝國艦隊B", "法蘭西帝國艦隊C", "法蘭西帝國艦隊D", "法蘭西帝國艦隊E", "大谷刑部" };
            string[] Test = new string[] { "大柳丁", "西產大柳丁", "y.c.lai" };
            string[] Other = new string[] { "騎山貓抓地鼠", "紅茶A", "紅茶B", "紅茶C", "紅茶D", "紅茶E", "綠茶A", "綠茶B", "綠茶C", "綠茶D", "綠茶E", "奶茶A", "奶茶B", "奶茶C", "奶茶D", "奶茶E" };
            string[] Pirate = new string[] { "花之妖精‧櫻", "七星寶", "六星寶", "小藍c湄", "小獸", "棉花枕頭", "小早川加奈子", "えみこ", "井上乃香", "戀上初音", "傻气a咩咩", "蓮娜亞" };

            if (!string.IsNullOrWhiteSpace(ServerName) && !string.IsNullOrWhiteSpace(Name))
            {
                for (int i = 0; i < Pirate.Length; i++)
                    if (Name.Contains(Pirate[i]))
                        return "Pirate";
                for (int i = 0; i < Test.Length; i++)
                    if (ServerName == "戰列艦" && Name == Test[i])
                        return "TEST";
                for (int i = 0; i < VIP.Length; i++)
                    if (ServerName == "戰列艦" && Name == VIP[i])
                        return "猜猜看";
                //for (int i = 0; i < Other.Length; i++)
                    //if (ServerName == "探索號(推薦)" && Name == Other[i])
                        //return "VIP";
            }
            return "ELSE";
        }

        //取得Log
        public string GetLog(IntPtr hWnd, ref int Ptr, ref int End, ref int Str_Color)
        {
            int pid = GetPid(hWnd);
            int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

            string str = null;
            int WordAddr, StartAddr, EndAddr, AllWordNum = 0;
            WinAPI.ReadProcessMemory(hProcess, LogAddr + 0x4, out EndAddr, 4, 0);

            if (End != EndAddr)
            {
                Ptr = End;
                bool Check_Read = true;

                WinAPI.ReadProcessMemory(hProcess, LogAddr, out StartAddr, 4, 0);
                WinAPI.ReadProcessMemory(hProcess, LogWordAddr, out WordAddr, 4, 0);

                int LastEnd, LastType;
                while (StartAddr != 0)
                {
                    LastEnd = End;
                    End = StartAddr;

                    int WordNum, ret = 0;
                    WinAPI.ReadProcessMemory(hProcess, StartAddr + 0x14, out WordNum, 4, 0);
                    byte[] Word = new byte[WordNum *= 2];

                    if (!Check_Read)
                    {
                        LastType = Str_Color;
                        // 0-說話 1-大喊 2-n大喊 3-隊伍 4-商會 6-密語 14-系統 15-NPC
                        WinAPI.ReadProcessMemory(hProcess, StartAddr + 0x1C, out Str_Color, 1, 0);
                        WinAPI.ReadProcessMemory(hProcess, WordAddr + AllWordNum, Word, WordNum, ref ret);

                        //Console.WriteLine(Type + " " + System.Text.Encoding.Unicode.GetString(Word));
                        if (string.IsNullOrWhiteSpace(str))
                            str += System.Text.Encoding.Unicode.GetString(Word);
                        else if (str.Contains("：") && !System.Text.Encoding.Unicode.GetString(Word).Contains("：") && LastType == Str_Color)
                            str += System.Text.Encoding.Unicode.GetString(Word);
                        else
                        {
                            Str_Color = LastType;
                            End = LastEnd;
                        }
                    }
                    AllWordNum += WordNum;

                    if (StartAddr == Ptr)
                        Check_Read = false;
                    WinAPI.ReadProcessMemory(hProcess, StartAddr, out StartAddr, 4, 0);
                }
            }
            WinAPI.CloseHandle(hProcess);
            return str;
        }


        //Chat//
        public void Chat(IntPtr hWnd, int mode, string str)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);


                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x5000, Encoding.Unicode.GetBytes(str), Encoding.Unicode.GetBytes(str).Length + 2, 0);
                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x6000, Encoding.UTF8.GetBytes(str), Encoding.UTF8.GetBytes(str).Length + 2, 0);

                WinAPI.CloseHandle(hProcess);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(0x0);
                asm.Push(0x0);
                asm.Push(TextSPACE + 0x5000);//Unicode
                asm.Push(TextSPACE + 0x6000);//UTF8
                asm.Push(0x0);
                asm.Push(0x0);
                asm.Push(mode);
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x85AE20);//chat
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //Tell//
        public void Tell(IntPtr hWnd, string targer_name, string str)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x5000, Encoding.Unicode.GetBytes(str), Encoding.Unicode.GetBytes(str).Length + 2, 0);
                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x6000, Encoding.Unicode.GetBytes(targer_name), Encoding.Unicode.GetBytes(targer_name).Length + 2, 0);

                WinAPI.CloseHandle(hProcess);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TextSPACE + 0x5000);//unicode
                asm.Push(TextSPACE + 0x6000);//target_name
                asm.Mov_ECX(CALL_ECX);
                asm.Mov_EAX(0x85B6C0);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //裝備道具..
        public void Equip(IntPtr hWnd, Form1.uInfo.Item.ItemInfo[] _Item)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                //身體 頭部 足部 手部 武器 飾品
                Form1.uInfo.Item.ItemInfo[] Equip_List = new Form1.uInfo.Item.ItemInfo[6];

                for (int i = 0; i < _Item.Length; i++)
                {
                    if (_Item[i].ID != 0 && _Item[i].Equip_Type != -1)
                    {
                        if ((Equip_List[_Item[i].Equip_Type].ID == _Item[i].ID && Equip_List[_Item[i].Equip_Type].Durability > _Item[i].Durability) || Equip_List[_Item[i].Equip_Type].Attack < _Item[i].Attack || Equip_List[_Item[i].Equip_Type].Defense < _Item[i].Defense)
                        {
                            Equip_List[_Item[i].Equip_Type] = _Item[i];

                            WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x7000 + _Item[i].Equip_Type * 0x8, BitConverter.GetBytes(Equip_List[_Item[i].Equip_Type].Code), 4, 0);
                            Console.WriteLine(Equip_List[_Item[i].Equip_Type].Name);
                        }
                    }
                }

                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x8000 + 0x4, BitConverter.GetBytes(TextSPACE + 0x7000), 4, 0);
                WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x8000 + 0x8, BitConverter.GetBytes(6), 4, 0);

                WinAPI.CloseHandle(hProcess);

                AsmClassLibrary asm = new AsmClassLibrary();

                asm.Pushad();
                asm.Push(TextSPACE + 0x8000);
                asm.Mov_ECX(CALL_ECX);
                //asm.Mov_EAX(0x89CFF0);
                asm.Mov_EAX(0x90B420);
                asm.Call_EAX();
                asm.Popad();
                asm.Ret();
                asm.RunAsm(pid);
            }
        }
        //丟棄道具..
        public void DropItem(IntPtr hWnd, Form1.uInfo.Item.ItemInfo[] _Item)
        {
            if (!SceneChange(hWnd))
            {
                int pid = GetPid(hWnd);
                int hProcess = WinAPI.OpenProcess(WinAPI.OPEN_PROCESS_ALL | WinAPI.PROCESS_CREATE_THREAD | WinAPI.PROCESS_VM_WRITE, 0, pid);

                string[] Drop_Name = new String[50];
                int[] Drop_Num = new int[50];
                LoadDropItemInfo(ref Drop_Name, ref Drop_Num);

                int Num = 0;
                for (int i = 0; i < _Item.Length; i++)
                {
                    if (_Item[i].ID == 0)
                        break;
                    if (!string.IsNullOrWhiteSpace(_Item[i].Name) && !_Item[i].Equip)
                    {
                        for (int Drop_Index = 0; Drop_Index < 50; Drop_Index++)
                        {
                            if (string.IsNullOrWhiteSpace(Drop_Name[Drop_Index]))
                                break;
                            else
                            {
                                if (_Item[i].Name == Drop_Name[Drop_Index])
                                {
                                    if (Drop_Num[Drop_Index] > 0)
                                        --Drop_Num[Drop_Index];
                                    else
                                    {
                                        //物品資料
                                        WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9200 + 0x0 + Num * 0x14, BitConverter.GetBytes(_Item[i].Code), 4, 0);
                                        WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9200 + 0x4 + Num * 0x14, BitConverter.GetBytes(0), 4, 0);
                                        WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9200 + 0x8 + Num * 0x14, BitConverter.GetBytes(_Item[i].ID), 4, 0);
                                        WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9200 + 0xC + Num * 0x14, BitConverter.GetBytes(_Item[i].Num), 4, 0);
                                        WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9200 + 0x10 + Num * 0x14, BitConverter.GetBytes(0), 4, 0);
                                        ++Num;
                                    }
                                }
                            }
                        }
                    }
                }


                if (Num > 0)
                {
                    //PTR
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9100 + 0x4, BitConverter.GetBytes(TextSPACE + 0x9200), 4, 0);
                    //總物品數
                    WinAPI.WriteProcessMemory(hProcess, TextSPACE + 0x9100 + 0x8, BitConverter.GetBytes(Num), 4, 0);

                    AsmClassLibrary asm = new AsmClassLibrary();

                    asm.Pushad();
                    asm.Push(TextSPACE + 0x9000);
                    asm.Push(TextSPACE + 0x9100);
                    asm.Mov_ECX(CALL_ECX);
                    //asm.Mov_EAX(0x89D0F0);
                    asm.Mov_EAX(0x90B530);
                    asm.Call_EAX();
                    asm.Popad();
                    asm.Ret();
                    asm.RunAsm(pid);
                }

                WinAPI.CloseHandle(hProcess);
            }
        }
    }
}
