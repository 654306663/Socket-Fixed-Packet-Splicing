using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP服务器端
{
    class SpliceByte
    {
        private byte[] data = new byte[1024];
        private int usedCount = 0;
        

        public byte[] Data { get { return data; } }
        public int UsedCount { get { return usedCount; } }
        public int RemainSize { get { return data.Length - usedCount; } }

        public void ReadData(int length)
        {
            usedCount += length;

            while (true)
            {
                if (usedCount <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((usedCount - 4) >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, usedCount - 4 - count);
                    usedCount -= count + 4;
                }
                else
                    return;
            }
        }
    }
}
