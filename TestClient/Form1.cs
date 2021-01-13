using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace vrclient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConnectToServer();
        }

        public string ipaddress = "127.0.0.1";
        public int port = 7788;
        //public UIInput textInput;//输入框
        //public UILabel chatLabel;//文字显示框

        private Socket clientSocket;
        private Thread t;
        private bool isStop = false;
        private byte[] data = new byte[1024];//数据容器
        private string message = "";//消息容器
                                    // Use this for initialization

        void ConnectToServer()
        {
            TextReader tr;
            string fullpath = Environment.CurrentDirectory + "\\ipconfig.txt";
            tr = new StreamReader(fullpath);

            ipaddress = tr.ReadLine();
            port = Convert.ToInt32(tr.ReadLine());

            if (ipaddress.Contains("."))
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //跟服务器端建立连接
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));
            }
            else
            {
                clientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                //跟服务器端建立连接
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));
            }

            

            //创建一个新的线程 用来接收消息
            t = new Thread(ReceiveMessage);
            t.Start();
        }
        /// <summary>
        /// 这个线程方法 用来循环接收消息
        /// </summary>
        void ReceiveMessage()
        {
            while (true)
            {
                if (clientSocket.Connected == false)
                    break;
                try
                {
                    int length = clientSocket.Receive(data);
                    message = Encoding.UTF8.GetString(data, 0, length);
                    //outputTB.CheckForIllegalCrossThreadCalls = false;
                    outputTB.Text +=  message + "\r\n";
                }
                catch
                {

                }
                if (isStop)
                    break;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        private void SendBTN_Click(object sender, EventArgs e)
        {
            string value = inputTB.Text;//获取输入框的值
            SendMessage(value);
            inputTB.Text = "";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            isStop = true;
            GC.Collect();
            this.Dispose();
            this.Close();
        }
    }
}
