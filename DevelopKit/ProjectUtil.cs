using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class ProjectUtil
    {
        public static Project NewProject(string vtype, string name, string path, string dev)
        {
            Project project = new Project();
            project.VehicleType = vtype;
            project.ProjectName = name;
            project.ProjectPath = path;
            project.Developer = dev;
            project.CreateTime = DateTime.Now.ToString();
            project.Status = ProjectStatus.StartCreateProject;

            return project;
        }

        //初始化项目目录
        public static bool StartCreateProject(Project project, bool overwrite, out string error, out string errordetail)
        {
            error = "";
            errordetail = "";

            try
            {
                if (!Directory.Exists(project.ProjectPath))
                {
                    Directory.CreateDirectory(project.ProjectPath);
                }

                if (!Directory.Exists(project.GetConfigDir()))
                {
                    Directory.CreateDirectory(project.GetConfigDir());
                }

                if (!overwrite && File.Exists(project.GetConfigXml()))
                {
                    error = Errors.ProjectAlreadyExist;
                    return false;
                }

                StreamWriter streamWriter = new StreamWriter(project.GetConfigXml(), false, Encoding.UTF8);
                streamWriter.Write(project.ToXml());
                streamWriter.Flush();
                streamWriter.Dispose();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                error = "初始化项目失败";
                errordetail = ex.ToString();
                return false;
            }
            return true;
        }

    }
}
