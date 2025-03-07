using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Component.Form.Application.UseCase.UpdateForm.Model;
using Component.Core.Application;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Component.Form.API.Triggers;

// COPILOT: using ProcessForm as an example, implement a functions app http trigger that uses the UpdateForm use case to update the form
public class UpdateForm
{
    private readonly IRequestResponseUseCase<UpdateFormRequestModel, UpdateFormResponseModel> _updateFormUseCase;
    private readonly ILogger<UpdateForm> _logger;

    public UpdateForm(IRequestResponseUseCase<UpdateFormRequestModel, UpdateFormResponseModel> updateFormUseCase, ILogger<UpdateForm> logger)
    {
        _updateFormUseCase = updateFormUseCase;
        _logger = logger;
    }

    [Function("UpdateForm")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "updateForm")] HttpRequest req)
    {
        _logger.LogInformation("Updating form");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updateFormRequest = JsonConvert.DeserializeObject<UpdateFormRequestModel>(requestBody);

        if (updateFormRequest == null)
        {
            _logger.LogError("Invalid request payload");
            return new BadRequestObjectResult("Invalid request payload");
        }

        var response = await _updateFormUseCase.HandleAsync(updateFormRequest);

        if (!response.Success)
        {
            return new BadRequestObjectResult("Failed to update form");
        }

        return new OkObjectResult(response);
    }
}
