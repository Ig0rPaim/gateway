using Flunt.Notifications;
using Flunt.Validations;

namespace BuilderAux.DTO_s
{
    public class Login : Notifiable<Notification>
    {
        public string email { get; set; }
        public string Senha { get; set; }

        public Login()
        {
            var contract = new Contract<Login>()
                .IsNotNullOrEmpty(email, "email", "o campo email não pode esta vazio")
                .IsNotNullOrEmpty(Senha, "Senha", "o campo não pode esta vazio");
        }
    }
}
