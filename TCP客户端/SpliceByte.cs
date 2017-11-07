using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP客户端
{
    class SpliceByte
    {

        /// <summary>
        /// 将传入的数据转换为byte数组，并在头部加入4字节数组长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;
        }
    }
}
