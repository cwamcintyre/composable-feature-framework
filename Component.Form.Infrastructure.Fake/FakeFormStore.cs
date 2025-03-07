using Newtonsoft.Json;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Model;

namespace Component.Form.Infrastructure.Fake;

// COPILOT DEMO
// refactor FakeFormStore so that it loads all available form json in the constructor and puts the loaded files in a dictionary. 
// GetFormAsync will then return FormModel from the dictionary and SaveFormAsync will update the dictionary.
public class FakeFormStore : IFormStore
{
    public async Task<FormModel> GetFormAsync(string formId)
    {
        var json = File.ReadAllText($"{formId}.json");
        var formModel = JsonConvert.DeserializeObject<FormModel>(json);

        return formModel;
    }

    public async Task SaveFormAsync(string formId, FormModel formModel)
    {
        var json = JsonConvert.SerializeObject(formModel, Formatting.Indented);
        await File.WriteAllTextAsync($"{formId}.json", json);
    }
}
