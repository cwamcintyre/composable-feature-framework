using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Component.Form.Application.UseCase.AddRepeatingSection.Model;
using Component.Core.Application;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Component.Form.API.Triggers;

public class AddRepeatingSection
{
    private readonly IRequestResponseUseCase<AddRepeatingSectionRequestModel, AddRepeatingSectionResponseModel> _addRepeatingSectionUseCase;
    private readonly ILogger<AddRepeatingSection> _logger;

    public AddRepeatingSection(IRequestResponseUseCase<AddRepeatingSectionRequestModel, AddRepeatingSectionResponseModel> addRepeatingSectionUseCase, ILogger<AddRepeatingSection> logger)
    {
        _addRepeatingSectionUseCase = addRepeatingSectionUseCase;
        _logger = logger;
    }

    [Function("AddRepeatingSection")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "addRepeatingSection")] HttpRequest req)
    {
        _logger.LogInformation("Adding repeating section");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var addRepeatingSectionRequest = JsonConvert.DeserializeObject<AddRepeatingSectionRequestModel>(requestBody);

        if (addRepeatingSectionRequest == null)
        {
            _logger.LogError("Invalid request payload");
            return new BadRequestObjectResult("Invalid request payload");
        }

        try
        {
            var response = await _addRepeatingSectionUseCase.HandleAsync(addRepeatingSectionRequest);

            if (!response.Success)
            {
                return new BadRequestObjectResult("Failed to add repeating section");
            }

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the repeating section");
            return new ObjectResult("An internal error occurred") { StatusCode = 500 };
        }
    }
}