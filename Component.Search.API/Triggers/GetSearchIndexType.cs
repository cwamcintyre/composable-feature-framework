using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Component.Core.Application;
using Component.Search.Application.UseCase.GetSearchIndexConfiguration.Model;
namespace Component.Search.API.Triggers
{
    public class GetSearchIndexType
    {
        private readonly ILogger<GetSearchIndexType> _logger;
        private readonly IResponseUseCase<GetSearchIndexResponseModel> _getSearchIndexConfigurationUseCase;

        public GetSearchIndexType(ILogger<GetSearchIndexType> logger, IResponseUseCase<GetSearchIndexResponseModel> getSearchIndexConfigurationUseCase)
        {
            _logger = logger;
            _getSearchIndexConfigurationUseCase = getSearchIndexConfigurationUseCase;
        }

        [Function("GetSearchIndexType")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "searchIndexType")] HttpRequest req)
        {
            _logger.LogInformation("GetSearchIndexType function called.");
            var searchIndexType = await _getSearchIndexConfigurationUseCase.HandleAsync();
            return new OkObjectResult(searchIndexType);
        }
    }
}
