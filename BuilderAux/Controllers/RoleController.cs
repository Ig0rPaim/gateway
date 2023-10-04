using BuilderAux.Repository.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BuilderAux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRolesRepository _rolesRepository;
        public RoleController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        [HttpPost]
        public ActionResult PostRole([FromBody] string cargo)
        {
            try
            {
                _rolesRepository.Post(cargo);
                return Ok(cargo);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }
    }
}
