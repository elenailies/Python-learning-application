using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProiectDAW.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private int _ProdusePerPagina = 9;


        // GET: Product
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index(string OrderByString)
        {
            var produse = from produs in db.Products.Include("Category").Include("User")
                          where produs.Request == true
                          select produs;

            // sortare dupa pret si rating crescator si descrescator
            switch (OrderByString)
            {
                case "pret_descrescator":
                    produse = produse.OrderByDescending(m => m.Title);
                    break;
                case "pret_crescator":
                    produse = produse.OrderBy(m => m.Title);
                    break;
                case "rating_descrescator":
                    produse = produse.OrderByDescending(m => m.Rating);
                    break;
                case "rating_crescator":
                    produse = produse.OrderBy(m => m.Rating);
                    break;
                default:
                    produse = produse.OrderBy(m => m.Title);
                    break;
            }

            // cautare
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<int> productIds = db.Products.Where(at => at.Category.CategoryName.Contains(search) && at.Request.Equals(true)).Select(a => a.ProductId).ToList();
                //List<int> mergeIds = productIds.Union(productIds).ToList();
                produse = db.Products.Where(product => productIds.Contains(product.ProductId)).Include("Category").Include("User").OrderBy(async => async.Title);
            }
            ViewBag.SearchString = search;

            // paginare
            var numarProduse = produse.Count();
            var paginaCurenta = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;
            if (!paginaCurenta.Equals(0))
            {
                offset = (paginaCurenta - 1) * this._ProdusePerPagina;
            }
            var produsePaginate = produse.Skip(offset).Take(this._ProdusePerPagina);

            var categories = from categorie in db.Categories
                             orderby categorie.CategoryName
                             select categorie;

            ViewBag.Categories = categories;

            ViewBag.ProdusePerPagina = this._ProdusePerPagina;
            ViewBag.NumarProduse = numarProduse;
            ViewBag.UltimaPagina = Math.Ceiling((float)numarProduse / (float)this._ProdusePerPagina);
            ViewBag.PaginaCurenta = paginaCurenta;

            ViewBag.Products = produsePaginate;

            if (String.IsNullOrEmpty(OrderByString))
            {
                ViewBag.Sort = "implicit";
                TempData["sortare"] = "implicit";
            }
            else
            {
                ViewBag.Sort = OrderByString;
                TempData["sortare"] = OrderByString;
            }

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        // SHOW
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            if (TempData.ContainsKey("sortare"))
            {
                ViewBag.Sort = TempData["sortare"].ToString();
                TempData["sortare"] = TempData["sortare"];
            }

            Product product = db.Products.Find(id);
            SetAccessRights();

            if (User.IsInRole("Admin") || User.IsInRole("Colaborator") || User.IsInRole("User"))
            {
                ViewBag.Rating = true; 
            }
            else
            {
                ViewBag.Rating = false;
            }
            return View(product);
        }


        // Metoda SHOW cu post
        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Show(Review review)
        {
            review.ReviewDate = DateTime.Now;
            review.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat!";
                    return Redirect("/Products/Show/" + review.ProductId);
                }
                else
                {
                    Product a = db.Products.Find(review.ProductId);
                    SetAccessRights();
                    return View(a);
                }
            }

            catch (Exception e)
            {
                Product a = db.Products.Find(review.ProductId);
                SetAccessRights();
                return View(a);
            }

        }

        //EDIT
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            product.Categ = GetAllCategories();
            product.Imag = GetAllImages();
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati o postare pe care nu ati adaugat-o!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Edit(int id, Product requestProduct)
        {

            if (TempData.ContainsKey("sortare"))
            {
                ViewBag.Sort = TempData["sortare"].ToString();
                TempData["sortare"] = TempData["sortare"];
            }

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);

                    if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        var review = from r in db.Ratings.Include("Product").Include("User")
                                     where r.ProductId == product.ProductId
                                     select r;
                        var medie = "";

                        if (TryUpdateModel(product))
                        {
                            product.Title = requestProduct.Title;
                            product.ImageId = requestProduct.ImageId;
                           // product.Price = requestProduct.Price;
                            product.Description = requestProduct.Description;
                            if (review.Count() > 0)
                            {
                                medie = (review.Sum(x => x.RatingValue) / review.Count()).ToString("0.00");
                                product.Rating = float.Parse(medie, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                product.Rating = 0;
                            }
                            product.CategoryId = requestProduct.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Postarea s-a modificat!";
                            return Redirect("/Products/Show/" + product.ProductId);
                        }

                        else
                        {
                            requestProduct.Categ = GetAllCategories();
                            requestProduct.Imag = GetAllImages();
                            return View(requestProduct);
                        }
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa modificati o postare pe care nu ati adaugat-o!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestProduct.Categ = GetAllCategories();
                    requestProduct.Imag = GetAllImages();
                    return View(requestProduct);
                }
            }

            catch (Exception e)
            {
                requestProduct.Categ = GetAllCategories();
                requestProduct.Imag = GetAllImages();
                return View(requestProduct);
            }
        }


        //NEW
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult New()
        {
            Product product = new Product();
            // preluam lista de categorii din metoda GetAllCategories()
            product.Categ = GetAllCategories();
            product.Imag = GetAllImages();
            product.UserId = User.Identity.GetUserId(); // user-ul care adauga postarea
            return View(product);
        }


        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult New(Product product)
        {
            product.UserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                product.Request = true;
            }
            else product.Request = false;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    if(User.IsInRole("Colaborator"))
                        TempData["message"] = "Cererea de adaugare a postarii a fost trimisa catre admin!";
                    if (User.IsInRole("User"))
                        TempData["message"] = "Cererea de adaugare a postarii a fost trimisa catre admin!";
                    if (User.IsInRole("Admin"))
                        if (User.IsInRole("Admin"))
                        TempData["message"] = "Postarea a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    product.Categ = GetAllCategories();
                    product.Imag = GetAllImages();
                    return View(product);
                }
            }
            catch (Exception e)
            {
                product.Categ = GetAllCategories();
                product.Imag = GetAllImages();
                return View(product);
            }
        }

        //metoda DELETE
        [HttpDelete]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o postare pe care nu ati adaugat-o!";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllImages()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            var user_curent = User.Identity.GetUserId();

            // extragem toate imaginile din baza de date
            if (User.IsInRole("Admin"))
            { 
                var images = from img in db.Images
                             select img;
                // iteram prin imagini
                foreach (var image in images)
                {
                    // adaugam in lista elementele necesare pentru dropdown
                    selectList.Add(new SelectListItem
                    {
                        Value = image.ImageId.ToString(),
                        Text = image.ImageName.ToString()
                    });
                }
                // returnam lista de imagini
                return selectList;
            }
            else
            {
                 var images = from img in db.Images
                         where img.UserId == user_curent 
                         select img;
                // iteram prin imagini
                foreach (var image in images)
                {
                    // adaugam in lista elementele necesare pentru dropdown
                    selectList.Add(new SelectListItem
                    {
                        Value = image.ImageId.ToString(),
                        Text = image.ImageName.ToString()
                    });
                }
                // returnam lista de imagini
                return selectList;
            }
           
        }

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Colaborator") || User.IsInRole("Admin") || User.IsInRole("User"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }



    }
}