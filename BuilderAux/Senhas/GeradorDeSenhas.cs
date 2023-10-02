namespace BuilderAux.Senhas
{
    public class GeradorDeSenhas
    {
        public static string GerarSenha()
        {
            string carac = "abcdefhijkmnopqrstuvxwyz123456789!@#$%&*()_-+=ABCDEFJCLMNOPQRSTUVXWYZ";
            char[] letras = carac.ToCharArray();
            Embaralhar(ref letras, 5);
            string senha = new String(letras).Substring(0, 8);
            return senha;
        }

        static void Embaralhar(ref char[] array, int vezes)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 1; i <= vezes; i++)
            {
                for (int x = 1; x <= array.Length; x++)
                {
                    Trocar(ref array[rand.Next(0, array.Length)],
                      ref array[rand.Next(0, array.Length)]);
                }
            }
        }

        static void Trocar(ref char arg1, ref char arg2)
        {
            char strTemp = arg1;
            arg1 = arg2;
            arg2 = strTemp;
        }
    }
}
