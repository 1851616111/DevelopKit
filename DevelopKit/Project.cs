using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DevelopKit
{
    [Serializable]
    [XmlRoot("project")]
    public class Project
    {
        public static string RuntimeConfigDirName = ".kit";
        public static string RuntimeConfigXmlName = "project.xml";
        public static string UserProjectResourcesDir = "Resources";
        public static string UserProjectUploadDir = "upload";

        [XmlElement(ElementName = "car_info")]
        public CarInfo CarInfo;  //车型信息

        [XmlElement(ElementName = "car_config")]
        public CarConfig CarConfig;  //车型皮肤配置

        [XmlElement(ElementName = "project_name")]
        public string ProjectName;  //项目名称

        [XmlElement(ElementName = "local_path")]
        public string LocalPath;  //项目路径

        [XmlElement(ElementName = "developer")]
        public string Developer;    //开发者

        [XmlElement(ElementName = "editer")]
        public PropertyiesEditer<int, Property> Editer;

        [XmlIgnore]
        public ProjectStatus Status; //项目状态

        public Project()
        {
        }

        public Project(CarInfo car, CarConfig ccfg, string name, string path, string dev)
        {
            CarInfo = car;
            CarConfig = ccfg;
            ProjectName = name;
            LocalPath = path;
            Developer = dev;
            Status = ProjectStatus.StartCreateProject;
            Editer = new PropertyiesEditer<int, Property>();
        }

        public void RestoreEditedProperties(List<Property> properties)
        {
            if (Editer == null || Editer.Count == 0)
            {
                return;
            }

            foreach (Property property in properties)
            {
                if (Editer.ContainsKey(property.Id))
                {
                    property.Value = Editer[property.Id].Value;
                    property.DefaultValue = Editer[property.Id].DefaultValue;
                }
            }
            Editer.Clear();
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

        public string SerializeToStrInMemory()
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(Project));
            //序列化对象
            xml.Serialize(Stream, this);
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        public void WriteXmlFile()
        {
            Error error;
            lock (this)
            {
                error = FileUtil.SerializeObjectToFile(this, GetConfigXml());
            }

            if (error != null)
            {
                Log.Error("Project core", "Save to xml file " + GetConfigXml(), error.Message);
            }
        }

        public byte[] ReadXmlFileBytes()
        {
            byte[] fileData;
            bool ok;
            lock (this)
            {
                ok = FileUtil.ReadBytes(GetConfigXml(), out fileData);
            }

            if (!ok)
            {
                Log.Error("Project模块", "LoadXmlFileBytes ", "读取配置文件数据失败");
                return null;
            }
            return fileData;
        }

        public string GetUserSpaceDir()
        {
            return LocalPath + @"\" + ProjectName;
        }

        public string GetRuntimeConfigDir()
        {
            return GetUserSpaceDir() + @"\" + RuntimeConfigDirName;
        }

        public string GetConfigXml()
        {
            return GetRuntimeConfigDir() + @"\" + RuntimeConfigXmlName;
        }

        public string GetAppResourcesDir()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CarInfo.ResourcesDir);
        }

        public string GetUserResourcesDir()
        {
            return Path.Combine(GetUserSpaceDir(), UserProjectResourcesDir);
        }

        public string GetUserUploadImageDir()
        {
            string dir = Path.Combine(GetUserResourcesDir(), UserProjectUploadDir);
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public string GetPropertyImagePath(string image)
        {
            return Path.Combine(UserProjectUploadDir, image);
        }

        //初始化项目目录
        public bool StartCreateProject(bool overwrite, out string error, out string errordetail)
        {
            error = "";
            errordetail = "";

            try
            {
                if (!Directory.Exists(LocalPath))
                {
                    Directory.CreateDirectory(LocalPath);
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

                WriteXmlFile();

                Error err = FileUtil.CopyDir(GetAppResourcesDir(), GetUserSpaceDir(), UserProjectResourcesDir);
                if (err != null)
                {
                    error = "初始化项目资源文件失败";
                    errordetail = err.ToString();
                    return false;
                }
               
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