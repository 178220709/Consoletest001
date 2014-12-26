using System.Web.Mvc;
using System.Web.SessionState;

namespace MyMvcDemo
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ControllerBase : AuthController
    {

    }
}
