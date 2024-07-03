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
    public class FunctionController : Controller
    {
        [HttpGet]
        public IActionResult IssueBook()
        { 
            return View(); 
        }
        [HttpGet]
        public IActionResult ReturnBook()
        {
            ViewBag.user = "lib";
            return View();
        }
        [HttpPost]
        public IActionResult SearchMember(int txtmid)
        {
            ViewBag.flag = 1;
            ViewBag.Issuedate = Convert.ToDateTime(DateTime.Now);
            DateTime today = Convert.ToDateTime(DateTime.Now);
            ViewBag.Returndate = today.AddDays(7);
            FunctionModel mod = new FunctionModel();
            int memid = txtmid;
            DataTable dt = mod.GetMemberbyId(memid);
            return View("IssueBook", dt);
        }
        [HttpPost]
        public IActionResult IssueBookinsert(IFormCollection fm)
        {
            FunctionModel mod = new FunctionModel();
            int bookid = Convert.ToInt32(fm["txtbookid"]);
            string idate = fm["txtidate"];
            string rdate = fm["txtrdate"];
            string user = "lib";
            int a = mod.InsertIBookRecord(bookid, idate, rdate,user);
            if (a == 1)
            {
                int b = mod.Findissuebookid();
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book Issued, Note IssueBook id of this book is: "+b;
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 2)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "Invalid Bookid,This book does'nt exist";
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 3)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "This book is not available all copies are already issued, kindly search another book of similar category";
                return View("~/Views/Shared/Message.cshtml");
            }
            else if (a == 4)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "ERROR";
                ViewBag.Message = "One student cannot issue more than two books at a time";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
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
            return View("ReturnBook", dt);
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
            if (today<=rdate)
            {
                daysdelay = 0;
            }
            else
            {
                daysdelay = Convert.ToInt32((today - rdate).TotalDays);

            }
            int a = mod.InsertRBookRecord(bookid,mid,daysdelay);
            if (a>0)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book Returned";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult IssuedBooks()
        {
            FunctionModel model = new FunctionModel();
            DataTable dt = model.GetAllIssuedBooks();
            return View("IssuedBooks", dt);
        }
        public IActionResult ReturnedBooks()
        {
            FunctionModel model = new FunctionModel();
            DataTable dt = model.GetAllReturnedBooks();
            return View("ReturnedBooks", dt);
        }
        public IActionResult ViewIssueBookRequest()
        {
            FunctionModel model = new FunctionModel();
            DataTable dt = model.GetIssueBookRequest();
            return View("ViewIssueBookRequest", dt);
        }

        public IActionResult Accept(int Ibookid)
        {
            FunctionModel model = new FunctionModel();
            int a = model.AcceptIssuebookRequest(Ibookid);
            if (a > 0)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book Issued";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }


    }
}
