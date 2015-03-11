using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BaseFeatureDemo.Reflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Newtonsoft.Json;

namespace TestProject1
{


    /// <summary>
    ///这是 ThisTestTest 的测试类，旨在
    ///包含所有 ThisTestTest 单元测试
    ///</summary>
    [TestClass()]
    public class ThisTestTest2
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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

        public interface IFake
        {

            bool DoSomething(string actionname);
        }

        [TestMethod]
        public void TestNavigationSyncWithSelection()
        {
            //make a mock Object by Moq

            var mo = new Mock<IFake>();

            //Setup our mock object

            Func<IFake, bool> ttt = fake => true;

            mo.Setup(foo => foo.DoSomething("Ping"))

                .Returns(new Func<IFake, bool>(fake => true));

            IList<int> testa = new List<int>() {1, 2, 3, 4, 5};


            //Assert it!

            Assert.AreEqual(true, mo.Object.DoSomething("Ping"));
            Assert.AreEqual(false, mo.Object.DoSomething("Ping2"));
            Assert.AreEqual(true, mo.Object.DoSomething("Ping2"));
        }

        public interface IEmailSender
        {
            bool Send(string subject, string body, string email);
        }

        [TestMethod]

        public void User_Can_Send_Password()
        {

            var emailMock = new Mock<IEmailSender>();

            emailMock.Setup(sender => sender.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);



        }


        [TestMethod]

        public void Test_FindByName_GetCalled()
        {
            // create some mock data
            IList<Product> products = new List<Product>

            {
                new Product
                {
                    ProductId = 1,
                    Name = "C# Unleashed",
                    Description = "Short description here",
                    Price = 49.99
                },

                new Product
                {
                    ProductId = 2,
                    Name = "ASP.Net Unleashed",
                    Description = "Short description here",
                    Price = 59.99
                },

                new Product
                {
                    ProductId = 3,
                    Name = "Silverlight Unleashed",
                    Description = "Short description here",
                    Price = 29.99
                }

            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(sender => sender.FindById(It.IsAny<int>()))
                .Returns((int s) => products.Where(x => x.ProductId == s).Single());

            mock.Object.FindById(1);

            mock.Verify(x => x.FindById(1), Times.Once());
        }


        public interface IProductRepository
        {
            Product FindById(int i);
        }

        public class Product
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
        }
    }
}
