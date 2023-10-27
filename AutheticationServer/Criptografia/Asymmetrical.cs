using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutheticationServer.Criptografia
{
    public static class Asymmetrical
    {
        private static RSAParameters publicKey;
        private static RSAParameters privateKey;
        private static readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();

        //public Asymmetrical()
        //{
        //    using RSA rsa = RSA.Create();

        //    publicKey = rsa.ExportParameters(false);
        //    privateKey = rsa.ExportParameters(true);
        //}

        public static byte[] Criptografar(byte[] dados)
        {
            using RSA rsa = RSA.Create();
            string PublicKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PublicKey")
                    .Value ?? throw new ArgumentNullException();
            byte[] publicKeyBytes = Convert.FromBase64String(PublicKey);
            //RSAParameters publicKeyRSA = new RSAParameters { Modulus = publicKeyBytes };
            rsa.ImportRSAPublicKey(publicKeyBytes, out _);

            RSAParameters publicKeyParams = rsa.ExportParameters(true);
            RSAParameters publicKeyRSA = rsa.ExportParameters(false);

            rsa.ImportParameters(publicKeyParams);
            return rsa.Encrypt(dados, RSAEncryptionPadding.OaepSHA256);
        }

        public static byte[] Descriptografar(byte[] dadosCriptografados)
        {
            //using RSA rsa = RSA.Create();
            string privateKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PrivateKey")
                    .Value ?? throw new ArgumentNullException();
            //byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            //rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            //RSAParameters privateKeyParams = rsa.ExportParameters(true);

            ////RSAParameters privateKeyRSA = rsa.ExportParameters(false);

            //rsa.ImportParameters(privateKeyParams);
            //return rsa.Decrypt(dadosCriptografados, RSAEncryptionPadding.OaepSHA256);


            using RSA rsa = RSA.Create();
            //string PrivateKey = "<RSAKeyValue><Modulus>xh1w19MA0yzCcxyydh2Pu1AKECl4aZskeo98YUo8NF67RTSUmKlI/UL/twysGi2kUnrEeOzkZsF8VJACf50//hzmv+J/Bhs+eKwul15EwnDiZdKRJiQceZLnWg+Id3drYQB9m5gtr5B1ABMw8i7Cto2AOuXoaIcUMvlL14kmeHH9yzZuABCcSTlHDW8MibGBdjX3xTfte3LH3OAS1ZbMUNNKcAhsTNbIXAlNRCU+vyAph2fOxs4w0FI15+yLwhCo80IJdlE1esozcP2PPmR+kJcBMCSwIQnoK9cOokQjGgWJEPqyjrPMa64b2qqz4V4chCLIeTi/sKdp3J6vf35dpQ==</Modulus><Exponent>AQAB</Exponent><P>92XRvCmLEzcTv1JwTDOF4Uwl5nJR8PIK5CDhUSRTD2x40Jb7RfvSPLngKRhBkj36cnslxJFpCkxnA6TWhbj0jjH0bR5e6rym32B3nSgkKoNb+B0XNezGEu8PHUewMJ0w1TgloK7XVk8KcvDZjYj7doAncS14B64PNV5Pcdwj/c8=</P><Q>zQDv9Exzik3fF8+2tP6TljYsWqAgdoObxXwQwVwZJ91ID86X0PotM2G58ZYzhg+GhXnlsirzf4QhIV3OuJ3e3DCD9JiBp96DFWzjcg6XAy6tMOywsj6nJj1to7AU8HRb0c2Y9s4RpBHfFX7HLSfmYMI5MiCGoAbYmTmE1LNvXks=</Q><DP>RVtZKjwnTjLBqYBuFR7YqKnVcj6YdEkW/o+tOzdIxekuEaRSO3N2pCmkC0FOmLYhcVjO5MWzQfzbYG0k6wZIIVkl4jWCfKJ40eTUg82OHhEHho55RFfly8cTYK0JWwemehAslFiP7BVqo4CcLQ67GKSi6hrKLLUCdH7FHPGPcYc=</DP><DQ>EPal5HI7EVO2RTMj7EAwmDptWyZq6UOkrSfB/y4OytSttNkkiN9axCivCUWjh9FbEUpv7llNgty9HRS1GWO+4DuHf8Lq36gy8p/rBjIc3t+K50R+rpEupaRpoSTd5rPKMUcxBCMzJH9yFUZDVwI5NwJ7VyGH4lAOtkcjLbKXYZc=</DQ><InverseQ>7/yzwt4zdDb61zjSc+kItyemJgrXmmB2NgSZmPBceSvl0Jpzug5NKkod/KnUKAP2P5xuKRcHix726ATkBUN09awAvl4GS3H+7GU+ZEjxirBLZHLF5WvZxlRS7yUv+u7Igxky/3P/DXsvEg805Knoj4rLcWPd6Xdw2bKgej0WRyI=</InverseQ><D>iAbS4f1IxbYn1O3RQvRNKels32f5+1jAlT5Lwmk8KSEaKAxQMvmY2fvUpBSZXmgUOWV93SGaOUuEWHOAo6qOs6WYcgvL959hJ23hl6DsSS1E5XDMpImK25aYLugXZR+jpLi3hcvZIoBnNSP4KXEd1EYX01jkWFDW96DgCB9tQKidX3IgadqtOIq8canHz97ehozFdr3UpcpewrphlBhmtrJYCNJDRBsbxzISY2DNJJqPaNTqxlD+vwY4z5RDfexRNwyelv15d14JZEVA7wG8OvLdhImT6foo1b9lMDmAZdzKJ7ifRXoi+a27InV4mWfaDaqF5sY+jWYj7/miT4jmSQ==</D></RSAKeyValue>";

            // Carrega a chave privada a partir do XML
            rsa.FromXmlString(privateKey);

            return rsa.Decrypt(dadosCriptografados, RSAEncryptionPadding.OaepSHA256);
        }


        public static bool teste()
        {
            string token = "123456";
            byte[] data = Encoding.ASCII.GetBytes(token);
            using RSA rsa = RSA.Create();
            string PublicKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PublicKey")
                    .Value ?? throw new ArgumentNullException();
            byte[] publicKeyBytes = Convert.FromBase64String(PublicKey);
            //RSAParameters publicKeyRSA = new RSAParameters { Modulus = publicKeyBytes };
            rsa.ImportRSAPublicKey(publicKeyBytes, out _);


            var encrip  = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);

            //using RSA rsa = RSA.Create();
            string privateKey = builder
                    .Configuration.GetSection("Keys")
                    .GetSection("PrivateKey")
                    .Value ?? throw new ArgumentNullException();
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            RSAParameters privateKeyParams = rsa.ExportParameters(true);

            RSAParameters privateKeyRSA = rsa.ExportParameters(false);

            rsa.ImportParameters(privateKeyParams);
            var descrip = rsa.Decrypt(encrip, RSAEncryptionPadding.OaepSHA256);
            if(data == descrip) return true;
            else return false;
        }
    }


}

