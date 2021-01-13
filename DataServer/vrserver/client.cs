using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace vrserver
{
    class Client
    {
        private Socket clientSocket;
        public string clientip;
        private Thread t;
        private byte[] data = new byte[1024];//这个是一个数据容器

        public Client(Socket s)
        {
            clientSocket = s;
            clientip = ((IPEndPoint)s.RemoteEndPoint).Address.ToString();
            //启动一个线程 处理客户端的数据接收
            t = new Thread(ReceiveMessage);
            t.Start();
        }
        /// <summary>
        /// 接收客户端数据
        /// </summary>
        private void ReceiveMessage()
        {
            //一直接收客户端的数据
            while (true)
            {
                try
                {
                    //在接收数据之前  判断一下socket连接是否断开,10是超时时间
                    if (clientSocket.Poll(10, SelectMode.SelectRead))
                    {
                        clientSocket.Close();
                        break;//跳出循环 终止线程的执行
                    }

                    int length = clientSocket.Receive(data);
                    string message = Encoding.UTF8.GetString(data, 0, length);
                    //接收到数据的时候 要把这个数据 分发到客户端
                    //广播这个消息
                    Client client = new Client(clientSocket);
                    if (message != "")
                    {
                        //Console.WriteLine("收到了消息:" + message);
                        Console.WriteLine("received:" + message);
                        Program.BroadcastMessage(message, client.clientip);
                        
                    }
                }
                catch
                {

                }

            }
        }
        /// <summary>
        /// 给客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        /// <summary>
        /// 获取当前客户端连接的状态false表示断开连接
        /// </summary>
        public bool Connected
        {
            get { return clientSocket.Connected; }
        }
    }
}
