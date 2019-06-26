using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{

    public class Project
    {
        static string RuntimeConfigDirName = ".kit";
        static string RuntimeConfigXmlName = "project.xml";

        public string VehicleType;  //车型
        public string ProjectName;  //项目名称
        public string ProjectPath;  //项目路径
        public string Developer;    //开发者
        public ProjectStatus Status; //项目状态
        public string CreateTime;   //创建时间
        public string LastOpenTime; //上次打开时间
        public string LastInternalError; //上次内部错误信息; 

        public ProjectFileEditer filesEditer; // 文件编辑器

        public SortedList sl; //车型_场景       vs 组件
        public SortedList s2; //车型_场景_组建  vs 属姓  

        public Project(string vtype, string name, string path, string dev)
        {
            VehicleType = vtype;
            ProjectName = name;
            ProjectPath = path;
            Developer = dev;
            CreateTime = DateTime.Now.ToString();
            Status = ProjectStatus.StartCreateProject;
            filesEditer = new ProjectFileEditer();
        }

        public void SetStatusOpen()
        {
            Status = ProjectStatus.StartOpenProject;
        }

        public ProjectStatus NextStatus()
        {
            switch (Status)
            {
                case ProjectStatus.StartCreateProject:
                    Status = ProjectStatus.FinishCreateProject;
                    NextStatus();
                    break;
                case ProjectStatus.FinishCreateProject:
                    Status = ProjectStatus.StartOpenProject;
                    break;
                case ProjectStatus.StartOpenProject:
                    Status = ProjectStatus.FinishOpenProject;
                    break;
                case ProjectStatus.FinishOpenProject:
                    Status = ProjectStatus.FinishOpenProject;
                    break;
            }

            return Status;
        }

        public string ToXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Project");
            element.SetElementValue(nameof(VehicleType), VehicleType);
            element.SetElementValue(nameof(ProjectName), ProjectName);
            element.SetElementValue(nameof(ProjectPath), ProjectPath);
            element.SetElementValue(nameof(Developer), Developer);
            element.SetElementValue(nameof(LastOpenTime), LastOpenTime);
            element.SetElementValue(nameof(LastInternalError), LastInternalError);

            return element.ToString();
        }

        public string GetUserSpaceDir()
        {
            return ProjectPath + @"\" + ProjectName;
        }

        public string GetRuntimeConfigDir()
        {
            return GetUserSpaceDir() + @"\" + RuntimeConfigDirName;
        }

        public string GetConfigXml()
        {
            return GetRuntimeConfigDir() + @"\" + RuntimeConfigXmlName;
        }


        // 菜单 --> 文件 --> 打开 --> 图片
        public bool NewOpenImage(string filepath, out string error)
        {
            error = "";
            //若已在项目中， 返回错误
            if (filesEditer.IsFileInProjectDir(filepath))
            {
                error = Errors.ProjectFileAlreadyExist;
                    return false;
            }
            filesEditer.OpenImage(filepath);
            return true;
        }

        //初始化项目目录
        public bool StartCreateProject(bool overwrite, out string error, out string errordetail)
        {
            error = "";
            errordetail = "";

            try
            {
                if (!Directory.Exists(ProjectPath))
                {
                    Directory.CreateDirectory(ProjectPath);
                }

                if (!Directory.Exists(GetRuntimeConfigDir()))
                {
                    Directory.CreateDirectory(GetRuntimeConfigDir());
                }

                if (!overwrite && File.Exists(GetConfigXml()))
                {
                    error = Errors.ProjectAlreadyExist;
                    return false;
                }

                StreamWriter streamWriter = new StreamWriter(GetConfigXml(), false, Encoding.UTF8);
                streamWriter.Write(ToXml());
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

    public enum ProjectStatus : int
    {
        StartCreateProject,
        FinishCreateProject,
        StartOpenProject,
        FinishOpenProject
    }
}