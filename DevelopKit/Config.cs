using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

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
        private Dictionary<int, Group> groupMapping;

        //sceneid : group_index : group 由于同一优先级的group会存在多个， 这里只存默认显示的一个
        private SortedDictionary<int, SortedDictionary<int, Group>> defaultOrderedGroups;

        private Dictionary<int, Property> propertyIdMapping;

        private Dictionary<int, Scene> sceneMapping;
        private Dictionary<int, List<Property>> groupIdToPropertyMapping;

        [XmlArray("scenes"), XmlArrayItem("item")]
        public List<Scene> Scenes { get => scenes; set { scenes = value; } }
        [XmlArray("properties"), XmlArrayItem("item")]
        public List<Property> Properties { get => properties; set { properties = value; } }
        [XmlIgnore]
        public Dictionary<int, Property> PropertyIdMapping { get => propertyIdMapping; set => propertyIdMapping = value; }
        [XmlIgnore]
        SortedDictionary<int, SortedDictionary<int, Group>> DefaultOrderedGroups { get => defaultOrderedGroups; set => defaultOrderedGroups = value; }
        [XmlIgnore]
        public Dictionary<int, Group> GroupMapping  { get => groupMapping; set => groupMapping = value; }
        [XmlIgnore]
        public Dictionary<int, List<Property>> GroupIdToPropertyMapping { get => groupIdToPropertyMapping; set => groupIdToPropertyMapping = value; }

        public Scene GetSceneById(int id)
        {
            return sceneMapping[id];
        }

        public List<Property> GetPropertiesByGroupId(int gid)
        {
            return GroupIdToPropertyMapping[gid];
        }

        // 按照组的优先级列出组内所有

        public List<Group> ListGroupsByIDAndIndex(int sid, int gid, int groupIndex)
        {
            List<Group> groups = new List<Group>();
            if (groupIndex > 0)
            {
                for (int index = 0; index < groupIndex; index++)
                {
                    groups.Add(defaultOrderedGroups[sid][groupIndex]);
                }
                groups.Add(groupMapping[gid]);
            }
            else {
                groups.Add(groupMapping[gid]);
            }

            foreach (Group group in groups)
            {
                Console.WriteLine("--------group------>" + group.Id);
            }
            return groups;
        }

        public void MakeMappingCache()
        {
            if (propertyIdMapping == null)
            {
                propertyIdMapping = new Dictionary<int, Property>();
                foreach (Property property in properties)
                {
                    propertyIdMapping[property.Id] = property;
                }
            }

            if (sceneMapping == null)
            {
                sceneMapping = new Dictionary<int, Scene>();
                foreach (Scene scene in scenes)
                {
                    sceneMapping[scene.Id] = scene;
                }
            }

            if (GroupIdToPropertyMapping == null)
            {
                if (defaultOrderedGroups == null) {
                    defaultOrderedGroups = new SortedDictionary<int, SortedDictionary<int, Group>>();
                }
                if (groupMapping == null)
                {
                    groupMapping = new Dictionary<int, Group>();
                }

                GroupIdToPropertyMapping = new Dictionary<int, List<Property>>();
                foreach (Property property in properties)
                {
                    if (!groupMapping.ContainsKey(property.GroupId))
                    {
                        groupMapping.Add(property.GroupId, new Group {
                            Id = property.GroupId,
                            Name = property.GroupName
                        });
                    }

                    if (!defaultOrderedGroups.ContainsKey(property.SceneId))
                    {
                        defaultOrderedGroups.Add(property.SceneId, new SortedDictionary<int, Group>());
                    }

                    if (!defaultOrderedGroups[property.SceneId].ContainsKey(property.GroupLayerIdx))
                    {
                        Console.WriteLine("----->" + property.GroupLayerIdx);
                            
                        defaultOrderedGroups[property.SceneId].Add(property.GroupLayerIdx, new Group
                        {
                            Id = property.GroupId,
                            Name = property.GroupName
                        });
                    }

                    if (!GroupIdToPropertyMapping.ContainsKey(property.GroupId))
                    {
                        GroupIdToPropertyMapping.Add(property.GroupId, new List<Property>());
                    }
                    GroupIdToPropertyMapping[property.GroupId].Add(property);
                }
            }
        }
    }

    [Serializable]
    public class Property
    {
        private int id;
        private int sceneId;
        private string groupName;
        private int groupId;
        private string name;
        private string type;
        private bool inGroup;
        private bool canEdit;
        private int groupLayerIdx;
        private int propertyLayerIdx;
        private string value;
        private string location;
        //private Content content;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }
        [XmlElement("scene_id")]
        public int SceneId { get => sceneId; set => sceneId = value; }
        [XmlElement("group_id")]
        public int GroupId { get => groupId; set => groupId = value; }
        [XmlElement("group_name")]
        public string GroupName { get => groupName; set => groupName = value; }
        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
        [XmlElement("type")]
        public string Type { get => type; set => type = value; }
        [XmlElement("in_group")]
        public bool InGroup { get => inGroup; set => inGroup = value; }
        [XmlElement("can_edit")]
        public bool CanEdit { get => canEdit; set => canEdit = value; }
        [XmlElement("group_layer_idx")]
        public int GroupLayerIdx { get => groupLayerIdx; set => groupLayerIdx = value; }
        [XmlElement("property_layer_idx")]
        public int PropertyLayerIdx { get => propertyLayerIdx; set => propertyLayerIdx = value; }
        [XmlElement("value")]
        public string Value { get => value; set => this.value = value; }
        [XmlElement("location")]
        public string Location { get => location; set => location = value; }

        //[XmlElement("content")]
        //public Content Content { get => content; set => content = value; }

        public Location GetLocation()
        {
            string[] ss = location.Split(',');
            if (ss.Length == 0 || ss.Length == 1)
            {
                return null;
            }
            else {
                return new Location
                {
                    X = Convert.ToInt32(ss[0]),
                    Y = Convert.ToInt32(ss[1]),
                };
            }
        }

        public string GetPictureBoxId()
        {
            return string.Format("pb_{0}_{1}_{2}", sceneId, groupId, id);
        }
    }

    public class Location
    {
        public int X;
        public int Y;
    }



    //[Serializable]
    //[XmlRoot("content")]
    //public class Content
    //{
    //    private List<TextItem> texts;
    //    private List<ImageItem> images;

    //    [XmlArray("text"), XmlArrayItem("item")]
    //    public List<TextItem> Texts { get => texts; set => texts = value; }

    //    [XmlArray("image"), XmlArrayItem("item")]
    //    public List<ImageItem> Images { get => images; set => images = value; }
    //}

    //[Serializable]
    //public class TextItem
    //{
    //    private string font;
    //    private string location;
    //    private int size;
    //    private string text;

    //    [XmlElement("text")]
    //    public string Text { get => text; set => text = value; }
    //    [XmlElement("size")]
    //    public int Size { get => size; set => size = value; }
    //    [XmlElement("location")]
    //    public string Location { get => location; set => location = value; }
    //    [XmlElement("font")]
    //    public string Font { get => font; set => font = value; }
    //}

    //[Serializable]
    //public class ImageItem
    //{
    //    private string filepath;
    //    private string location;

    //    [XmlElement("filepath")]
    //    public string Filepath { get => filepath; set => filepath = value; }
    //    [XmlElement("location")]
    //    public string Location { get => location; set => location = value; }
    //}

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

        public string GetTablePanelId()
        {
            return string.Format("tab_{0}", id);
        }
    }
}