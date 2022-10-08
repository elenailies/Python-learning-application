using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminRequest
        public ActionResult Index()
        {
            var products = from product in db.Products.Include("Category").Include("User")
                           where product.Request == false
                           orderby product.Title
                           select product;
            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }

        // SHOW
        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        public ActionResult Accepted(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);

                    if (TryUpdateModel(product))
                    {
                         product.Request = true;
                         db.SaveChanges();
                         TempData["message"] = "Postarea a fost acceptata!";
                         return RedirectToAction("Index");
                    }

                    else
                    {
                        return RedirectToAction("Index");

                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }

        //metoda Rejected
        [HttpDelete]
        public ActionResult Rejected(int id)
        {
            Product product = db.Products.Find(id);

            db.Products.Remove(product);
            db.SaveChanges();
            TempData["message"] = "Cererea a fost stearsa!";
            return RedirectToAction("Index");

        }

    }
}