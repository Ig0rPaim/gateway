using System.Security.Cryptography;

namespace BuilderAux.Criptografia
{
    public class CriptografiaAssimetrica
    {
        public static readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();
        public static byte[] Criptografar(byte[] dados)
        {
            try
            {
                using RSA rsa = RSA.Create();
                string PublicKey = builder
                        .Configuration.GetSection("Keys")
                        .GetSection("PublicKey")
                        .Value ?? throw new ArgumentNullException();
                rsa.FromXmlString(PublicKey);
                return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);
            }
            catch (Exception er)
            {

                throw new Exception(er.Message);
            }
        }
    }
}
