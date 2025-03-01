using Microsoft.Extensions.DependencyInjection;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Infrastructure.Fake;
using Component.Form.Application.UseCase.GetForm.Model;
using Component.Form.Application.UseCase.GetForm;
using Component.Core.Application;

namespace Component.Form.Application;
public static class FormApplicationServiceBuilder
{
    public static IServiceCollection AddFormApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel>, GetForm>();
        services.AddScoped<IFormStore, FakeFormStore>();

        return services;
    }
}