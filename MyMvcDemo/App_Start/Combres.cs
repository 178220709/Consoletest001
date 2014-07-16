using System.Web.Routing;
using Combres;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MyMvcDemo.Combres), "PreStart")]
namespace MyMvcDemo {
    public static class Combres {
        public static void PreStart() {
          //  RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}