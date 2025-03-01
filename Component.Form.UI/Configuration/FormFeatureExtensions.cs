using System;
using Component.Form.UI.Services;
using GovUk.Frontend.AspNetCore;

namespace Component.Form.UI.Configuration;

public static class FormFeatureExtensions
{
    public static IMvcBuilder AddFormFeature(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(FormFeatureExtensions).Assembly);
        return builder;
    }

    public static IServiceCollection AddFormFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<FormAPIService>();
        services.AddScoped<IFormPresenter, FormPresenter>();
        services.AddGovUkFrontend();
        services.AddHttpClient();
        return services;
    }
}
