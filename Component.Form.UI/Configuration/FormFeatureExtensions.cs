using System;
using Component.Form.UI.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Component.Form.UI.Configuration;

public static class FormFeatureExtensions
{
    public static IMvcBuilder AddFormFeature(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(FormFeatureExtensions).Assembly);
        return builder;
    }

    public static IServiceCollection AddFormFeatureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FormAPIService>();
        services.AddScoped<IFormPresenter, FormPresenter>();
        services.AddGovUkFrontend();
        services.AddHttpClient();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["RedisCache:ConnectionString"];
            options.InstanceName = "SessionInstance";
        });

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}
