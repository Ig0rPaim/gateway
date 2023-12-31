﻿using Flunt.Validations;
using Flunt.Notifications;
using System.Numerics;

namespace BuilderAux.Models
{
    public class Usuarios : Notifiable<Notification>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public byte[] Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Role { get; set; }

       
        public Usuarios(Guid id, string nome, byte[] password, string email, DateTime dataCadastro, DateTime? dataAtualizacao, string role, string telefone)
        {
            var contract = new Contract<Usuarios>()
                .IsNotNull(id, "Id", "Campo Id vazio")
                .IsNotNullOrEmpty(nome, "Nome", "Campo nome vazio")
                .IsNotNull(password, "Password", "Campo senha vazio")
                .IsNotNullOrEmpty(role, "Role", "Campo Cargo vazio")
                .IsNotNullOrEmpty(telefone, "Telefone", "Campo telefone vazio")
                .IsEmailOrEmpty(email, "Email", "Campo email vazio ou invalido");
            AddNotifications(contract);
            
            Id = id;
            Name = nome;
            Senha = password;
            Email = Criptografia
                .CriptrografiaAndDescriptografia
                 .Criptografar(
                    email
                 );
            Telefone = telefone;
            DataCadastro = dataCadastro;
            DataAtualizacao = dataAtualizacao;
            Role = role;
        }
    }
}
