using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class AddPage
    {
        private readonly IPage _page;

        public AddPage(IPage page)
        {
            _page = page;
        }

        public ILocator PageIdInput => _page.Locator("#page-id-input");
        public ILocator TitleInput => _page.Locator("#title-input");
        public ILocator PageTypeInput => _page.Locator("#page-type-input");
        public ILocator NextPageIdInput => _page.Locator("#next-page-id-input");
        public ILocator SubmitButton => _page.Locator("#add-page-submit");

        public async Task FillFormAsync(string pageId, string title, string pageType, string nextPageId)
        {
            await PageIdInput.FillAsync(pageId);
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
