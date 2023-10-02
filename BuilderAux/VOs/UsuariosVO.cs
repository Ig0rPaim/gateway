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

        public UsuariosVO(string name, string email)
        {
            var contract = new Contract<Usuarios>()
                .IsNotNullOrEmpty(name, "Nome", "Campo nome vazio")
                .IsEmailOrEmpty(email, "Email", "Campo email vazio ou invalido");
            AddNotifications(contract);

            Name = name;
            Email = email;
        }
    }


}
