using System.Collections.Generic;
using System.Web;

namespace MVService
{
    public class Handler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            var authorizationHeader = request.Headers["Authorization"];
            var accessMethod = request.HttpMethod.ToUpper();

            var paramsList = ParseSegments("Service", context);

            var session = new Session();

            var subroutineName = paramsList[0] ;

            var subroutine = new MvSubroutine(subroutineName, 2, new[] {"", paramsList[1]});

            session.Call(subroutine);
            if (subroutine.ReturnParameters == null)
            {
                context.Response.Write("No Return");
            } else {
                context.Response.Write(accessMethod + " " + subroutine.ReturnParameters[0]);
            }
        }

        public static string Version => "MVService_Handler_1A";


        public bool IsReusable => true;



        public static string[] ParseSegments(string handlerName, HttpContext context)
        {
            var paramList = new List<string>();

            var mode = 0;

            var segments = context.Request.Url.Segments;

            var cSeg = handlerName.ToLower();

            foreach (var seg in segments)
            {
                var oSeg = seg;
                if (oSeg.EndsWith("/"))
                {
                    oSeg = oSeg.Substring(0, oSeg.Length - 1);
                }

                var uSeg = oSeg.ToLower();
                //                if (uSeg.Length <= 1) continue;

                switch (mode)
                {
                    case 0:
                        if (uSeg == cSeg) mode = 1;
                        break;
                    case 1:
                        paramList.Add(oSeg);
                        break;
                }
            }

            return paramList.ToArray();
        }

    }
}
