using System.Reflection;
using MyMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using ServiceStack;

namespace MyMvcDemo.Extend
{
    public  static  class ControllerHelper
    {
        public static IEnumerable<ModuleDTO> GetIndexModules( )
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var typeArr = assembly.GetTypes().Where(a => a.FullName.StartsWith("MyMvcDemo.Controllers")).ToList()
                .Where(a => typeof(Controller).IsAssignableFrom(a)).ToList();
            //循环所有的controller 取出用特性标记的action
            IList<ModuleDTO> modules = typeArr.Where(t => t.HasAttribute<ModuleAttribute>())
                .Select(t => GetControllerActions(t))
                .Where(module => module != null).ToList();
            return modules.OrderBy(a=>a.Sort).ToList();
        }

        public static ModuleDTO GetControllerActions( Type type)
        {
           
            if (!typeof(Controller).IsAssignableFrom(type))
            {
                return null;
            }
            var controllerName = type.Name.Replace("Controller","");
            var parentAttr = type.GetAttribute<ModuleAttribute>();
            var actions = type.GetMethods().Where(a => a.HasMyAttribute<ModuleAttribute>()).ToList();
            if (!actions .Any())
            {
                return null;
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
                    child.VName = controllerName +"_"+ a.Name;
                    child.Url = string.Format("/{0}/{1}", controllerName, a.Name);
                    return child;
                }).ToList()
            };
            return parentModule;
        }

        public static ModuleDTO GetControllerActions(WebViewPage page)
        {
            var cntr = page.ViewContext.Controller;
            var module = GetControllerActions(cntr.GetType());
            return module;
        }

        private static bool HasMyAttribute<T>(this MemberInfo type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
               return true;
            }
            return false;
        }
        private static T GetAttribute<T>(this MemberInfo type) where T : Attribute
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
