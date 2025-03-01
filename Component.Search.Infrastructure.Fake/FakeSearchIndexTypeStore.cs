using Newtonsoft.Json;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Infrastructure;
using Component.Search.Model;

namespace Component.Search.Infrastructure.Fake;

public class FakeSearchIndexTypeStore : ISearchIndexTypeStore
{
    public async Task<SearchIndexTypeModel> GetSearchIndexTypeAsync()
    {
        var json = await File.ReadAllTextAsync("searchIndexTypeTest.json");
        return JsonConvert.DeserializeObject<SearchIndexTypeModel>(json);
    }
}
