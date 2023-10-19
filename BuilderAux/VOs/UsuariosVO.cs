using BuilderAux.Models;
using Flunt.Notifications;
using Flunt.Validations;
using System.Data;

namespace BuilderAux.VOs
{
    public class UsuariosVO : Notifiable<Notification>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string? Role { get; set; }
        public string? Senha { get; set; }


        public UsuariosVO(string name, string email, string telefone, string? role)
        {
            var contract = new Contract<Usuarios>()
                .IsNotNullOrEmpty(name, "Nome", "Campo nome vazio")
                .IsNotNullOrEmpty(telefone, "Telefone", "Campo telefone vazio")
                .IsEmailOrEmpty(email, "Email", "Campo email vazio ou invalido");
            AddNotifications(contract);

            Name = name;
            Email = email;
            Telefone = telefone;
            if(role != null) { Role = role; }

        }
    }


}
