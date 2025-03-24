using System.Dynamic;
using Component.Form.Application.Helpers;
using Component.Form.Model.ComponentHandler.DateParts;
using Component.Form.UI.ComponentHandler;
using Component.Form.UI.ComponentHandler.Default;
using Component.Form.UI.PageHandler;
using Component.Form.UI.PageHandler.Default;
using Component.Form.UI.PageHandler.InlineRepeatingPageHandler;
using Component.Form.UI.Presenters;
using Component.Form.UI.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        services.AddScoped<IRepeatingPagePresenter, RepeatingPagePresenter>();
        services.AddGovUkFrontend();
        services.AddHttpClient();

        services.AddSingleton<ComponentHandlerFactory>();
        services.AddSingleton<IComponentHandler, UkAddressHandler>();
        services.AddSingleton<IComponentHandler, DatePartsHandler>();
        services.AddSingleton<IComponentHandler, DefaultHandler>();

        services.AddSingleton<PageHandlerFactory>();
        services.AddSingleton<IPageHandler, DefaultPageHandler>();
        services.AddSingleton<IPageHandler, InlineRepeatingPageHandler>();

        services.AddSingleton(serviceProvider => {
            var allTypes = new HashSet<string>();
            allTypes.Add(DefaultPageHandler.GetSafeType());
            allTypes.Add(InlineRepeatingPageHandler.GetSafeType());

            return new SafeJsonHelper(allTypes.ToHashSet());
        });
        
        if (Convert.ToBoolean(configuration["UseRedisCache"]) == true)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisCache:ConnectionString"];
                options.InstanceName = "SessionInstance";
            });

        }
        else 
        {
            services.AddDistributedMemoryCache();
        }

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}
