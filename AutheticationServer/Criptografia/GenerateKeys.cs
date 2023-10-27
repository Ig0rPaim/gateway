using System.Security.Cryptography;

namespace AutheticationServer.Criptografia
{
    public static class GenerateKeys
    {
        private static RSAParameters publicKey;
        private static RSAParameters privateKey;

        public static string gerarPublic()
        {
            using RSA rsa = RSA.Create();
            string Public = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            return Public;
        }
        public static string gerarPrivate()
        {
            using RSA rsa = RSA.Create();
            string Private = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            return Private;
        }

        public static Dictionary<string, string> GetKeys()
        {
            using RSA rsa = RSA.Create();

            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);
            string Private = gerarPrivate(); 
            string Public = gerarPublic(); 
            return new Dictionary<string, string> { 
                {"PrivateKey", Private },
                { "PublicKey", Public }
            };
        }

        public static void PrintKeys()
        {
            Dictionary<string,string> keys = GetKeys();
            Console.WriteLine(keys["PublicKey"]);
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(keys["PrivateKey"]);
        }

        public static void SaveKeys()
        {
            Dictionary<string, string> keys = GetKeys();
            string diretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoPublic = Path.Combine(diretorio, "PublicKey.txt");
            string caminhoPrivate = Path.Combine(diretorio, "PrivateKey.txt");
            string conteudoPublic = keys["PublicKey"];
            string conteudoPrivate = keys["PrivatecKey"];
            File.WriteAllText(caminhoPublic, conteudoPublic);
            File.WriteAllText(caminhoPrivate, conteudoPrivate);

        }
    }
}
