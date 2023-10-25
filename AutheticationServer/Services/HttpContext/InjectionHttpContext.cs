namespace AutheticationServer.Services.HttpContext
{
    public class InjectionHttpContext
    {
        public IHttpContextAccessor ContextAccessor { get; }

        public InjectionHttpContext(IHttpContextAccessor contextAccessor) =>
            ContextAccessor = contextAccessor;
        
    }
}
