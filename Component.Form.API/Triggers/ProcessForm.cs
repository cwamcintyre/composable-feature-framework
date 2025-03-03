using Component.Core.Application;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Component.Form.API
{
    public class ProcessForm
    {
        private readonly ILogger<ProcessForm> _logger;
        private readonly IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel> _processFormUseCase;

        public ProcessForm(ILogger<ProcessForm> logger, IRequestResponseUseCase<ProcessFormRequestModel, ProcessFormResponseModel> processFormUseCase)
        {
            _logger = logger;
            _processFormUseCase = processFormUseCase;
        }

        [Function("ProcessForm")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "processForm")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dataRequestModel = JsonConvert.DeserializeObject<ProcessFormRequestModel>(requestBody);

            if (dataRequestModel == null)
            {
                _logger.LogError("Failed to deserialize request body to ProcessFormRequestModel.");
                return new BadRequestObjectResult("Invalid request payload.");
            }

            _logger.LogInformation("Processing form with id {id}", dataRequestModel.FormId);

            var response = await _processFormUseCase.HandleAsync(dataRequestModel);

            return new OkObjectResult(response);
        }        
    }
}