using System.IO;
using System.Text;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace DevelopKit
{
    public static class FileUtil
    {
        public static Error SerializeObjectToFile(Object obj, string file)
        {
            try
            {
                FileStream fs;
                if (!File.Exists(file))
                {
                    fs = File.Create(file);
                }
                else
                {
                    fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite);
                }

                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(sw, obj);
                fs.Close();
                fs.Dispose();

                return null;
            }
            catch (Exception ex)
            {
                return new Error(ex.ToString(), typeof(FileUtil), "SerializeObjectToFile");
            }
        }

        public static Object DeserializeObjectFromFile(Type type, string file)
        {
            Object obj;
            try
            {
                FileStream fs = File.OpenRead(file);
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                obj = xmlSerializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Log.Error("FileUtil", "open file:" + file, ex.ToString());
                return null;
            }
            return obj;
        }

        public static bool ReadText(string file, out string content)
        {
            content = "";
            try
            {
                StreamReader sr = new StreamReader(file, Encoding.UTF8);
                content = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                Log.Error("FileUtil.ReadText", "读取文本文件错误", ex.ToString());
                return false;
            }

            return true;
        }

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

        public static bool FlushStringToFile(string file, string str)
        {
            try
            {
                FileStream fileStream = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite);

                byte[] bytes = Encoding.UTF8.GetBytes(str);

                //向文件中写入字节数组
                fileStream.Write(bytes, 0, bytes.Length);
                //刷新缓冲区
                fileStream.Flush();
                //关闭流
                fileStream.Close();

                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("FileUtil.FlushBytesToFile", "cath exception", ex.ToString());
                return false;
            }

            return true;
        }


        public static bool FlushBytesToFile(string file, byte[] bytes)
        {
            try
            {
                FileStream fileStream = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //向文件中写入字节数组
                fileStream.Write(bytes, 0, bytes.Length);
                //刷新缓冲区
                fileStream.Flush();
                //关闭流
                fileStream.Close();

                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("FileUtil.FlushBytesToFile", "cath exception", ex.ToString());
                return false;
            }

            return true;
        }


        static readonly string[] commonImageFileExt = new string[]{"PNG","ICO", "BMP","PCX","TIF","GIF","JPEG","TGA", "EXIF",
        "FPX","SVG", "PSD", "CDR", "PCD", "DXF", "UFO","EPS",
        "AI","HDRI", "RAW", "WMF", "FLIC", "EMF", "Webp", };

        public static bool IsFileImage(string file_ext)
        {
            foreach (string ext in commonImageFileExt)
            {

                if (file_ext.ToLower().EndsWith(ext.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }


        //参考组件 treeviewImageList 的定义顺序
        //0 文本文件
        //1 文件夹
        //2 图片
        //3 xml
        // 文件夹不参与判断
        public enum ImageListIndexOfTreeView
        {
            Text,
            Directory,
            Image,
            XML
        }

        public static ImageListIndexOfTreeView GetImageIndexByFileName(string filename)
        {
            if (filename.ToLower().EndsWith(".xml"))
            {
                return ImageListIndexOfTreeView.XML;
            }
            else if (IsFileImage(filename))
            {
                return ImageListIndexOfTreeView.Image;
            }
            else {
                return ImageListIndexOfTreeView.Text;
            }
        }
    }
}
