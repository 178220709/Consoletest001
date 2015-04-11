using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Redis;

namespace MyProject.RedisDemo
{
    [TestClass]
    public class DemoBase
    {
        protected string _Host = "localhost";
    }
}