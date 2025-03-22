using System;
using Component.Files.Model;

namespace Component.Files.Application.Shared.Infrastructure;

public interface IVirusScanner
{
    Task<ScanResult> ScanFileAsync(string fileName, CancellationToken cancellationToken = default);
}
