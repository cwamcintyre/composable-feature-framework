using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Model;

namespace Component.Form.Infrastructure.Fake
{
    public class FakeFormStore : IFormStore
    {
        private readonly Dictionary<string, FormModel> _store = new();

        public Task<FormModel> GetFormAsync(string formId)
        {
            _store.TryGetValue(formId, out var formModel);
            return Task.FromResult(formModel);
        }

        public Task SaveFormAsync(string formId, FormModel model)
        {
            _store[formId] = model;
            return Task.CompletedTask;
        }
    }
}
