using System;
using System.Threading.Tasks;
using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Application.UseCase.UpdateForm.Model;
using Component.Core.Application;
using Component.Form.Model;

namespace Component.Form.Application.UseCase.UpdateForm;

// COPILOT: using ProcessForm as an example, implement UpdateForm which takes a FormModel and uses IFormStore to update the form
// use the IRequestResponse interface and generate UpdateForm request and response models
public class UpdateForm : IRequestResponseUseCase<UpdateFormRequestModel, UpdateFormResponseModel>
{
    private readonly IFormStore _formStore;

    public UpdateForm(IFormStore formStore)
    {
        _formStore = formStore;
    }

    public async Task<UpdateFormResponseModel> HandleAsync(UpdateFormRequestModel request)
    {
        if (request.Form == null)
        {
            throw new ArgumentNullException(nameof(request.Form));
        }

        await _formStore.SaveFormAsync(request.Form.FormId, request.Form);

        return new UpdateFormResponseModel { Success = true };
    }
}
