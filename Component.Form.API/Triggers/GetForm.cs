using Component.Core.Application;
using Component.Form.Application.UseCase.GetForm.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Component.Form.API
{
    public class GetForm
    {
        private readonly ILogger<GetForm> _logger;
        private readonly IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel> _getFormUseCase;

        public GetForm(ILogger<GetForm> logger, IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel> getFormUseCase)
        {
            _logger = logger;
            _getFormUseCase = getFormUseCase;
        }

        [Function("GetForm")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getForm/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("Fetching form with id {id}", id);

            var request = new GetFormRequestModel
            {
                FormId = id
            };

            var response = await _getFormUseCase.HandleAsync(request);

            return new OkObjectResult(response);
        }        
    }
}
