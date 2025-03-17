using System.Net;
using System.Text.Json;
using Component.Core.Application;
using Component.Core.SafeJson;
using Component.Form.Application.UseCase.GetForm.Model;
using Component.Form.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Component.Form.API
{
    public class GetForm
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new PolymorphicJsonConverter<PageBase>() } // Ensure $type is included
        };

        private readonly ILogger<GetForm> _logger;
        private readonly IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel> _getFormUseCase;

        public GetForm(ILogger<GetForm> logger, IRequestResponseUseCase<GetFormRequestModel, GetFormResponseModel> getFormUseCase)
        {
            _logger = logger;
            _getFormUseCase = getFormUseCase;
        }

        [Function("GetForm")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getForm/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("Fetching form with id {id}", id);

            var request = new GetFormRequestModel
            {
                FormId = id
            };

            var response = await _getFormUseCase.HandleAsync(request);

            // Serialize with $type hints explicitly
            var httpResponse = req.CreateResponse();
            httpResponse.StatusCode = HttpStatusCode.OK;
            httpResponse.Headers.Add("Content-Type", "application/json");
            await httpResponse.WriteStringAsync(JsonSerializer.Serialize(response, SerializerOptions));

            return httpResponse;
        }        
    }
}