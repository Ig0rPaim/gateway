using FrontBuilderAux.DTOs;
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
        public async Task<IActionResult> CriarContaAsync(UsuariosCreateAccount user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await _gateWay.PostAsync(user);
                    if (result) 
                    {
                        TempData["MensagemSucesso"] = $"Você foi Cadastrado com sucesso";
                        return RedirectToAction("Index", "Login"); 
                    }
                }

                var mensagensErro = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                TempData["MensagemErro"] = $"Opa deu merda, hein: {mensagensErro}";
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
