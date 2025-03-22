using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Component.Core.Application;
using Component.Files.Application.GetFile.Model;
using Microsoft.AspNetCore.Http;

namespace Component.Files.API.Triggers
{
    public class GetFileTrigger
    {
        private readonly ILogger<GetFileTrigger> _logger;
        private readonly IRequestResponseUseCase<GetFileRequest, GetFileResponse> _getFileUseCase;

        public GetFileTrigger(IRequestResponseUseCase<GetFileRequest, GetFileResponse> getFileUseCase, ILogger<GetFileTrigger> logger)
        {
            _logger = logger;
            _getFileUseCase = getFileUseCase;
        }

        [Function("GetFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "file/{fileName}")] HttpRequest req, string fileName)
        {
            _logger.LogInformation($"Processing a GetFile request for file: {fileName}");

            var request = new GetFileRequest { FileName = fileName };
            try 
            {
                var response = await _getFileUseCase.HandleAsync(request);
                if (response?.Stream == null)
                {
                    return new NotFoundResult();
                }

                if (response.Stream.CanRead == false)
                {
                    return new NotFoundObjectResult("File stream is not readable.");
                }

                return new FileStreamResult(response.Stream, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
            catch (FileNotFoundException e)
            {
                _logger.LogError(e, $"File not found: {fileName}");
                return new NotFoundObjectResult($"File not found: {fileName}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing the GetFile request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}