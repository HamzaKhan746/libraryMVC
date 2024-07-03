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
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertBook(IFormCollection fm)
        {
            BookModel mod = new BookModel();
            string bname = fm["txtbname"];
            string author = fm["txtauthorname"];
            string publisher = fm["txtpublisher"];
            string category = fm["txtcategory"];
            int price = Convert.ToInt32(fm["txtprice"]);
            int quantity = Convert.ToInt32(fm["txtquantity"]);
            int a = mod.InsertBookRecord(bname, author, publisher, category, price, quantity);
            if (a > 0)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book Adeded, Note Book id of this Book is: " +a;
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        [HttpGet]
        public IActionResult UpdateBook()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchBook(int txtbid)
        {
            ViewBag.flag = 1;
            BookModel mod = new BookModel();
            int bid = txtbid;
            DataTable dt = mod.GetBookbyId(bid);
            return View("UpdateBook", dt);
        }
        [HttpGet]
        [HttpPost]
        public IActionResult EditBook(IFormCollection fm)
        {
            BookModel mod = new BookModel();
            string bname = fm["txtbname"];
            string author = fm["txtauthorname"];
            string publisher = fm["txtpublisher"];
            string category = fm["txtcategory"];
            int price = Convert.ToInt32(fm["txtprice"]);
            int quantity = Convert.ToInt32(fm["txtquantity"]);
            int a = mod.EditBookRecord(bname, author, publisher, category, price, quantity);
            if (a > 0)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book updated";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult RemoveBook(IFormCollection frm)
        {

            BookModel model = new BookModel();
            int bid = Convert.ToInt32(frm["txtbid"]);
            int a = model.DeleteBookRecord(bid);
            if (a > 0)
            {
                ViewBag.dashboard = "lib";
                ViewBag.Heading = "SUCCESS";
                ViewBag.Message = "Book deleted";
                return View("~/Views/Shared/Message.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public IActionResult ViewBooks()
        {
            
            if(HomeController.user=="mem")
            {
                ViewBag.user = "mem";
            }
            BookModel model = new BookModel();
            DataTable dt = model.GetAllBooks();
            return View("ViewBooks", dt);
        }
        public IActionResult Searchviewbook(IFormCollection fm)
        {
            if (HomeController.user == "mem")
            {
                ViewBag.user = "mem";
            }
            BookModel model = new BookModel();
            
            string category = fm["txtcategory"];
            if (category != "All")
            {
                DataTable dt = model.GetBookbyCategory(category);
                return View("ViewBooks", dt);
            }
            else
            {
                DataTable dt = model.GetAllBooks();
                return View("ViewBooks", dt);
            }

        }

    }
}
