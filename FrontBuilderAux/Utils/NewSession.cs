


namespace FrontBuilderAux.Utils
{
    public static class NewSession
    {
        private static readonly HttpContext _httpContext;

        private static void Salvar(this HttpContext _httpContext)
        {
            try
            {
                _httpContext.Session.SetString("DataUser", "qualquer coisa");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
