using BuilderAux.DTO_s;
using BuilderAux.Repository.Usuarios;
using BuilderAux.VOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuilderAux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuariosRepository _usuariosRepository;
        public UsuarioController(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                return Ok(await _usuariosRepository.GetAsync());
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{email}")]
        [Authorize]
        public async Task<ActionResult> GetByEmailAsync(string email)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                return Ok(await _usuariosRepository.GetByEmailAsync(email));
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAsync([FromBody] UsuariosVO value)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                UsuariosVO retorno = await _usuariosRepository.PostAsync(value);

                return Ok(retorno);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); ;
            }
        }
        
        [HttpPut("{email}")]
        [Authorize]
        public async Task<ActionResult> PutAsync(string email, [FromBody] UsuariosVO user)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                await _usuariosRepository.PutAsync(email, user);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); ;
            }
        }

        [HttpDelete("{email}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(string email)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                bool retorno = await _usuariosRepository.DeleteAsync(email);
                return Ok(retorno);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{email}")]
        [Authorize]
        public async Task<ActionResult> AtualizarSenha([FromBody] AtualizarSenha value)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                await _usuariosRepository.MudarSenha(value.novaSenha, value.email);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] Login value)
        {
            try
            {
                string userName = User.Identity.Name ?? string.Empty;
                string result = await _usuariosRepository.Login(value);
                if (!string.IsNullOrEmpty(result)) return Ok(result);
                else return BadRequest("Usuario não encontrado");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
