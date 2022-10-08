using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace ProiectDAW2.Controllers
{
    public class TutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tutorials
        public ActionResult Tutorial1()
        {
            return View();
        }

        public ActionResult Tutorial2()
        {
            return View();
        }

        public ActionResult Tutorial3()
        {
            return View();
        }

        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            var tuts = from tut in db.Tuts
                             select tut;

            ViewBag.Tuts = tuts;
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();

            }
            return View();
        }

        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Start()
        {
           // List<int> tutIds = db.Tuts.Select(a => a.TutId).ToList();

            //if (tutIds.Count == 1)
            //{
            
            //}

            return View();
        }

        //metoda NEW
        /*public ActionResult New()
        {
            Tut tut = new Tut();
            return View(tut);
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost adaugata cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                return View(category);
            }
        }*/

        // Metoda SHOW cu post
        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Add(Tut tut)
        {
            tut.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tuts.Add(tut);
                    db.SaveChanges();
                    TempData["message"] = "Tutorialul a fost completat!";
                    return Redirect("/Tuts/Index" );
                }
                //else
                //{
                // Product a = db.Products.Find(review.ProductId);
                // SetAccessRights();
                // return View(a);
                //}
                return Redirect("/Tuts/Index");
            }

            catch (Exception e)
            {
                //Product a = db.Products.Find(review.ProductId);
                //SetAccessRights();
                //return View(a);
                return Redirect("/Tuts/Index");
            }

        }
    }
}


