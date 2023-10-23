using FrontBuilderAux.DTOs;
using FrontBuilderAux.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace FrontBuilderAux.Controllers
{
    public class LoginController : Controller
    {
        private readonly IBuilderAuxGateWayService _gateWay;
        //private readonly HttpContext _httpContext;
        public LoginController(IBuilderAuxGateWayService gateWay)
        {
            _gateWay = gateWay ?? throw new ArgumentNullException(nameof(gateWay));
            //_httpContext = httpContext ?? throw new ArgumentNullException();

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
                    bool result = await _gateWay.Login(user, HttpContext);
                    //if (result) { return RedirectToAction("Index", "Home"); }
                    if (result)
                    {
                        // Armazene os dados de sessão em TempData
                        TempData["DataUser"] = HttpContext.Session.GetString("DataUser");
                        return RedirectToAction("Index", "Home");
                    }


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
