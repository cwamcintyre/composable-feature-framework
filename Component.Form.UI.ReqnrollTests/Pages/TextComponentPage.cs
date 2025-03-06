using Component.Form.UI.Tests.StepDefinitions;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class TextComponentPage(Hooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        // Locators
        public ILocator SubmitButton => _page.Locator("button[type='submit']");

        // Methods
        public async Task FillNameAsync(string name, string data)
        {
            await _page.Locator($"#{name}").FillAsync(data);
        }

        public async Task SubmitFormAsync()
        {
            await SubmitButton.ClickAsync();
        }
    }
}
