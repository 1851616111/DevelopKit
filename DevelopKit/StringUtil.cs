using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class StringUtil
    {
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        public static string GetFileName(string filepath)
        {
            if (filepath.Length == 0) return "";

            int index = filepath.LastIndexOf('\\');

            if (index > 0)
            {
                return filepath.Substring(index + 1);
            }
            else
            {
                return "";
            }
        }
        //tabcontrol 的bug 如果后面不增加空格， 随着标签字数越多，星号就无法显示
        public static string markFileAsUnsafed(string filename)
        {
            if (isFileUnSafed(filename))
            {
                return filename;
            }
            else
                return string.Format("{0}＊  ", filename);  
        }

        public static string markFileAsSaved(string filename)
        {
            int index = filename.LastIndexOf("＊  ");
            if (index == filename.Length -1 -2)
            {
                return filename.Remove(index, 3);
            }
            else
                return filename;
        }

        public static bool isFileUnSafed(string filename)
        {
            return filename.LastIndexOf("＊  ") == filename.Length - 1 - 2;
        }

        public static bool isFileSafed(string filename)
        {
            return filename.LastIndexOf("＊  ") != filename.Length - 1 - 2;
        }
    }
}
