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

        //public void SetType3()
        //{
        //    var p1 = this._p1;
        //    var p2 = this._p2;
        //     //......Deal p1 and p2
        //    this._p3 = xxx;
        //}

        //public static void SetType3(MyClass obj)  //静态函数,但修改了实例的成员 不是纯函数
        //{
        //    var p1 = obj._p1;
        //    var p2 = obj._p2;
        //    //......Deal p1 and p2
        //    obj._p3 = xxx;
        //}

        //public static void SetType3(Type1 p1, Type2 p2, MyClass obj)  //静态函数,但修改了实例的成员 不是纯函数
        //{
        //    //......Deal p1 and p2
        //    obj._p3 = xxx;
        //}


        //public static Type3 GetType3(Type1 p1, Type2 p2)
        //{
        //    //......Deal p1 and p2
        //    Type3 p3 = xxx;
        //    return p3;
        //}
    }
}
