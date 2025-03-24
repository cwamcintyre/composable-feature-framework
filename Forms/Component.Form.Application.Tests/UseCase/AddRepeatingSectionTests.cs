using Component.Form.Application.UseCase.AddRepeatingSection;
using Component.Form.Application.UseCase.AddRepeatingSection.Model;
using Component.Form.Application.Tests.Doubles.Infrastructure;
using Component.Form.Application.Tests.Doubles.UseCase;
using Xunit;

namespace Component.Form.Application.Tests.UseCase;

public class AddRepeatingSectionTests
{
    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenFormNotFound()
    {
        // Arrange
        var formDataStore = new FormDataStoreTestBuilder().Build();
        var addRepeatingSection = new AddRepeatingSectionTestBuilder()
            .WithFormDataStore(formDataStore)
            .Build();

        var request = new AddRepeatingSectionRequestModel
        {
            FormId = "nonexistent-form",
            PageId = RepeatingSectionExamples.PageId,
            ApplicantId = RepeatingSectionExamples.ApplicantId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => addRepeatingSection.HandleAsync(request));
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPageNotFound()
    {
        // Arrange
        var formDataStore = new FormDataStoreTestBuilder().Build();
        var addRepeatingSection = new AddRepeatingSectionTestBuilder()
            .WithFormDataStore(formDataStore)
            .Build();

        var request = new AddRepeatingSectionRequestModel
        {
            FormId = RepeatingSectionExamples.FormId,
            PageId = "nonexistent-page",
            ApplicantId = RepeatingSectionExamples.ApplicantId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => addRepeatingSection.HandleAsync(request));
    }

    [Fact]
    public async Task HandleAsync_ShouldAddRepeatingSectionSuccessfully()
    {
        // Arrange
        var formDataStoreMock = new FormDataStoreTestBuilder()
            .WithGetFormDataAsync(RepeatingSectionExamples.ApplicantId, RepeatingSectionExamples.ExistingFormData)
            .WithSaveFormDataAsync(RepeatingSectionExamples.FormId, RepeatingSectionExamples.ApplicantId, RepeatingSectionExamples.AddRepeatingSectionSaved);
        
        var formDataStore = formDataStoreMock.Build();

        var addRepeatingSection = new AddRepeatingSectionTestBuilder()
            .WithFormDataStore(formDataStore)
            .Build();

        var request = new AddRepeatingSectionRequestModel
        {
            FormId = RepeatingSectionExamples.FormId,
            PageId = RepeatingSectionExamples.PageId,
            ApplicantId = RepeatingSectionExamples.ApplicantId
        };

        // Act
        var response = await addRepeatingSection.HandleAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(1, response.NewRepeatIndex);
        Assert.Equal("what-do-you-want-to-do-today", response.StartPageId);
        formDataStoreMock.VerifySaveFormDataAsync(RepeatingSectionExamples.FormId, RepeatingSectionExamples.ApplicantId, RepeatingSectionExamples.AddRepeatingSectionSaved);
    }
}