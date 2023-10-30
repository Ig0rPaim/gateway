using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutheticationServer.Criptografia
{
    public static class Asymmetrical
    {
        private static readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();

        public static byte[] Criptografar(byte[] dados)
        {
            string PublicKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PublicKey")
                    .Value ?? throw new ArgumentNullException();
            using RSA rsa = RSA.Create();
            rsa.FromXmlString(PublicKey);
            return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);
        }

        public static byte[] Descriptografar(byte[] dadosCriptografados)
        {
            string privateKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PrivateKey")
                    .Value ?? throw new ArgumentNullException();
            using RSA rsa = RSA.Create();
            rsa.FromXmlString(privateKey);
            return rsa.Decrypt(dadosCriptografados, RSAEncryptionPadding.OaepSHA256);
        }
    }
}

