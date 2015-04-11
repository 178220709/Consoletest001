using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace MyProject.RedisDemo
{
    //http://www.cnblogs.com/davidgu/p/3263485.html
    [TestClass]
    public class Demo002:DemoBase
    {
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public string Profession { get; set; }
        }
        public class Phone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public string Manufacturer { get; set; }
            public Person Owner { get; set; }
        }

        [TestMethod]
        public void EntityDemo()
        {
            using (var redisClient = new RedisClient(_Host))
            {
                IRedisTypedClient<Phone> phones = redisClient.As<Phone>();
                Phone phoneFive = phones.GetValue("5");

                if (phoneFive == null)
                {
                    // make a small delay
                    Thread.Sleep(2000);
                    // creating a new Phone entry
                    phoneFive = new Phone
                    {
                        Id = 5,
                        Manufacturer = "Nokia",
                        Model = "Lumia 200",
                        Owner = new Person
                        {
                            Id = 1,
                            Age = 90,
                            Name = "OldOne",
                            Profession = "sportsmen",
                            Surname = "OldManSurname"
                        }
                    };

                    // adding Entry to the typed entity set
                    phones.SetEntry(phoneFive.Id.ToString(), phoneFive);
                }

                string message = "Phone model is " + phoneFive.Manufacturer;
                message += "\nPhone Owner Name is: " + phoneFive.Owner.Name;

                Console.WriteLine(message);
            }
        }
    }
}