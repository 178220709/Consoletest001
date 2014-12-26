using System.Xml.Linq;
using Omu.ValueInjecter;

namespace MyProject.WeixinModel.Injection
{
    public class XmlStrInjection : KnownSourceValueInjection<string>
    {
        protected override void Inject(string source, object target)
        {
            XElement xElement = XElement.Parse(source);
            var nodes = xElement.Nodes();
            foreach (XElement xNode in nodes)
            {
                var name = xNode.Name.LocalName;
                var value = xNode.Value;
                var targetPro = target.GetProps().GetByName(name);
                if (targetPro == null) continue;
                targetPro.SetValue(target, value);
            }
        }
    }
}