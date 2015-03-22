using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using MongoDB.Driver.Linq;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.MyHtmlAgility.Project.SpiderBase;
using MyProject.MyHtmlAgility.Project.Youmin;
using MyProject.TestHelper;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Suijing.Utils;

namespace  MyProject.MyHtmlAgility.ProjectTest
{
    /// <summary>
    /// 将lteAdmin原html文件 去掉头尾 只取内容 转换成cshtml
    /// </summary>
    [TestClass]
    public class FixHahaJokeFlag
    {

        [TestMethod]
        public void FixFlag()
        {
            var manager = SpiderServiceFactory.GetByTypeId(2);

            manager.Entities.ToList().ForEach(a =>
            {
                if ( string.IsNullOrEmpty(a.Flag) || a.Flag=="0")
                {
                    a.Flag =   YouminWebReader.GetFlagFromUrl(a.Url) ;
                    manager.AddEdit(a);
                }
            });
        } 
        
        [TestMethod]
        public void FixWeight()
        {
            var manager = HahaJokeService.Instance;
            var reader = new HahaWebReader();
            manager.Entities.ToList().ForEach(a =>
            {
                if (a.Weight==0)
                {
                    var newEn = reader.GetHtmlContent(a.Url);
                    a.Weight = newEn.Weight;
                    manager.AddEdit(a);
                }
              
            });
        }
    }
}
