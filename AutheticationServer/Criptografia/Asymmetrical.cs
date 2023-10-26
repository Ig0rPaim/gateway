using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;

namespace AutheticationServer.Criptografia
{
    public class Asymmetrical
    {
        using RSA RSA = RSA.Create();

        // Obtém as chaves pública e privada
        RSAParameters publicKey = rsa.ExportParameters(false);
    RSAParameters privateKey = rsa.ExportParameters(true);

    // Criptografa os dados usando a chave pública
    byte[] data = new byte[] { 1, 2, 3, 4, 5 };
    byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);

    // Descriptografa os dados usando a chave privada
    byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

    // Salva as chaves em arquivos separados
    File.WriteAllBytes("public.key", publicKey.Modulus);
        File.WriteAllBytes("private.key", privateKey.D);
    }
}
