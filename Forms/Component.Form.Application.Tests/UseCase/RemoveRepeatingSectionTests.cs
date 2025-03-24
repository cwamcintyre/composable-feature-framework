using System;
using System.Threading.Tasks;
using Component.Form.Application.UseCase.RemoveRepeatingSection;
using Component.Form.Application.UseCase.RemoveRepeatingSection.Model;
using Component.Form.Application.Tests.Doubles.Infrastructure;
using Component.Form.Application.Tests.Doubles.UseCase;
using Xunit;
using Component.Form.Model;

namespace Component.Form.Application.Tests.UseCase;

public class RemoveRepeatingSectionTests
{
    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenFormNotFound()
    {
        // Arrange
        var removeRepeatingSection = new RemoveRepeatingSectionTestBuilder()
            .Build();

        var request = new RemoveRepeatingSectionRequestModel
        {
            FormId = "nonexistent-form",
            PageId = "page-id",
            ApplicantId = "applicant-id",
            Index = 0
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => removeRepeatingSection.HandleAsync(request));
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPageNotFound()
    {
        // Arrange
        var removeRepeatingSection = new RemoveRepeatingSectionTestBuilder()
            .Build();

        var request = new RemoveRepeatingSectionRequestModel
        {
            FormId = "textComponent",
            PageId = "nonexistent-page",
            ApplicantId = "applicant-id",
            Index = 0
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => removeRepeatingSection.HandleAsync(request));
    }

    [Theory]
    [InlineData(RepeatingSectionExamples.ExistingFormData_OneEntry, 0, RepeatingSectionExamples.EmptyTasksStringSaved)]
    [InlineData(RepeatingSectionExamples.ExistingFormData_Three_Entries, 0, RepeatingSectionExamples.RemovedFromThreeEntries_StartSaved)]
    [InlineData(RepeatingSectionExamples.ExistingFormData_Three_Entries, 1, RepeatingSectionExamples.RemovedFromThreeEntries_MiddleSaved)]
    [InlineData(RepeatingSectionExamples.ExistingFormData_Three_Entries, 2, RepeatingSectionExamples.RemovedFromThreeEntries_EndSaved)]
    public async Task HandleAsync_ShouldRemoveRepeatingSectionSuccessfully(string existingData, int index, string resultData)
    {
        // Arrange
        var formDataStoreMock = new FormDataStoreTestBuilder()
            .WithGetFormDataAsync(RepeatingSectionExamples.ApplicantId, new FormData { Data = existingData })
            .WithSaveFormDataAsync(RepeatingSectionExamples.FormId, RepeatingSectionExamples.ApplicantId, resultData);

        var removeRepeatingSection = new RemoveRepeatingSectionTestBuilder()
            .WithFormDataStore(formDataStoreMock.Build())
            .Build();

        var request = new RemoveRepeatingSectionRequestModel
        {
            FormId = RepeatingSectionExamples.FormId,
            PageId = RepeatingSectionExamples.PageId,
            ApplicantId = RepeatingSectionExamples.ApplicantId,
            Index = index
        };

        // Act
        var response = await removeRepeatingSection.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        formDataStoreMock.VerifySaveFormDataAsync(
            RepeatingSectionExamples.FormId,
            RepeatingSectionExamples.ApplicantId, 
            resultData);
    }
}