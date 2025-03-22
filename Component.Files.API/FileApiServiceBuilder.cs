using System;
using System.Reflection;
using Component.Core.Application;
using Component.Files.Application.DeleteFile;
using Component.Files.Application.DeleteFile.Model;
using Component.Files.Application.GetFile;
using Component.Files.Application.GetFile.Model;
using Component.Files.Application.Shared.Infrastructure;
using Component.Files.Application.UploadFile;
using Component.Files.Application.UploadFile.Model;
using Component.Files.Infrastructure.Blob;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Component.Files.API;

public static class FileApiConfigurationExtensions
{
    public static IServiceCollection AddFileApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(x => new BlobServiceClient(configuration["BlobStorage:ConnectionString"]));

        services.AddScoped<IFileStore, BlobFileStore>();
        services.AddScoped<IVirusScanner, AzureOnUploadVirusScanner>();

        services.AddScoped<IRequestResponseUseCase<UploadFileRequest, UploadFileResponse>, UploadFile>();
        services.AddScoped<IRequestUseCase<DeleteFileRequest>, DeleteFile>();
        services.AddScoped<IRequestResponseUseCase<GetFileRequest, GetFileResponse>, GetFile>();

        // Increase request size for multipart form data
        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 512 * 1024 * 1024; // 512MB
        });

        // Configure Kestrel server to allow larger request body size
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 512 * 1024 * 1024; // 512MB
        });

        return services;
    }
}
