using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using System.Net;
using System.Net.Sockets;

namespace GvoHelper
{
    public partial class Form3 : Form
    {
        private GVOCall Call = new GVOCall();

        private string[] BoatName = { "小型阿拉伯帆船", "武裝快艇", "中型阿拉伯帆船", "大型阿拉伯帆船" };
        private int[] BoatWarehouse = { 0xB4, 0xF0, 0x190, 0x21C };//船倉
        private int[] BoatID = { 0x19, 0xAB, 0x1A, 0x1B };//船ID
        private int[] BoatbuildingTime = { 6, 8, 12, 20};//造船天數
        //private int Warehouse;

        //==小==
        //輕木帆船                  0x32    0x1     2
        //探險用輕木帆船            0x3C    0x30    2
        //軍用輕木帆船              0x37    0x2F    2
        //軍用卡拉維爾帆船          0xB4    0x21    10
        //商用輕木帆船              0x41    0x2D    2
        //單船桅小型船              0x4B    0x49    2
        //漢薩‧柯克帆船            0x50    0x2     4
        //武裝柯克帆船              0x46    0x31    4
        //小型卡拉維爾帆船          0x82    0x5     8
        //輕型卡拉維爾帆船          0x78    0x32    8
        //運輸用小型卡拉維爾帆船    0x96    0xD2    8
        //沿岸航行探險雙船桅船      0x8C    0x4A    8
        //雙桅柯克帆船              0x96    0x3     6
        //英國式柯克帆船            0x87    0x51    6
        //沃裡克式柯克帆船          0x10E   0x4     10
        //軍用卡拉維爾帆船
        //卡拉維爾帆船
        //商用卡拉維爾帆船
        //小型阿拉伯帆船            0xB4    0x19    6
        //突擊型阿拉伯帆船
        //商用阿拉伯帆船
        //戰鬥用大型高性能北歐帆船
        //高性能北歐帆船
        //商用大型高性能北歐帆船
        //輕型卡瑞克帆船
        //小型卡瑞克帆船
        //運輸型卡瑞克帆船
        //武裝快艇                  0xF0    0xAB    8
        //==中==
        //卡瑞克帆船
        //軍用卡瑞克帆船
        //商用卡瑞克帆船
        //輕型蓋倫帆船
        //小型蓋倫帆船
        //運輸型蓋倫帆船
        //中型阿拉伯帆船            0x190   0x1A    12
        //武裝中型阿拉伯帆船
        //商用中型阿拉伯帆船
        //護衛艦
        //==大==
        //大型阿拉伯帆船            0x21C   0x1B    20
        //商用大型阿拉伯帆船
        //武裝大型阿拉伯帆船

        //大型卡瑞克帆船
        //商用大型卡瑞克帆船
        //探險用大型卡瑞克帆船
        //商用武裝卡瑞克帆船
        //高速帆船
        //商用高速帆船
        //重型卡瑞克帆船
        //改裝重型卡瑞克帆船
        //蓋倫帆船
        //商用蓋倫帆船
        //軍用蓋倫帆船
        //商用大型蓋倫帆船
        //大型蓋倫帆船
        //重型蓋倫帆船
        //戰列艦

        //加萊排槳輕帆船
        //加萊排槳小型帆船
        //運輸用加萊排槳帆船
        //強襲用加萊排槳帆船  0x168   0x3E    10==
        //加萊排排槳帆船
        //商用加萊排槳帆船
        //阿拉伯加萊排槳帆船

        private TcpClient NetworkClient;
        private Byte[] MsgBuffer; 

        public Form3()
        {
            InitializeComponent();
        }

        private void GethWndMatrix(object sender, EventArgs e)
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
                    if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Call.GetConnectState(Form1.UsingProcess[User].hWnd) && Call.GetUserId(Form1.UsingProcess[User].hWnd) != 0)
                    {
                        timer1.Stop();

                        int Str_Color = 0;
                        string Str_Type = null;
                        string line = Call.GetLog(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User].LogStart, ref Form1.UsingProcess[User].LogEnd, ref Str_Color);
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            //Console.WriteLine(Str_Color + " " + line);
                            bool show = false;
                            string time = "[" + System.DateTime.Now.Hour.ToString("00") + "：" + System.DateTime.Now.Minute.ToString("00") + "]";

                            if ((line.Contains("<<") && line.Contains(">>")) || (line.Contains("前往﹝") && line.Contains("﹞...")))
                            {
                                Str_Type = "[輔助]";
                            }
                            else if (line.Contains("獲得") && line.Contains("點經驗。"))
                            {
                                Str_Type = "[經驗]";
                                if (exp_msg.Checked)
                                    show = true;
                            }
                            else if (line.Contains("技能的熟練度達到"))
                            {
                                Str_Type = "[熟練]";
                                if (skillexp_msg.Checked)
                                    show = true;
                            }
                            else if (line.Contains("："))
                            {
                                if (new CityCall().GetTargetID(User, line.Substring(0, line.IndexOf("：")), "") > 0x1000000)
                                {
                                    Str_Type = "[NPC]";
                                    if (npc_msg.Checked)
                                        show = true;
                                }
                                else
                                {
                                    switch (Str_Color)
                                    {
                                        case 0:
                                            Str_Type = "[公開]";
                                            if (say_msg.Checked)
                                                show = true;
                                            break;
                                        case 1:
                                        case 2:
                                            Str_Type = "[大喊]";
                                            if (shout_msg.Checked)
                                                show = true;
                                            break;
                                        case 3:
                                            Str_Type = "[艦隊]";
                                            if (party_msg.Checked)
                                                show = true;
                                            break;
                                        case 4:
                                            Str_Type = "[商會]";
                                            if (company_msg.Checked)
                                                show = true;
                                            break;
                                        case 6:
                                            Str_Type = "[悄悄話]";
                                            if (tell_msg.Checked)
                                                show = true;
                                            string Name = (line.Substring(0, line.IndexOf("："))).Replace(">>", "");
                                            Name = Name.Replace(" ", "");
                                            if (!TargetToolStripComboBox.Items.Contains(Name))
                                                TargetToolStripComboBox.Items.Add(Name);
                                            break;

                                    }
                                }
                            }
                            else
                            {
                                if (Str_Color >= 14)
                                    Str_Type = "[其它]";
                                if (other_msg.Checked)
                                    show = true;
                            }

                            if (show)
                                textBox1.AppendText(time + Str_Type + line + "\r\n");
                        }

                        timer1.Start();
                    }
                }
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TelltoolStripMenuItem.Checked = false;

            SayToolStripMenuItem.Checked = false;
            ShoutToolStripMenuItem.Checked = false;
            PartyToolStripMenuItem.Checked = false;
            CompanyToolStripMenuItem.Checked = false;
            NshoutToolStripMenuItem.Checked = false;
            ServerToolStripMenuItem.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true; ;

            switch (((ToolStripMenuItem)sender).Text)
            {
                case "Tell":
                    頻道ToolStripDropDownButton.Text = "悄悄話 >>" + TargetToolStripComboBox.Text;
                    break;
                case "Say":
                    頻道ToolStripDropDownButton.Text = "公開";
                    break;
                case "Shout":
                    頻道ToolStripDropDownButton.Text = "Shout";
                    break;
                case "Party":
                    頻道ToolStripDropDownButton.Text = "艦隊";
                    break;
                case "Company":
                    頻道ToolStripDropDownButton.Text = "商會";
                    break;
                case "NShout":
                    頻道ToolStripDropDownButton.Text = "Nshout";
                    break;
                case "聊天伺服器":
                    頻道ToolStripDropDownButton.Text = "聊天伺服器";
                    break;

            }
        }

        private void TargetToolStripComboBox_TextChanged(object sender, EventArgs e)
        {
            SayToolStripMenuItem.Checked = false;
            ShoutToolStripMenuItem.Checked = false;
            PartyToolStripMenuItem.Checked = false;
            CompanyToolStripMenuItem.Checked = false;
            NshoutToolStripMenuItem.Checked = false;
            ServerToolStripMenuItem.Checked = false;

            TelltoolStripMenuItem.Checked = true;
            頻道ToolStripDropDownButton.Text = "悄悄話 >>" + TargetToolStripComboBox.Text;
        }

        private void link_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserComboBox.Text))
            {
                textBox1.AppendText("請選擇角色\r\n");
                return;
            }

            try
            {
                if (link_button.Text == "伺服器連線")
                {
                    MsgBuffer = new Byte[65535];
                    CheckForIllegalCrossThreadCalls = false;

                    IPAddress ipAddress = IPAddress.Parse(Dns.GetHostAddresses("gvohelper.no-ip.info").GetValue(0).ToString());
                    IPEndPoint ServerInfo = new IPEndPoint(ipAddress, 8080);
                    NetworkClient = TimeOutSocket.Connect(ServerInfo, 1000);

                    if (NetworkClient.Connected)
                    {
                        link_button.Text = "中斷連線";
                        UserComboBox.Enabled = false;

                        NetworkClient.Client.Send(Encoding.Unicode.GetBytes(UserComboBox.Text + " 已連線！版本號 " + System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion + "\r\n"));

                        NetworkClient.Client.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
                    }
                }
                else if (link_button.Text == "中斷連線")
                {
                    link_button.Text = "伺服器連線";
                    UserComboBox.Enabled = true;

                    if (NetworkClient.Connected)
                        NetworkClient.Client.Shutdown(SocketShutdown.Both);
                    NetworkClient.Close();
                }
            }
            catch { 
                textBox1.AppendText("伺服器連線失敗！\r\n"); 
            }
        }

        private void ReceiveCallBack(IAsyncResult AR)
        {
            string Time = "[" + System.DateTime.Now.Hour.ToString("00") + "：" + System.DateTime.Now.Minute.ToString("00") + "]";

            try
            {
                int REnd = NetworkClient.Client.EndReceive(AR);

                if (REnd > 0)
                {
                    if (!Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Contains("："))
                    {
                        if (Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Contains("[活動]"))
                        {
                            string str = Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Replace("[活動]", "");

                            if (str.Contains("穫得測試權限。\r\n"))
                            {
                                string User_Server = str.Substring(0, str.IndexOf("﹞") + 1);
                                string User_Name = str.Replace(User_Server, "").Replace(" ", "").Replace("穫得測試權限。\r\n", "");

                                for (int User = 0; User < 5; User++)
                                    if (Form1.UsingProcess[User].Name == User_Name && Form1.UsingProcess[User].User_Type == "ELSE")
                                    {
                                        Form1.UsingProcess[User].User_Type = "TEST";
                                        lock (textBox1)
                                            textBox1.AppendText(Encoding.Unicode.GetString(MsgBuffer, 0, REnd));
                                    }
                            }
                        }
                        else if (Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Contains("[通知]"))
                        {
                            lock (textBox1)
                                textBox1.AppendText(Encoding.Unicode.GetString(MsgBuffer, 0, REnd));
                        }
                    }
                    else
                    {
                        lock (textBox1)
                            textBox1.AppendText(Time + "[聊天]" + Encoding.Unicode.GetString(MsgBuffer, 0, REnd));
                    }

                    if (NetworkClient.Connected)
                        NetworkClient.Client.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
                }
            }
            catch
            {
                //textBox1.AppendText("與伺服器連線已中斷！\r\n");
            }
        }

        private void ChatToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(ChatToolStripTextBox.Text))
                {
                    if (ServerToolStripMenuItem.Checked)
                    {
                        if (NetworkClient.Client.Connected)
                            NetworkClient.Client.Send(Encoding.Unicode.GetBytes(UserComboBox.Text + "：" + ChatToolStripTextBox.Text + "\r\n"));
                        else
                            textBox1.AppendText("與伺服器連線已中斷，無法發送訊息！\r\n");
                    }
                    else
                    {
                        for (int User = 0; User < 5; User++)
                        {
                            if (UserComboBox.Text == "﹝" + Form1.UsingProcess[User].ServerName + "﹞" + Form1.UsingProcess[User].Name)
                            {
                                if (TelltoolStripMenuItem.Checked && !string.IsNullOrWhiteSpace(TargetToolStripComboBox.Text))
                                    Call.Tell(Form1.UsingProcess[User].hWnd, TargetToolStripComboBox.Text, ChatToolStripTextBox.Text);
                                else
                                {
                                    int mode = 0;
                                    if (SayToolStripMenuItem.Checked)
                                        mode = 1;
                                    else if (ShoutToolStripMenuItem.Checked)
                                        mode = 2;
                                    else if (PartyToolStripMenuItem.Checked)
                                        mode = 3;
                                    else if (CompanyToolStripMenuItem.Checked)
                                        mode = 4;
                                    else if (NshoutToolStripMenuItem.Checked)
                                        mode = 5;

                                    Call.Chat(Form1.UsingProcess[User].hWnd, mode, ChatToolStripTextBox.Text);
                                }

                            }
                        }
                    }
                    ChatToolStripTextBox.Text = "";
                }
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (NetworkClient.Connected)
                    NetworkClient.Client.Shutdown(SocketShutdown.Both);
                NetworkClient.Close();
                //this.Close();
            }
            catch {}
        }
    }
}
