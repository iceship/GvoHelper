using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace GvoHelper
{
    public class Navigator
    {
        private GVOCall Call = new GVOCall();

        public struct Navigation_Info
        {
            public struct Route_Info
            {
                public float X;
                public float Y;
                public string CityName;
                public int CityID;
                public bool Done;
            }

            public int Move, Point;
            public Route_Info[] Route;
        }

        public bool Navigate(int User, ref Navigation_Info _navigation, bool Anchor)
        {
            if (_navigation.Move < _navigation.Point)
            {
                if (Form1.UsingProcess[User].Place == 0x4)
                {
                    #region 海上導航
                    if (!Form1.UsingProcess[User].Coordinate.IsEmpty)
                    {
                        while (Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Move].X - Form1.UsingProcess[User].Coordinate.X / 4, 2)) >= Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Move].X + 4096 - Form1.UsingProcess[User].Coordinate.X / 4, 2)))
                            _navigation.Route[_navigation.Move].X += 4096;
                        while (Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Move].X - Form1.UsingProcess[User].Coordinate.X / 4, 2)) > Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Move].X - 4096 - Form1.UsingProcess[User].Coordinate.X / 4, 2)))
                            _navigation.Route[_navigation.Move].X -= 4096;

                        float Check_X = _navigation.Route[_navigation.Move].X * 4 - Form1.UsingProcess[User].Coordinate.X, Check_Y = _navigation.Route[_navigation.Move].Y * 4 - Form1.UsingProcess[User].Coordinate.Y;
                        //座標點到目標點的距離
                        float Distance = (float)Math.Sqrt(Math.Pow(Check_X, 2) + Math.Pow(Check_Y, 2));
                        //座標點到目標點的COS、SIN角度
                        float TCos = Check_X / Distance - Form1.UsingProcess[User].Cos;
                        float TSin = Check_Y / Distance - Form1.UsingProcess[User].Sin;

                        //暴風雨&雪
                        if (Anchor && Call.GetBadWeather(Form1.UsingProcess[User].hWnd))
                            Tempest(Form1.UsingProcess[User].hWnd);
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(_navigation.Route[_navigation.Move].CityName) && _navigation.Route[_navigation.Move].CityID == 0)
                            {
                                for (int i = 0; i < Form1.UsingProcess[User]._Regular.Length; i++)
                                {
                                    if (Call.Distance(new PointF(_navigation.Route[_navigation.Move].X * 4, _navigation.Route[_navigation.Move].Y * 4), Form1.UsingProcess[User]._Regular[i].Coordinate) < 20)
                                        _navigation.Route[_navigation.Move].CityID = Form1.UsingProcess[User]._Regular[i].ID;
                                }
                            }

                            if (Distance < 25 && _navigation.Route[_navigation.Move].CityName != null && _navigation.Route[_navigation.Move].CityID != 0)
                                Call.IntoScene(Form1.UsingProcess[User].hWnd, _navigation.Route[_navigation.Move].CityID);
                            else if (Distance < 10 && Call.GetSailStatus(Form1.UsingProcess[User].hWnd) > 0)
                            {
                                _navigation.Route[_navigation.Move++].Done = true;

                                if (_navigation.Move < _navigation.Point && !_navigation.Route[_navigation.Move].Done)
                                    Call.Turn(Form1.UsingProcess[User].hWnd, _navigation.Route[_navigation.Move].X * 4, _navigation.Route[_navigation.Move].Y * 4);
                                else
                                {
                                    if (Call.GetSailStatus(Form1.UsingProcess[User].hWnd) != 0)
                                        Call.ChangeSailStatus(Form1.UsingProcess[User].hWnd, 0);
                                }
                            }
                            else
                            {
                                if (Call.GetSailStatus(Form1.UsingProcess[User].hWnd) > 0 && (TCos >= 0.085 || TCos <= -0.085 || TSin >= 0.085 || TSin <= -0.085))
                                {
                                    #region 航向錯誤
                                    ++Form1.UsingProcess[User].TurnCount;
                                    if (Form1.UsingProcess[User].TurnCount >= 5)
                                    {
                                        Console.WriteLine("轉向[" + _navigation.Move + "] TX = " + _navigation.Route[_navigation.Move].X * 4 + " TY = " + _navigation.Route[_navigation.Move].Y * 4);
                                        Call.Turn(Form1.UsingProcess[User].hWnd, _navigation.Route[_navigation.Move].X * 4, _navigation.Route[_navigation.Move].Y * 4);
                                        Form1.UsingProcess[User].TurnCount = -15;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Distance < 30)
                                        Form1.UsingProcess[User].TurnCount = 1;
                                    else
                                        Form1.UsingProcess[User].TurnCount = 0;
                                }

                                #region 調整帆位
                                if (Call.GetSailStatus(Form1.UsingProcess[User].hWnd) != 4)
                                {
                                    if (Form1.UsingProcess[User].TimeCount >= 10)
                                    {
                                        Call.ChangeSailStatus(Form1.UsingProcess[User].hWnd, 4);
                                        Form1.UsingProcess[User].TimeCount = 0;
                                        Form1.UsingProcess[User].TurnCount = 0;
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User].PlaceName) && !Form1.UsingProcess[User].PlaceName.Contains("海域"))
                    {
                        if (!string.IsNullOrWhiteSpace(_navigation.Route[_navigation.Move].CityName) && Form1.UsingProcess[User].PlaceName.Contains(_navigation.Route[_navigation.Move].CityName))
                        {
                            _navigation.Route[_navigation.Move++].Done = true;
                            return true;
                        }
                        else
                            new CityCall().ToSailing(User);
                    }
                }
            }
            else
            {
                //到達終點
                _navigation = new Navigation_Info();
                _navigation.Route = new Navigator.Navigation_Info.Route_Info[512];
                return true;
            }
            return false;
        }

        public void Tempest(IntPtr hWnd)
        {
            for (int User = 0; User < 5; User++)
            {
                if (Form1.UsingProcess[User].hWnd != IntPtr.Zero)
                {
                    if (Call.GetBadWeather(Form1.UsingProcess[User].hWnd) && !Call.GetFollowStatus(Form1.UsingProcess[User].hWnd))
                        if (Call.GetSailStatus(Form1.UsingProcess[User].hWnd) != 0)
                            Call.ChangeSailStatus(Form1.UsingProcess[User].hWnd, 0);
                }
            }
        }
    }
}
