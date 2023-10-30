using System.Security.Cryptography;

namespace AutheticationServer.Criptografia
{
    public static class GenerateKeys
    {
        private static string gerarPublic()
        {
            using RSA rsa = RSA.Create();
            string publicKeyString = rsa.ToXmlString(false);
            return publicKeyString;
        }
        private static string gerarPrivate()
        {
            using RSA rsa = RSA.Create();
            string privateKeyString = rsa.ToXmlString(true);
            return privateKeyString;
        }

        public static Dictionary<string, string> GetKeys()
        {
            using RSA rsa = RSA.Create();
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
