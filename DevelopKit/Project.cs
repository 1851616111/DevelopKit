﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
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

        public ProjectFileEditer filesEditer; // 文件编辑器

        public SortedList sl; //车型_场景       vs 组件
        public SortedList s2; //车型_场景_组建  vs 属姓  
        
        public Project(string vtype, string name, string path, string dev)
        {
            VehicleType = vtype;
            ProjectName = name;
            ProjectPath = path;
            Developer = dev;
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

        public XElement ToXElement()
        {
            XElement element = new XElement("project");
            element.SetElementValue("vehicle_type", VehicleType);
            element.SetElementValue("name", ProjectName);
            element.SetElementValue("path", ProjectPath);
            element.SetElementValue("developer", Developer);
            element.Add(filesEditer.ToXElement());

            return element;
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

                if (!overwrite && Directory.Exists(GetUserSpaceDir()))
                {
                    error = Errors.ProjectAlreadyExist;
                    return false;
                }

                if (!Directory.Exists(GetRuntimeConfigDir()))
                {
                    Directory.CreateDirectory(GetRuntimeConfigDir());
                }

                XElement element = ToXElement();

                FileUtil.WriteStringToFile(GetConfigXml(), element.ToString());
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