﻿using BookCheap.Busines.DomainModel;
using BookCheap.Persistence.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCheap.Clients.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {

            if (ModelState.IsValid)
            {
                var v = db.Users.Where(a => a.Login.Equals(user.Login)).FirstOrDefault();

                if (v == null)
                {
                    user.SetPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    ModelState.Clear();
                    user = null;
                }
                else
                {
                    ViewBag.message = "Użytkownik o podanym loginie już istnieje.";
                    return View("~/Views/Home/Register.cshtml");
                }
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if(ModelState.IsValid)
            {
                user.SetPassword(user.Password);
                var v = db.Users.Where(a => a.Login.Equals(user.Login) && a.Password.Equals(user.Password)).FirstOrDefault();
                if(v!=null)
                {
                    //if (user.IsBlocked == false)
                    //{
                        Session["LoggedUser"] = v.Login.ToString();
                        HttpCookie Logged = new HttpCookie("LoggedUser", user.Login);
                        return RedirectToAction("AfterLogin");
                    //}
                    //else
                    //{
                    //    ViewBag.Message = "Użytkownik zablokowany.";
                    //    return View("Login");
                    //}

                }
                else
                {
                    ViewBag.Message = "Niepoprawne dane logowania.";
                    return View("Login");
                }
            }

            return View("Index");
        }

        public ActionResult AfterLogin()
        {
            if(Session["LoggedUser"] !=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}