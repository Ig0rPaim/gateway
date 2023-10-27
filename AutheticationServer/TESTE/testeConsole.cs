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
            //using RSA rsa = RSA.Create();
            //string PublicKey = "<RSAKeyValue><Modulus>xh1w19MA0yzCcxyydh2Pu1AKECl4aZskeo98YUo8NF67RTSUmKlI/UL/twysGi2kUnrEeOzkZsF8VJACf50//hzmv+J/Bhs+eKwul15EwnDiZdKRJiQceZLnWg+Id3drYQB9m5gtr5B1ABMw8i7Cto2AOuXoaIcUMvlL14kmeHH9yzZuABCcSTlHDW8MibGBdjX3xTfte3LH3OAS1ZbMUNNKcAhsTNbIXAlNRCU+vyAph2fOxs4w0FI15+yLwhCo80IJdlE1esozcP2PPmR+kJcBMCSwIQnoK9cOokQjGgWJEPqyjrPMa64b2qqz4V4chCLIeTi/sKdp3J6vf35dpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            ////byte[] publicKeyBytes = Convert.FromBase64String(PublicKey);
            ////RSAParameters publicKeyRSA = new RSAParameters { Modulus = publicKeyBytes };
            //rsa.ImportRSAPublicKey(publicKeyBytes, out _);

            //rsa.FromXmlString(PublicKey);

            //return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);

            using RSA rsa = RSA.Create();
            string PublicKey = "<RSAKeyValue><Modulus>xh1w19MA0yzCcxyydh2Pu1AKECl4aZskeo98YUo8NF67RTSUmKlI/UL/twysGi2kUnrEeOzkZsF8VJACf50//hzmv+J/Bhs+eKwul15EwnDiZdKRJiQceZLnWg+Id3drYQB9m5gtr5B1ABMw8i7Cto2AOuXoaIcUMvlL14kmeHH9yzZuABCcSTlHDW8MibGBdjX3xTfte3LH3OAS1ZbMUNNKcAhsTNbIXAlNRCU+vyAph2fOxs4w0FI15+yLwhCo80IJdlE1esozcP2PPmR+kJcBMCSwIQnoK9cOokQjGgWJEPqyjrPMa64b2qqz4V4chCLIeTi/sKdp3J6vf35dpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            // Carrega a chave pública a partir do XML
            rsa.FromXmlString(PublicKey);

            return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);


        }

        public static byte[] Descriptografar(byte[] dadosCriptografados)
        {
            using RSA rsa = RSA.Create();
            string PrivateKey = "<RSAKeyValue><Modulus>xh1w19MA0yzCcxyydh2Pu1AKECl4aZskeo98YUo8NF67RTSUmKlI/UL/twysGi2kUnrEeOzkZsF8VJACf50//hzmv+J/Bhs+eKwul15EwnDiZdKRJiQceZLnWg+Id3drYQB9m5gtr5B1ABMw8i7Cto2AOuXoaIcUMvlL14kmeHH9yzZuABCcSTlHDW8MibGBdjX3xTfte3LH3OAS1ZbMUNNKcAhsTNbIXAlNRCU+vyAph2fOxs4w0FI15+yLwhCo80IJdlE1esozcP2PPmR+kJcBMCSwIQnoK9cOokQjGgWJEPqyjrPMa64b2qqz4V4chCLIeTi/sKdp3J6vf35dpQ==</Modulus><Exponent>AQAB</Exponent><P>zM3YAcU2vwt9AGZTGoFJRevo/tEorSd1ebNGQeuilrI+XSyKWhYImqkWuISyy58vE04bB+kE1NWxlLBlZbM4Iy7HHu0WtsdZM9YQ5zqpdPPnZdWDMT9or9A==</P><Q>zRMn/jSPQz8j/Pk76ZViogwG5xguNmy5vnGCU0ddTQuCTqBpArRHn16ohSBpKXAk5Oaz5PvJq25ft5nZX8zNnW9Z3rtqEAOeMK+X4BJUQHSMmZzDRwEcv5Uqg8WuBCXfuqzjW9Bz44qfJy/L7yt+bg+DJ9tL8UEF7zV9Uyc/U9Hqk=</Q><DP>BlbHdK/Qom5U00YPYXX7jQIAzPCegTDAyydI8gS/fDdKXTRKibT7Y/WJadOY8xHlA/NCdnHHRJ3R7q8wvgy1Yiw4eygsjzAJUTmy3/Np4dGjnlS1djHPm+7vQ==</DP><DQ>vlXjjLQvd0yYpBisGZw8UaR8/VZzVcfuQsEl7CT2UliNrYoqLr0bKvn8sgvV5P1TKf7C6z9GZJlVEFkVroDMZ2aYd2lAtGTRirpN+RcI7pSzrt8tzl/qq+VXjPM7VKTtNgbya/h1lPm+K0YKqE5B4pL/DNBbzeO0T96VUn7Z4pkFy3E=</DQ><InverseQ>q/gyv2wFjV1VlrdcHw9slMAnF2b1h1G/va98k2hddYB8PD5Nz5d3sCxemxfU/F99H7iNRlJGmXH0BB5Ih3Q36keMZImh7blUN1zCT7YJQlG0+Hec2d7TlPbckgpkQy1s/59Eq++mTCYDJKhDWvws7Qw==</InverseQ><D>zN7tdPchJzzp9NsV6L8gCggYmeYgnkZ13tw1MHvEdbxkF5CnIMJ0mGCUpkcugANPP7yZa4LAnqWcXVpCR21pGDBB5aQtMv9W/3Pms9eFYkgVzASdoo4vGcL3W5WvGvUwXRUduaX6T9owXMxcBp2QTGUO28nLEx0WY/3YBKyPnnXy4GxZu7dUb9/BvfbNHE5nM7dKHe3K0k3jRnYIe4agjzgheWJQeUkQEx0UdLX5X/2KFc1iYNy1D9PzL2s/1RA3+4a2TV4GY8RcxHXvW/iLlQoH7uZXZpEGCDJ9AZ/QWSZR5VheVtP4aBsOv7v3wrhJKb6i3svVmKUy4EYX9A==</D></RSAKeyValue>";

            // Carrega a chave privada a partir do XML
            rsa.FromXmlString(PrivateKey);

            return rsa.Decrypt(dadosCriptografados, RSAEncryptionPadding.OaepSHA256);
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
