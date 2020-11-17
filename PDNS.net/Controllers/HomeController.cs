using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDNS.net.Data;
using PDNS.net.Filters;
using PDNS.net.Models;

namespace PDNS.net.Controllers
{
    [Route("{action}")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DBContext _context;
        private IMemoryCache _cache;

        public HomeController(DBContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            ViewData["UsersCount"] = _context.Users.Count();
            ViewData["DomainsCount"] = _context.Domains.Count();
            ViewData["Clients"] = Tools.GetCurrentClients();
            ViewBag.UpTime = await Tools.PingHost("8.8.8.8");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            return View(await _context.Logs.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Map()
        {
            var Countries = await await
                _cache.GetOrCreateAsync("IPCountries", async entry =>
                {
                    var IPList = await _context.Records.Where(x => x.Type.ToUpper() != "SOA").Select(x => x.Content).ToListAsync();
                    var Countries = await Tools.IPCountries(IPList);
                    entry.SlidingExpiration = TimeSpan.FromHours(3);
                    return Task.FromResult(Countries);
                });
            ViewBag.Countries = Countries;
            return PartialView("_Map");
        }

        [HttpGet]
        public async Task<IActionResult> ServerStatus()
        {
            ViewBag.ServerStatus = new Dictionary<string, PingResult> {
            { "Google", await Tools.PingHost("8.8.8.8") },        //Google
            { "CloudFlare", await Tools.PingHost("1.1.1.1") },    //CloudFlare
            { "Quad9", await Tools.PingHost("9.9.9.9") },         //Quad9
            { "Cisco", await Tools.PingHost("208.67.222.222") }   //Cisco OpenDNS
            };
            return PartialView("_PingCard");
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult Login()
        {
            return PartialView("~/Views/Auth/Login.cshtml");
        }
    }
}
