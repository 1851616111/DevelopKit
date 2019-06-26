﻿using System;
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

        public static string GetFileUnsavedTitle(string filename)
        {
            if (filename.LastIndexOf("＊") == filename.Length - 1)
            {
                return filename;
            }
            else
                return string.Format("{0}＊", filename);
        }
    }
}
