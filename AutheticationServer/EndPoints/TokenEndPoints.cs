using AutheticationServer.Filters;
using AutheticationServer.Models;
using AutheticationServer.Services;
using AutheticationServer.Services.HttpContext;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutheticationServer.EndPoints
{
    public static class TokenEndPoints
    {
        public static void MapTokenEndPoints(this WebApplication app)
        {
            app.MapPost("token/gerar", Create).AddEndpointFilter<ValidationFilter<UsuarioModel>>();
        }

        public static async Task<IResult> Create(UsuarioModel user, IValidator<UsuarioModel> validator, HttpResponse response)
        {
            //usar o properties para o obj anonimo
            try
            {
                var tokenServices = new TokenServices();
                var token = await tokenServices.CreateToken(user);
                //string Token  = token.Get
                response.Headers.Add("TokenAuth", token.);
                return Results.Ok();
            }

            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }

        public static IResult ValidateToken(UsuarioModel user, string Token)
        {
            try
            {
                //var tokenServices = new TokenServices();
                //var 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
