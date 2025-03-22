using Component.Core.Application;
using Component.Files.Application.UploadFile.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Component.Files.API.Triggers
{
    public class UploadFile
    {
        private readonly ILogger<UploadFile> _logger;
        private readonly IRequestResponseUseCase<UploadFileRequest, UploadFileResponse> _uploadFileUseCase;

        public UploadFile(IRequestResponseUseCase<UploadFileRequest, UploadFileResponse> uploadFileUseCase, ILogger<UploadFile> logger)
        {
            _logger = logger;
            _uploadFileUseCase = uploadFileUseCase;
        }

        [OpenApiOperation(operationId: "UploadFile", tags: new[] { "File" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UploadFileResponse), Description = "The result of the file upload")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "Internal server error")]
        [Function("UploadFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload")] HttpRequest req)
        {
            _logger.LogInformation("Processing an UploadFile request.");

            var formCollection = await req.ReadFormAsync();
            if (formCollection.Files.Count == 0)
            {
                return new BadRequestObjectResult("Uploaded file is empty.");
            }

            var uploadFileRequest = new UploadFileRequest
            {
                Files = formCollection.Files
            };

            var response = await _uploadFileUseCase.HandleAsync(uploadFileRequest);

            if (response == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(response);
        }
    }
}