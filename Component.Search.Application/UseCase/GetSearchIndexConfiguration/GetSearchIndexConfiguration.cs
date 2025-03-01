using System;
using Component.Core.Application;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Infrastructure;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Model;

namespace Component.Search.Application.UseCase.GetSearchIndexConfiguration;

public class GetSearchIndexConfiguration : IResponseUseCase<GetSearchIndexResponseModel>
{
    private readonly ISearchIndexTypeStore _searchIndexTypeStore;

    public GetSearchIndexConfiguration(ISearchIndexTypeStore searchIndexTypeStore)
    {
        _searchIndexTypeStore = searchIndexTypeStore;
    }

    public async Task<GetSearchIndexResponseModel> HandleAsync()
    {
        var searchIndexType = await _searchIndexTypeStore.GetSearchIndexTypeAsync();
        return new GetSearchIndexResponseModel
        {
            SearchIndexType = searchIndexType
        };
    }
}
