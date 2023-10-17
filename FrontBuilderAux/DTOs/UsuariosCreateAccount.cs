using System.ComponentModel.DataAnnotations;

namespace FrontBuilderAux.DTOs
{
    public class UsuariosCreateAccount
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Role { get; set; }


        public UsuariosCreateAccount()
        {

        }
        
        public UsuariosCreateAccount(string name, string email, string telefone, string role)
        {
            Name = name ?? throw new ArgumentNullException();
            Email = email ?? throw new ArgumentNullException();
            Telefone = telefone ?? throw new ArgumentNullException();
            Role = role ?? throw new ArgumentNullException();
        }
    }
}
