using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using Microsoft.EntityFrameworkCore;

namespace MIPPA.Controllers.Web
{
    public class HomeController : Controller
    {
        private readonly MippaContext _context;

        public HomeController(MippaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Leagues()
        {
            var managers = _context.Managers.Include(x=>x.Sessions);

            return View(managers);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
