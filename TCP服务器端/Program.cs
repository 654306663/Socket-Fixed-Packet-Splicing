using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP服务器端
{
    public class Program
    {
        static void Main(string[] args)
        {
            StartServer();
            Console.ReadKey();
        }
        
        #region 跟多个客户端异步接收
        static void StartServer()
        {
            // 1. 创建服务器端Socket实例
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,  // 局域网
                SocketType.Stream, // 类型：流套接字
                ProtocolType.Tcp);  // 协议类型：TCP

            // 2. 指定服务器IP
            IPAddress ipAddress = IPAddress.Parse("192.168.3.79");
            // 3. 规定开放哪些端口
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);
            // 4. 绑定开放的端口
            serverSocket.Bind(ipEndPoint);      // 绑定地址和端口号
            // 5. 开始监听
            serverSocket.Listen(0);     // 监听端口号  0代表监听数量不限
            // 6. 异步监听客户端连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);     // 异步接收连接

        }

        static SpliceByte message = new SpliceByte();
        /// <summary>
        /// 接收连接
        /// </summary>
        /// <param name="result"></param>
        static void AcceptCallBack(IAsyncResult result)
        {
            Socket serverSocket = result.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(result);

            // 向客户端发送一条消息
            string msg = "Hello! 你好！";
            byte[] data = Encoding.UTF8.GetBytes(msg);     // 修改编码为UTF-8 并将字符串转成字节数组
            clientSocket.Send(data);


            serverSocket.BeginAccept(AcceptCallBack, serverSocket);     // 异步接收连接

            // 7. 异步接收客户端消息
            clientSocket.BeginReceive(message.Data, message.UsedCount, message.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);        // 异步接收消息
        }

        /// <summary>
        /// 接收消息内容
        /// </summary>
        /// <param name="result"></param>
        static void ReceiveCallBack(IAsyncResult result)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = result.AsyncState as Socket;
                int count = clientSocket.EndReceive(result);
                // 如果接收消息长度为0，则关闭该客户端连接
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                message.ReadData(count);        // 解决粘包问题

                clientSocket.BeginReceive(message.Data, message.UsedCount, message.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);        // 异步接收消息
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                if (clientSocket != null) clientSocket.Close();     // 如果客户端非正常关闭，则关闭该客户端连接
            }
        }

        #endregion
    }
}
