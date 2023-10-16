using FrontBuilderAux.Models;
using FrontBuilderAux.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontBuilderAux.Controllers
{
    public class CreateAccount : Controller
    {
        private readonly IBuilderAuxGateWayService _gateWay;
        public CreateAccount(IBuilderAuxGateWayService gateWay)
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
        public async Task<IActionResult> CriarContaAsync(Usuarios user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ = await _gateWay.PostAsync(user);
                }

                var mensagensErro = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                TempData["MessageErro"] = mensagensErro;
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
