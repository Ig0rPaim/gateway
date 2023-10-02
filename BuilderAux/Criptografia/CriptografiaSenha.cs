using System.Security.Cryptography;

namespace BuilderAux.Criptografia
{
    public class CriptografiaSenha
    {
        public static byte[] getInByteArray(string input)
        {
            try
            {
                HashAlgorithm sha = new SHA1CryptoServiceProvider();
                char[] charArray = input.ToCharArray();
                byte[] byteArray = new byte[charArray.Length];
                byte[] byteHash;
                string texto = string.Empty;
                for (int i = 0; i < charArray.Length; i++)
                    byteArray[i] = Convert.ToByte(charArray[i]);
                byteHash = sha.ComputeHash(byteArray);

                return byteHash;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
