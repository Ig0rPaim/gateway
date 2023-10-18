using FrontBuilderAux.DTOs;
using FrontBuilderAux.Models;
using FrontBuilderAux.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontBuilderAux.Controllers
{
    public class LoginController : Controller
    {
        private readonly IBuilderAuxGateWayService _gateWay;
        public LoginController(IBuilderAuxGateWayService gateWay)
        {
            _gateWay = gateWay ?? throw new ArgumentNullException(nameof(gateWay));
        }
        public IActionResult Index()
        {
			try
			{
                return View();
            }
			catch (Exception er)
			{
                TempData["MensagemErro"] = $"opa! Deu merda, hein: {er.Message}";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public async Task<IActionResult> EntrarAsync(UsuariosLogin user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await _gateWay.Login(user);
                    if (result) { return RedirectToAction("Index", "Home"); }
                    
                }
                TempData["MensagemErro"] = $"Usário e/ou senha invalido(s). Tente novamente.";
                return View("Index");
            }

            catch (Exception er)
            {
                TempData["MensagemErro"] = $"opa! Deu merda, hein: {er.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
