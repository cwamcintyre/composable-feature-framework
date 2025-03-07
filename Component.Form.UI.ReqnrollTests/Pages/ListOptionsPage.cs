using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class ListOptionsPage
    {
        private readonly IPage _page;

        public ListOptionsPage(IPage page)
        {
            _page = page;
        }

        public ILocator AddNewOptionButton => _page.Locator("#add-new-option");
        public ILocator EditOptionLink(string key) => _page.Locator($"#edit-option-{key}");
        public ILocator DeleteOptionLink(string key) => _page.Locator($"#delete-option-{key}");

        public async Task ClickAddNewOptionAsync()
        {
            await AddNewOptionButton.ClickAsync();
        }

        public async Task ClickEditOptionAsync(string key)
        {
            await EditOptionLink(key).ClickAsync();
        }

        public async Task ClickDeleteOptionAsync(string key)
        {
            await DeleteOptionLink(key).ClickAsync();
        }
    }
}
