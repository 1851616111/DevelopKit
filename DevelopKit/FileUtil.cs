using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DevelopKit
{
    public static class FileUtil
    {
        public static bool WriteStringToFile(string file, string data)
        {
            try
            {
                FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                //定义学号
                //将字符串转换为字节数组
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                //向文件中写入字节数组
                fileStream.Write(bytes, 0, bytes.Length);
                //刷新缓冲区
                fileStream.Flush();
                //关闭流
                fileStream.Close();

                fileStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("write file=" + file + "err: " + ex.ToString());
                return false;
            }
        }
    }
}
