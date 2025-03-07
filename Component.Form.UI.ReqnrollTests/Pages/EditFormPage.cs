using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class EditFormPage
    {
        private readonly IPage _page;

        public EditFormPage(IPage page)
        {
            _page = page;
        }

        public ILocator TitleInput => _page.Locator("#title-input");
        public ILocator DescriptionInput => _page.Locator("#description-input");
        public ILocator CurrentPageInput => _page.Locator("#current-page-input");
        public ILocator TotalPagesInput => _page.Locator("#total-pages-input");
        public ILocator StartPageInput => _page.Locator("#start-page-input");
        public ILocator SubmitButton => _page.Locator("#edit-form-submit");

        public async Task FillFormAsync(string title, string description, string currentPage, string totalPages, string startPage)
        {
            await TitleInput.FillAsync(title);
            await DescriptionInput.FillAsync(description);
            await CurrentPageInput.FillAsync(currentPage);
            await TotalPagesInput.FillAsync(totalPages);
            await StartPageInput.FillAsync(startPage);
        }

        public async Task SubmitFormAsync()
        {
            await SubmitButton.ClickAsync();
        }
    }
}
