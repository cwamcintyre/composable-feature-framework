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
        private readonly IRequestResponseUseCase<GetSearchIndexRequestModel, GetSearchIndexResponseModel> _getSearchIndexConfigurationUseCase;

        public GetSearchIndexType(ILogger<GetSearchIndexType> logger, IRequestResponseUseCase<GetSearchIndexRequestModel, GetSearchIndexResponseModel> getSearchIndexConfigurationUseCase)
        {
            _logger = logger;
            _getSearchIndexConfigurationUseCase = getSearchIndexConfigurationUseCase;
        }

        [Function("GetSearchIndexType")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "searchIndexType/{searchIndexTypeId}")] HttpRequest req, string searchIndexTypeId)
        {
            _logger.LogInformation("GetSearchIndexType function called.");
            
            var searchIndexType = await _getSearchIndexConfigurationUseCase.HandleAsync(new GetSearchIndexRequestModel()
            {
                SearchIndexTypeId = searchIndexTypeId
            });

            return new OkObjectResult(searchIndexType);
        }
    }
}
