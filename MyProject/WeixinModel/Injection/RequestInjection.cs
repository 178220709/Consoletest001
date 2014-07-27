using System;
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
                var value = source[name];
                if (value == null) continue;
                try
                {
                    targetPro.SetValue(target, value);
                }
                catch (Exception)
                {
                    try
                    {
                        var result = targetPro.Converter.ConvertFromString(value);
                        targetPro.SetValue(target, result);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}