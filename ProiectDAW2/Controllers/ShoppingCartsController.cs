using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingCarts
        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult Index()
        {
            var utilizator_curent = User.Identity.GetUserId();
            var addedproducts = from produse in db.Addings.Include("Product").Include("ShoppingCart")
                                where produse.ShoppingCart.UserId == utilizator_curent
                                      && produse.ShoppingCart.UsedForOrder == false
                                select produse;

            ViewBag.Addedproducts = addedproducts;

            if (addedproducts.Count() > 0)
            {
                ViewBag.Total = addedproducts.Sum(x => x.TotalPricePerProduct).ToString();
            }
            else
            {
                ViewBag.Total = "0";
            }


            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();

            }

            if (TempData.ContainsKey("sortare"))
            {
                ViewBag.Sort = TempData["sortare"].ToString();
                TempData["sortare"] = TempData["sortare"];
            }

            return View();
        }

        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult CreateShoppingCart(int id) // id = id-ul produsului care se doreste a fi adaugat in cos
        {
            if (TempData.ContainsKey("sortare"))
            {
                ViewBag.Sort = TempData["sortare"].ToString();
                TempData["sortare"] = TempData["sortare"];
            }

            var cosuri_de_cump = from cosuri in db.ShoppingCarts
                                 where cosuri.UsedForOrder == false
                                 select cosuri.UserId;

            if (cosuri_de_cump.Contains(User.Identity.GetUserId()))
            {
                return Redirect("/ShoppingCarts/AddToCart/" + id);
            }
            else
            {
                // cream cosul de cumparaturi pentru user-ul care se inregistreaza
                try
                {
                    ShoppingCart shopCart = new ShoppingCart();
                    shopCart.UserId = User.Identity.GetUserId();
                    shopCart.UsedForOrder = false;
                    if (ModelState.IsValid)
                    {
                        db.ShoppingCarts.Add(shopCart);
                        db.SaveChanges();

                        return Redirect("/ShoppingCarts/AddToCart/" + id);
                    }
                    else
                    {
                        TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                        return Redirect("/Products/Index");
                    }
                }

                catch (Exception e)
                {
                    TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                    return Redirect("/Products/Index");
                }

            }
        }

        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult AddToCart(int id)
        {
            if (TempData.ContainsKey("sortare"))
            {
                ViewBag.Sort = TempData["sortare"].ToString();
                TempData["sortare"] = TempData["sortare"];
            }

            try
            {
                Product product = db.Products.Find(id);
                var utilizator_curent = User.Identity.GetUserId();
                var produse_din_cos = from produse in db.Addings.Include("Product").Include("ShoppingCart")
                                      where produse.ShoppingCart.UserId == utilizator_curent
                                            && produse.ShoppingCart.UsedForOrder == false
                                      select produse.ProductId; // id-ul produselor deja existente in cosul user-ului
                if (produse_din_cos.Contains(id))
                {
                    if (ModelState.IsValid)
                    {
                        var addingId = (from adding in db.Addings.Include("Product").Include("ShoppingCart")
                                        where adding.ProductId == id && adding.ShoppingCart.UserId == utilizator_curent
                                              && adding.ShoppingCart.UsedForOrder == false
                                        select adding.AddingId).FirstOrDefault(); // gasesc in tabelul asociativ id-ul liniei in care
                                                                                  // produsul e produsul care se doreste a fi adaugat din nou
                                                                                  // si cosul de cumparaturi e al utilizatorului curent
                        Adding findProduct = db.Addings.Find(addingId);
                        if (TryUpdateModel(findProduct))
                        {

                            findProduct.Quantity = findProduct.Quantity + 1;
                           // findProduct.TotalPricePerProduct = findProduct.Quantity * findProduct.Product.Price;
                            db.SaveChanges();
                            TempData["message"] = "Produsul a fost adaugat in cos!";
                            return Redirect("/Products/Index?OrderByString="+ ViewBag.Sort);
                        }
                        else
                        {
                            TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                            return RedirectToAction("Index", "Products");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                        return RedirectToAction("Index", "Products");
                    }
                }
                else
                {
                    Adding addProduct = new Adding();
                    addProduct.ProductId = product.ProductId;
                    var shopId = from cart in db.ShoppingCarts
                                 where cart.UserId == utilizator_curent && cart.UsedForOrder == false
                                 select cart.ShoppingCartId;
                    addProduct.ShoppingCartId = shopId.FirstOrDefault(); // gasim cosul de cumprataturi al user-ului curent

                    addProduct.Quantity = 1;
                    //addProduct.TotalPricePerProduct = product.Price;
                    if (ModelState.IsValid)
                    {
                        db.Addings.Add(addProduct);
                        db.SaveChanges();
                        TempData["message"] = "Produsul a fost adaugat in cos!";
                        return Redirect("/Products/Index?OrderByString=" + ViewBag.Sort);
                    }
                    else
                    {
                        TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                        return RedirectToAction("Index", "Products");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "Adaugarea produsului in cos a esuat! Reincercati!";
                return RedirectToAction("Index", "Products");
            }
        }

        // Cand se apasa pe +
        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult IncreaseQuantity(int id)
        {
            Adding findAdding = db.Addings.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(findAdding))
                    {
                        findAdding.Quantity = findAdding.Quantity + 1;
                        //findAdding.TotalPricePerProduct = findAdding.Quantity * findAdding.Product.Price;
                        db.SaveChanges();
                        TempData["message"] = "Cantitatea produsului s-a modificat!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu s-a putut creste cantitatea!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["message"] = "Nu s-a putut creste cantitatea!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "Nu s-a putut creste cantitatea!";
                return RedirectToAction("Index");
            }
        }

        // Cand se apasa pe -
        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult DecreaseQuantity(int id)
        {
            Adding findAdding = db.Addings.Find(id);
            if (findAdding.Quantity > 1) // daca produsul care cantitatea 1, atunci nu se mai poate scade cantitatea
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (TryUpdateModel(findAdding))
                        {
                            findAdding.Quantity = findAdding.Quantity - 1;
                           // findAdding.TotalPricePerProduct = findAdding.Quantity * findAdding.Product.Price;
                            db.SaveChanges();
                            TempData["message"] = "Cantitatea produsului s-a modificat!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["message"] = "Nu s-a putut micsora cantitatea!";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Nu s-a putut micsora cantitatea!";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    TempData["message"] = "Nu s-a putut micsora cantitatea!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "Pentru a sterge produsul din cos apasati butonul corespunzator!";
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult DeleteProduct(int id)
        {
            Adding findAdding = db.Addings.Find(id);
            db.Addings.Remove(findAdding);
            db.SaveChanges();
            TempData["message"] = "Produsul a  fost eliminat din cos!";
            return RedirectToAction("Index");
        }

    }
}