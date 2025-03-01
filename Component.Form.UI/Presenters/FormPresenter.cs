using Component.Form.Model;
using Microsoft.AspNetCore.SignalR;

public interface IFormPresenter
{
    Task<IndexResult> HandleIndex(int page, FormModel formModel);
    Task<SubmitResult> HandleSubmit(HttpRequest request, FormModel formModel);
}

public class FormPresenter : IFormPresenter
{
    public virtual string IndexViewName { get; set; } = "Index";

    public async Task<IndexResult> HandleIndex(int page, FormModel formModel)
    {
        var currentPage = formModel.Pages.Find(p => p.PageId == $"page_{page}");
        if (currentPage == null) return null;

        return new IndexResult
        {
            PageModel = currentPage,
            CurrentPage = page,
            TotalPages = formModel.TotalPages,
            NextAction = IndexViewName
        };
    }

    public async Task<SubmitResult> HandleSubmit(HttpRequest request, FormModel formModel)
    {
        var currentPageFromForm = request.Form["PageId"];
        var currentPage = formModel.Pages.Find(p => p.PageId == currentPageFromForm);
        if (currentPage == null) return new SubmitResult { Errors = new List<string> { "Page not found" } };

        var page = Convert.ToInt32(currentPageFromForm.First().Split('_')[1]);

        var formData = new Dictionary<string, string>();
        var errors = new List<string>();

        // Loop through questions and validate them
        foreach (var question in currentPage.Questions)
        {
            string inputName = question.QuestionId;

            // Check if the required field is filled
            if (question.Required && !request.Form.ContainsKey(inputName) && question.Type != "file")
            {
                errors.Add($"The field '{question.Label}' is required.");
                continue;
            }

            // Handle file uploads
            if (question.Type == "file" && request.Form.Files.Count > 0)
            {
                var file = request.Form.Files[0];
                if (file.Length > 0)
                {
                    // Validate File Type
                    if (question.FileOptions?.AcceptedFormats != null &&
                        !question.FileOptions.AcceptedFormats.Contains(file.ContentType))
                    {
                        errors.Add($"Invalid file type for '{question.Label}'. Allowed formats: {string.Join(", ", question.FileOptions.AcceptedFormats)}");
                        continue;
                    }

                    // Validate File Size
                    if (question.FileOptions?.MaxSizeMB > 0 &&
                        file.Length > question.FileOptions.MaxSizeMB * 1024 * 1024)
                    {
                        errors.Add($"The file '{question.Label}' exceeds the allowed size of {question.FileOptions.MaxSizeMB}MB.");
                        continue;
                    }

                    // Save the file to the server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    formData[inputName] = file.FileName; // Save file reference in formData
                }
            }
            else if (request.Form.ContainsKey(inputName))
            {
                formData[inputName] = request.Form[inputName];
            }
        }

        // If there are validation errors, return to the same page
        if (errors.Any())
        {
            return new SubmitResult
            {
                Errors = errors,
                CurrentPage = page,
                PageModel = currentPage
            };
        }

        // Branching logic based on answers
        foreach (var question in currentPage.Questions)
        {
            if (question.Branching != null && formData.ContainsKey(question.QuestionId))
            {
                string answer = formData[question.QuestionId];
                if (question.Branching.ContainsKey(answer))
                {
                    string nextPageId = question.Branching[answer];
                    var nextPage = formModel.Pages.Find(p => p.PageId == nextPageId);
                    if (nextPage != null)
                    {
                        int nextPageNumber = int.Parse(nextPageId.Split('_')[1]);
                        return new SubmitResult
                        {
                            NextAction = IndexViewName,
                            NextPage = nextPageNumber
                        };
                    }
                }
            }
        }

        // Move to the next page or submit the form if on the last page
        if (page < formModel.TotalPages)
        {
            return new SubmitResult
            {
                NextAction = IndexViewName,
                NextPage = page + 1
            };
        }

        return new SubmitResult
        {
            NextAction = "ThankYou"
        };
    }
}

public class IndexResult
{
    public Page PageModel { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string NextAction { get; set; }
}

public class SubmitResult
{
    public List<string> Errors { get; set; } = new List<string>();
    public int CurrentPage { get; set; }
    public Page PageModel { get; set; }
    public string NextAction { get; set; }
    public int? NextPage { get; set; }
}