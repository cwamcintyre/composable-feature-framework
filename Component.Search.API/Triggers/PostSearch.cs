using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Component.Core.Application;
using Component.Search.Application.UseCase.GetData.Model;
using Component.Search.Application.UseCase.GetData;

namespace Components.Search.API.Triggers
{
    public class PostSearch
    {
        private readonly ILogger<GetData> _logger;
        private readonly IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel> _getDataUseCase;

        public PostSearch(ILogger<GetData> logger, IRequestResponseUseCase<GetDataRequestModel, GetDataResponseModel> getDataUseCase)
        {
            _logger = logger;
            _getDataUseCase = getDataUseCase;
        }

        [Function("PostSearch")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "search")] HttpRequest req)
        {
            _logger.LogInformation("PostSearch processing request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dataRequestModel = JsonConvert.DeserializeObject<GetDataRequestModel>(requestBody);

            if (dataRequestModel == null)
            {
                _logger.LogError("Failed to deserialize request body to GetDataRequestModel.");
                return new BadRequestObjectResult("Invalid request payload.");
            }

            var result = await _getDataUseCase.HandleAsync(dataRequestModel);

            return new OkObjectResult(result);
        }
    }
}
