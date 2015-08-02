using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LiteDbLog.LiteDBLog;

namespace LiteDbLog.Facade
{
    public static class DBLogInstances
    {
        public static DBLogDao Spider = new DBLogDao("Spider");

        public static DBLogDao Weixin = new DBLogDao("Weixin");

        public static DBLogDao System = new DBLogDao("System");


        private static readonly Dictionary<string, DBLogDao> DicInstances = new Dictionary<string, DBLogDao>()
        {
            {"Spider",Spider},
            {"Weixin",Weixin},
            {"System",System},
        };

        public static IList<string> GetAllInstanceName()
        {
            //return typeof(DBLogInstances).GetMembers()
            //    .Where(a => a.MemberType.ToString() == "Field")
            //    .Select(a => a.Name)
            //    .ToList();
            return DicInstances.Keys.ToList();
        }
        public static DBLogDao GetnstanceByName(string name)
        {
            if (DicInstances.ContainsKey(name))
            {
                return DicInstances[name];
            }
            return System;
        }
    }
}