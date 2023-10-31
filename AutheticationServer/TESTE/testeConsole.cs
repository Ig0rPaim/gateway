namespace AutheticationServer.TESTE
{
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    internal class Program
    {
        private static RSAParameters publicKey;
        private static RSAParameters privateKey;
        //private static readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();

        //private Program()
        //{
        //    using RSA rsa = RSA.Create();

        //    // Obtém as chaves pública e privada
        //    publicKey = rsa.ExportParameters(false);
        //    privateKey = rsa.ExportParameters(true);
        //}   

        public static string gerarPublic()
        {
            using RSA rsa = RSA.Create();
            string publicKeyString = Convert.ToBase64String(rsa.ExportRSAPublicKey());

            return publicKeyString;
        }
        public static string gerarPrivate()
        {
            using RSA rsa = RSA.Create();
            string privateKeyString = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            return privateKeyString;
        }

        public static byte[] Criptografar(byte[] dados)
        {

            using RSA rsa = RSA.Create();
            string PublicKey = "<RSAKeyValue><Modulus>xh1w19MA0yzCcxyydh2Pu1AKECl4aZskeo98YUo8NF67RTSUmKlI/UL/twysGi2kUnrEeOzkZsF8VJACf50//hzmv+J/Bhs+eKwul15EwnDiZdKRJiQceZLnWg+Id3drYQB9m5gtr5B1ABMw8i7Cto2AOuXoaIcUMvlL14kmeHH9yzZuABCcSTlHDW8MibGBdjX3xTfte3LH3OAS1ZbMUNNKcAhsTNbIXAlNRCU+vyAph2fOxs4w0FI15+yLwhCo80IJdlE1esozcP2PPmR+kJcBMCSwIQnoK9cOokQjGgWJEPqyjrPMa64b2qqz4V4chCLIeTi/sKdp3J6vf35dpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            // Carrega a chave pública a partir do XML
            rsa.FromXmlString(PublicKey);

            return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);


        }

     

        private static void Main(string[] args)
        {
            //using RSA rsa = RSA.Create();

            //// Obtém as chaves pública e privada
            //publicKey = rsa.ExportParameters(false);
            //privateKey = rsa.ExportParameters(true);
            ////string Public = Convert.ToBase64String(publicKey.Modulus);
            //Console.WriteLine(gerarPublic());
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine(gerarPrivate());
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine("------------------------------------------------");
            string token = "BUILDEAUXGATEWAY";
            byte[] data = Convert.FromBase64String(token);
            Console.WriteLine($"{Convert.ToBase64String(Criptografar(data))}");
            //"h2m/p6Ca4x9UWru3ug8vDXoAcpaWm8D/Hzvc3YTq3KV5j1DC/jJE8TK+OQ2suWSkzEPVhuGBGBX62SvYSgn5HHj6ChueCvLJpzYylUSpjB2bRMj6clpKAC3XOVuvcTBU/W5+Ej/Z4eJ+7I/DCE0d1Iw01jML59FN6++4CxXyuymcTq4XQoTjW0J7kuIlUiwm7RuDhScNjKUIWjcOHPeCI8NKhA2kwmOTVfvrkKlOGiVXw5U1J/iWS7KX560lhdOuGFvhvZPPN9xlikravMbY5DEDrr3SyIgAIiD/750gd7vspT98pUpykMqb3A+tnIH069uhrhjmKzP5zSaMJlNoTw=="

            //Gera um novo par de chaves RSA com 2048 bits de tamanho.
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            //{
            //    // Exporta os parâmetros da chave privada para XML, incluindo D, DP, DQ, InverseQ, Modulus, P e Q.
            //    string privateKeyXml = rsa.ToXmlString(true);

            //    // Exporta os parâmetros da chave pública para XML.
            //    string publicKeyXml = rsa.ToXmlString(false);

            //    // Agora você pode armazenar essas strings em algum lugar seguro para uso posterior.
            //    Console.WriteLine("Chave Privada (XML):");
            //    Console.WriteLine(privateKeyXml);
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("------------------------------------------------");
            //    Console.WriteLine("\nChave Pública (XML):");
            //    Console.WriteLine(publicKeyXml);
        }
    }

}
