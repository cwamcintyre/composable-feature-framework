using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class EditPage
    {
        private readonly IPage _page;

        public EditPage(IPage page)
        {
            _page = page;
        }

        public ILocator TitleInput => _page.Locator("#title-input");
        public ILocator PageTypeInput => _page.Locator("#page-type-input");
        public ILocator NextPageIdInput => _page.Locator("#next-page-id-input");
        public ILocator SubmitButton => _page.Locator("#edit-page-submit");

        public async Task FillFormAsync(string title, string pageType, string nextPageId)
        {
            await TitleInput.FillAsync(title);
            await PageTypeInput.FillAsync(pageType);
            await NextPageIdInput.FillAsync(nextPageId);
        }

        public async Task SubmitFormAsync()
        {
            await SubmitButton.ClickAsync();
        }
    }
}
