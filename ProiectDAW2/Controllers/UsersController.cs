using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Users
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            // cautare
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> Ids = db.Users.Where(at => at.UserName.Contains(search) /*&& at.Request.Equals(true))*/).Select(a => a.Id).ToList();
                //List<int> mergeIds = productIds.Union(productIds).ToList();
                users = db.Users.Where(test => Ids.Contains(test.Id))/*.Include("Category").Include("User")*/.OrderBy(async => async.UserName);
            }
            ViewBag.SearchString = search;

            ViewBag.UsersList = users;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }

        // Metoda SHOW
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id); // user-ul care se doreste a fi afisat

            ViewBag.utilizatorCurent = User.Identity.GetUserId(); // id-ul user-ului curent (mai exact al adminului logat)

            string currentRole = user.Roles.FirstOrDefault().RoleId; // id-ul rolului curent al utilizatorului de afisat
                                // Deoarece un user are un singur rol putem folosi user.Roles.FirstOrDefault().
            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First(); // numele rolului curent
            ViewBag.roleName = userRoleName;
            return View(user);
        }


        // Metoda EDIT
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id); // gasesc user-ul
            user.AllRoles = GetAllRoles(); // construiesc o colectie cu toate rolurile
            var userRole = user.Roles.FirstOrDefault(); // rolul curent al user-ului
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role; // luam toate rolurile
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {

            ApplicationUser user = db.Users.Find(id); // gasesc user-ul
            user.AllRoles = GetAllRoles(); // iau o colectie cu toate rolurile
            var userRole = user.Roles.FirstOrDefault(); // // rolul curent al user-ului
            ViewBag.userRole = userRole.RoleId;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name); // user-ul nu o sa mai aiba rolul vechi
                    }
                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole")); 
                    UserManager.AddToRole(id, selectedRole.Name); // ii dam user-ului noul rol
                    TempData["message"] = "User-ul a fost editat!";
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }

        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);

            var products = db.Products.Where(a => a.UserId == id);
            foreach (var product in products)
            {
                db.Products.Remove(product);

            }

            var reviews = db.Reviews.Where(comm => comm.UserId == id);
            foreach (var review in reviews)
            {
                db.Reviews.Remove(review);
            }
            TempData["message"] = "User-ul a fost sters!";

            // Commit pe articles
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }
    }
}