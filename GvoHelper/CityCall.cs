using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace GvoHelper
{
    class CityCall
    {
        private GVOCall Call = new GVOCall();

        public bool Check_Produce_Formula(int User, ref Form1.uInfo.Action_Sequence _Action_Sequence, bool Use)
        {
            //檢查已初始化的配方
            for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
            {
                //配方已初始化
                if (Form1.UsingProcess[User]._Item._BookInfo[i].Formula_Init)
                {
                    //檢查配方內容
                    for (int Formula_Index = 0; Formula_Index < Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo.Length; Formula_Index++)
                    {
                        if (Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo[Formula_Index].Formula_Name == _Action_Sequence.Formula_Name)
                        {
                            _Action_Sequence.Book_Name = Form1.UsingProcess[User]._Item._BookInfo[i].Name;

                            if (Use)
                            {
                                if (Form1.UsingProcess[User].TimeCount > 1)
                                {
                                    Call.UseItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._BookInfo[i].Code);
                                    Form1.UsingProcess[User].TimeCount = 0;
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Produce(int User, ref Form1.uInfo.Action_Sequence _Action_Sequence)
        {
            if (Call.GetBattleStatus(Form1.UsingProcess[User].hWnd) < 5)
            {

            }

            switch (Call.CheckWindows(Form1.UsingProcess[User].hWnd))
            {
                case null:
                case "物品欄":
                    if (!Check_Produce_Formula(User, ref _Action_Sequence, true))
                    {
                        //檢查未初始化的配方
                        for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
                        {
                            //配方未初始化
                            if (!Form1.UsingProcess[User]._Item._BookInfo[i].Formula_Init)
                            {
                                if (Form1.UsingProcess[User].TimeCount > 2)
                                {
                                    Call.UseItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._BookInfo[i].Code);
                                    Form1.UsingProcess[User].TimeCount = 0;
                                }
                            }
                        }
                    }
                    break;
                case "選擇配方":
                    if (Call.GetFormula(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User]._Item))
                    {
                        if (string.IsNullOrWhiteSpace(_Action_Sequence.Book_Name))
                        {
                            //讀取配方資料
                            if (!Check_Produce_Formula(User, ref _Action_Sequence, false))
                                Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                        }
                        else
                        {
                            if (Call.GetBookName(Form1.UsingProcess[User].hWnd) != _Action_Sequence.Book_Name)
                                Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                            else
                            {
                                for (int i = 0; i < Form1.UsingProcess[User]._Item.BookNum; i++)
                                {
                                    if (Form1.UsingProcess[User]._Item._BookInfo[i].Name == _Action_Sequence.Book_Name && Form1.UsingProcess[User]._Item._BookInfo[i].Formula_Init)
                                    {
                                        for (int j = 0; j < Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo.Length; j++)
                                        {
                                            if (Form1.UsingProcess[User]._Item._BookInfo[i]._FormulaInfo[j].Formula_Name == _Action_Sequence.Formula_Name)
                                                Call.SelectFormula(Form1.UsingProcess[User].hWnd, j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "生產":
                    if (_Action_Sequence.Formula_Name != Call.GetFormulaName(Form1.UsingProcess[User].hWnd))
                        Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                    else
                    {
                        if (!Call.GetProduceState(Form1.UsingProcess[User].hWnd))
                        {
                            Call.Produce_Continuous(Form1.UsingProcess[User].hWnd);
                            Form1.UsingProcess[User].TimeCount = 0;
                        }
                        else
                        {
                            if (Form1.UsingProcess[User].Power < 5)
                            {
                                _Action_Sequence.Recovery_Power = true;
                                Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                            }
                            else if (Form1.UsingProcess[User].TimeCount > 3 && !Call.GetProduceBoxState(Form1.UsingProcess[User].hWnd))
                                return true;
                        }
                    }
                    break;
                default:
                    Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                    break;
            }
            return false;
        }

        public bool ToTrader(int User)
        {
            if (Form1.UsingProcess[User].Place == 0x1C)
            {
                if (GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode != 0)
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode);
                else
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, 0x8F);//港口廣場
            }
            else if (Form1.UsingProcess[User].Place == 0xC)
            {
                if (Form1.UsingProcess[User]._Regular.Length > 0 && Form1.UsingProcess[User]._Regular[0].Coordinate != PointF.Empty)
                {
                    if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[0].Coordinate) > 500)
                    {
                        if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                        {
                            Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].Coordinate, true);
                        }
                    }
                    else
                        Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].ID);
                }
            }
            else if (Form1.UsingProcess[User].Place == 0x8)
            {
                if (FindNPC(User, "交易所學徒", "交易所店主"))
                    return true;
            }
            return false;
        }

        public bool BuyTrade(int User, string BuyList)
        {
            if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) == "買入交易物品")
            {
                Call.Haggle(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID);

                int[] Multiple = new int[] { 1, 2, 5, 10, 20, 50, 100 };
                int BuyMenuNum = 0;

                GVOCall.CityInfo.BuyTradeInfo[] _BuyTradeInfo = new GVOCall.CityInfo.BuyTradeInfo[15];

                if (Call.GetBuyTradeMenu(Form1.UsingProcess[User].hWnd, ref BuyMenuNum, ref _BuyTradeInfo))
                {
                    Form1.TradeInfo[] TradeInfo = new Form1.TradeInfo[BuyMenuNum * 3];
                    for (int i = 0; i < BuyMenuNum; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(BuyList) && (BuyList.Contains(_BuyTradeInfo[i].Name) || BuyList == "全部"))
                        {
                            int index = 0;
                            int TradeNum = _BuyTradeInfo[i].MaxNum;//購買量                            
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
                                    int Num = TradeNum / _BuyTradeInfo[i].Multiple[j];
                                    if (Num > 0)
                                    {
                                        for (int k = 0; k < Multiple.Length; k++)
                                            if (_BuyTradeInfo[i].Multiple[j] == Multiple[k])
                                            {
                                                TradeInfo[index].Multiple = k;
                                                break;
                                            }
                                        TradeInfo[index].ID = _BuyTradeInfo[i].ID;
                                        TradeInfo[index].Num = Num;
                                        TradeInfo[index].MultiplePrice = _BuyTradeInfo[i].MultiplePrice[j];

                                        TradeNum -= Num * _BuyTradeInfo[i].Multiple[j];
                                        ++index;
                                    }
                                }
                            }
                            */
                            #endregion

                            if (index > 0)
                            {
                                Call.BuyTrade(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, index);
                                Call.Delay(100);
                            }
                        }
                    }
                    return true;
                }
            }
            else
            {
                if (Form1.UsingProcess[User].TimeCount > 3)
                {
                    Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, "購買商品");
                    Form1.UsingProcess[User].TimeCount = 0;
                }
            }
            return false;
        }

        public bool SellTrade(int User, string SellList)
        {
            if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) == "出售交易物品")
            {
                Call.Haggle(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID);

                GVOCall.CityInfo.SellTradeInfo[] _SellTradeInfo = new GVOCall.CityInfo.SellTradeInfo[25];
                int SellMenuNum = 0;
                if (Call.GetSellTradeMenu(Form1.UsingProcess[User].hWnd, ref SellMenuNum, ref _SellTradeInfo))
                {
                    Form1.UsingProcess[User].DungeonSellIdleCount = 0;

                    Form1.TradeInfo[] TradeInfo = new Form1.TradeInfo[SellMenuNum];
                    for (int i = 0; i < SellMenuNum; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(SellList) && ((!string.IsNullOrWhiteSpace(_SellTradeInfo[i].Name) && SellList.Contains(_SellTradeInfo[i].Name)) || SellList == "全部"))
                        {
                            TradeInfo[i].Code = _SellTradeInfo[i].Code;
                            TradeInfo[i].Num = _SellTradeInfo[i].MaxNum;
                            TradeInfo[i].Price = _SellTradeInfo[i].SellPrice;
                        }
                    }
                    if (SellMenuNum > 0)
                    {
                        Call.SellTrade(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, TradeInfo, SellMenuNum);
                        return true;
                    }
                }
            }
            else
            {
                if (Form1.UsingProcess[User].TimeCount > 3)
                {
                    Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "交易所學徒", "交易所店主", "", "").ID, "賣出商品");
                    Form1.UsingProcess[User].TimeCount = 0;
                }

                ++Form1.UsingProcess[User].DungeonSellIdleCount;
                if (Form1.UsingProcess[User].DungeonSellIdleCount > 10)
                {
                    Form1.UsingProcess[User].DungeonSellIdleCount = 0;
                    return true;
                }
            }
            return false;
        }

        public bool ToPub(int User)
        {
            if (Form1.UsingProcess[User].CityNo != -1)
            {
                if (GetNpcInfo(User, "休息站老闆", "餐廳老闆"))
                {
                    if (Form1.UsingProcess[User].Place == 0xC)
                    {
                        if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User].PlaceName))
                        {
                            if (Form1.UsingProcess[User]._Regular.Length > 0 && Form1.UsingProcess[User]._Regular[0].Coordinate != PointF.Empty)
                            {
                                if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[0].Coordinate) > 500)
                                {
                                    if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                                        Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].Coordinate, true);
                                }
                                else
                                    Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].ID);
                            }
                        }
                    }
                    else
                    {
                        if (FindNPC(User, "休息站老闆", "餐廳老闆"))
                            return true;
                    }
                }
                else
                {
                    if (FindScene(User, "酒館"))
                        if (FindNPC(User, "酒館老闆", ""))
                            return true;
                }
            }
            return false;
        }

        public void Ordering(int User)
        {
            if (GVOCall.City[Form1.UsingProcess[User].CityNo].Food == 0 && GVOCall.City[Form1.UsingProcess[User].CityNo].Drink == 0)
            {
                if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) != "點餐")
                {
                    if (Form1.UsingProcess[User].TimeCount > 3)
                    {
                        Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "酒館老闆", "休息站老闆", "餐廳老闆", "").ID, "點餐");
                        //if (Form1.UsingProcess[User].Place == 0x8)
                        //Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "休息站老闆", "餐廳老闆", "", "").ID, "點餐");
                        //else
                        //Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "酒館老闆", "", "", "").ID, "點餐");
                        Form1.UsingProcess[User].TimeCount = 0;
                    }
                }
            }
            else
            {
                if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) != null)
                    Call.CloseWindows(Form1.UsingProcess[User].hWnd);
                else
                {
                    if (Form1.UsingProcess[User].TimeCount > 2)
                    {
                        Call.Ordering(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "酒館老闆", "休息站老闆", "餐廳老闆", "").ID, GVOCall.City[Form1.UsingProcess[User].CityNo].Food, GVOCall.City[Form1.UsingProcess[User].CityNo].Drink);
                        //if (Form1.UsingProcess[User].Place == 0x8)
                            //Call.Ordering(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "休息站老闆", "餐廳老闆", "", "").ID, GVOCall.City[Form1.UsingProcess[User].CityNo].Food, GVOCall.City[Form1.UsingProcess[User].CityNo].Drink);
                        //else
                            //Call.Ordering(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "酒館老闆", "", "", "").ID, GVOCall.City[Form1.UsingProcess[User].CityNo].Food, GVOCall.City[Form1.UsingProcess[User].CityNo].Drink);
                        Form1.UsingProcess[User].TimeCount = 0;
                    }
                }
            }
        }

        public bool ToLibrary(int User)
        {
            if (Form1.UsingProcess[User].CityNo != -1)
            {
                if (FindScene(User, "書庫"))
                {
                    if (FindNPC(User, "學者", ""))
                        return true;
                }
            }
            return false;
        }

        public bool ReadBook(int User, int ReadBook_Type)
        {
            if (ToLibrary(User))
            {
                if (Form1.UsingProcess[User].ReadBook)
                {
                    if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) != "閱覽書籍")
                        return true;
                    else
                    {
                        if (Call.DiscoverMsgBox(Form1.UsingProcess[User].hWnd) >= 1)
                            Form1.UsingProcess[User].GetMap = true;

                        Call.Readbook_Continuous(Form1.UsingProcess[User].hWnd, ReadBook_Type);
                    }
                }
                else
                {
                    if (Form1.UsingProcess[User].Money < 100000)
                        new CityCall().ToBank(User, 500);
                    else if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) == "閱覽書籍")
                        Form1.UsingProcess[User].ReadBook = true;
                    else
                    {
                        if (Form1.UsingProcess[User].TimeCount > 3)
                        {
                            Call.InfoButton(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "學者", "", "", "").ID, "閱覽書籍");
                            Form1.UsingProcess[User].TimeCount = 0;
                        }
                    }
                }
            }
            return false;
        }

        public bool Research(int User, string research)
        {
            if (ToLibrary(User))
            {
                if (!Form1.UsingProcess[User].Talk)
                {
                    Call.Talk(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "學者", "", "", "").ID);
                    Form1.UsingProcess[User].Talk = true;
                }
                else
                {
                    Call.Research(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "學者", "", "", "").ID, research);
                    return true;
                }
            }
            return false;
        }

        public void ToBank(int User, int Money)
        {
            if (Form1.UsingProcess[User].Place == 0x1C)
            {
                if (GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode != 0)
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode);
                else
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, 0x8F);//港口廣場
            }
            else if (Form1.UsingProcess[User].Place == 0xC)
            {
                if (Form1.UsingProcess[User]._Regular.Length > 0 && Form1.UsingProcess[User]._Regular[0].Coordinate != PointF.Empty)
                {
                    if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[0].Coordinate) > 500)
                    {
                        if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                            Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].Coordinate, true);
                    }
                    else
                        Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].ID);
                }
            }
            else if (Form1.UsingProcess[User].Place == 0x8)
            {
                if (FindNPC(User, "銀行職員", ""))
                {
                    if (Form1.UsingProcess[User].TimeCount > 3)
                    {
                        Call.MoneyManagement(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "銀行職員", "", "", "").ID, Form1.UsingProcess[User].Money, Money);
                        Form1.UsingProcess[User].TimeCount = 0;
                    }
                }
            }
        }

        public void ToPier(int User)
        {
            if (Form1.UsingProcess[User].Place == 0x8)
            {
                if (FindNPC(User, "碼頭引航員", "碼頭官員"))
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, GetClosestTarget(User, "碼頭引航員", "碼頭官員", "", "").ID, 0x43);
            }
            else if (Form1.UsingProcess[User].Place == 0xC)
            {
                if (Form1.UsingProcess[User]._Regular.Length > 0 && Form1.UsingProcess[User]._Regular[0].Coordinate != PointF.Empty)
                {
                    if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[0].Coordinate) > 500)
                    {
                        if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                        {
                            Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].Coordinate, true);
                        }
                    }
                    else
                        Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].ID);
                }
            }
        }

        public void ToSailing(int User)
        {
            if (Form1.UsingProcess[User].Place == 0x1C)
            {
                if (Form1.UsingProcess[User].TimeCount > 3)
                {
                    Call.Sailing(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID);
                    Form1.UsingProcess[User].TimeCount = 0;
                }
            }
            else
                ToPier(User);
        }

        public bool CheckTreasure(int User, int skill)
        {
            if (Call.DiscoverMsgBox(Form1.UsingProcess[User].hWnd) != 0)
            {
                if (Call.DiscoverMsgBox(Form1.UsingProcess[User].hWnd) >= 2)
                {
                    Call.StopSkill(Form1.UsingProcess[User].hWnd, skill);
                    Form1.UsingProcess[User].Report = true;

                    return true;
                }
            }
            else
            {
                for (int i = 0; i < 3 && Form1.UsingProcess[User].UsingSkill[i] != skill; i++)
                {
                    if (Form1.UsingProcess[User].UsingSkill[i] == 0)
                    {
                        Call.UseSkill(Form1.UsingProcess[User].hWnd, skill);
                        break;
                    }
                }
            }
            return false;
        }

        public bool FindTreasure(int User, string coordinate)
        {
            string place = coordinate.Substring(0, coordinate.IndexOf("﹝"));
            string skill_name = coordinate.Substring(coordinate.IndexOf("﹞") + 1);
            int skill = 0;
            coordinate = coordinate.Replace(place, "").Replace(skill_name, "").Replace("﹝", "").Replace("﹞", "");
            PointF place_coordinate = new PointF(Convert.ToInt32(coordinate.Substring(0, coordinate.IndexOf(","))), Convert.ToInt32(coordinate.Substring(coordinate.IndexOf(",") + 1)));
            if (skill_name == "生態調查")
                skill = 10;
            else if (skill_name == "視認")
                skill = 16;
            else if (skill_name == "搜索")
                skill = 20;

            if (Form1.UsingProcess[User].Place == 0x4)
            {
                if (Call.Distance(Form1.UsingProcess[User].Coordinate, place_coordinate) <= 20)
                {
                    if (!string.IsNullOrWhiteSpace(place) && Form1.UsingProcess[User].PlaceName == place)
                    {
                        if (CheckTreasure(User, skill))
                            return true;
                    }
                }
            }
            else if (Form1.UsingProcess[User].Place == 0x1C)
            {
                if (GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode != 0)
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode);
                else
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, 0x8F);//港口廣場
            }
            else
            {
                if (Form1.UsingProcess[User].CityNo != -1)
                {
                    if (!string.IsNullOrWhiteSpace(place) && place.Contains(GVOCall.City[Form1.UsingProcess[User].CityNo].Name))
                    {
                        if (new CityCall().FindScene(User, place) && !place_coordinate.IsEmpty)
                        {
                            if (Call.Distance(Form1.UsingProcess[User].Coordinate, place_coordinate) > 100)
                            {
                                if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                                    Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, place_coordinate, true);
                            }
                            else
                            {
                                if (CheckTreasure(User, skill))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool Report(int User, string Map_Type)
        {
            int Code = 0;
            switch (Map_Type)
            {
                case "美3(男性大理石像地圖)":
                    Code = 0x124;//回報編號
                    break;
                case "地3(未完成的地圖)":
                    Code = 0x287;//回報編號
                    break;
            }

            if (new CityCall().FindScene(User, "府"))
            {
                if (new CityCall().FindNPC(User, Form1.UsingProcess[User].PlaceName.Replace("府", ""), ""))
                {
                    if (Call.MsgBox(Form1.UsingProcess[User].hWnd) != 0)
                    {
                        Form1.UsingProcess[User].GetMap = false;
                        Form1.UsingProcess[User].Report = false;
                        return true;
                    }
                    else
                        Call.Report(Form1.UsingProcess[User].hWnd, new CityCall().GetClosestTarget(User, Form1.UsingProcess[User].PlaceName.Replace("府", ""), "", "", "").ID, Code);
                }
            }
            return false;
        }

        public int GetTargetID(int User, string NpcName, string NpcName2)
        {
            int targetID = 0;
            Form1.uInfo.TargetInfo[] _People = Call.GetAllPeople(Form1.UsingProcess[User].hWnd);

            for (int i = 0; i < _People.Length; i++)
            {
                if (!_People[i].Coordinate.IsEmpty && (_People[i].Name == NpcName || _People[i].Name == NpcName2))
                {
                    targetID = _People[i].ID;
                    break;
                }
            }
            return targetID;
        }

        public bool GetNpcInfo(int User, string NpcName, string NpcName2)
        {
            if (Form1.UsingProcess[User].CityNo != -1)
            {
                Form1.uInfo.TargetInfo[] _People = Call.GetAllPeople(Form1.UsingProcess[User].hWnd);

                for (int i = 0; i < _People.Length; i++)
                {
                    if (_People[i].ID != 0 && (_People[i].Name == NpcName || _People[i].Name == NpcName2))
                    {
                        for (int NPC_Index = 0; NPC_Index < GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo.Length; NPC_Index++)
                        {
                            if (GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID == 0 || _People[i].ID == GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID || _People[i].Coordinate == GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Coordinate)
                            {
                                //Console.WriteLine(_People[i].Name + " " + _People[i].ID + " " + _People[i].Coordinate);
                                GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID = _People[i].ID;
                                GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name = _People[i].Name;
                                if (!_People[i].Coordinate.IsEmpty)
                                {
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Coordinate = _People[i].Coordinate;
                                    Call.GetPeopleAngle(Form1.UsingProcess[User].hWnd, _People[i].ID, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].cos, ref GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].sin);
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        //取得最近目標
        public GVOCall.CityInfo.TargetInfo GetClosestTarget(int User, string NpcName, string NpcName2, string NpcName3, string NpcName4)
        {
            GVOCall.CityInfo.TargetInfo Target = new GVOCall.CityInfo.TargetInfo();
            if (Form1.UsingProcess[User].CityNo != -1)
            {
                float Target_Distance, Min_Distance = 999999;
                for (int NPC_Index = 0; NPC_Index < GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo.Length; NPC_Index++)
                {
                    if (GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].ID == 0)
                        break;
                    else if (GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name == NpcName || GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name == NpcName2 || GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name == NpcName3 || GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Name == NpcName4)
                    {
                        Target_Distance = Call.Distance(Call.GetCoordinate(Form1.UsingProcess[User].hWnd), GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index].Coordinate);
                        //最近的目標
                        if (Min_Distance > Target_Distance)
                            Target = GVOCall.City[Form1.UsingProcess[User].CityNo]._NPCInfo[NPC_Index];
                    }
                }
            }
            return Target;
        }
        //取得最遠目標座標
        public PointF GetFarthestTargetCoordinate(int User)
        {
            PointF Coordinate = new PointF();
            float Target_Distance, Max_Distance = 0;
            Form1.uInfo.TargetInfo[] _People = Call.GetAllPeople(Form1.UsingProcess[User].hWnd);

            for (int i = 0; i < _People.Length; i++)
            {
                if (_People[i].ID > 0x1000000 && !_People[i].Coordinate.IsEmpty)
                {
                    Target_Distance = Call.Distance(Call.GetCoordinate(Form1.UsingProcess[User].hWnd), _People[i].Coordinate);
                    //最遠的目標
                    if (Max_Distance < Target_Distance)
                    {
                        Max_Distance = Target_Distance;
                        Coordinate = _People[i].Coordinate;
                    }
                }
            }
            return Coordinate;
        }

        public bool GoToNPC(int User, GVOCall.CityInfo.TargetInfo target)
        {
            if (target.Coordinate.IsEmpty)
                return false;
            else if (Call.Distance(Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), target.Coordinate) > 550)
            {
                if (Form1.UsingProcess[User].Place == 0xC)
                    Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, target.Coordinate, true);
                else
                {
                    Console.WriteLine(target.Coordinate + " " + target.cos + "   " +target.sin);
                    PointF coordinate = new PointF(target.Coordinate.X + (target.cos > 0 ? 350 : -350) + new Random().Next((int)target.cos * -200, (int)target.cos * 200), target.Coordinate.Y + (target.sin > 0 ? 350 : -350) + new Random().Next((int)target.sin * -200, (int)target.sin * 200));
                    Console.WriteLine(coordinate);
                    Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, coordinate, true);
                }
            }
            else
            {
                if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                {
                    if (Form1.UsingProcess[User].Place != 0xC && Call.CheckWindows(Form1.UsingProcess[User].hWnd) == null)
                    {
                        if (Call.Distance(Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), target.Coordinate) > 250)
                        {
                            Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, target.Coordinate, true);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool FindNPC(int User, string NpcName, string NpcName2)
        {
            if (GetNpcInfo(User, NpcName, NpcName2))
            {
                if (GoToNPC(User, GetClosestTarget(User, NpcName, NpcName2, "", "")))
                    return true;
            }
            else if (Form1.UsingProcess[User].Place != 0xC)
            {
                if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                {
                    Form1.UsingProcess[User].Rand_Coordinate = GetFarthestTargetCoordinate(User);
                    if (!Form1.UsingProcess[User].Rand_Coordinate.IsEmpty)
                    {
                        Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Rand_Coordinate, true);

                        if (NpcName2 != "")
                            Call.PostMessage(Form1.UsingProcess[User].hWnd, "搜尋﹝" + NpcName + "、" + NpcName2 + "﹞...");
                        else
                            Call.PostMessage(Form1.UsingProcess[User].hWnd, "搜尋﹝" + NpcName + "﹞...");
                    }
                }
                else if (Call.GetTCoordinate(Form1.UsingProcess[User].hWnd) != Form1.UsingProcess[User].Rand_Coordinate)
                {
                    Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), true);
                    Form1.UsingProcess[User].Rand_Coordinate = Call.GetTCoordinate(Form1.UsingProcess[User].hWnd);
                }
            }
            return false;
        }

        public bool CheckScene(int User, string SceneName)
        {
            for (int i = 0; i < GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo.Length; i++)
            {
                if (GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].ID == 0)
                    break;
                else if (!GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate.IsEmpty && !string.IsNullOrWhiteSpace(GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name))
                {
                    if (GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name.Contains(SceneName))
                    {
                        if (Call.Distance(Form1.UsingProcess[User].Coordinate, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate) > 500)
                        {
                            if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                            {
                                Call.PostMessage(Form1.UsingProcess[User].hWnd, "前往﹝" + SceneName + "﹞...");
                                Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate, true);
                            }
                            else if (Call.GetTCoordinate(Form1.UsingProcess[User].hWnd) != GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate)
                            {
                                if (Call.Distance(Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate) <= 750)
                                {
                                    Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), true);
                                    GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate = Call.GetTCoordinate(Form1.UsingProcess[User].hWnd);
                                }
                            }
                        }
                        else
                            Call.IntoScene(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].ID);
                        return true;
                    }
                }

            }
            return false;
        }

        public bool FindScene(int User, string SceneName)
        {
            if (Form1.UsingProcess[User].Place == 0x8)
            {
                if (!CheckScene(User, SceneName))
                {
                    for (int i = 0; i < GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo.Length; i++)
                    {
                        if (GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].ID != 0)
                        {
                            if (!GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate.IsEmpty && string.IsNullOrWhiteSpace(GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Name))
                            {
                                if (Call.Distance(Form1.UsingProcess[User].Coordinate, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate) > 500)
                                {
                                    if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                                    {
                                        Call.PostMessage(Form1.UsingProcess[User].hWnd, "搜尋﹝" + SceneName + "﹞..." + GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate);
                                        Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate, true);
                                    }
                                    else if (Call.GetTCoordinate(Form1.UsingProcess[User].hWnd) != GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate)
                                    {
                                        if (Call.Distance(Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate) <= 750)
                                        {
                                            Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), true);
                                            GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].Coordinate = Call.GetTCoordinate(Form1.UsingProcess[User].hWnd);
                                        }
                                    }
                                }
                                else
                                    Call.IntoScene(Form1.UsingProcess[User].hWnd, GVOCall.City[Form1.UsingProcess[User].CityNo]._SceneInfo[i].ID);
                                break;
                            }
                        }
                        else
                        {
                            if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                            {
                                Form1.UsingProcess[User].Rand_Coordinate = GetFarthestTargetCoordinate(User);
                                if (!Form1.UsingProcess[User].Rand_Coordinate.IsEmpty)
                                {
                                    Call.PostMessage(Form1.UsingProcess[User].hWnd, "﹝" + SceneName + "﹞座標未儲存，開始隨機移動...");
                                    Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Rand_Coordinate, true);
                                }
                            }
                            else if (Call.GetTCoordinate(Form1.UsingProcess[User].hWnd) != Form1.UsingProcess[User].Rand_Coordinate)
                            {
                                Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Call.GetTCoordinate(Form1.UsingProcess[User].hWnd), true);
                                Form1.UsingProcess[User].Rand_Coordinate = Call.GetTCoordinate(Form1.UsingProcess[User].hWnd);
                            }
                            break;
                        }
                    }
                }
            }
            else if (Form1.UsingProcess[User].Place == 0xC)
            {
                if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User].PlaceName))
                {
                    if (Form1.UsingProcess[User].PlaceName.Contains(SceneName))
                        return true;
                    else
                    {
                        if (Form1.UsingProcess[User]._Regular.Length > 0 && Form1.UsingProcess[User]._Regular[0].Coordinate != PointF.Empty)
                        {
                            if (Call.Distance(Form1.UsingProcess[User].Coordinate, Form1.UsingProcess[User]._Regular[0].Coordinate) > 500)
                            {
                                if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd))
                                {
                                    Call.MoveNearbyCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].Coordinate, true);
                                }
                            }
                            else
                                Call.IntoScene(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Regular[0].ID);
                        }
                    }
                }
            }
            else if (Form1.UsingProcess[User].Place == 0x1C)
            {
                if (GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode != 0)
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, GVOCall.City[Form1.UsingProcess[User].CityNo].EnterMode);
                else
                    Call.IntoCity(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, 0x8F);//港口廣場
            }
            return false;
        }
    }
}
