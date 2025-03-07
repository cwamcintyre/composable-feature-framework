using Newtonsoft.Json;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Model;
using System.Collections.Generic;

namespace Component.Form.Infrastructure.Fake;

public class FakeFormStore : IFormStore
{
    private static Dictionary<string, FormModel> _formStore;

    public FakeFormStore()
    {
        if (_formStore == null) 
        {
            _formStore = new Dictionary<string, FormModel>();
            LoadForms();
        }
    }

    private void LoadForms()
    {        
        var formFiles = Directory.GetFiles("..\\..\\..\\..\\Component.Form.Infrastructure.Fake", "*.json");
        foreach (var file in formFiles)
        {
            var json = File.ReadAllText(file);
            var formModel = JsonConvert.DeserializeObject<FormModel>(json);
            if (formModel != null && formModel.FormId != null)
            {
                var formId = Path.GetFileNameWithoutExtension(file);
                _formStore[formId] = formModel;
            }
        }
    }

    public async Task<FormModel> GetFormAsync(string formId)
    {
        if (_formStore.TryGetValue(formId, out var formModel))
        {
            return formModel;
        }
        return null;
    }

    public async Task SaveFormAsync(string formId, FormModel formModel)
    {
        _formStore[formId] = formModel;
    }
}
