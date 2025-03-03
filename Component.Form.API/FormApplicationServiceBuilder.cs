using Microsoft.Extensions.DependencyInjection;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Infrastructure.Fake;
using Component.Form.Application.UseCase.GetForm.Model;
using Component.Form.Application.UseCase.GetForm;
using Component.Form.Application.UseCase.ProcessForm.Infrastructure;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Component.Form.Application.UseCase.ProcessForm;
using Component.Core.Application;
using Component.Form.Application.UseCase.GetData.Model;
using Component.Form.Application.UseCase.GetData;

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

        return services;
    }
}