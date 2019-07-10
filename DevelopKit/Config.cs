using System;
using System.Collections;
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

        [XmlArray("cars"), XmlArrayItem("item")]
        public Car[] cars;

        public Car GetCar(string mname, string car_name, string car_version)
        {
            Car[] cars = ListCarsByManufacturer(mname);

            foreach (Car car in cars)
            {
                if (car.Name == car_name && car.Version == car_version)
                {
                    return car;
                }
            }

            return null;
        }

        //根据汽车厂商ID查询厂商下的汽车
        public Car[] ListCarsByManufacturer(string name)
        {
            foreach (Manufacturer manufacturer in manufacturers)
            {
                if (manufacturer.Name == name)
                {
                    ArrayList list = new ArrayList();
                    foreach (Car car in cars)
                     {
                        if (car.ManufacturerId != manufacturer.Id)
                        {
                            continue;
                        }
                        else {
                            list.Add(car);
                        }
                    }
                    return (Car[])list.ToArray(typeof(Car));
                }
            }
            return null;
        }

        //如果不同厂商叫同一个汽车名称过滤时会出现多余的情况
        public Car[] ListCarsByCarName(string name)
        {
            ArrayList list = new ArrayList();

            foreach (Car car in cars)
            {
                if (name == car.Name)
                {
                    list.Add(car);
                }
            }
            return (Car[])list.ToArray(typeof(Car));
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
    public class Car
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
    }
}
