using BuilderAux.Repository.Roles;
using BuilderAux.Repository.Usuarios;
using BuilderAux.SevicesGateWay.AutheticationServer;
using BuilderAux.SevicesGateWay.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        // Obriga uso do HTTPs
        x.RequireHttpsMetadata = false;

        // Salva os dados de login no AuthenticationProperties
        x.SaveToken = true;

        // Configurações para leitura do Token
        x.TokenValidationParameters = new TokenValidationParameters
        {
            // Chave que usamos para gerar o Token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder
                    .Configuration.GetSection("Keys")
                    .GetSection("TokenKey")
                    .Value ?? string.Empty)),
            // Validações externas
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<IAuthenticationServer, AuthenticationServer>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:AuthAPI"] ?? throw new ArgumentNullException("Ops! problemas com a rota!"))
    );



//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddControllers(); // para usar o http context
builder.Services.AddHttpContextAccessor(); // para usar o http context

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
