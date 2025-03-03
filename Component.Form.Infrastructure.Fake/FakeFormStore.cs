using Newtonsoft.Json;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Model;

namespace Component.Form.Infrastructure.Fake;

public class FakeFormStore : IFormStore
{
    public async Task<FormModel> GetFormAsync(string formId)
    {
        var json = File.ReadAllText($"{formId}.json");
        var formModel = JsonConvert.DeserializeObject<FormModel>(json);

        return formModel;
    }
}
