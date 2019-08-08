using System;
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
    [XmlRoot("outputs")]
    public class Outputs
    {
        [XmlArray("image_outputs"), XmlArrayItem("item")]
        public ImageOutput[] ImageOutputs;
        [XmlArray("xml_outputs"), XmlArrayItem("item")]
        public XmlOutput[] XmlOutputs;
        [XmlArray("merge_image_outputs"), XmlArrayItem("item")]
        public MergeImageOutput[] MergeImageOutputs;
    }

    public class ImageOutput
    {
        [XmlElement("property_id")]
        public int PropertyId;
        [XmlElement("target")]
        public string Target;
    }

    public class XmlOutput
    {
        [XmlElement("property_id")]
        public int PropertyId;
        [XmlElement("target")]
        public string Target;
    }

    public class MergeImageOutput
    {

        [XmlElement("target")]
        public string Target;


        [XmlElement("property_ids")]
        public string PropertyIds;

        public Dictionary<int, Property> GetPropertyMap()
        {
            string[] idStrs = PropertyIds.Split(',');
            Dictionary<int, Property> map = new Dictionary<int, Property>();
            foreach (string idStr in idStrs)
            {
                map.Add(Convert.ToInt32(idStr), null);
            }
           return map;
        }
    }
}
