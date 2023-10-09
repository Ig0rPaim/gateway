using FrontBuilderAux.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontBuilderAux.Controllers
{
    public class LoginController : Controller
    {
        private readonly IBuilderAuxGateWayService _gateWayService;

        public LoginController(IBuilderAuxGateWayService gateWayService)
        {
            _gateWayService = gateWayService ?? throw new ArgumentNullException(nameof(gateWayService));
        }

        public ActionResult Index()
        {
            return View(); // Retorna a página de login
        }

        [HttpPost]
        public ActionResult Autenticar(string Usuario, string Senha)
        {
            // Lógica de autenticação aqui
            // Verifica se o usuário e senha são válidos

            // Se a autenticação for bem-sucedida, redireciona para a página inicial
            if (Usuario == "usuario" && Senha == "senha")
            {
                // Lógica de autenticação bem-sucedida
                return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
            }
            else
            {
                // Se a autenticação falhar, exibe uma mensagem de erro
                ViewBag.MensagemErro = "Usuário ou senha incorretos";
                return View("Index"); // Retorna a página de login com uma mensagem de erro
            }
        }
    }
}
