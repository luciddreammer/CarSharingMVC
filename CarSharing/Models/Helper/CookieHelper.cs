using Microsoft.AspNetCore.Mvc;

namespace CarSharing.Models.Helper
{
    public class CookieHelper : Controller
    {
        public CookieHelper()
        {

        }

        public string GetCookie()
        {
            var cookie = HttpContext.Request.Cookies["Session_Id"];
            return cookie;
        }
    }
}
