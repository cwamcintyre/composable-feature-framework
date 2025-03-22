using Component.Core.Application;
using Component.Form.Application.UseCase.ProcessForm.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Component.Form.API
{
    public class ProcessChangeInForm
    {
        private readonly ILogger<ProcessForm> _logger;
        private readonly IRequestResponseUseCase<ProcessChangeInFormRequestModel, ProcessChangeInFormResponseModel> _processChangeInFormUseCase;

        public ProcessChangeInForm(ILogger<ProcessForm> logger, IRequestResponseUseCase<ProcessChangeInFormRequestModel, ProcessChangeInFormResponseModel> processChangeInFormUseCase)
        {
            _logger = logger;
            _processChangeInFormUseCase = processChangeInFormUseCase;
        }

        [Function("ProcessChangeInForm")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "processChange")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dataRequestModel = JsonConvert.DeserializeObject<ProcessChangeInFormRequestModel>(requestBody);

            if (dataRequestModel == null)
            {
                _logger.LogError("Failed to deserialize request body to ProcessChangeInFormRequestModel.");
                return new BadRequestObjectResult("Invalid request payload.");
            }

            _logger.LogInformation("Processing change in form with id {id}", dataRequestModel.FormId);

            var response = await _processChangeInFormUseCase.HandleAsync(dataRequestModel);

            return new OkObjectResult(response);
        }        
    }
}