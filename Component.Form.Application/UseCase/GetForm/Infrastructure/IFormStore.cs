using Component.Form.Model;

namespace Component.Form.Application.UseCase.GetForm.Infrastructure;

public interface IFormStore
{
    Task<FormModel> GetFormAsync(string formId);
    Task SaveFormAsync(string formId, FormModel model);
}
