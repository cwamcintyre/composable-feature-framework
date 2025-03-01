using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using GovUk.Frontend.AspNetCore;
using Component.Search.UI.Presenters;
using Component.Search.UI.Service;

namespace Component.Search.UI.Configuration;

public static class SearchFeatureExtensions
{
    public static IMvcBuilder AddSearchFeature(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(SearchFeatureExtensions).Assembly);
        return builder;
    }

    public static IServiceCollection AddSearchFeatureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISearchPresenter, SearchPresenter>();
        services.AddScoped<IDetailPresenter, DetailPresenter>();
        services.AddScoped<SearchAPIService>();
        services.AddGovUkFrontend();
        services.AddHttpClient();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["SearchUI:RedisCache:ConnectionString"];
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
