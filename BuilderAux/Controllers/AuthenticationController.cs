using BuilderAux.DTO_s;
using BuilderAux.SevicesGateWay.AutheticationServer;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuilderAux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServer _authenticationServer;

        public AuthenticationController()
        {
            
        }

        public AuthenticationController(IAuthenticationServer authenticationServer)
        {
            _authenticationServer = authenticationServer ?? throw new ArgumentNullException(nameof(authenticationServer));
        }

        [HttpPost]
        public async Task<ActionResult> CreateToken(Authentication acesso)
        {
            var setHeader = HttpContext.Response;
            setHeader.Headers.Add("Code", "h2m/p6Ca4x9UWru3ug8vDXoAcpaWm8D/Hzvc3YTq3KV5j1DC/jJE8TK+OQ2suWSkzEPVhuGBGBX62SvYSgn5HHj6ChueCvLJpzYylUSpjB2bRMj6clpKAC3XOVuvcTBU/W5+Ej/Z4eJ+7I/DCE0d1Iw01jML59FN6++4CxXyuymcTq4XQoTjW0J7kuIlUiwm7RuDhScNjKUIWjcOHPeCI8NKhA2kwmOTVfvrkKlOGiVXw5U1J/iWS7KX560lhdOuGFvhvZPPN9xlikravMbY5DEDrr3SyIgAIiD/750gd7vspT98pUpykMqb3A+tnIH069uhrhjmKzP5zSaMJlNoTw==");
            var response = await _authenticationServer.CreateToken(acesso, HttpContext);
            return Ok("ok");

        }
    }
}
