﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Mvc;

namespace EComm.Web.Controllers
{
    public class HomeController : Controller
    {

        private ECommContext _context;

        public HomeController(ECommContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //            return Content("Hello from HomeController");

            //return Content("<strong>Hello</strong> from <em>HomeController</em>", "text/html");

            //var person = new {FirstName = "Bill", LastName = "Gates"};
            //return Json(person);

            //return View();

            return Content($"Number of products: {_context.Products.Count()}");

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
