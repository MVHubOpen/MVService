using System.Web;

namespace MVService
{
    public class Handler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var sess = new Session();
            
            context.Response.Write("Hello from custom handler.");
        }

        public static string Version => "MVService_Handler_1A";


        public bool IsReusable => true;
    }
}
