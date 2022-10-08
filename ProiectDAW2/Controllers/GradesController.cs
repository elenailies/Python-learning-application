using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProiectDAW2.Controllers
{
    public class GradesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tests
        public ActionResult Index()
        {
            var grades = from grade in db.Grades
                         select grade;

            ViewBag.Grades = grades;
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            return View();
        }


    }
}