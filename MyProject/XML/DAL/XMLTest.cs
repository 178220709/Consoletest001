using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using BaseFeatureDemo.XML.Utility;

namespace BaseFeatureDemo.XML.DAL
{
    public class XMLTest
    {
        public const string path = "e:\\test.xml";

        public static void main1()
        {
            CreateCategories();
        }

        public static void CreateDocument()
        {
            XDocument xdoc = new XDocument
                (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Root", "root")
                );
            xdoc.Save(path);
        }

        public static void CreateCategories()
        {
            XElement root = new XElement("Categories",
                new XElement("Category",
                    new XElement("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName",
                        new XElement("test", "gahahah"))
                    ),
                new XElement("Category",
                    new XElement("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Condiments")
                    ),
                new XElement("Category",
                    new XElement("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Confections")
                    )
                );

            root.Save(path);
        }

       
        
    }
}