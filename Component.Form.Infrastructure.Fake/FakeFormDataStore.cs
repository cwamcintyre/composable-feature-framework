using System;
using Component.Form.Application.UseCase.ProcessForm.Infrastructure;
using Component.Form.Model;

namespace Component.Form.Infrastructure.Fake;

public class FakeFormDataStore : IFormDataStore
{
    private static Dictionary<string, Dictionary<string, string>> _formData = new Dictionary<string, Dictionary<string, string>>();
    private static Dictionary<string, Stack<string>> _routeData = new Dictionary<string, Stack<string>>();

    public async Task<FormData> GetFormDataAsync(string applicantId)
    {
        if (!_formData.ContainsKey(applicantId))
        {
            return new FormData()
            {
                Data = new Dictionary<string, string>(),
                Route = new Stack<string>()
            };
        }

        return new FormData() 
        {
            Data = _formData[applicantId],
            Route = _routeData[applicantId]
        };
    }

    public async Task SaveFormDataAsync(string formId, string applicantId, Dictionary<string, string> formData, Stack<string> routeData)
    {
        _formData[applicantId] = formData;
        _routeData[applicantId] = routeData;
    }
}
