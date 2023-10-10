using FrontBuilderAux.Models;
using FrontBuilderAux.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontBuilderAux.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuilderAuxGateWayService _gateWay;

        public HomeController(IBuilderAuxGateWayService gateWay)
        {
            _gateWay = gateWay ?? throw new ArgumentNullException(nameof(gateWay));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}