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
using System.Windows.Forms;

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
        [XmlElement("id")]
        public int Id;

        [XmlElement("name")]
        public string Name;

        [XmlElement("version")]
        public string Version;

        [XmlElement("config_file")]
        public string ConfigFile;

        [XmlElement("manufacturer_id")]
        public int ManufacturerId;

        private string resourcesDir;

        [XmlElement("resources_dir")]
        public string ResourcesDir
        {
            get
            {
                return resourcesDir;
            }
            set
            {
                if (value[value.Length - 1] == '\\' || value[value.Length - 1] == '/')
                    resourcesDir = value.Remove(value.Length - 1, 1);
                else
                    resourcesDir = value;
            }
        }

        public string GetResourcesDir()
        {
            if (ResourcesDir[ResourcesDir.Length - 1] == '\\' || ResourcesDir[ResourcesDir.Length - 1] == '/')
                ResourcesDir = ResourcesDir.Remove(ResourcesDir.Length - 1, 1);
            return ResourcesDir;
        }

        public CarConfig GetCarConfig()
        {
            CarConfig carCfg = (CarConfig)FileUtil.DeserializeObjectFromFile(typeof(CarConfig), ConfigFile);
            carCfg.MakeMappingCache();
            return carCfg;
        }

        public bool Validate()
        {
            return File.Exists(ConfigFile);
        }
    }

    [Serializable]
    [XmlRoot("car_config")]
    public class CarConfig
    {
        private List<Scene> scenes;
        private Dictionary<int, Scene> sceneMapping;

        private List<Property> properties;
        private Dictionary<int, Group> groupMapping;

        private Dictionary<int, List<Group>> sceneIdToGroupsMapping;

        //sceneid: group_layer_idx: group
        private SortedDictionary<int, SortedDictionary<int, List<Group>>> groupLayerIndexMapping;

        private Dictionary<int, Property> propertyMapping;
        private Dictionary<int, List<Property>> groupIdToPropertyMapping;

        [XmlArray("scenes"), XmlArrayItem("item")]
        public List<Scene> Scenes { get => scenes; set { scenes = value; } }
        [XmlArray("properties"), XmlArrayItem("item")]
        public List<Property> Properties { get => properties; set { properties = value; } }

        [XmlElement("outputs")]
        public Outputs outputs;

        [XmlIgnore]
        public Dictionary<int, Property> PropertyIdMapping { get => propertyMapping; }
        [XmlIgnore]
        public Dictionary<int, Group> GroupMapping { get => groupMapping; }
        [XmlIgnore]
        public Dictionary<int, List<Property>> GroupIdToPropertyMapping { get => groupIdToPropertyMapping; }
        [XmlIgnore]
        public Dictionary<int, List<Group>> SceneIdToGroupsMapping { get => sceneIdToGroupsMapping; }

        public int GetTotalSceneNum()
        {
            int total = 0;
            foreach (Scene scene in Scenes)
            {
                if (scene.children.Count > 0)
                    total += scene.children.Count;

                total += 1;

            }
            return total;
        }

        public Scene GetSceneById(int id)
        {
            return sceneMapping.ContainsKey(id) ? sceneMapping[id] : null;
        }

        //sid scene id
        //glid group layer id
        public List<Group> ListGroupByLayerId(int sid, int glid)
        {
            return groupLayerIndexMapping[sid][glid];
        }

        //在同一个Group中，获取与Property的LayerId相同的Property
        //相同LayerId的Property目前会出现两种情况
        //1. Property B的RefPropertyId = Property A.ID
        //2. Property A Property B无引用关系(不能同时出现)
        public SortedDictionary<int, Property> GetGroupSameLayerProperties(int gid, Property property)
        {
            List<Property> groupProperties = GroupIdToPropertyMapping[gid];
            if (groupProperties == null || groupProperties.Count == 0)
                return null;

            SortedDictionary<int, Property> res = new SortedDictionary<int, Property>();
            foreach (Property propertyItem in groupProperties)
            {
                if (propertyItem.PropertyLayerIdx != property.PropertyLayerIdx ||
                    propertyItem.RefPropertyId == property.Id ||
                    property.RefPropertyId > 0)
                    continue;
                res.Add(propertyItem.Id, propertyItem);
            }
            return res;
        }

        public void MakeMappingCache()
        {
            if (sceneMapping == null)
            {
                sceneMapping = new Dictionary<int, Scene>();
            }
            else
            {
                return;
            }
            if (groupMapping == null)
            {
                groupMapping = new Dictionary<int, Group>();
            }
            if (propertyMapping == null)
            {
                propertyMapping = new Dictionary<int, Property>();
            }
            if (groupLayerIndexMapping == null)
            {
                groupLayerIndexMapping = new SortedDictionary<int, SortedDictionary<int, List<Group>>>();
            }
            if (groupIdToPropertyMapping == null)
            {
                groupIdToPropertyMapping = new Dictionary<int, List<Property>>();
            }
            if (sceneIdToGroupsMapping == null)
            {
                sceneIdToGroupsMapping = new Dictionary<int, List<Group>>();
            }


            foreach (Scene scene in scenes)
            {
                sceneMapping[scene.Id] = scene;
                if (scene.children.Count > 0)
                {
                    foreach (Scene childScene in scene.children)
                    {
                        sceneMapping[childScene.Id] = childScene;
                    }
                }
            }

            foreach (Property property in properties)
            {
                propertyMapping[property.Id] = property;

                if (!sceneIdToGroupsMapping.ContainsKey(property.SceneId))
                {
                    sceneIdToGroupsMapping[property.SceneId] = new List<Group>();
                }
                if (!CollectionUtil.Contains(sceneIdToGroupsMapping[property.SceneId], property.GroupId))
                {
                    sceneIdToGroupsMapping[property.SceneId].Add(new Group
                    {
                        Id = property.GroupId,
                        Name = property.GroupName,
                        LayerIndex = property.GroupLayerIdx,
                        Sceneid = property.SceneId,
                        Size = property.GetGroupSize(),
                    });
                }

                if (!groupMapping.ContainsKey(property.GroupId))
                {
                    groupMapping.Add(property.GroupId, new Group
                    {
                        Id = property.GroupId,
                        Name = property.GroupName,
                        LayerIndex = property.GroupLayerIdx,
                        Sceneid = property.SceneId,
                        Size = property.GetGroupSize(),
                    });
                }

                if (!groupIdToPropertyMapping.ContainsKey(property.GroupId))
                {
                    groupIdToPropertyMapping.Add(property.GroupId, new List<Property>());
                }
                groupIdToPropertyMapping[property.GroupId].Add(property);


                if (!groupLayerIndexMapping.ContainsKey(property.SceneId))
                {
                    groupLayerIndexMapping[property.SceneId] = new SortedDictionary<int, List<Group>>();
                }
                if (!groupLayerIndexMapping[property.SceneId].ContainsKey(property.GroupLayerIdx))
                {
                    groupLayerIndexMapping[property.SceneId][property.GroupLayerIdx] = new List<Group>();
                }
                if (!CollectionUtil.Contains(groupLayerIndexMapping[property.SceneId][property.GroupLayerIdx], property.GroupId))
                {
                    groupLayerIndexMapping[property.SceneId][property.GroupLayerIdx].Add(new Group
                    {
                        Id = property.GroupId,
                        Name = property.GroupName,
                        LayerIndex = property.GroupLayerIdx,
                        Sceneid = property.SceneId,
                        Size = property.GetGroupSize(),
                    });
                }
            }
        }
    }

    [Serializable]
    public class Scene
    {
        [XmlElement("id")]
        public int Id;
        [XmlElement("name")]
        public string Name;

        [XmlArray("child_scenes"), XmlArrayItem("item")]
        public List<Scene> children;

        public List<Group> GetGroups()
        {
            if (GlobalConfig.Project.CarConfig.SceneIdToGroupsMapping.ContainsKey(Id))
            {
                return GlobalConfig.Project.CarConfig.SceneIdToGroupsMapping[Id];
            } else
            {
                return null;
            }
        }

        public Scene SearchScenes(int sid)
        {
            foreach (Scene scene in children)
            {
                if (scene.Id == sid)
                    return scene;
            }
            return null;
        }

        public Scene SearchTopScene()
        {
            foreach (Scene topScene in GlobalConfig.Project.CarConfig.Scenes)
            {
                if (topScene.Id == Id)
                    return topScene;

                if (topScene.SearchScenes(Id) != null)
                    return topScene;
            }

            return this;
        }

    }

    [Serializable]
    public class Group
    {
        private int id;
        private string name;
        private int layerIndex;
        private int sceneid;
        private GroupSize size;

        [XmlElement("id")]
        public int Id { get => id; set => id = value; }
        [XmlElement("name")]
        public string Name { get => name; set => name = value; }
        [XmlElement("layer_idx")]
        public int LayerIndex { get => layerIndex; set => layerIndex = value; }
        [XmlElement("scene_id")]
        public int Sceneid { get => sceneid; set => sceneid = value; }
        [XmlElement("size")]
        public GroupSize Size { get => size; set => size = value; }

        public List<Property> GetProperties()
        {
            return GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[id];
        }

        public SortedDictionary<int, Property> GetPropertiesByLayer()
        {
            SortedDictionary<int, Property> sortedDictionary = new SortedDictionary<int, Property>();

            foreach (Property property in GetProperties())
            {
                if (!sortedDictionary.ContainsKey(property.PropertyLayerIdx))
                    sortedDictionary.Add(property.PropertyLayerIdx, property);
            }

            return sortedDictionary;
        }

        public Scene SearchTopScene()
        {
            foreach (Scene topScene in GlobalConfig.Project.CarConfig.Scenes)
            {
                if (topScene.Id == sceneid)
                    return topScene;
                if (topScene.SearchScenes(sceneid) != null)
                    return topScene;
            }
            return null;
        }

        private Scene SearchScenes(List<Scene> scenes, int sid)
        {
            if (scenes == null)
                return null;

            foreach (Scene scene in scenes)
            {
                if (scene.Id == sid)
                    return scene;
            }
            return null;
        }

        public string GetTablePanelId()
        {
            return string.Format("tab_{0}", id);
        }

        public string GetCachedPictureBoxId()
        {
            return string.Format("pb_cache_{0}_{1}", sceneid, id);
        }
    }

    [Serializable]
    public class Property
    {
        [XmlElement("id")]
        public int Id;
        [XmlElement("scene_id")]
        public int SceneId;
        [XmlElement("group_id")]
        public int GroupId;
        [XmlElement("group_name")]
        public string GroupName;
        [XmlElement("group_size")]
        public string GroupSize;
        [XmlElement("position")]
        public string Position;
        [XmlElement("name")]
        public string Name;
        [XmlElement("type")]
        public string Type;
        [XmlElement("operate_type")]
        public string OperateType;
        [XmlElement("show_label")]
        public bool ShowLabel;
        [XmlElement("can_edit")]
        public bool CanEdit;
        [XmlElement("group_layer_idx")]
        public int GroupLayerIdx;
        [XmlElement("property_layer_idx")]
        public int PropertyLayerIdx;
        [XmlElement("value")]
        public string Value;
        [XmlElement("default_value")]
        public string DefaultValue;
        [XmlElement("ref_property_id")]
        public int RefPropertyId;
        [XmlElement("size")]
        public string Size;
        [XmlElement("location")]
        public string Location;
        [XmlElement("allow_value")]
        public string AllowValue;


        public Property Clone()
        {
            return new Property
            {
                Id = Id,
                SceneId = SceneId,
                GroupId = GroupId,
                GroupName = GroupName,
                GroupSize = GroupSize,
                Name = Name,
                Type = Type,
                OperateType = OperateType,
                ShowLabel = ShowLabel,
                CanEdit = CanEdit,
                GroupLayerIdx = GroupLayerIdx,
                PropertyLayerIdx = PropertyLayerIdx,
                Value = Value,
                RefPropertyId = RefPropertyId,
                Size = Size,
                Location = Location,
                AllowValue = AllowValue
            };
        }

        public Group GetGroup()
        {
            return new Group
            {
                Id = GroupId,
                Sceneid = SceneId,
                Name = GroupName,
                LayerIndex = GroupLayerIdx,
                Size = GetGroupSize(),
            };
        }

        public Location GetLocation()
        {
            string[] ss = Location.Split(',');
            if (ss.Length == 0 || ss.Length == 1)
            {
                return null;
            }
            else
            {
                return new Location
                {
                    X = Convert.ToInt32(ss[0]),
                    Y = Convert.ToInt32(ss[1]),
                };
            }
        }

        public GroupSize GetGroupSize()
        {
            string[] ss = GroupSize.Split('*');
            if (ss.Length == 0 || ss.Length == 1)
            {
                return null;
            }
            else
            {
                return new GroupSize
                {
                    Width = Convert.ToInt32(ss[0]),
                    Height = Convert.ToInt32(ss[1]),
                };
            }
        }

        public string GetPictureBoxId()
        {
            return string.Format("pb_{0}_{1}_{2}", SceneId, GroupId, Id);
        }

        public string GetGroupPictureBoxId()
        {
            return string.Format("pb_{0}_{1}", SceneId, GroupId);
        }

        public string GetCachedPictureBoxId()
        {
            return string.Format("pb_cache_{0}_{1}", SceneId, GroupId);
        }

        public string GetCheckBoxId()
        {
            return string.Format("cb_{0}_{1}_{2}", SceneId, GroupId, Id);
        }

        public string GetTextBoxAlphaID()
        {
            return string.Format("tb_alpha_{0}_{1}_{2}", SceneId, GroupId, Id);
        }

        public string GetTextBoxColorID()
        {
            return string.Format("tb_color_{0}_{1}_{2}", SceneId, GroupId, Id);
        }

        public bool GetRangeAllowValue(out int min, out int max)
        {
            min = 0;
            max = 0;
            if (AllowValue == null || AllowValue.Length == 0)
                return false;

            if (AllowValue.Contains("-"))
            {
                string[] l = AllowValue.Split('-');
                if (l.Length == 2)
                {
                    min = Convert.ToInt32(l[0]);
                    max = Convert.ToInt32(l[1]);
                    return true;
                }
            }
        
            return false;
        }
    }

    public static class DisplayType
    {
        public const string Image = "image";
        public const string Text = "text";
    }

    public class GroupSize
    {
        public int Width;
        public int Height;
    }

    public class Location
    {
        public int X;
        public int Y;
    }

    public static class PropertyType
    {
        public const string Nil = "/";
        public const string Image = "image";
        public const string TxtColor = "txt_color";
        public const string ImageAlpha = "alpha";
        public const string Int = "int";
        public const string Color = "color";
    }

    public static class PropertyOperateType
    {
        public const string Nil = "/";
        public const string ReplaceImage = "image_replace";
        public const string AlphaWhiteImageSetColor = "alpha_white_image_set_color";
        public const string AlphaWhiteImageSetAlpha = "alpha_white_image_set_alpha";
        public const string ImageFilterColor = "image_filter_color";

        public static bool IsThirdPartType(string tp)
        {
            return tp.StartsWith("ThirdPart");
        }
    }
}
