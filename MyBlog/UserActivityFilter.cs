using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Data;
using MyBlog.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;



namespace MyBlog
{
    public class UserActivityFilter : IActionFilter
    {
        private IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        public UserActivityFilter(ApplicationDbContext context, IWebHostEnvironment enviroment )
        {
            _context = context;
            _environment = enviroment;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var data = "";
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            var url = $"{controllerName}/{actionName}";


            //user url has query or not
            if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                data = context.HttpContext.Request.QueryString.Value;

            }
            else
            {
                //If action has multiple params we have to loop into ActionArguments
                var userData = context.ActionArguments.FirstOrDefault();
                var stringUserData = JsonConvert.SerializeObject(userData);

                data = stringUserData;

            }

            string wwwPath = this._environment.WebRootPath+"\\Logs";
           // string contentPath = this._environment.ContentRootPath;
            var userName = context.HttpContext.User.Identity.Name;
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();


            //WRITE TO FILE
            string[] logLines = { "Data :"+data, "URL :"+url, "UserName :"+userName, "IP Address :"+ipAddress, "Date :"+DateTime.Now.ToString(),"--------------------------------------------------------------------------" };
            File.AppendAllLines(Path.Combine(wwwPath, DateTime.Now.ToString("MMMM dd, yyyy") + ".txt"), logLines);
           


            //WRITE TO DATABASE
            StoreUserActivity(data, url, userName, ipAddress);

        }

        //METHOD FOR SAVING TO DATABASE
        public void StoreUserActivity(string data, string url, string userName, string ipAddress)
        {

            var userActivity = new UserActivity
            {
                Data = data,
                Url = url,
                UserName = userName,
                IpAddress = ipAddress
            };

            _context.UserActivities.Add(userActivity);
            _context.SaveChanges();

        }
    }

}
