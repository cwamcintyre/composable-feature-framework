using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class AddOptionPage
    {
        private readonly IPage _page;

        public AddOptionPage(IPage page)
        {
            _page = page;
        }

        public ILocator KeyInput => _page.Locator("#key-input");
        public ILocator ValueInput => _page.Locator("#value-input");
        public ILocator SubmitButton => _page.Locator("#add-option-submit");

        public async Task FillFormAsync(string key, string value)
        {
            await KeyInput.FillAsync(key);
            await ValueInput.FillAsync(value);
        }

        public async Task SubmitFormAsync()
        {
            await SubmitButton.ClickAsync();
        }
    }
}
