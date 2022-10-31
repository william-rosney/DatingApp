using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); //Permet de logger les erreurs dans le terminal
                context.Response.ContentType = "application/json"; //Set le content type de la réponse que l'on renvoie suite à la requete HTTP
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //Set le StatusCode

                //Test si l'environnement est en mode developpement ou pas
                ApiException response = _env.IsDevelopment() 
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) // Si oui on détail bien le contenu de notre response
                : new ApiException(context.Response.StatusCode, "Internal Servor Error"); // Si non on précise juste Internal Servor Error

                //On va convertir notre response en JSON
                //On commence par set les options pour respecter le camelCase 
                var options = new JsonSerializerOptions{
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                };

                //Serialize notre response en JSON
                var json = JsonSerializer.Serialize(response, options);

                //On envoie le response
                await context.Response.WriteAsync(json);

            }
        }
    }
}