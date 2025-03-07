using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class ListFormPage
    {
        private readonly IPage _page;

        public ListFormPage(IPage page)
        {
            _page = page;
        }

        public ILocator EditFormLink(string formId) => _page.Locator($"#edit-form-{formId}");
        public ILocator AddPageButton => _page.Locator("#add-new-page");

        public async Task ClickEditFormAsync(string formId)
        {
            await EditFormLink(formId).ClickAsync();
        }

        public async Task ClickAddPageAsync()
        {
            await AddPageButton.ClickAsync();
        }
    }
}
