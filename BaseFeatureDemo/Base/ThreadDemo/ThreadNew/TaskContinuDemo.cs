using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadNew
{
    [TestClass]
    public class TaskContinuDemo
    {
        [TestMethod]
        public  static void Main1()
        {
            Func<string> ReceiveXml = () =>
            {
                Console.WriteLine(" Thread{0} is start at {1}   ", Task.CurrentId, ConsoleTestHelper.GetCurrentTime());
                return "haha";
            };

            Func<bool> ResolveXml = () =>
            {
                Console.WriteLine(" Thread{0} is start at {1}   ", Task.CurrentId, ConsoleTestHelper.GetCurrentTime());
                return true;
            };
            Func<object, string> SendFeedBack = (str) =>
            {
                Console.WriteLine(" Thread{0} is start at {1}   ", Task.CurrentId, ConsoleTestHelper.GetCurrentTime());
                return str.ToString();
            };

            var task = new Task<string>(ReceiveXml);
           

            var SendFeedBackTask = Task.Factory.StartNew(ReceiveXml)
                             .ContinueWith<bool>(s => ResolveXml())
                             .ContinueWith<string>(r => SendFeedBack(r.Result));
            Console.WriteLine(SendFeedBackTask.Result);

        }


    }
}