using AutheticationServer.Filters;
using AutheticationServer.Models;
using AutheticationServer.Services;
using AutheticationServer.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AutheticationServer.Criptografia;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<UsuarioModel>, UsuariosValidator>();
builder.Services.AddScoped<ITokenServices, TokenServices>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

app.MapPost("token/gerar", async (HttpContext context,
    [FromHeader (Name = "Code")] string Code,
    [FromBody] UsuarioModel user) =>
{
	try
	{
        ITokenServices tokenServices = new TokenServices();
        HttpResponse response = context.Response;
        var token = await tokenServices.CreateToken(user, Code);
        if (token.authenticated)
        {
            string nameSession = user.Email;
            string dataUser = $"{user.Role};{user.Aplication};{user.Expires}";
            context.Session.SetString(nameSession, dataUser);

            #region contruindo response
            response.Headers.Add("authenticated", token.authenticated.ToString());
            response.Headers.Add("TokenAuth", token.accessToken);
            response.Headers.Add("created", token.created);
            response.Headers.Add("expiration", token.expiration);
            response.StatusCode = 201;
            response.ContentType = "text/plain";
            await response.WriteAsync("Token Criado com sucesso");
            #endregion
            
        }
        else
        {
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.WriteAsync(token.message);
        }
    }
	catch (Exception er)
	{
        throw new Exception(er.Message);
	}

})
    .AddEndpointFilter<ValidationFilter<UsuarioModel>>();


app.MapPost("token/validar", async (HttpContext context,
    [FromHeader(Name = "TokenAuth")] string token,
    [FromHeader(Name = "Code")] string Code,
    [FromBody] UsuarioModel user) =>
{
    try
    {
        ITokenServices tokenServices = new TokenServices();
        var retorno = await tokenServices.ValidateToken(token, user, Code);
        HttpResponse response = context.Response;

        if (retorno.authenticated)
        {
            #region contruindo response
            response.Headers.Add("authenticated", retorno.authenticated.ToString());
            response.Headers.Add("lifetime", retorno.lifetime);
            response.Headers.Add("accessToken", retorno.accessToken);
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            await response.WriteAsync("Token validado");
            #endregion
        }
        else if (retorno.message == "Token informado não pode ser utilizado para a requisição")
        {
            #region contruindo response
            response.Headers.Add("authenticated", retorno.authenticated.ToString());
            response.StatusCode = 401;
            response.ContentType = "text/plain";
            await response.WriteAsync(retorno.message);
            #endregion
        }
        else if (retorno.message == "Token expirado e não pode ser utilizado")
        {
            #region construindo response
            response.Headers.Add("authenticated", retorno.authenticated.ToString());
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.WriteAsync(retorno.message);
            #endregion
        }
        else
        {
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.WriteAsync("O token não pode ser validado");
        }

    }
    catch (Exception er)
    {
        throw new Exception(er.Message);
    }
})
    .AddEndpointFilter<ValidationFilter<UsuarioModel>>();

app.UseHttpsRedirection();

app.Run();
