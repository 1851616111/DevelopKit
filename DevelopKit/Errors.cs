using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class Errors {
        static public string ProjectAlreadyExist = "项目已存在";
        static public string ProjectFileAlreadyExist = "文件已存在项目中";
        static public bool IsProjectAlreadyExistErr(string error)
        {
            return error == ProjectAlreadyExist;
        }
    }

    public class Error
    {
        public string Class;
        public string Function;
        public string Message;
        public Error(string message, Type type, string src_function)
        {
            if (type != null)
            {
                Class = type.GetType().ToString();
            }    
            Function = src_function;
            Message = message;
        }
    }
}
