using System.ComponentModel;
using System.Web;
using Omu.ValueInjecter;

namespace MyProject.WeixinModel.Injection
{
    public class RequestInjection : KnownSourceValueInjection<HttpRequestBase>
    {
        protected override void Inject(HttpRequestBase source, object target)
        {
            var targetPros = target.GetProps();
            foreach (PropertyDescriptor targetPro in targetPros)
            {
                var name = targetPro.Name;
                var vaule = source[name];
                if (vaule == null) continue;
                targetPro.SetValue(target, vaule);
            }
        }
    }
}
