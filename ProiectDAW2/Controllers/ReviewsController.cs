using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProiectDAW.Controllers
{
    public class ReviewsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // Metoda INDEX
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            return View();
        }

        // Metoda SHOW
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Show(int id)
        {
            return View();
        }

        //EDIT
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Edit(int id)
        {
            TempData["message"] = "Postarea a fost adaugata!";
            Review review = db.Reviews.Find(id);
            //return View(review);
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(review);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui comentariu care nu va apartine";
                return RedirectToAction("Index");
            }
        }

        // Metoda EDIT
        [HttpPut]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Review review = db.Reviews.Find(id);
                    if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(review))
                        {

                            review.ReviewContent = requestReview.ReviewContent;
                            db.SaveChanges();
                            TempData["message"] = "Comentariul a fost editat!";
                            return Redirect("/Products/Show/" + review.ProductId);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui comentariu care nu va apartine";
                        return RedirectToAction("Index");
                    }
                }
                return View(requestReview);
            }
            catch (Exception e)
            {
                return View(requestReview);
            }

        }

        // Metoda DELETE
        [HttpDelete]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost sters!";
                return Redirect("/Products/Show/" + review.ProductId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine";
                return Redirect("/Products/Show/" + review.ProductId);
            }
        }


        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteColaborator = User.IsInRole("Colaborator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }

    }
}