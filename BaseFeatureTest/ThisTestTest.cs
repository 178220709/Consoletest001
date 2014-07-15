using System.Collections;
using System.Diagnostics;
using BaseFeatureDemo.Reflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Newtonsoft.Json;

namespace TestProject1
{
    
    
    /// <summary>
    ///这是 ThisTestTest 的测试类，旨在
    ///包含所有 ThisTestTest 单元测试
    ///</summary>
    [TestClass()]
    public class ThisTestTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///maintTest 的测试
        ///</summary>
        [TestMethod()]
        public void maintTestTest()
        {
            ThisTest.maintTest();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///maintTest 的测试
        ///</summary>
        [TestMethod()]
        public void maintTestTest2()
        {
            var str = "sfasdfasdf".GetHashCode();
            DictionaryEntry item1 = new DictionaryEntry(1, "中国");
            var str2 = item1.GetHashCode();
            var str22 = ((object)item1).GetHashCode();
            Hashtable ht = new Hashtable();
            ht.Add(1, "中国");
            ht.Add(2, "法国");
            ht.Add(3, "德国");
            ht.Add(4, "意大利");
            ht.Add(5, "西班牙");
            
            foreach (DictionaryEntry item in ht)
            {
                Trace.WriteLine(item.GetHashCode());
                Trace.WriteLine(string.Format("Key:{0},Value:{1}", item.Key.ToString(), item.Value.ToString()));
            }

            for (int i = 0; i < 5; i++)
            {
                Trace.WriteLine( i + " hash is : "+   i.GetHashCode());
            }

            Assert.Inconclusive("无法验证不返回值的方法。");

        }


    }
}
