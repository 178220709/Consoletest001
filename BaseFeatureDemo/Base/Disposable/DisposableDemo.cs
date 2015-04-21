using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.Disposable
{
    /// <summary>
    /// 不能在using里面返回 这里不能形成闭包
    /// </summary>
    public class DisposableDemo : IDisposable
    {
        public string Contain = "Contain";

        public void Dispose()
        {
            Contain = "nothing";
            TestHelper.Log("Dispose is called");
        }
        public void Call()
        {
            TestHelper.Log(Contain);
        }
        public static void Main1()
        {
            using (var dis  = new DisposableDemo())
            {
                dis.Call();
            }

            Func<DisposableDemo> getNew = () =>
            {
                using (var dis = new DisposableDemo())
                {
                    return dis;
                }
            };

            var dis2 = getNew();
            dis2.Call();

            Console.ReadLine();
        }

    }


}
