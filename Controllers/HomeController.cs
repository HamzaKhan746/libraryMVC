using Microsoft.AspNetCore.Mvc;
using library.Models;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace library.Controllers
{
    
    public class HomeController : Controller
    {
        public static string user;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Login");
        }
       
        public IActionResult Validation(IFormCollection frm, string usertype)
        {
            return RedirectToAction("ViewMemberdetails", "Member");

            if (usertype == "Librarian")
            {
                string choice = "lib";
                MemberModel model = new MemberModel();
                string username = frm["txtusername"];
                string password = frm["txtpassword"];
                int status = model.Loginvalidation(username, password, choice);
                if (status > 0)
                {
                    return RedirectToAction("ViewMemberdetails", "Member");
                }
                return View("Error");
            }
            else
            {
                string choice1 = "mem";
                MemberModel model = new MemberModel();
                string username1 = frm["txtusername"];
                string password1 = frm["txtpassword"];
                int status1 = model.Loginvalidation(username1, password1, choice1);
                SideMemberController obj = new SideMemberController();
                obj.id(Convert.ToInt32(username1));
                SideMemberModel obj2 = new SideMemberModel();
                obj2.Mid(Convert.ToInt32(username1));
                if (status1 > 0)
                {
                    user = "mem";
                    return RedirectToAction("ViewBooks", "Book");
                }
                return View("Error");
            }

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