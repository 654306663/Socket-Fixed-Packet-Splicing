using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. 创建Socket实例
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 2. 指定服务器端IP地址, 字符串转换为IP地址类
            IPAddress ipAddress = IPAddress.Parse("192.168.3.79");
            // 3. 创建IP实体类对象  IP地址+端口号
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);
            // 4. 连接服务器端
            clientSocket.Connect(ipEndPoint);

            // 5. 接收服务器消息
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msg);

            // 6. 向服务器发送消息
            while (true)
            {
                string s = Console.ReadLine();
                if(s == "c")
                {
                    // 关闭连接
                    clientSocket.Close();
                    return;
                }
                if (s == "a")
                {
                    // 发送消息 测试粘包
                    for (int i = 0; i < 100; i++)
                    {
                        clientSocket.Send(SpliceByte.GetBytes(i.ToString()));       // 发送信息  该byte已经过添加头处理 
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
