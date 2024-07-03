using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using library.Models;

namespace library.Controllers
{
    public class SideMemberController : Controller
    {
        public static int LMemberid;
        
        public IActionResult IssueBookRequest()
        {
            ViewBag.Issuedate = Convert.ToDateTime(DateTime.Now);
            DateTime today = Convert.ToDateTime(DateTime.Now);
            ViewBag.Returndate = today.AddDays(7);
            FunctionModel mod = new FunctionModel();
            DataTable dt = mod.GetMemberbyId(LMemberid);
            return View("IssueBookRequest",dt);
        }
        public IActionResult IssueBookrequestinsert(IFormCollection fm)
        {
            FunctionModel mod = new FunctionModel();
            int bookid = Convert.ToInt32(fm["txtbookid"]);
            string idate = fm["txtidate"];
            string rdate = fm["txtrdate"];
            string user = "mem";
            int a = mod.InsertIBookRecord(bookid, idate, rdate,user);
            if (a == 1)
            {
                int b = mod.Findissuebookid();
                ViewBag.dashboard = "mem";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Request Send";
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 2)
            {
                ViewBag.dashboard = "mem";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "Invalid Bookid,This book does'nt exist";
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 3)
            {
                ViewBag.dashboard = "mem";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "This book is not available all copies are already issued";
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 4)
            {
                ViewBag.dashboard = "mem";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "One student cannot issue more than two books at a time";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult MyIssuedBooks()
        {
            SideMemberModel mod = new SideMemberModel();
            DataTable dt = mod.GetMyIssuedBooks();
            return View("MyIssuedBooks",dt);
        }
        public IActionResult Mydetails()
        {
            FunctionModel mod = new FunctionModel();
            DataTable dt = mod.GetMemberbyId(LMemberid);
            return View("Mydetails",dt);
        }
        public IActionResult MReturnBook()
        {
            ViewBag.user = "mem";
            SideMemberModel mod = new SideMemberModel();
            DataTable dt = mod.GetIBooklist();
            return View("MReturnBook", dt);
        }
        [HttpPost]
        public IActionResult Searchreturnbook(IFormCollection frm)
        {
            ViewBag.flag = 1;
            FunctionModel mod = new FunctionModel();
            int ibookid = Convert.ToInt32(frm["txtibookid"]);
            DataTable dt = mod.GetMemberbyIssuebook(ibookid);
            DateTime rdate = Convert.ToDateTime(dt.Rows[0]["returndate"]);
            DateTime today = Convert.ToDateTime(DateTime.Now);
            int daysdelay;
            if (today <= rdate)
            {
                daysdelay = 0;
            }
            else
            {
                daysdelay = Convert.ToInt32((today - rdate).TotalDays);

            }
            ViewBag.Daysdelay = daysdelay;
            ViewBag.Fine = 50 * daysdelay;
            return View("MReturnBook", dt);
        }
        [HttpPost]
        public IActionResult ReturnBookinsert(IFormCollection fm)
        {
            FunctionModel mod = new FunctionModel();
            int mid = Convert.ToInt32(fm["txtmid"]);
            int bookid = Convert.ToInt32(fm["txtbookid"]);
            string idate = fm["txtidate"];
            DateTime rdate = Convert.ToDateTime(fm["txtrdate"]);
            DateTime today = Convert.ToDateTime(DateTime.Now);
            int daysdelay;
            if (today <= rdate)
            {
                daysdelay = 0;
            }
            else
            {
                daysdelay = Convert.ToInt32((today - rdate).TotalDays);

            }
            int a = mod.InsertRBookRecord(bookid, mid, daysdelay);
            if (a > 0)
            {
                ViewBag.dashboard = "mem";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book Returned";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult MyReturnedBooks()
        {
            SideMemberModel mod = new SideMemberModel();
            DataTable dt = mod.GetMyReturnedbooks();
            return View("MyReturnedBooks", dt);
        }
        
        public void id(int id)
        {
            LMemberid = id;
        }
    }
}
