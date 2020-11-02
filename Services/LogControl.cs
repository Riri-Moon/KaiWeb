using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kai_Stranka.Services
{
    public class LogControl : Controller
    {
        public ActionResult IsLogged()
        {
            var isLogged = true;

            ViewBag.IsLogged = isLogged;
            return View();
        }
    }
}
