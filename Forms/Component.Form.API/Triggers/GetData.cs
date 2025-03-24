using Component.Core.Application;
using Component.Form.Application.UseCase.GetData.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Component.Form.API
{
    public class GetData
    {
        private readonly ILogger<GetForm> _logger;
        private readonly IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel> _getDataUseCase;

        public GetData(ILogger<GetForm> logger, IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel> getDataUseCase)
        {
            _logger = logger;
            _getDataUseCase = getDataUseCase;
        }

        [Function("GetData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getData/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("Fetching form with id {id}", id);

            var request = new GetDataRequestModel
            {
                ApplicantId = id
            };

            var response = await _getDataUseCase.HandleAsync(request);

            return new OkObjectResult(response);
        }        
    }
}