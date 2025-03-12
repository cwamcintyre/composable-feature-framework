using Microsoft.Extensions.DependencyInjection;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Infrastructure.Fake;
using Component.Form.Application.UseCase.GetForm.Model;
using Component.Form.Application.UseCase.GetForm;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Application.UseCase.ProcessForm;
using Component.Core.Application;
using Component.Form.Application.UseCase.GetData.Model;
using Component.Form.Application.UseCase.GetData;
using Component.Form.Model.ComponentHandler;
using Component.Form.Application.UseCase.UpdateForm.Model;
using Component.Form.Application.UseCase.UpdateForm;

namespace Component.Form.Application;
public static class FormApplicationServiceBuilder
{
    public static IServiceCollection AddFormApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel>, GetForm>();
        services.AddScoped<IFormStore, FakeFormStore>();

        services.AddScoped<IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel>, ProcessForm>();
        services.AddScoped<IFormDataStore, FakeFormDataStore>();

        services.AddScoped<IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel>, GetData>();

        services.AddScoped<IRequestResponseUseCase<UpdateFormRequestModel, UpdateFormResponseModel>, UpdateForm>();

        services.AddSingleton<ComponentHandlerFactory>();
        services.AddSingleton<IComponentHandler, UkAddressHandler>();
        services.AddSingleton<IComponentHandler, DatePartsHandler>();
        services.AddSingleton<IComponentHandler, EmailHandler>();
        services.AddSingleton<IComponentHandler, PhoneNumberHandler>();

        return services;
    }
}