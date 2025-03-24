using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Component.Form.Application.UseCase.RemoveRepeatingSection.Model;
using Component.Core.Application;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Component.Form.API.Triggers;

public class RemoveRepeatingSection
{
    private readonly IRequestResponseUseCase<RemoveRepeatingSectionRequestModel, RemoveRepeatingSectionResponseModel> _removeRepeatingSectionUseCase;
    private readonly ILogger<RemoveRepeatingSection> _logger;

    public RemoveRepeatingSection(IRequestResponseUseCase<RemoveRepeatingSectionRequestModel, RemoveRepeatingSectionResponseModel> removeRepeatingSectionUseCase, ILogger<RemoveRepeatingSection> logger)
    {
        _removeRepeatingSectionUseCase = removeRepeatingSectionUseCase;
        _logger = logger;
    }

    [Function("RemoveRepeatingSection")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "removeRepeatingSection")] HttpRequest req)
    {
        _logger.LogInformation("Removing repeating section");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var removeRepeatingSectionRequest = JsonConvert.DeserializeObject<RemoveRepeatingSectionRequestModel>(requestBody);

        if (removeRepeatingSectionRequest == null)
        {
            _logger.LogError("Invalid request payload");
            return new BadRequestObjectResult("Invalid request payload");
        }

        try
        {
            var response = await _removeRepeatingSectionUseCase.HandleAsync(removeRepeatingSectionRequest);

            if (!response.Success)
            {
                return new BadRequestObjectResult("Failed to remove repeating section");
            }

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing the repeating section");
            return new ObjectResult("An internal error occurred") { StatusCode = 500 };
        }
    }
}