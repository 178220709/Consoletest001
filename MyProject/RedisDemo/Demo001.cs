using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Redis;

namespace MyProject.RedisDemo
{
    [TestClass]
    public class Demo001 : DemoBase
    {
        [TestMethod]
        public void SimpleDemo()
        {
          
            string elementKey = "testKeyRedis";

            using (RedisClient redisClient = new RedisClient(_Host))
            {
                if (redisClient.Get<string>(elementKey) == null)
                {
                    // adding delay to see the difference
                    Thread.Sleep(2000);
                    // save value in cache
                    redisClient.Set(elementKey, "default value");
                }

                //change the value
                redisClient.Set(elementKey, "fuck you value");

                // get value from the cache by key
                string message = "Item value is: " + redisClient.Get<string>(elementKey);

                Console.WriteLine(message);
            }
        }
    }
}