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
using Component.Form.Application.UseCase.GetDataForPage.Model;
using Component.Form.Application.ComponentHandler;
using Component.Form.Application.ComponentHandler.DateParts;
using Component.Form.Application.ComponentHandler.Email;
using Component.Form.Application.ComponentHandler.PhoneNumber;
using Component.Form.Application.PageHandler;
using Component.Form.Application.PageHandler.Default;
using Component.Form.Application.ComponentHandler.Default;
using Component.Form.Application.PageHandler.InlineRepeating;
using Component.Form.Application.Helpers;
using Component.Form.Application.UseCase.ProcessChangeInForm;
using Component.Form.Application.UseCase.RemoveRepeatingSection;
using Component.Form.Application.UseCase.RemoveRepeatingSection.Model;
using Component.Form.Application.UseCase.AddRepeatingSection;
using Component.Form.Application.UseCase.AddRepeatingSection.Model;

namespace Component.Form.Application;
public static class FormApplicationServiceBuilder
{
    public static IServiceCollection AddFormApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel>, GetForm>();
        services.AddScoped<IFormStore, FakeFormStore>();

        services.AddScoped<IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel>, ProcessForm>();
        services.AddScoped<IFormDataStore, FakeFormDataStore>();

        services.AddScoped<IRequestResponseUseCase<ProcessChangeInFormRequestModel, ProcessChangeInFormResponseModel>, ProcessChangeInForm>();

        services.AddScoped<IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel>, GetData>();

        services.AddScoped<IRequestResponseUseCase<UpdateFormRequestModel, UpdateFormResponseModel>, UpdateForm>();

        services.AddScoped<IRequestResponseUseCase<GetDataForPageRequestModel, GetDataForPageResponseModel>, GetDataForPage>();

        services.AddScoped<IRequestResponseUseCase<AddRepeatingSectionRequestModel, AddRepeatingSectionResponseModel>, AddRepeatingSection>();

        services.AddScoped<IRequestResponseUseCase<RemoveRepeatingSectionRequestModel, RemoveRepeatingSectionResponseModel>, RemoveRepeatingSection>();

        services.AddSingleton<IComponentHandlerFactory, ComponentHandlerFactory>();
        services.AddSingleton<IComponentHandler, UkAddressHandler>();
        services.AddSingleton<IComponentHandler, DatePartsHandler>();
        services.AddSingleton<IComponentHandler, EmailHandler>();
        services.AddSingleton<IComponentHandler, PhoneNumberHandler>();
        services.AddSingleton<IComponentHandler, DefaultHandler>();

        services.AddSingleton<IPageHandlerFactory, PageHandlerFactory>();
        services.AddSingleton<IPageHandler, DefaultPageHandler>();
        services.AddSingleton<IPageHandler, InlineRepeatingPageHandler>();

        services.AddSingleton(serviceProvider => {
            var componentHandlerFactory = serviceProvider.GetService<IComponentHandlerFactory>();
            var allTypes = componentHandlerFactory.GetAllTypes();

            // will have to do the page handlers manually as they have SafeJsonHelper injected in them...
            allTypes.Add(DefaultPageHandler.GetSafeType());
            allTypes.Add(InlineRepeatingPageHandler.GetSafeType());

            return new SafeJsonHelper(allTypes.ToHashSet());
        });

        return services;
    }
}