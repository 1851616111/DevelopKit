using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class Log
    {
        public static LogLevel DefaultLevel = LogLevel.Error;
        static private FileStream logFile;
        static private BufferedStream logStream;

        public static void Init(string file)
        {
            logFile = new FileStream(file, FileMode.OpenOrCreate);
            logStream = new BufferedStream(logFile, 4096 * 6);
        }

        public static void Write(LogLevel level, string model, string key, string text)
        {
            if (level < DefaultLevel) {
                return;
            }  

            string spaces = "   ";
            byte[] date_time = Encoding.Default.GetBytes(DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss") + spaces);
            byte[] date_level = Encoding.Default.GetBytes(toString(level) + spaces);
            byte[] data_model = Encoding.UTF8.GetBytes(model + spaces);
            byte[] data_key = Encoding.UTF8.GetBytes(key + spaces);
            byte[] data_text = Encoding.UTF8.GetBytes(text +"\r\n");
            lock (logStream)
            {
                int offset = 0;
                logStream.Write(date_time, 0, date_time.Length);
                offset += date_time.Length;

                logStream.Write(date_level, 0, date_level.Length);
                offset += date_level.Length;

                logStream.Write(data_model, 0, data_model.Length);
                offset += data_model.Length;

                logStream.Write(data_key, 0, data_key.Length);
                offset += data_key.Length;

                logStream.Write(data_text, 0, data_text.Length);

                logStream.Flush();
            }
        }

        public static void Error(string model, string key, string text)
        {
            Write(LogLevel.Error, model, key, text);
        }

        public static void Fatal(string model, string key, string text)
        {
            Write(LogLevel.Fatal, model, key, text);
        }

        public static void Info(string model, string key, string text)
        {
            Write(LogLevel.Info, model, key, text);
        }

        public static void Debug(string model, string key, string text)
        {
            Write(LogLevel.Debug, model, key, text);
        }

        public enum LogLevel
        {
            Debug,
            Info,
            Warnning,
            Error,
            Fatal,
        }

        static private string toString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "Info";
                case LogLevel.Warnning:
                    return "Warnning";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Fatal:
                    return "Fatal";
                default:
                    return "Error";
            }
        }
    }
}