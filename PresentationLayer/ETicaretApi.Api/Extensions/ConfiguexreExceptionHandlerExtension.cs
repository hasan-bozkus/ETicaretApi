﻿using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace ETicaretApi.Api.Extensions
{
    static public class ConfiguexreExceptionHandlerExtension
    {

        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if(contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.Message);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            statusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Title = "Hata Alındı."
                        }));
                    }
                });
            });
        }
    }
}
