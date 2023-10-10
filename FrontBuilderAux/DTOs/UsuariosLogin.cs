using System.ComponentModel.DataAnnotations;

namespace FrontBuilderAux.DTOs
{
    public class UsuariosLogin
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string email { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string senha { get; set; }

        public UsuariosLogin()
        {
            
        }
        public UsuariosLogin(string email, string senha)
        {
            this.email = email ?? throw new ArgumentNullException(nameof(email));
            this.senha = senha ?? throw new ArgumentNullException(nameof(senha));
        }
    }
}
