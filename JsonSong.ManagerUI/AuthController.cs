using JsonSong.ManagerUI.Filters;


namespace JsonSong.ManagerUI
{
    [CustomAuthorize(false,Order=2)]
    public class AuthController : JsonNetController
    {

    }
}
