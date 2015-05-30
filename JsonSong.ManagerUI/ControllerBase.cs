using System.Web.Mvc;
using System.Web.SessionState;

namespace JsonSong.ManagerUI
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ControllerBase : AuthController
    {

    }
}
