namespace FrontBuilderAux.Models
{
    public class Usuarios
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? Senha { get; set; }


        public Usuarios(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public Usuarios(string name, string email, string role, string senha)
        {
            Name = name;
            Email = email;
            Role = role;
            Senha = senha;
        }


    }

}
