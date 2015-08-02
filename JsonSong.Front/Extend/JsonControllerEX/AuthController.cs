using JsonSong.Front.Filters;


namespace JsonSong.Front
{
    [CustomAuthorize(false,Order=2)]
    public class AuthController : JsonNetController
    {

    }
}
