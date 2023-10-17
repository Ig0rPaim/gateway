using System.ComponentModel.DataAnnotations;
using System.Net.Security;

namespace FrontBuilderAux.Models
{
    public class Usuarios
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Role { get; set; }
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; }


        public Usuarios()
        {
            
        }
        public Usuarios(string name, string email, string telefone, string role, string senha, string confirmarSenha)
        {
            Name = name ?? throw new ArgumentNullException();
            Email = email ?? throw new ArgumentNullException();
            Telefone = telefone ?? throw new ArgumentNullException();
            Role = role ?? throw new ArgumentNullException();
            Senha = senha ?? throw new ArgumentNullException();
            ConfirmarSenha = confirmarSenha ?? throw new ArgumentNullException();
        }
        public Usuarios(string name, string email, string telefone, string role)
        {
            Name = name ?? throw new ArgumentNullException();
            Email = email ?? throw new ArgumentNullException();
            Telefone = telefone ?? throw new ArgumentNullException();
            Role = role ?? throw new ArgumentNullException();
        }


    }

}
