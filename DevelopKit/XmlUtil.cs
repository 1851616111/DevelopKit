using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DevelopKit
{
    class XmlUtil
    {

    }
}

namespace example
{
    public static class Example
    {
        public static void Show()
        {
            Page[] pages = new Page[100];
            for (int p = 0; p <= 99; p++)
            {
                pages[p] = new Page(p);
            }

            Book book = new Book();
            book.Id = 1;
            book.Name = "肖申克的救赎";
            book.Writer = new Writer(1, "michael pan", "China", 40);
            book.Pages = pages;


            using (FileStream fs = File.OpenWrite(@"D:\tmp.text"))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                XmlSerializer xmlSerializer = new XmlSerializer(book.GetType());
                xmlSerializer.Serialize(sw, book);
            }

            Book newbook;
            using (FileStream fs = File.OpenRead(@"D:\tmp.text"))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                XmlSerializer xmlSerializer = new XmlSerializer(book.GetType());
                Object bookObj = xmlSerializer.Deserialize(sr);
                newbook = (Book)bookObj;
            }

            using (FileStream fs = File.OpenWrite(@"D:\tmp2.text"))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                XmlSerializer xmlSerializer = new XmlSerializer(newbook.GetType());
                xmlSerializer.Serialize(sw, newbook);
            }
        }
    }

    [Serializable]
    [XmlRoot("book")]
    public class Book
    {
        [XmlElement(ElementName = "id")]
        public int Id;
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "writer")]
       public Writer Writer;
        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public Page[] Pages;
        public Book()
        {
            Id = 1;
        }
    }

    [Serializable]
    public class Writer
    {
        [XmlElement(ElementName = "id")]
        public int Id;
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "country")]
        public string Country;
        [XmlElement(ElementName = "age")]
        public int Age;
        public Writer()
        { }

        public Writer(int id, string name, string country, int age)
        {
            Id = id;
            Name = name;
            Country = country;
            Age = age;
        }
    }

    public class Page
    {
        [XmlElement(ElementName = "number")]
        public int Number;

        public Page()
        { }
        public Page(int Number)
        {
            this.Number = Number;
        }
    }
}
