using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
namespace DevelopKit
{
    [Serializable]
    [XmlRoot("app")]
    public class App
    {
        [XmlArray("manufacturers"), XmlArrayItem("item")]
        public Manufacturer[] manufacturers;

        [XmlArray("car_info_list"), XmlArrayItem("item")]
        public CarInfo[] carInfoList;

        public CarInfo GetCarInfo(string mname, string car_name, string car_version)
        {
            CarInfo[] cars = ListCarsByManufacturer(mname);

            foreach (CarInfo car in cars)
            {
                if (car.Name == car_name && car.Version == car_version)
                {
                    return car;
                }
            }

            return null;
        }

        //根据汽车厂商ID查询厂商下的汽车
        public CarInfo[] ListCarsByManufacturer(string name)
        {
            foreach (Manufacturer manufacturer in manufacturers)
            {
                if (manufacturer.Name == name)
                {
                    ArrayList list = new ArrayList();
                    foreach (CarInfo car in carInfoList)
                    {
                        if (car.ManufacturerId != manufacturer.Id)
                        {
                            continue;
                        }
                        else
                        {
                            list.Add(car);
                        }
                    }
                    return (CarInfo[])list.ToArray(typeof(CarInfo));
                }
            }
            return null;
        }

        //如果不同厂商叫同一个汽车名称过滤时会出现多余的情况
        public CarInfo[] ListCarsByCarName(string name)
        {
            ArrayList list = new ArrayList();

            foreach (CarInfo car in carInfoList)
            {
                if (name == car.Name)
                {
                    list.Add(car);
                }
            }
            return (CarInfo[])list.ToArray(typeof(CarInfo));
        }

    }

    [Serializable]
    public class Manufacturer
    {
        private int id;
        private string name;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }

        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
    }

    [Serializable]
    public class CarInfo
    {
        private int id;
        private string name;
        private string version;
        private string configFile;
        private int manufacturerId;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }

        [XmlElement("name")]
        public string Name { get => name; set => name = value; }

        [XmlElement("version")]
        public string Version { get => version; set => version = value; }

        [XmlElement("config_file")]
        public string ConfigFile { get => configFile; set => configFile = value; }

        [XmlElement("manufacturer_id")]
        public int ManufacturerId { get => manufacturerId; set => manufacturerId = value; }

        public CarConfig GetCarConfig()
        {
            CarConfig carCfg = (CarConfig)FileUtil.DeserializeObjectFromFile(typeof(CarConfig), configFile);
            carCfg.MakeMappingCache();
            return carCfg;
        }

        public bool Validate()
        {
            return File.Exists(configFile);
        }
    }


    [Serializable]
    [XmlRoot("car_config")]
    public class CarConfig
    {
        private List<Scene> scenes;
        private List<Property> properties;

        private Dictionary<int, Scene> sceneMapping;
        private Dictionary<int, List<Property>> groupIdToPropertyMapping;

        [XmlArray("scenes"), XmlArrayItem("item")]
        public List<Scene> Scenes { get => scenes; set { scenes = value; } }
        [XmlArray("properties"), XmlArrayItem("item")]
        public List<Property> Properties { get => properties; set { properties = value; } }

        public Scene GetSceneById(int id)
        {
            return sceneMapping[id];
        }

        public List<Property> GetPropertiesByGroupId(int gid)
        {
            return groupIdToPropertyMapping[gid];
        }

        public void MakeMappingCache()
        {
            if (sceneMapping == null)
            {
                sceneMapping = new Dictionary<int, Scene>();
                foreach (Scene scene in scenes)
                {
                    sceneMapping[scene.Id] = scene;
                }
            }

            if (groupIdToPropertyMapping == null)
            {
                groupIdToPropertyMapping = new Dictionary<int, List<Property>>();
                foreach (Property property in properties)
                {
                    if (!groupIdToPropertyMapping.ContainsKey(property.GroupId))
                    {
                        groupIdToPropertyMapping.Add(property.GroupId, new List<Property>());
                    }
                    groupIdToPropertyMapping[property.GroupId].Add(property);
                }
            }
        }
    }

    [Serializable]
    public class Property
    {
        private int id;
        private int groupId;
        private string name;
        private string type;
        private bool inGroup;
        private bool canEdit;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }
        [XmlElement("group_id")]
        public int GroupId { get => groupId; set => groupId = value; }
        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
        [XmlElement("type")]
        public string Type { get => type; set => type = value; }
        [XmlElement("in_group")]
        public bool InGroup { get => inGroup; set => inGroup = value; }
        [XmlElement("can_edit")]
        public bool CanEdit { get => canEdit; set => canEdit = value; }
    }

    public static class PropertyType
    {
        public const string Nil = "/";
        public const string Image = "image";
        public const string TxtColor = "txt_color";
        public const string ImageAlpha = "alpha";
    }

    [Serializable]
    public class Scene
    {
        private int id;
        private string name;

        [XmlArray("groups"), XmlArrayItem("item")]
        public List<Group> groups;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }
        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
    }

    [Serializable]
    public class Group
    {
        private int id;
        private string name;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }
        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
    }
}