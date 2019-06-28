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
        public static bool ReadBytes(string file, out byte[] data)
        {
            FileStream fileStream = File.OpenRead(file);
            data = new byte[fileStream.Length];
            try
            {
                fileStream.Read(data, 0, (int)(fileStream.Length));
            }
            catch (Exception ex)
            {
                Log.Write(Log.LogLevel.Fatal, nameof(FileUtil), "文件读取字节失败", string.Format("read file={0} err:{1}", file, ex.ToString()));
                return false;
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
            }

            return true;
        }

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
                Log.Write(Log.LogLevel.Fatal, nameof(FileUtil), "文件写入字符串失败", string.Format("write file={0} err:{1}", file, ex.ToString()));
                return false;
            }
        }

        public static bool WriteBytesToFile(string file, byte[] bytes)
        {
            try
            {
                FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

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
                Log.Write(Log.LogLevel.Fatal, nameof(FileUtil), "文件写入字节失败", string.Format("write file={0} err:{1}", file, ex.ToString()));
                return false;
            }
        }
    }
}
