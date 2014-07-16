using MyMvcDemo.Filters;


namespace MyMvcDemo
{
    [CustomAuthorize(false,Order=2)]
    public class AuthController : JsonNetController
    {

    }
}
