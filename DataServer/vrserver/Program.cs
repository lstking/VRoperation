using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace vrserver
{
    class Program
    {
        static string tempip = "";
        //所有连接的客户端的容器
        static List<Client> clientList = new List<Client>();
        static List<string> clientipList = new List<string>();
        static Dictionary<string, Client> clientDict = new Dictionary<string, Client>();
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
        public static void BroadcastMessage(string message, string clientip)
        {
            var notConnectedList = new List<string>();//断开连接的容器
            foreach (KeyValuePair<string, Client> kvp in clientDict)
            {
                if (kvp.Key != clientip)
                {
                    if (kvp.Value.Connected)
                    {
                        kvp.Value.SendMessage(message + "|" + clientip);
                        //Console.WriteLine("发送" + message + "|" + kvp.Key.ToString());
                        Console.WriteLine("send" + message + "|" + kvp.Key.ToString());
                    }
                    else
                    {
                        notConnectedList.Add(kvp.Key);
                    }
                }
            }
            foreach (var temp in notConnectedList)
            {
                //clientList.Remove(temp);//将断开连接的客户端移除
                tempip = temp;
                clientDict.Remove(temp);
                notConnectedList.Remove(temp);
            }
        }
        static void Main(string[] args)
        {
            TextReader tr;
            string fullpath = Environment.CurrentDirectory + "\\ipconfig.txt";
            tr = new StreamReader(fullpath);

            string serverip = tr.ReadLine();
            int serverport = Convert.ToInt32(tr.ReadLine());

            Socket tcpServer;

            if (serverip.Contains("."))
            {
                tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//开启一个Socket
                tcpServer.Bind(new IPEndPoint(IPAddress.Parse(serverip), serverport));//绑定IP和端口号
                tcpServer.Listen(100);//开启监听,最大等待数100个
            }
            else
            {
                tcpServer = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//开启一个Socket
                tcpServer.Bind(new IPEndPoint(IPAddress.Parse(serverip), serverport));//绑定IP和端口号
                tcpServer.Listen(100);//开启监听,最大等待数100个
            }

            //Console.WriteLine("本机ip为{0}.端口为{1}", serverip,serverport);
            Console.WriteLine("localaddress ip is{0}.port is{1}", serverip,serverport);
            Console.WriteLine("server running...");

            while (true)
            {
                try
                {
                    Socket clientSocket = tcpServer.Accept();//接收服务器连接
                    string clientip = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
                    Client client = new Client(clientSocket);//把与每个客户端通信的逻辑(收发消息)放到client类里面进行处理
                    //clientList.Add(client);
                    clientDict.Add(clientip, client);
                    Console.WriteLine("a client is connected !" + clientip);
                    Program.BroadcastMessage("0|login|" + clientip, clientip);
                }
                catch
                {
                    //Console.WriteLine(tempip + "已经断开连接！");
                    Console.WriteLine(tempip + "disconnected！");
                }
            }
        }
    }
}
