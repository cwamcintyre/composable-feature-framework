using Microsoft.Extensions.DependencyInjection;
using Component.Core.Application;
using Component.Search.Application.UseCase.GetDataItem.Infrastructure;
using Component.Search.Application.UseCase.GetDataItem.Model;
using Component.Search.Application.UseCase.GetDataItem.Service;
using Component.Search.Application.UseCase.GetDataItem;
using Component.Search.Application.UseCase.GetData.infrastructure;
using Component.Search.Application.UseCase.GetData.Service;
using Component.Search.Application.UseCase.GetData.Model;
using Component.Search.Application.UseCase.GetData;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Infrastructure;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Model;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration;

using Component.Search.Infrastructure.Fake;

namespace Component.Search.API;

public static class SearchApplicationServiceBuilder
{
    public static IServiceCollection AddSearchApplicationServices(this IServiceCollection services, SearchApplicationServiceOptions options)
    {
        if (options.UseFakes) 
        {
            services.AddScoped<IDetailStore, FakeDetailStore>();
            services.AddScoped<IDetailTypeStore, FakeDetailTypeStore>();
            services.AddScoped<ISearchDataStore, FakeSearchDataStore>();
            services.AddScoped<ISearchTypeStore, FakeSearchTypeStore>();
            services.AddScoped<ISearchIndexTypeStore, FakeSearchIndexTypeStore>();
        }
        else 
        {

        }

        services.AddScoped<DetailService>();
        services.AddScoped<IRequestResponseUseCase<GetDataItemRequestModel, GetDataItemResponseModel>, GetDataItem>();

        services.AddScoped<SearchService>();
        services.AddScoped<IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel>, GetData>();

        services.AddScoped<IRequestResponseUseCase<GetSearchIndexRequestModel, GetSearchIndexResponseModel>, GetSearchIndexConfiguration>();

        return services;
    }
}
