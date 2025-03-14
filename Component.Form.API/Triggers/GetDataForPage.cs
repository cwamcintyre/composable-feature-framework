using Component.Core.Application;
using Component.Form.Application.UseCase.GetDataForPage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Component.Form.API
{
    public class GetDataForPage
    {
        private readonly ILogger<GetDataForPage> _logger;
        private readonly IRequestResponseUseCase<GetDataForPageRequestModel, GetDataForPageResponseModel> _getDataForPageUseCase;

        public GetDataForPage(ILogger<GetDataForPage> logger, IRequestResponseUseCase<GetDataForPageRequestModel, GetDataForPageResponseModel> getDataForPageUseCase)
        {
            _logger = logger;
            _getDataForPageUseCase = getDataForPageUseCase;
        }

        [Function("GetDataForPage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getDataForPage/{formId}/{pageId}/{applicantId}/{repeatIndex}")] HttpRequest req, 
            string formId, 
            string pageId, 
            string applicantId,
            int repeatIndex)
        {
            _logger.LogInformation("Fetching data for form {formId}, page {pageId}, applicant {applicantId}", formId, pageId, applicantId);

            var request = new GetDataForPageRequestModel
            {
                FormId = formId,
                PageId = pageId,
                ApplicantId = applicantId,
                RepeatIndex = repeatIndex
            };

            var response = await _getDataForPageUseCase.HandleAsync(request);

            return new OkObjectResult(response);
        }
    }
}
