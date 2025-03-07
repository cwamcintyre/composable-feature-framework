using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class ListComponentsPage
    {
        private readonly IPage _page;

        public ListComponentsPage(IPage page)
        {
            _page = page;
        }

        public ILocator AddNewComponentButton => _page.Locator("#add-new-component");
        public ILocator EditComponentLink(string componentName) => _page.Locator($"#edit-component-{componentName}");
        public ILocator DeleteComponentLink(string componentName) => _page.Locator($"#delete-component-{componentName}");

        public async Task ClickAddNewComponentAsync()
        {
            await AddNewComponentButton.ClickAsync();
        }

        public async Task ClickEditComponentAsync(string componentName)
        {
            await EditComponentLink(componentName).ClickAsync();
        }

        public async Task ClickDeleteComponentAsync(string componentName)
        {
            await DeleteComponentLink(componentName).ClickAsync();
        }
    }
}
