using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace ProiectDAW.Controllers
{
    public class TestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tests
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Index()
        {
            var tests = from test in db.Tests
                             orderby test.TestName
                             select test;


            // cautare
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<int> testIds = db.Tests.Where(at => at.TestName.Contains(search) /*&& at.Request.Equals(true))*/).Select(a => a.TestId).ToList();
                //List<int> mergeIds = productIds.Union(productIds).ToList();
                tests = db.Tests.Where(test => testIds.Contains(test.TestId))/*.Include("Category").Include("User")*/.OrderBy(async => async.TestName);
            }
            ViewBag.SearchString = search;


            ViewBag.Tests = tests;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();

            }

            return View();
        }

        // SHOW
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Show(int id)
        {
            int idd;
            idd = id;

            Test test = db.Tests.Find(id);
            return View(test);
        }

        //metoda NEW
        public ActionResult New()
        {
            Test test = new Test();
            return View(test);
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin")]
        public ActionResult New(Test test)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tests.Add(test);
                    db.SaveChanges();
                    TempData["message"] = "Testul a fost adaugat cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(test);
                }
            }
            catch (Exception e)
            {
                return View(test);
            }
        }



        //metoda EDIT
        [Authorize(Roles = "Colaborator,Admin")]
        public ActionResult Edit(int id)
        {
            Test test = db.Tests.Find(id);
            ViewBag.Test = test;
            return View(test);
        }

        [HttpPut]
        public ActionResult Edit(int id, Test requestTest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Test test = db.Tests.Find(id);
                    if (TryUpdateModel(test))
                    {

                        test.TestName = requestTest.TestName;

                        test.intrebare_1 = requestTest.intrebare_1;
                        test.raspuns_1_1 = requestTest.raspuns_1_1;
                        test.raspuns_1_2 = requestTest.raspuns_1_2;
                        test.raspuns_1_3 = requestTest.raspuns_1_3;
                        test.raspuns_1_4 = requestTest.raspuns_1_4;
                        test.raspuns_corect_1 = requestTest.raspuns_corect_1;

                        test.intrebare_2 = requestTest.intrebare_2;
                        test.raspuns_2_1 = requestTest.raspuns_2_1;
                        test.raspuns_2_2 = requestTest.raspuns_2_2;
                        test.raspuns_2_3 = requestTest.raspuns_2_3;
                        test.raspuns_2_4 = requestTest.raspuns_2_4;
                        test.raspuns_corect_2 = requestTest.raspuns_corect_2;

                        test.intrebare_3 = requestTest.intrebare_3;
                        test.raspuns_3_1 = requestTest.raspuns_3_1;
                        test.raspuns_3_2 = requestTest.raspuns_3_2;
                        test.raspuns_3_3 = requestTest.raspuns_3_3;
                        test.raspuns_3_4 = requestTest.raspuns_3_4;
                        test.raspuns_corect_3 = requestTest.raspuns_corect_3;

                        db.SaveChanges();
                        TempData["message"] = "Testul a fost editatat!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestTest);
                    }
                }
                return View(requestTest);
            }
            catch (Exception e)
            {
                return View(requestTest);
            }
        }

        //metoda DELETE
        [HttpDelete]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult Delete(int id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
            db.SaveChanges();
            TempData["message"] = "Testul a fost sters!";
            return RedirectToAction("Index");
        }

        public ActionResult GetGrades()
        {
            var grades = from grade in db.Grades
                       select grade;

            ViewBag.Grades = grades;
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            
            return View();
        }

        // Metoda SHOW cu post
        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin,User")]
        public ActionResult AddGrade(Grade grade)
        {
            grade.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Grades.Add(grade);
                    db.SaveChanges();
                    TempData["message"] = "Testul a fost completat!";
                    return Redirect("/Tests/Index");
                }
               
                return Redirect("/Tests/Index");
            }

            catch (Exception e)
            {
                return Redirect("/Tests/Index");
            }

        }


    }
}