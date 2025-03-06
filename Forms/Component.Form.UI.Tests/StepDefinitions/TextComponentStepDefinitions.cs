using Component.Form.UI.Tests.Pages;
using Microsoft.Playwright;
using Reqnroll;

namespace Component.Form.UI.Tests.StepDefinitions
{
    [Binding]
    public class TextComponentStepDefinitions(Hooks hooks, TextComponentPage textComponentPage, SummaryPage summaryPage)
    {
        private readonly IPage _page = hooks.Page;
        private readonly TextComponentPage _textComponent = textComponentPage;
        private readonly SummaryPage _summaryPage = summaryPage;

        [Given(@"the user is on the {string} page")]
        public async Task GivenTheUserIsOnTheStartPage(string url)
        {
            await _page.GotoAsync(url);
        }

        [Given(@"the user can see the {string} question")]
        public async Task GivenTheUserCanSeeTheQuestion(string questionString)
        {
            var question = await _page.TextContentAsync($"label[for='{questionString}']");
            Assert.Equal("What is your name?", question);
        }

        [When(@"the user clicks continue")]
        public async Task WhenTheUserClicksContinue()
        {
            await _page.ClickAsync("button[type='submit']");
        }

        [Then(@"the user sees an error message, {string}")]
        public async Task ThenTheUserSeesAnErrorMessage(string message)
        {
            var errorMessage = await _page.TextContentAsync(".govuk-error-message");
            Assert.Equal(message, errorMessage);
        }

        [When(@"the user enters {string} in the {string} field")]
        public async Task WhenTheUserEntersName(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}", name);
        }

        [Then(@"the user sees the summary page")]
        public async Task ThenTheUserSeesTheSummaryPage()
        {
            var pageTitle = await _page.TextContentAsync("h1");
            Assert.Equal("Check your answers before sending your application", pageTitle);
        }

        [Then(@"the summary page shows the answer to {string} is {string}")]
        public async Task ThenTheSummaryPageShowsTheAnswerToNameIs(string question, string name)
        {
            var summaryName = await _page.TextContentAsync($"#{question}-value");
            Assert.Equal(name, summaryName);
        }
    }
}
