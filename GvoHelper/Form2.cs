using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace GvoHelper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private GVOCall Call = new GVOCall();

        private Navigator.Navigation_Info _navigation = new Navigator.Navigation_Info();

        public string SeaMapPath, LandMapPath;
        private Image S_SourceMap, S_DrawMap;
        private Image L_SourceMap, L_DrawMap;
        private PointF M_Old = new PointF();//Picturer1中取得的鼠標起始點的XY坐標值
        private PointF M_Scr = new PointF();//原始圖片中的XY坐標值
        private float M_MouseX, M_MouseY;
        private float L_MouseX, L_MouseY;
        private int M_Scr_MaxX, M_Scr_MaxY;

        private bool InDungeon, Navigate;

        private bool CheckCtrl, CheckLock, CheckMouseDown, ShowParty;
        private float Zoom = 1;

        private void Form2_Load(object sender, EventArgs e)
        {
            mapBox.Image = new Bitmap(mapBox.Width, mapBox.Height);

            //讀取圖片到S_SourceMap
            Image Map = Image.FromFile(SeaMapPath);
            S_SourceMap = new Bitmap(Map);
            Map.Dispose();

            S_DrawMap = (Image)S_SourceMap.Clone();
            PictureRefresh();
            S_DrawMap.Dispose();

            _navigation.Route = new Navigator.Navigation_Info.Route_Info[512];

            //權限
            //for (int User = 0; User < 5; User++ )
            //if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Form1.UsingProcess[User].User_Type == "猜猜看")
            {
                //StartButton.Enabled = true;
                //PauseButton.Enabled = true;
                //break;
            }
            timer1.Start();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //釋放圖片的記憶體
                if (mapBox.Image != null)
                    mapBox.Image.Dispose();
                if (S_SourceMap != null)
                    S_SourceMap.Dispose();
                if (S_DrawMap != null)
                    S_DrawMap.Dispose();
                if (LandMap.Image != null)
                    LandMap.Image.Dispose();
                if (L_SourceMap != null)
                    L_SourceMap.Dispose();
                if (L_DrawMap != null)
                    L_DrawMap.Dispose();
            }
            catch (Exception) { }
        }
        //航行路徑
        private void DrawPath(float X, float Y)
        {
            int User = 0;
            Graphics surfaceLine = Graphics.FromImage(S_DrawMap);
            surfaceLine.SmoothingMode = SmoothingMode.AntiAlias;//反鋸齒

            Pen myPen = new Pen(Color.White);
            myPen.DashStyle = DashStyle.Dash;//虛線

            float TempX = X, TempY = Y;//船隻座標
            for (int i = _navigation.Move; i < _navigation.Point; i++)
            {
                X = TempX;
                Y = TempY;

                if (Form1.UsingProcess[User].Place != 0x4 && i == _navigation.Move)
                {
                    if (Form1.UsingProcess[User].CityNo != -1)
                    {
                        X = GVOCall.City[Form1.UsingProcess[User].CityNo].Coordinate.X / 4;
                        Y = GVOCall.City[Form1.UsingProcess[User].CityNo].Coordinate.Y / 4;
                    }
                    else
                        continue;
                }
                else
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (!_navigation.Route[j].Done)
                        {
                            //上一個座標點
                            X = _navigation.Route[j].X;
                            Y = _navigation.Route[j].Y;
                            break;
                        }
                    }
                }

                //計算距離修正座標
                while (Math.Pow(Math.Pow(_navigation.Route[i].X - X, 2), 0.5) >= Math.Pow(Math.Pow(_navigation.Route[i].X + S_SourceMap.Width - X, 2), 0.5))
                    _navigation.Route[i].X += S_SourceMap.Width;
                while (Math.Pow(Math.Pow(_navigation.Route[i].X - X, 2), 0.5) > Math.Pow(Math.Pow(_navigation.Route[i].X - S_SourceMap.Width - X, 2), 0.5))
                    _navigation.Route[i].X -= S_SourceMap.Width;

                int n = (int)_navigation.Route[i].X / (int)S_SourceMap.Width;

                surfaceLine.DrawLine(myPen, _navigation.Route[i].X - S_SourceMap.Width * (n - 2), _navigation.Route[i].Y, X - S_SourceMap.Width * (n - 2), Y);
                surfaceLine.DrawLine(myPen, _navigation.Route[i].X - S_SourceMap.Width * (n - 1), _navigation.Route[i].Y, X - S_SourceMap.Width * (n - 1), Y);
                surfaceLine.DrawLine(myPen, _navigation.Route[i].X - S_SourceMap.Width * n, _navigation.Route[i].Y, X - S_SourceMap.Width * n, Y);
                surfaceLine.DrawLine(myPen, _navigation.Route[i].X - S_SourceMap.Width * (n + 1), _navigation.Route[i].Y, X - S_SourceMap.Width * (n + 1), Y);
            }
            surfaceLine.Dispose();
            myPen.Dispose();

            DrawPoint();
        }

        private void DrawPoint()
        {
            //航行路徑-點
            Graphics surfaceLine = Graphics.FromImage(S_DrawMap);
            Pen myPen = new Pen(Color.White, 0.1f);

            for (int i = 0; i < _navigation.Point; i++)
            {
                if (_navigation.Route[i].Done)
                    continue;

                int n = (int)_navigation.Route[i].X / (int)S_SourceMap.Width;

                int PointSize = 5;
                if (Zoom > 1.5)
                    PointSize = 3;

                for (int j = n - 2; j < n + 2; j++)
                {
                    surfaceLine.FillRectangle(Brushes.Orange, _navigation.Route[i].X - PointSize / 2 - S_SourceMap.Width * j, _navigation.Route[i].Y - PointSize / 2, PointSize, PointSize);
                    surfaceLine.DrawRectangle(myPen, _navigation.Route[i].X - PointSize / 2 - S_SourceMap.Width * j, _navigation.Route[i].Y - PointSize / 2, PointSize, PointSize);
                }
            }
            surfaceLine.Dispose();
            myPen.Dispose();
        }

        private void DrawDirection(float X, float Y)
        {
            int User = 0;
            if (Form1.UsingProcess[User].Place != 0x4)
                return;

            Graphics surface = Graphics.FromImage(S_DrawMap);
            surface.SmoothingMode = SmoothingMode.AntiAlias;//反鋸齒

            //航行方向
            int Rdis = 100;
            surface.DrawLine(Pens.Red, X, Y, X + Rdis * Form1.UsingProcess[User].Cos, Y + Rdis * Form1.UsingProcess[User].Sin);

            if (X + Rdis * Form1.UsingProcess[User].Cos > S_SourceMap.Width)
                surface.DrawLine(Pens.Red, X + Rdis * Form1.UsingProcess[User].Cos - S_SourceMap.Width, Y + Rdis * Form1.UsingProcess[User].Sin, X - S_SourceMap.Width, Y);
            else if (X + Rdis * Form1.UsingProcess[User].Cos < 0)
                surface.DrawLine(Pens.Red, X + Rdis * Form1.UsingProcess[User].Cos + S_SourceMap.Width, Y + Rdis * Form1.UsingProcess[User].Sin, X + S_SourceMap.Width, Y);
            surface.Dispose();

        }

        private void DrawBoat(float X, float Y)
        {
            int User = 0;
            if (Form1.UsingProcess[User].Place != 0x4)
                return;

            Graphics surface = Graphics.FromImage(S_DrawMap);
            surface.SmoothingMode = SmoothingMode.AntiAlias;//反鋸齒
            
            int alen = 4;//到頂點距離
            int alen2 = 5;//到兩邊頂點距離
            //三角型點
            float siteX2 = X + alen * Form1.UsingProcess[User].Cos, siteY2 = Y + alen * Form1.UsingProcess[User].Sin, siteX3, siteY3, siteX4, siteY4;

            if ((Form1.UsingProcess[User].Cos * Form1.UsingProcess[User].Sin) > 0)
            {
                siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型左邊點Y
                siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型右邊點Y
            }
            else
            {
                siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型左邊點Y
                siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型右邊點Y
            }

            PointF[] pts = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
            surface.DrawPolygon(Pens.Lime, pts);
            surface.FillPolygon(Brushes.Lime, pts);

            if (siteX2 > 4096 || siteX3 > 4096 || siteX4 > 4096)
            {
                siteX2 -= 4096;
                siteX3 -= 4096;
                siteX4 -= 4096;
                PointF[] pts2 = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
                surface.DrawPolygon(Pens.Lime, pts2);
                surface.FillPolygon(Brushes.Lime, pts2);
            }
            else if (siteX2 < 0 || siteX3 < 0 || siteX4 < 0)
            {
                siteX2 += 4096;
                siteX3 += 4096;
                siteX4 += 4096;
                PointF[] pts2 = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
                surface.DrawPolygon(Pens.Lime, pts2);
                surface.FillPolygon(Brushes.Lime, pts2);
            }
            surface.Dispose();
        }

        private void DrawPartyBoat()
        {
            Graphics surface = Graphics.FromImage(S_DrawMap);
            surface.SmoothingMode = SmoothingMode.AntiAlias;//反鋸齒

            int alen = 4;//到頂點距離
            int alen2 = 5;//到兩邊頂點距離

            for (int User = 1; User < 5; User++)
            {
                if (!ShowParty || Call.GetFollowStatus(Form1.UsingProcess[User].hWnd))
                    continue;
                float X = Form1.UsingProcess[User].Coordinate.X / 4;
                float Y = Form1.UsingProcess[User].Coordinate.Y / 4;
                //三角型點
                float siteX2 = X + alen * Form1.UsingProcess[User].Cos, siteY2 = Y + alen * Form1.UsingProcess[User].Sin, siteX3, siteY3, siteX4, siteY4;

                if ((Form1.UsingProcess[User].Cos * Form1.UsingProcess[User].Sin) > 0)
                {
                    siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                    siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型左邊點Y
                    siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                    siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型右邊點Y
                }
                else
                {
                    siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                    siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型左邊點Y
                    siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                    siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型右邊點Y
                }

                PointF[] pts = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
                surface.DrawPolygon(Pens.Honeydew, pts);
                surface.FillPolygon(Brushes.Honeydew, pts);

                if (siteX2 > 4096 || siteX3 > 4096 || siteX4 > 4096)
                {
                    siteX2 -= 4096;
                    siteX3 -= 4096;
                    siteX4 -= 4096;
                    PointF[] pts2 = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
                    surface.DrawPolygon(Pens.Honeydew, pts2);
                    surface.FillPolygon(Brushes.Honeydew, pts2);
                }
                else if (siteX2 < 0 || siteX3 < 0 || siteX4 < 0)
                {
                    siteX2 += 4096;
                    siteX3 += 4096;
                    siteX4 += 4096;
                    PointF[] pts2 = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
                    surface.DrawPolygon(Pens.Honeydew, pts2);
                    surface.FillPolygon(Brushes.Honeydew, pts2);
                }
            }
            surface.Dispose();
        }

        private void PictureRefresh()
        {
            try
            {
                Bitmap img = new Bitmap(mapBox.Width, mapBox.Height);
                Graphics newGraphics = Graphics.FromImage(img);

                if (M_Scr.X <= -mapBox.Width)
                {
                    M_Scr.X = M_Scr.X + S_SourceMap.Width;
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0, 0, mapBox.Width, mapBox.Height), new RectangleF(M_Scr.X, M_Scr.Y, mapBox.Width, mapBox.Height), GraphicsUnit.Pixel);
                }
                else if (M_Scr.X < 0 && M_Scr.X > -mapBox.Width)
                {
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0, 0, 0 - M_Scr.X, mapBox.Height), new RectangleF(M_Scr.X + S_SourceMap.Width, M_Scr.Y, 0 - M_Scr.X, mapBox.Height ), GraphicsUnit.Pixel);
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0 - M_Scr.X, 0, mapBox.Width + M_Scr.X, mapBox.Height), new RectangleF(0, M_Scr.Y, mapBox.Width + M_Scr.X, mapBox.Height), GraphicsUnit.Pixel);
                }
                else if (M_Scr.X > S_SourceMap.Width - mapBox.Width && M_Scr.X < S_SourceMap.Width)
                {
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0, 0, S_SourceMap.Width - M_Scr.X, mapBox.Height), new RectangleF(M_Scr.X, M_Scr.Y, S_SourceMap.Width - M_Scr.X, mapBox.Height), GraphicsUnit.Pixel);
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(S_SourceMap.Width - M_Scr.X, 0, mapBox.Width - S_SourceMap.Width + M_Scr.X, mapBox.Height), new RectangleF(0, M_Scr.Y, mapBox.Width - S_SourceMap.Width + M_Scr.X, mapBox.Height), GraphicsUnit.Pixel);
                }
                else if (M_Scr.X > S_SourceMap.Width)
                {
                    M_Scr.X = M_Scr.X - S_SourceMap.Width;
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0, 0, mapBox.Width, mapBox.Height), new RectangleF(M_Scr.X, M_Scr.Y, mapBox.Width, mapBox.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    newGraphics.DrawImage(S_DrawMap, new RectangleF(0, 0, mapBox.Width, mapBox.Height), new RectangleF(M_Scr.X, M_Scr.Y, mapBox.Width, mapBox.Height), GraphicsUnit.Pixel);
                }
                newGraphics.Dispose();

                //縮放地圖
                Graphics ZoomGraphics = Graphics.FromImage(mapBox.Image);
                ZoomGraphics.DrawImage(img, new RectangleF(0, 0, mapBox.Width, mapBox.Height), new RectangleF(0, 0, mapBox.Width / Zoom, mapBox.Height / Zoom), GraphicsUnit.Pixel);
                ZoomGraphics.Dispose();

                img.Dispose();
            }
            catch (Exception) { }
        }

        private void PictureLock()
        {
            int User = 0;
            if (LockButton.Checked && !Call.SceneChange(Form1.UsingProcess[User].hWnd))
            {
                if (Form1.UsingProcess[User].Place == 0x4 && Form1.UsingProcess[User].Coordinate.X / 4 <= 4096 && Form1.UsingProcess[User].Coordinate.Y / 4 <= 2048)
                {
                    M_Scr.X = (int)(Form1.UsingProcess[User].Coordinate.X / 4 - (float)mapBox.Width / Zoom / 2);
                    M_Scr.Y = (int)(Form1.UsingProcess[User].Coordinate.Y / 4 - (float)mapBox.Height / Zoom / 2);
                }
                else
                {
                    if (Form1.UsingProcess[User].CityNo != -1)
                    {
                        M_Scr.X = (int)((float)GVOCall.City[Form1.UsingProcess[User].CityNo].Coordinate.X / 4 - (float)mapBox.Width / Zoom / 2);
                        M_Scr.Y = (int)((float)GVOCall.City[Form1.UsingProcess[User].CityNo].Coordinate.Y / 4 - (float)mapBox.Height / Zoom / 2);
                    }
                }
            }

            M_Scr_MaxX = S_SourceMap.Width - mapBox.Width;
            M_Scr_MaxY = S_SourceMap.Height - (int)(mapBox.Height / Zoom);
            //鎖定範圍
            M_Scr.Y = Math.Max(0, Math.Min(M_Scr_MaxY, M_Scr.Y));//鎖定上下範圍
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int User = 0;
            this.Text = "GVONavi(Beta) - " + Form1.UsingProcess[User].Name;

            if (Form1.UsingProcess[User].hWnd != IntPtr.Zero)
            {
                if (Form1.UsingProcess[User].PlaceName != null)
                    所在地ToolStripStatusLabel.Text = Form1.UsingProcess[User].PlaceName;

                if (Form1.UsingProcess[User].Place == 0x4)//海上
                {
                    if (!Call.SceneChange(Form1.UsingProcess[User].hWnd) && Form1.UsingProcess[User].Coordinate.X / 4 <= 4096 && Form1.UsingProcess[User].Coordinate.Y / 4 <= 2048)
                    {
                        if (Form1.UsingProcess[User].Coordinate != PointF.Empty && Form1.UsingProcess[User].Old_Coordinate != PointF.Empty)
                        {
                            YouSite.Text = "船艦座標：" + (int)Form1.UsingProcess[User].Coordinate.X + "," + (int)Form1.UsingProcess[User].Coordinate.Y;
                            shipspeed.Text = "航速：" + (Math.Sqrt(Math.Pow(Form1.UsingProcess[User].Coordinate.X - Form1.UsingProcess[User].Old_Coordinate.X, 2) + Math.Pow(Form1.UsingProcess[User].Coordinate.Y - Form1.UsingProcess[User].Old_Coordinate.Y, 2)) / 4 * 2 * 10).ToString("#0.##") + "節";
                        }
                    }
                }
                else if (Form1.UsingProcess[User].Place !=0xFF) //陸地
                {
                    string LandMapNo = Convert.ToString(Form1.UsingProcess[User].Place * 256 + Form1.UsingProcess[User].PlaceNo, 16) + "0000.png";
                    string TempLandMapPath = Path.Combine(Application.StartupPath + @"\MiniMaps", LandMapNo);

                    #region 陸地地圖
                    if (File.Exists(TempLandMapPath))
                    {
                        if (LandMapPath != TempLandMapPath || !LandMap.Visible)
                        {
                            LandMapPath = TempLandMapPath;
                            if (LandMap.Image == null)
                                LandMap.Image = new Bitmap(Math.Min(LandMap.Width, LandMap.Height), Math.Min(LandMap.Width, LandMap.Height));

                            Image Map = Image.FromFile(LandMapPath);
                            L_SourceMap = new Bitmap(Map);
                            Map.Dispose();

                            L_DrawMap = (Image)L_SourceMap.Clone();
                            //DrawPlayer(Form1.UsingProcess[User].Coordinate.X / (78000 / L_SourceMap.Width), Form1.UsingProcess[User].Coordinate.Y / (78000 / L_SourceMap.Height));
                            DrawPlayer(Form1.UsingProcess[User].Coordinate.X / 120, Form1.UsingProcess[User].Coordinate.Y / 120);
                            LandMapRefresh();
                            L_DrawMap.Dispose();
                            if (!LandMap.Visible)
                                LandMap.Visible = true;
                        }
                    }
                    else if (Call.InDungeon(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Place))
                    {
                        //地下城
                        InDungeon = true;

                        if (!LandMap.Visible)
                        {
                            if (LandMap.Image == null)
                                LandMap.Image = new Bitmap(Math.Min(LandMap.Width, LandMap.Height), Math.Min(LandMap.Width, LandMap.Height));

                            L_SourceMap = new Bitmap(300, 360);

                            L_DrawMap = (Image)L_SourceMap.Clone();
                            DrawPeople(Form1.UsingProcess[User]._People);
                            DrawPlayer(Form1.UsingProcess[User].Coordinate.X / 20, Form1.UsingProcess[User].Coordinate.Y / 20);
                            LandMapRefresh();
                            L_DrawMap.Dispose();

                            LandMap.Visible = true;
                        }
                    }
                    else
                    {
                        LandMap.Visible = false;
                        InDungeon = false;
                    }

                    #endregion

                    if (!Call.SceneChange(Form1.UsingProcess[User].hWnd))
                    {
                        YouSite.Text = "人物座標：" + (int)Form1.UsingProcess[User].Coordinate.X + "," + (int)Form1.UsingProcess[User].Coordinate.Y;
                        shipspeed.Text = "";
                    }
                }

                //自動導航
                if (Navigate)
                {
                    if (new Navigator().Navigate(User, ref _navigation, AnchorButton.Checked))
                    {
                        Navigate = false;
                        StartButton.Visible = true;
                        PauseButton.Visible = false;
                    }
                }
                else
                {
                    StartButton.Visible = true;
                    PauseButton.Visible = false;
                }

                #region 檢查是否要重繪
                bool CheckRedraw = false;

                for (User = 0; User < 5; User++)
                {
                    double Degree = Math.Sqrt(Math.Pow(Math.Acos(Form1.UsingProcess[User].Cos) * (180D / Math.PI) - Math.Acos(Form1.UsingProcess[User].Old_Cos) * (180D / Math.PI), 2));

                    if (Form1.UsingProcess[User].Coordinate != PointF.Empty )
                    {
                        if (Degree >= 0.01 || (int)Form1.UsingProcess[User].Old_Coordinate.X / 4 != (int)Form1.UsingProcess[User].Coordinate.X / 4 || (int)Form1.UsingProcess[User].Old_Coordinate.Y / 4 != (int)Form1.UsingProcess[User].Coordinate.Y / 4)
                        {
                            CheckRedraw = true;
                            break;
                        }
                    }

                    if (!ShowParty)
                        break;
                }
                User = 0;

                if (LandMap.Visible)
                {
                    L_DrawMap = (Image)L_SourceMap.Clone();

                    if (InDungeon)
                    {
                        DrawPeople(Form1.UsingProcess[User]._People);
                        DrawPlayer(Form1.UsingProcess[User].Coordinate.X / 20, Form1.UsingProcess[User].Coordinate.Y / 20);
                    }
                    else
                        DrawPlayer(Form1.UsingProcess[User].Coordinate.X / 120, Form1.UsingProcess[User].Coordinate.Y / 120);

                    LandMapRefresh();
                    L_DrawMap.Dispose();
                    LandMap.Invalidate();
                }

                if (!CheckMouseDown && CheckRedraw)
                {
                    if (Form1.UsingProcess[User].Place <= 0x4 || (Form1.UsingProcess[User].Place >= 0x1C && Form1.UsingProcess[User].Place <= 0x20))
                    {
                        if (Form1.UsingProcess[User].Coordinate != PointF.Empty)
                        {
                            PictureLock();
                            S_DrawMap = (Image)S_SourceMap.Clone();
                            DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                            DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                            if (ShowParty)
                                DrawPartyBoat();
                            DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                            PictureRefresh();
                            S_DrawMap.Dispose();
                            mapBox.Invalidate();
                        }
                    }
                }
                #endregion
            }
            else
            {
                StartButton.Visible = true;
                PauseButton.Visible = false;
            }
        }

        private void Image_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoom_size = 0.2F;
            float last_zoom = Zoom;
            if (e.Delta > 0)
            {
                if (Zoom < 4.0F)
                {
                    M_Scr.X += (int)((mapBox.Width / Zoom - mapBox.Width / (Zoom + zoom_size)) / 2);
                    M_Scr.Y += (int)((mapBox.Height / Zoom - mapBox.Height / (Zoom + zoom_size)) / 2);
                }
            }
            else
            {
                zoom_size *= -1;
                if (Zoom > 1.0F)
                {
                    M_Scr.X += (int)((mapBox.Width / Zoom - mapBox.Width / (Zoom + zoom_size)) / 2);
                    M_Scr.Y += (int)((mapBox.Height / Zoom - mapBox.Height / (Zoom + zoom_size)) / 2);
                }
            }

            Zoom += zoom_size;
            if (Zoom > 4.0F)
                Zoom = 4.0F;
            else if (Zoom < 1.0F)
                Zoom = 1.0F;

            if (last_zoom != Zoom)
            {
                int User = 0;
                PictureLock();
                S_DrawMap = (Image)S_SourceMap.Clone();
                DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                if (ShowParty)
                    DrawPartyBoat();
                DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                PictureRefresh();
                S_DrawMap.Dispose();
                mapBox.Invalidate();
            }
        }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            float zoom_size = 0.2F;
            float last_zoom = Zoom;

            if (e.KeyChar == '+' || e.KeyChar == '-')
            {
                if (e.KeyChar == '+')
                {
                    if (Zoom < 4.0F)
                    {
                        M_Scr.X += (int)((mapBox.Width / Zoom - mapBox.Width / (Zoom + zoom_size)) / 2);
                        M_Scr.Y += (int)((mapBox.Height / Zoom - mapBox.Height / (Zoom + zoom_size)) / 2);
                    }
                }
                else if (e.KeyChar == '-')
                {
                    zoom_size *= -1;
                    if (Zoom > 1.0F)
                    {
                        M_Scr.X += (int)((mapBox.Width / Zoom - mapBox.Width / (Zoom + zoom_size)) / 2);
                        M_Scr.Y += (int)((mapBox.Height / Zoom - mapBox.Height / (Zoom + zoom_size)) / 2);
                    }
                }

                Zoom += zoom_size;
                if (Zoom > 4.0F)
                    Zoom = 4.0F;
                else if (Zoom < 1.0F)
                    Zoom = 1.0F;

                if (last_zoom != Zoom)
                {
                    int User = 0;
                    PictureLock();
                    S_DrawMap = (Image)S_SourceMap.Clone();
                    DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                    DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                    if (ShowParty)
                        DrawPartyBoat();
                    DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                    PictureRefresh();
                    S_DrawMap.Dispose();
                    mapBox.Invalidate();
                }
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CheckCtrl && e.Button == MouseButtons.Left)
            {
                //算差值
                M_Scr.X -= (int)Math.Round((e.X / Zoom - M_Old.X), MidpointRounding.AwayFromZero);
                M_Scr.Y -= (int)Math.Round((e.Y / Zoom - M_Old.Y), MidpointRounding.AwayFromZero);
                PictureLock();

                M_Old = e.Location;
                M_Old.X /= Zoom;
                M_Old.Y /= Zoom;

                PictureRefresh();//PictureBox顯示圖片
                mapBox.Refresh();
            }

            if (e.X / Zoom + M_Scr.X < 0)
                M_MouseX = (e.X / Zoom + M_Scr.X + S_SourceMap.Width) * 4;
            else if (e.X / Zoom + M_Scr.X >= S_SourceMap.Width)
                M_MouseX = (e.X / Zoom + M_Scr.X - S_SourceMap.Width) * 4;
            else
                M_MouseX = (e.X / Zoom + M_Scr.X) * 4;
            M_MouseY = (e.Y / Zoom + M_Scr.Y) * 4;

            if (!toolTip1.GetToolTip(mapBox).Contains((int)M_MouseX + ", " + (int)M_MouseY))
                toolTip1.SetToolTip(mapBox, (int)M_MouseX + ", " + (int)M_MouseY);

            if (ShowParty)
            {
                for (int User = 0; User < 5; User++)
                {
                    double TempMouseX = Math.Pow(Math.Pow(M_MouseX / 4 - Form1.UsingProcess[User].Coordinate.X / 4, 2), 0.5);
                    double TempMouseY = Math.Pow(Math.Pow(M_MouseY / 4 - Form1.UsingProcess[User].Coordinate.Y / 4, 2), 0.5);
                    if (TempMouseX < 3 && TempMouseY < 3)
                        if (!toolTip1.GetToolTip(mapBox).Contains(Form1.UsingProcess[User].Name) && !Call.GetFollowStatus(Form1.UsingProcess[User].hWnd))
                            toolTip1.SetToolTip(mapBox, Form1.UsingProcess[User].Name + "(" + (int)M_MouseX + ", " + (int)M_MouseY + ")");
                }
            }
            for (int i = 0; i < GVOCall.City.Length; i++)
            {
                double TempMouseX = Math.Pow(Math.Pow(M_MouseX / 4 - GVOCall.City[i].Coordinate.X / 4, 2), 0.5);
                double TempMouseY = Math.Pow(Math.Pow(M_MouseY / 4 - GVOCall.City[i].Coordinate.Y / 4, 2), 0.5);
                if (TempMouseX < 3 && TempMouseY < 3)
                {
                    if (!GVOCall.City[i].Name.Contains("※") && !toolTip1.GetToolTip(mapBox).Contains(GVOCall.City[i].Name))
                        toolTip1.SetToolTip(mapBox, GVOCall.City[i].Name + "(" + (int)M_MouseX + ", " + (int)M_MouseY + ")");
                }
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int User = 0;
            if (CheckCtrl && e.Button == MouseButtons.Left)
            {
                _navigation.Route[_navigation.Point].X = (int)(M_MouseX / 4);
                _navigation.Route[_navigation.Point].Y = (int)(M_MouseY / 4);

                int TempPoint = _navigation.Point;
                //檢查是否相同座標
                for (int i = 0; i < _navigation.Point; i++)
                {
                    double TempPointX = _navigation.Route[i].X, TempPointY;

                    while (TempPointX > S_SourceMap.Width)
                        TempPointX -= S_SourceMap.Width;
                    while (TempPointX < 0)
                        TempPointX += S_SourceMap.Width;

                    TempPointX = Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Point].X - TempPointX, 2));
                    TempPointY = Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Point].Y - _navigation.Route[i].Y, 2));

                    if (TempPointX < 3 && TempPointY < 3 && !_navigation.Route[i].Done)
                    {
                        if (MsgBox.Show(this, "是否刪除航行路徑", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            _navigation.Route[i] = new Navigator.Navigation_Info.Route_Info();
                            for (int j = i; j + 1 < _navigation.Point; j++)
                                _navigation.Route[j] = _navigation.Route[j + 1];
                            --_navigation.Point;
                        }
                        --_navigation.Point;
                        CheckCtrl = false;
                    }
                }
                //檢查是否為登陸點
                if (TempPoint == _navigation.Point)//非相同點
                {
                    for (int i = 0; i < GVOCall.City.Length; i++)
                    {
                        double TempPointX = Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Point].X - GVOCall.City[i].Coordinate.X / 4, 2));
                        double TempPointY = Math.Sqrt(Math.Pow(_navigation.Route[_navigation.Point].Y - GVOCall.City[i].Coordinate.Y / 4, 2));
                        if (TempPointX < 3 && TempPointY < 3 && !GVOCall.City[i].Name.Contains("※"))
                        {
                            if (MsgBox.Show(this, "是否進入『" + GVOCall.City[i].Name + " 』", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                _navigation.Route[_navigation.Point].X = GVOCall.City[i].Coordinate.X / 4;
                                _navigation.Route[_navigation.Point].Y = GVOCall.City[i].Coordinate.Y / 4;
                                _navigation.Route[_navigation.Point].CityName = GVOCall.City[i].Name;
                                //_navigation.Route[_navigation.Point].CityID = City[i].ID;
                            }
                            CheckCtrl = false;
                        }
                    }
                }
                //Console.WriteLine(_navigation.Point + " " + _navigation.Route[_navigation.Point].X + "," + _navigation.Route[_navigation.Point].Y);
                ++_navigation.Point;

                PictureLock();
                S_DrawMap = (Image)S_SourceMap.Clone();
                DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                if (ShowParty)
                    DrawPartyBoat();
                DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                PictureRefresh();
                S_DrawMap.Dispose();
                mapBox.Invalidate();

            }
            else
            {
                M_Old = e.Location;
                M_Old.X /= Zoom;
                M_Old.Y /= Zoom;

                CheckMouseDown = true;

                PictureLock();
                S_DrawMap = (Image)S_SourceMap.Clone();
                DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                if (ShowParty)
                    DrawPartyBoat();
                DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (CheckMouseDown)
            {
                S_DrawMap.Dispose();
                CheckMouseDown = false;
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int User = 0;

            if (!CheckCtrl && e.Button == MouseButtons.Left)
            {
                if (Form1.UsingProcess[User].Coordinate.X > M_MouseX && M_MouseX + S_SourceMap.Width * 4 - Form1.UsingProcess[User].Coordinate.X < Form1.UsingProcess[User].Coordinate.X - M_MouseX)
                    M_MouseX += S_SourceMap.Width * 4;
                else if (M_MouseX > Form1.UsingProcess[User].Coordinate.X && Form1.UsingProcess[User].Coordinate.X + S_SourceMap.Width * 4 - M_MouseX < M_MouseX - Form1.UsingProcess[User].Coordinate.X)
                    M_MouseX -= S_SourceMap.Width * 4;
                if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && !Call.SceneChange(Form1.UsingProcess[User].hWnd))
                    Call.Turn(Form1.UsingProcess[User].hWnd, M_MouseX, M_MouseY);
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                CheckCtrl = true;
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                CheckCtrl = false;
        }

        private void LockButton_CheckedChanged(object sender, EventArgs e)
        {
            int User = 0;
            if (!CheckLock)
                CheckLock = true;
            else
                CheckLock = false;

            PictureLock();
            S_DrawMap = (Image)S_SourceMap.Clone();
            DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            if (ShowParty)
                DrawPartyBoat();
            DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            PictureRefresh();
            S_DrawMap.Dispose();
            mapBox.Invalidate();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Navigate = true;
            StartButton.Visible = false;
            PauseButton.Visible = true;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Navigate = false;
            StartButton.Visible = true;
            PauseButton.Visible = false;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            int User = 0;

            _navigation = new Navigator.Navigation_Info();
            _navigation.Route = new Navigator.Navigation_Info.Route_Info[512];

            StartButton.Visible = true;
            PauseButton.Visible = false;

            PictureLock();
            S_DrawMap = (Image)S_SourceMap.Clone();
            if (ShowParty)
                DrawPartyBoat();
            DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            PictureRefresh();
            S_DrawMap.Dispose();
            mapBox.Invalidate();
        }

        private void ShowPartyButton_CheckedChanged(object sender, EventArgs e)
        {
            int User = 0;
            if (!ShowParty)
                ShowParty = true;
            else
                ShowParty = false;

            PictureLock();
            S_DrawMap = (Image)S_SourceMap.Clone();
            DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            if (ShowParty)
                DrawPartyBoat();
            DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            PictureRefresh();
            S_DrawMap.Dispose();
            mapBox.Invalidate();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            int User = 0;
            if (this.WindowState != FormWindowState.Minimized)
            {
                mapBox.Image.Dispose();
                mapBox.Image = new Bitmap(mapBox.Width, mapBox.Height);

                PictureLock();
                S_DrawMap = (Image)S_SourceMap.Clone();
                DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                if (ShowParty)
                    DrawPartyBoat();
                DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
                PictureRefresh();
                S_DrawMap.Dispose();
                mapBox.Invalidate();
            }
        }

        private void LandMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (InDungeon)
            {
                int ZoomOffset = (Math.Max(LandMap.Width, LandMap.Height) - Math.Min(LandMap.Width, LandMap.Height)) / 2;

                L_MouseX = (e.X - ZoomOffset) * 20;
                L_MouseY = e.Y * 20 * 1.2F;

                if (L_MouseX > 0 && L_MouseX < 6000 && L_MouseY > 0 && L_MouseY < 7200)
                {
                    if (!toolTip1.GetToolTip(LandMap).Contains("(" + (int)L_MouseX + ", " + (int)L_MouseY + ")"))
                        toolTip1.SetToolTip(LandMap, "(" + (int)L_MouseX + ", " + (int)L_MouseY + ")");
                }
                else
                    if (toolTip1.GetToolTip(LandMap) != "")
                        toolTip1.SetToolTip(LandMap, "");
            }
            else
            {
                int MapSize = 120 * Math.Min(L_SourceMap.Width, L_SourceMap.Height);
                int LandMapZoom = MapSize / Math.Min(LandMap.Width, LandMap.Height);

                int ZoomOffset = (Math.Max(LandMap.Width, LandMap.Height) - Math.Min(LandMap.Width, LandMap.Height)) / 2;

                L_MouseX = e.X * LandMapZoom;
                L_MouseY = e.Y * LandMapZoom;

                if (LandMap.Width > LandMap.Height)
                    L_MouseX = (e.X - ZoomOffset) * LandMapZoom;
                else if (LandMap.Width < LandMap.Height)
                    L_MouseY = (e.Y - ZoomOffset) * LandMapZoom;

                if (L_MouseX > 0 && L_MouseX < MapSize && L_MouseY > 0 && L_MouseY < MapSize)
                {
                    if (!toolTip1.GetToolTip(LandMap).Contains("(" + (int)L_MouseX + ", " + (int)L_MouseY + ")"))
                        toolTip1.SetToolTip(LandMap, "(" + (int)L_MouseX + ", " + (int)L_MouseY + ")");
                }
                else
                    if (toolTip1.GetToolTip(LandMap) != "")
                        toolTip1.SetToolTip(LandMap, "");
            }
        }

        private void DrawPlayer(float X, float Y)
        {
            int User = 0;

            Graphics surface = Graphics.FromImage(L_DrawMap);
            surface.SmoothingMode = SmoothingMode.AntiAlias;//反鋸齒

            int alen = Math.Min(LandMap.Width, LandMap.Height) / 25;//到頂點距離
            int alen2 = Math.Min(LandMap.Width, LandMap.Height) / 30;//到兩邊頂點距離

            //三角型點
            float siteX2 = X + alen * Form1.UsingProcess[User].Cos, siteY2 = Y + alen * Form1.UsingProcess[User].Sin, siteX3, siteY3, siteX4, siteY4;
            if ((Form1.UsingProcess[User].Cos * Form1.UsingProcess[User].Sin) > 0)
            {
                siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型左邊點Y
                siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型右邊點Y
            }
            else
            {
                siteX3 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) + 5 * Math.PI / 6);//三角型左邊點X
                siteY3 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) - 5 * Math.PI / 6);//三角型左邊點Y
                siteX4 = X + alen2 * (float)Math.Cos(Math.Acos(Form1.UsingProcess[User].Cos) - 5 * Math.PI / 6);//三角型右邊點X
                siteY4 = Y + alen2 * (float)Math.Sin(Math.Asin(Form1.UsingProcess[User].Sin) + 5 * Math.PI / 6);//三角型右邊點Y
            }

            PointF[] pts = { new PointF(siteX2, siteY2), new PointF(siteX3, siteY3), new PointF(siteX4, siteY4) };
            surface.DrawPolygon(Pens.Lime, pts);
            surface.FillPolygon(Brushes.Lime, pts);
            surface.Dispose();
        }

        private void DrawPeople(Form1.uInfo.TargetInfo[] _People)
        {
            Graphics surfaceLine = Graphics.FromImage(L_DrawMap);
            float PointSize = 7;
            int User = 0;

            for (int i = 0; i < _People.Length; i++)
            {
                //判斷是否為隊友
                bool IsParty = false;
                for (int j = 0; j < 5; j++)
                {
                    if (_People[i].ID == Form1.UsingProcess[User].PartyID[j] || _People[i].ID == Form1.UsingProcess[User].ID)
                        IsParty = true;
                }

                if (_People[i].Name == "？？？")//出口
                    surfaceLine.FillRectangle(Brushes.Orange, (int)_People[i].Coordinate.X / 20, (int)_People[i].Coordinate.Y / 20, PointSize, PointSize);
                else if (_People[i].Name == "寶箱")
                    surfaceLine.FillRectangle(Brushes.Blue, (int)_People[i].Coordinate.X / 20, (int)_People[i].Coordinate.Y / 20, PointSize, PointSize);
                else if (_People[i].Name == "！？")//障礙物
                {
                    //surfaceLine.FillRectangle(Brushes.Pink, (int)_people[i].Coordinate.X / 20, (int)_people[i].Coordinate.Y / 20, PointSize, PointSize);
                }
                else//NPC
                {
                    if (!IsParty)
                        surfaceLine.FillRectangle(Brushes.Red, (int)_People[i].Coordinate.X / 20, (int)_People[i].Coordinate.Y / 20, PointSize, PointSize);
                }
            }
            surfaceLine.Dispose();
        }

        private void LandMapRefresh()
        {
            try
            {
                Graphics newGraphics = Graphics.FromImage(LandMap.Image);
                newGraphics.Clear(Color.Black);
                newGraphics.DrawImage(L_DrawMap, new RectangleF(0, 0, LandMap.Image.Width, LandMap.Image.Height), new RectangleF(0, 0, L_DrawMap.Width, L_DrawMap.Height), GraphicsUnit.Pixel);
                newGraphics.Dispose();
            }
            catch (Exception) { }
        }

        private void LandMap_DoubleClick(object sender, EventArgs e)
        {
            int User = 0;
            PointF Coordinate = new PointF(L_MouseX, L_MouseY);
            Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, Coordinate, true);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\route"))
                Directory.CreateDirectory(Application.StartupPath + "\\route");
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\route";

            openFileDialog1.Filter = "*.route|*.route";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _navigation.Route = new Navigator.Navigation_Info.Route_Info[512];
                int count = 0;
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        _navigation.Route[count].X = Convert.ToInt32(line.Substring(0, line.IndexOf(","))) / 4;
                        _navigation.Route[count].Y = Convert.ToInt32(line.Substring(line.IndexOf(",") + 1, 5)) / 4;
                        _navigation.Route[count].CityName = line.Substring(line.LastIndexOf(",") + 1);
                        ++count;
                    }
                }
                _navigation.Point = count;
                _navigation.Move = 0;
            }

            int User = 0;

            PictureLock();
            S_DrawMap = (Image)S_SourceMap.Clone();
            DrawPath(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            DrawDirection(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            if (ShowParty)
                DrawPartyBoat();
            DrawBoat(Form1.UsingProcess[User].Coordinate.X / 4, Form1.UsingProcess[User].Coordinate.Y / 4);
            PictureRefresh();
            S_DrawMap.Dispose();
            mapBox.Invalidate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\route"))
                Directory.CreateDirectory(Application.StartupPath + "\\route");
            saveFileDialog1.InitialDirectory = Application.StartupPath + "\\route" ;

            saveFileDialog1.Filter = "*.route|*.route";
            saveFileDialog1.FileName = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using(StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    for (int i = _navigation.Move; i < _navigation.Point; i++)
                    {
                        while (_navigation.Route[i].X > S_SourceMap.Width)
                            _navigation.Route[i].X -= S_SourceMap.Width;
                        while (_navigation.Route[i].X < 0)
                            _navigation.Route[i].X += S_SourceMap.Width;

                        sw.WriteLine(((_navigation.Route[i].X * 4).ToString()).PadLeft(5) + "," + ((_navigation.Route[i].Y * 4).ToString()).PadLeft(5) + "," + _navigation.Route[i].CityName);
                    }
                }
                
            }
        }

        
    }
}