using Component.Form.UI.Tests.StepDefinitions;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class SummaryPage(Hooks hooks)
    {
        private readonly IPage _page = hooks.Page;
      
        public async Task AcceptAndSendAsync()
        {
            await _page.ClickAsync("a.govuk-button");
        }

        public async Task<string> GetSummaryListValueAsync(string keyId)
        {
            return await _page.TextContentAsync($"#{keyId}-value");
        }

        public async Task ClickChangeForNameAsync(string keyId)
        {
            await _page.ClickAsync($"#{keyId}-change-link");
        }
    }
}
