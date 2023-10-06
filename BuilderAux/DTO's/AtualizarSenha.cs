using Flunt.Notifications;
using Flunt.Validations;

namespace BuilderAux.DTO_s
{
    public class AtualizarSenha : Notifiable<Notification>
    {
        public string email { get; set; }
        public string novaSenha { get; set; }

        public AtualizarSenha()
        {
            var contract = new Contract<AtualizarSenha>()
                .IsNotNullOrEmpty(email, "email", "o campo email não pode esta vazio")
                .IsNotNullOrEmpty(novaSenha, "novaSenha", "o campo não pode esta vazio");
        }
    }
}
