using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Model;

namespace Component.Form.Infrastructure.Fake
{
    public class FakeFormDataStore : IFormDataStore
    {
        private readonly Dictionary<string, FormData> _store = new();

        public async Task<FormData> GetFormDataAsync(string applicantId)
        {
            _store.TryGetValue(applicantId, out var formData);
            return formData;
        }

        public async Task SaveFormDataAsync(string formId, string applicantId, string formData, Stack<string> formRoute)
        {
            _store[applicantId] = new FormData
            {
                Data = formData,
                Route = formRoute
            };
        }
    }
}
