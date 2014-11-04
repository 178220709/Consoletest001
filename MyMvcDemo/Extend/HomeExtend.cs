using System.Reflection;
using Fasterflect;
using MyMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;

namespace MyMvcDemo.Extend
{
    public  static  class HomeHelper
    {
        public static IEnumerable<ModuleDTO> GetIndexModules( )
        {
            IList<ModuleDTO> modules = new List<ModuleDTO>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            var typeArr = assembly.GetTypes().Where(a => a.FullName.StartsWith("MyMvcDemo.Controllers")).ToList()
                .Where(a => typeof(Controller).IsAssignableFrom(a)).ToList();
            //循环所有的controller 取出用特性标记的action
            foreach (Type t in typeArr)
            {
                if (t.HasAttribute<ModuleAttribute>())
                {
                    t.GetControllerAction(modules);
                }
            }
            return modules.OrderBy(a=>a.Sort).ToList();
        }

        public static void GetControllerAction(this Type type, IList<ModuleDTO> modules)
        {
            if (!typeof(Controller).IsAssignableFrom(type))
            {
                return ;
            }
            var controllerName = type.Name.Replace("Controller","");
            var parentAttr = type.GetAttribute<ModuleAttribute>();
            var actions = type.GetMethods().Where(a => a.HasAttribute<ModuleAttribute>()).ToList();
            var actions2 = type.GetMethods().Where(a => a.GetType().HasAttribute<ModuleAttribute>()).ToList();
            if (!actions .Any())
            {
                return;
            }
            var parentModule = new ModuleDTO()
            {
                Name = parentAttr.Name??controllerName,
                CSS = parentAttr.CSS,
                Sort = parentAttr.Sort,
                Children = actions.Select(a =>
                {
                    var child = new ModuleDTO();
                    var attr = a.GetAttribute<ModuleAttribute>();
                    attr.Name = attr.Name ?? a.Name;
                    child.InjectFrom(attr);
                    child.VName = controllerName + a.Name;
                    child.Url = string.Format("/{0}/{1}", controllerName, a.Name);
                    return child;
                }).ToList()
            };
            modules.Add(parentModule);
        }

        public static bool HasMyAttribute<T>(this MemberInfo type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
               return true;
            }
            return false;
        }
        public static T GetAttribute<T>(this MemberInfo type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
                return attributes.First() as T;
            }
            return null;
        }

    }
}
