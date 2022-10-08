using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Controllers
{
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Images
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("succes"))
            {
                ViewBag.succes = TempData["succes"].ToString();
            }
            if (TempData.ContainsKey("fail"))
            {
                ViewBag.fail = TempData["fail"].ToString();
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Upload(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                // Se preia numele fisierul
                string uploadedFileName = uploadedFile.FileName;
                string uploadedFileExtension = Path.GetExtension(uploadedFileName);

                // Se poate verifica daca extensia este intr-o lista dorita
                if (uploadedFileExtension == ".png" || uploadedFileExtension == ".jpg")
                {
                    // Se stocheaza fisierul in folderul Files (folderul trebuie creat in proiect)

                    // 1. Se seteaza calea folderului de upload
                    string uploadFolderPath = Server.MapPath("~//Files//");

                    // 2. Se salveaza fisierul in acel folder
                    uploadedFile.SaveAs(uploadFolderPath + uploadedFileName);

                    // 3. Se face o instanta de model si se populeaza cu datele necesare
                    Image file = new Image();
                    file.Extension = uploadedFileExtension;
                    file.ImageName = uploadedFileName;
                    file.ImagePath = uploadFolderPath + uploadedFileName;
                    file.UserId = User.Identity.GetUserId();

                    // 4. Se adauga modelul in baza de date
                    db.Images.Add(file);
                    db.SaveChanges();

                    TempData["succes"] = "Imaginea a fost adaugata!";
                    return Redirect("Index");
                }
                {
                    TempData["fail"] = "Alegeti o imagine! Extensiile acceptate sunt png si jpg.";
                    return Redirect("Index");
                }
            }
            else
            {
                TempData["fail"] = "Nu ati adaugat o imagine!";
                return Redirect("Index");
            }
        }
    }
}