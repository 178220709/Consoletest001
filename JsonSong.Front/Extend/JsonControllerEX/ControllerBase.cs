using System.Web.Mvc;
using System.Web.SessionState;

namespace JsonSong.Front
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ControllerBase : AuthController
    {

    }
}
