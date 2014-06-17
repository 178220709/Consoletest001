using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BaseFeatureDemo.Linq
{
    public class LinqXMLDemo
    {
        public static void Select1()
        {

        }
        public static void CreateDocument()
        {
            string path = @"d:\website";
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                           new XElement("Root", "root"));
            xdoc.Save(path);
        }

        public static void CreateCategories()
        {
            const string path = @"e:\website.xml";
            XElement root = new XElement("Categories",
                new XElement("Category",
                    new XElement("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Beverages")
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
        public static XElement CreateCategoriesByXAttribute()
        {
            const string path = @"e:\website2.xml";
            XElement root = new XElement("Categories",
                new XElement("Category",
                    new XAttribute("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Beverages")
                    ),
                new XElement("Category",
                    new XAttribute("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Condiments")
                    ),
                new XElement("Category",
                    new XAttribute("CategoryID", Guid.NewGuid()),
                    new XElement("CategoryName", "Confections")
                    )
               );
            root.Save(path);
            return root;
        }

        public static void mainTest()
        {
            CreateCategories();
            CreateCategoriesByXAttribute();
        }
    }
}
