using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            return View();
        }
    }
}