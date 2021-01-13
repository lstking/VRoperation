using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

class Network : MonoBehaviour
{
    public string ipaddress = "127.0.0.1";
    public int port = 7788;

    public string triggername = "";//本机触发模型
    public string othertriggername = "";//其他客户端触发模型
    public string localmessage = ""; //本机坐标信息

    private Socket clientSocket;
    private Thread treceive;
    private Thread tsend;
    private bool isStop = false;
    private byte[] data = new byte[1024];//数据容器
    private string message = "";//消息容器
    private string[] getmessage ;
    float i = 0;
    
    private void Start()
    {
        ConnectToServer();
    }


    public void ConnectToServer()
    {
        TextReader tr;
        string fullpath = Environment.CurrentDirectory + "\\ipconfig.txt";
		//Debug.LogError(fullpath);
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
        treceive = new Thread(ReceiveMessage);
        treceive.Start();
		tsend = new Thread (SendMessageThread);
		tsend.Start ();
    }

    public void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
                break;
            try
            {
                int length = clientSocket.Receive(data);
                message = Encoding.UTF8.GetString(data, 0, length);
                getmessage = message.Split('|');

                if (getmessage[0] == "0")
                {
                    
                    tempdata.clientname = getmessage[2];
					tempdata.islogin = true;
                }
                if (getmessage[0] == "1")
                {
                    var go = GameObject.Find(getmessage[4]);
                    go.transform.position += new Vector3(Convert.ToInt64(getmessage[1]), Convert.ToInt64(getmessage[2]), Convert.ToInt64(getmessage[3]));
                }
                if (getmessage[0] == "2")
                {
                    tempdata.istriggered = true;
                }

            }
            catch
            {

            }
            if (isStop)
                break;
        }
    }

    public void SendMessageThread()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
                break;

            try
            {
                if(tempdata.sendlocal == true)
                {
                    SendMessage(tempdata.localmessage);
                    tempdata.sendlocal = false;
                }
                if(tempdata.sendtrigger == true)
                {
                    SendMessage(tempdata.triggermessage);
                    tempdata.sendtrigger = false;
                }
            }
            catch
            {

            }
            if (isStop)
                break;
        }
    }

    public void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }
}

