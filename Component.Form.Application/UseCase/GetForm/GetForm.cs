using Component.Core.Application;
using Component.Form.Application.Shared.Infrastructure;
using Component.Form.Application.UseCase.GetForm.Model;

namespace Component.Form.Application.UseCase.GetForm;

public class GetForm : IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel>
{
    private readonly IFormStore _formStore;

    public GetForm(IFormStore formStore)
    {
        _formStore = formStore;
    }

    public async Task<GetFormResponseModel> HandleAsync(GetFormRequestModel request)
    {
        var form = await _formStore.GetFormAsync(request.FormId);
        return new GetFormResponseModel
        {
            Form = form
        };
    }
}