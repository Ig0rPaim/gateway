using BuilderAux.Exceptions;
using BuilderAux.Repository.Roles;
using BuilderAux.Repository.Usuarios;
using BuilderAux.VOs;
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
        // GET: api/<UsuarioController>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] UsuariosVO value)
        {
            try
            {
               UsuariosVO retorno = await _usuariosRepository.Post(value);

                return Ok(retorno);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
        
        

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteAsync(string email)
        {
            try
            {
                bool retorno = await _usuariosRepository.Delete(email);
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
    }
}
