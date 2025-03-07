using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Component.Form.UI.ReqnrollTests.Pages
{
    public class EditComponentPage
    {
        private readonly IPage _page;

        public EditComponentPage(IPage page)
        {
            _page = page;
        }

        public ILocator QuestionIdInput => _page.Locator("#question-id-input");
        public ILocator TypeSelect => _page.Locator("#type-select");
        public ILocator LabelInput => _page.Locator("#label-input");
        public ILocator NameInput => _page.Locator("#name-input");
        public ILocator SubmitButton => _page.Locator("#edit-component-submit");

        public async Task FillFormAsync(string questionId, string type, string label, string name)
        {
            await QuestionIdInput.FillAsync(questionId);
            await TypeSelect.SelectOptionAsync(type);
            await LabelInput.FillAsync(label);
            await NameInput.FillAsync(name);
        }

        public async Task SubmitFormAsync()
        {
            await SubmitButton.ClickAsync();
        }
    }
}
