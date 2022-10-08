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

    public class CategoriesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Admin") || User.IsInRole("Colaborator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteColaborator = User.IsInRole("Colaborator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }

        // GET: Categories
        [Authorize(Roles = "Colaborator,Admin")]
        public ActionResult Index()
        {
            SetAccessRights();
            var categories = from categorie in db.Categories
                             orderby categorie.CategoryName
                             select categorie;

            ViewBag.Categories = categories;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();

            }

            return View();
        }

        // SHOW
        [Authorize(Roles = "Colaborator,Admin")]
        public ActionResult Show(int id)
        {
            SetAccessRights();
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //metoda NEW
        [Authorize(Roles = "Admin, Colaborator")]
        public ActionResult New()
        {
            SetAccessRights();
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Colaborator")]
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
        }


        //metoda EDIT
        [Authorize(Roles = "Admin, Colaborator")]
        public ActionResult Edit(int id)
        {
            SetAccessRights();
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View(category);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Colaborator")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);
                    if (TryUpdateModel(category))
                    {

                        category.CategoryName = requestCategory.CategoryName;
                        db.SaveChanges();
                        TempData["message"] = "Categoria a fost editata!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestCategory);
                    }
                }
                return View(requestCategory);
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        //metoda DELETE
        [HttpDelete]
        [Authorize(Roles = "Admin, Colaborator")]
        public ActionResult Delete(int id)
        {
            SetAccessRights();
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stersa!";
            return RedirectToAction("Index");
        }


    }
}