using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using SecurityTest.Web.Models;

namespace SecurityTest.Web.Controllers
{
    public class HomeController : Controller
    {
        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    Response.Cookies.Append("myCookie", "myCoookieValue");
        //    Response.Cookies.Append("myCookie2", "myCoookieValue2",
        //        new CookieOptions()
        //        {
        //            Expires = DateTime.Now.AddMinutes(15),
        //            HttpOnly = false,
        //            Secure = false
        //        });
        //}

        //[Produces(contentType: "application/json")]
        public IActionResult Index()
        {
            this.HttpContext.Session.Clear();
            this.HttpContext.Response.Cookies.Delete("c1");
            this.HttpContext.Response.Cookies.Delete("cookie2");
            return RedirectToAction("Home", "Home");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Home()
        {
            CookieOptions option1 = new CookieOptions();
            option1.Expires = DateTime.Now.AddMinutes(10);
            option1.HttpOnly = option1.Secure = false;
            Response.Cookies.Append("c1", "c1value", option1);


            //CookieHeaderValue myCookie = new CookieHeaderValue("MyCookie1", "MyCookieValue1");
            //myCookie.Path = "/";
            //myCookie.Expires = DateTime.Now.AddMinutes(10);
            //Response.Cookies.Append(myCookie);
            //Response.Headers["Set-Cookie"] = "name=value";

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(10);
            option.HttpOnly = option.Secure = true;
            //option.Domain = "localhost";
            option.Expires = DateTime.UtcNow.AddMinutes(20);
            option.MaxAge = DateTime.UtcNow.AddDays(1).TimeOfDay;
            //option.Path = "/home";
            option.SameSite = SameSiteMode.Strict;
            Response.Cookies.Append("cookie2", "c2value", option);

            return View("index");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
