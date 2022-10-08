using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult PlaceOrder()
        {
            Order order = new Order();

            //product.UserId = User.Identity.GetUserId(); 
            return View(order);
        }


        [HttpPost]
        [Authorize(Roles = "User,Colaborator,Admin")]
        public ActionResult PlaceOrder(Order order)
        {
            var user_curent = User.Identity.GetUserId();
            order.ShoppingCartId = (from cart in db.ShoppingCarts
                                    where cart.UserId == user_curent && cart.UsedForOrder == false
                                    select cart.ShoppingCartId).FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(order);
                    db.SaveChanges();

                    ShoppingCart cart = db.ShoppingCarts.Find(order.ShoppingCartId);
                    if (TryUpdateModel(cart))
                    {
                        cart.UsedForOrder = true;
                        db.SaveChanges();
                    }

                    string autorEmail = order.Email;

                    TempData["message"] = "Comanda a fost plasata!";

                    var produse = from p in db.Addings.Include("Product").Include("ShoppingCart")
                                  where p.ShoppingCartId == order.ShoppingCartId
                                  select p;

                    string emailBody = "<p>Buna ziua, " + order.Name + ". Comanda dvs. a fost inregistrata si va vom contacta in momentul expeditiei. </p>";
                    emailBody += "<p><strong> Detalii comanda </strong></p>";
                    emailBody += "<p> Nume si prenume: <i>" + order.Name + " </i></p>";
                    emailBody += "<p> Adresa de livrare: <i>" + order.Address + " </i></p>";
                    emailBody += "<p> Metoda de plata: <i>" + order.PaymentM + " </i></p>";
                    emailBody += "<br /><p>Produsele din comanda: </p>";
                    foreach (var p in produse)
                    {
                        emailBody += "<p><strong>" + p.Product.Title + "</strong></p>";
                        emailBody += "<p><i>" + p.Quantity + " X " + p.TotalPricePerProduct + " lei </i></p>";
                    }

                    emailBody += "<p><strong> Total plata: " + produse.Sum(x => x.TotalPricePerProduct).ToString() + " lei </strong></p>";
                    emailBody += " <p> O zi frumoasa! </p>";

                    SendEmailNotification(autorEmail, "Comanda dvs. a fost plasata", emailBody);

                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return View(order);
                }
            }
            catch (Exception e)
            {
                return View(order);
            }
        }


        private void SendEmailNotification(string toEmail, string subject, string content)
        {
            const string senderEmail = "ProiectDAW1@gmail.com";
            const string senderPassword = "Proiect123_DAW";
            const string smtpServer = "smtp.gmail.com";
            const int smtpPort = 587;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage email = new MailMessage(senderEmail, toEmail, subject, content);

            email.IsBodyHtml = true;

            email.BodyEncoding = UTF8Encoding.UTF8;

            try
            {
                System.Diagnostics.Debug.WriteLine("Sending email...");
                smtpClient.Send(email);
                System.Diagnostics.Debug.WriteLine("Email sent!");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error occured while trying to send email");
                System.Diagnostics.Debug.WriteLine(e.Message.ToString());
            }
        }

    }
}