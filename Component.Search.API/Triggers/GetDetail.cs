using Component.Core.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Component.Search.Application.UseCase.GetDataItem.Model;
using System.Threading.Tasks;

namespace Component.Search.API.Triggers
{
    public class GetDetail
    {
        private readonly ILogger<GetDetail> _logger;
        private readonly IRequestResponseUseCase<GetDataItemRequestModel, GetDataItemResponseModel> _getDataItemUseCase;

        public GetDetail(ILogger<GetDetail> logger, IRequestResponseUseCase<GetDataItemRequestModel, GetDataItemResponseModel> getDataItemUseCase)
        {
            _logger = logger;
            _getDataItemUseCase = getDataItemUseCase;
        }

        [Function("GetDetail")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "detail/{dataTypeId}/{itemId}")] HttpRequest req, 
            string dataTypeId, 
            string itemId)
        {
            _logger.LogInformation($"GetDetail function fetching data for {dataTypeId} and {itemId}.");
            var detail = await _getDataItemUseCase.HandleAsync(new GetDataItemRequestModel { ItemTypeId = dataTypeId, ItemId = itemId });
            return new OkObjectResult(detail);
        }
    }
}
