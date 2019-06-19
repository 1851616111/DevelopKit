using System;
using System.Collections;
using System.Collections.Generic;
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

        public SortedList sl; //车型_场景       vs 组件
        public SortedList s2; //车型_场景_组建  vs 属姓  


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


        public string GetConfigDir()
        {
            return ProjectPath + @"\" + ProjectName  + @"\" + RuntimeConfigDirName;
        }
        public string GetConfigXml()
        {
            return GetConfigDir() + @"\" + RuntimeConfigXmlName;
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