using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat_Server
{
    public partial class Form1 : Form
    {
        private Socket listener;
        private Thread ServerThread;//服务端运行的线程
        private Socket[] ClientSocket;//为客户端建立的SOCKET连接
        private String[] ClientName;
        private int ClientNumb;//存放客户端数量
        private Byte[] MsgBuffer;//存放消息数据

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSocket = new Socket[65535];//为客户端提供连接个数
            ClientName = new String[65535];
            MsgBuffer = new Byte[65535];//消息数据大小
            CheckForIllegalCrossThreadCalls = false;//不捕获对错误线程的调用

            ClientNumb = 0;//数量从0开始统计

            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                ServerThread = new Thread(new ThreadStart(RecieveAccept));//将接受客户端连接的方法委托给线程
                ServerThread.Start();//线程开始运行
                textBox1.AppendText("伺服器於 " + DateTime.Now + " 開始運行。\r\n");
            }
            catch { }
        }

        //接受客户端连接的方法
        private void RecieveAccept()
        {
            while (true)
            {
                //Accept 以同步方式从侦听套接字的连接请求队列中提取第一个挂起的连接请求，然后创建并返回新的 Socket。
                //在阻止模式中，Accept 将一直处于阻止状态，直到传入的连接尝试排入队列。连接被接受后，原来的 Socket 继续将传入的连接请求排入队列，直到您关闭它。
                ClientSocket[ClientNumb] = listener.Accept();

                ClientSocket[ClientNumb].BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallBack), ClientSocket[ClientNumb]);

                lock (textBox1)
                {
                    textBox1.AppendText("[" + DateTime.Now + "]" + ClientSocket[ClientNumb].RemoteEndPoint.ToString() + " 已連線至伺服器。\r\n");
                }
                ClientNumb++;
            }
        }

        //回发数据给客户端
        private void RecieveCallBack(IAsyncResult AR)
        {
            string Time = "[" + System.DateTime.Now.Hour.ToString("00") + "：" + System.DateTime.Now.Minute.ToString("00") + "]";

            Socket RSocket = (Socket)AR.AsyncState;
            try
            {
                int REnd = RSocket.EndReceive(AR);

                if (REnd > 0)
                {
                    lock (textBox1)
                        textBox1.AppendText(Time + Encoding.Unicode.GetString(MsgBuffer, 0, REnd));

                    for (int i = 0; i < ClientNumb; i++)
                    {
                        if (ClientSocket[i].Connected)
                        {
                            if (Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Contains("已連線！"))
                            {
                                if (ClientSocket[i].RemoteEndPoint == RSocket.RemoteEndPoint)
                                {
                                    ClientName[i] = Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Replace(" ", "").Substring(0, Encoding.Unicode.GetString(MsgBuffer, 0, REnd).IndexOf("已連線！") - 1);
                                    listBox1.Items.Add(ClientName[i] + " " + ClientSocket[i].RemoteEndPoint);

                                    if (!string.IsNullOrEmpty(ClientName[i]))
                                    {
                                        if (ClientName[i].Contains("﹝") && ClientName[i].Contains("﹞"))
                                        {
                                            ClientSocket[i].Send(Encoding.Unicode.GetBytes("[活動]" + ClientName[i] + " 穫得測試權限。\r\n"));
                                        }
                                    }

                                    string user_ver = Encoding.Unicode.GetString(MsgBuffer, 0, REnd).Replace(" ", "").Replace(ClientName[i] + "已連線！", "").Replace("版本號", "").Replace("\r\n", "");
                                    string ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\Users\Deeplife\Desktop\GvoHelper\GvoHelper\publish\Application Files\GvoHelper_1_0_0_0\GvoHelper.exe").FileVersion.ToString().Replace(" ", "");
                                    if (user_ver != ver)
                                    {
                                        //ClientSocket[i].Send(Encoding.Unicode.GetBytes("\r\n﹝目前已有新版本！﹞"));
                                    }
                                }
                            }
                            else
                            {
                                //回发数据到客户端
                                ClientSocket[i].Send(MsgBuffer, 0, REnd, SocketFlags.None);
                            }
                            //textBox1.AppendText(ClientName[i] + " " + ClientSocket[i].RemoteEndPoint + " " + ClientSocket[i].Connected + "\r\n");
                        }
                    }

                    if (RSocket.Connected)
                        RSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(RecieveCallBack), RSocket);
                }
                else
                {
                    if (RSocket.Connected)
                        RSocket.Shutdown(SocketShutdown.Both);
                }
            }
            catch 
            {
                
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerThread.Abort();//线程终止
            listener.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ClientNumb; i++)
            {
                if (!ClientSocket[i].Connected)
                    listBox1.Items.Remove(ClientName[i] + " " + ClientSocket[i].RemoteEndPoint);
            }
        }

        private void Chat_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(Chat_textBox.Text))
                {
                    string Time = "[" + System.DateTime.Now.Hour.ToString("00") + "：" + System.DateTime.Now.Minute.ToString("00") + "]";
                    textBox1.AppendText(Time + "﹝管理員﹞：" + Chat_textBox.Text + "\r\n");

                    for (int i = 0; i < ClientNumb; i++)
                    {
                        if (ClientSocket[i].Connected)
                            ClientSocket[i].Send(Encoding.Unicode.GetBytes("﹝管理員﹞：" + Chat_textBox.Text + "\r\n"));
                    }
                    Chat_textBox.Text = "";
                }
            }
        }

    }

}
