using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProiectDAW2.Controllers
{
    public class RatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ratings
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            return View();
        }

        // Rating nou
        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult New(String RatingDat, String IdulProdus)
        {
            Rating rating = new Rating();
            rating.UserId = User.Identity.GetUserId();
            rating.ProductId = Convert.ToInt32(IdulProdus);
            rating.RatingValue = float.Parse(RatingDat, System.Globalization.CultureInfo.InvariantCulture); 

            try
            {
                if (ModelState.IsValid)
                {
                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    Product product = db.Products.Find(rating.ProductId);
                    if (TryUpdateModel(product))
                    {
                        var review = from r in db.Ratings.Include("Product").Include("User")
                                     where r.ProductId == product.ProductId
                                     select r;
                        var medie = "";
                        if (review.Count() > 0)
                        {
                            medie = (review.Sum(x => x.RatingValue) / review.Count()).ToString("0.00");
                            product.Rating = float.Parse(medie, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            product.Rating = 0;
                        }
                        db.SaveChanges();
                    }
                    return Redirect("/Products/Show/" + rating.ProductId);
                }
                else
                {
                    return Redirect("/Products/Show/" + rating.ProductId);
                }
            }
            catch (Exception e)
            {
                return Redirect("/Products/Index");
            }

        }
    }
}