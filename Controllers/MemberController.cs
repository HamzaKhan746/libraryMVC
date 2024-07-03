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
    public class MemberController : Controller
    {
        [HttpGet]
        public IActionResult AddMember()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertMember(IFormCollection fm)
        {
            MemberModel mod = new MemberModel();
            string fname = fm["txtfname"];
            string lname = fm["txtlname"];
            string email = fm["txtemail"];
            string contact = fm["txtcontact"];
            string address = fm["txtaddress"];
            string password = fm["txtpassword"];
            int a = mod.InsertMemberRecord(fname,lname, email, contact, address, password);
            if(a > 0)
            {
                
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Member Adeded, Note Username of Member is Member id which is: "+a;
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        [HttpGet]
        public IActionResult UpdateMember()
        {
            return View();
        }
        public IActionResult DeleteMember()
        {
            return View();
        }
        public IActionResult ViewMemberdetails()
        {
            MemberModel model = new MemberModel();
            DataTable dt = model.GetAllMembers();
            return View("ViewMemberdetails",dt);
        }
        [HttpPost]
        public IActionResult SearchMember(int txtmid)
        {
            ViewBag.flag = 1;
            MemberModel mod = new MemberModel();
            int memid = txtmid;
            DataTable dt = mod.GetMemberbyId(memid);
            return View("UpdateMember",dt);
        }
        [HttpGet]
        [HttpPost]
        public IActionResult EditMember(IFormCollection frm)
        {
           
            MemberModel model1 = new MemberModel();
            string fname = frm["txtfname"];
            string lname = frm["txtlname"];
            string email = frm["txtemail"];
            string contact = frm["txtcontact"];
            string address = frm["txtaddress"];
            string password = frm["txtpassword"];
            int b = model1.EditMemberRecord(fname, lname, email, contact, address, password);
            if (b > 0)
            {
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Member updated";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult RemoveMember(IFormCollection frm)
        {
            
            MemberModel model = new MemberModel();
            int memid = Convert.ToInt32(frm["txtmid"]);
            int a = model.DeleteMemberRecord(memid);
            if (a > 0)
            {
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Member deleted";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        [HttpGet]
        public IActionResult UpdateLibrarian()
        {
            MemberModel model = new MemberModel();
            DataTable dt = model.SearchLibraian();
            return View("UpdateLibrarian", dt);
        }
        public IActionResult UpdateLibrarianInfo(IFormCollection frm)
        {

            MemberModel model1 = new MemberModel();
            string libname = frm["txtlibname"];
            string email = frm["txtemail"];
            string contact = frm["txtcontact"];
            string address = frm["txtaddress"];
            string uname = frm["txtusername"];
            string password = frm["txtpassword"];
            int b = model1.UpdateLibrarianRecord(libname, email, contact, address, uname, password);
            if (b > 0)
            {
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Librarian updated";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult ViewLibrariandetails()
        {
            MemberModel model = new MemberModel();
            DataTable dt = model.GetLibrarian();
            return View("ViewLibrariandetails", dt);
        }
    }
}
