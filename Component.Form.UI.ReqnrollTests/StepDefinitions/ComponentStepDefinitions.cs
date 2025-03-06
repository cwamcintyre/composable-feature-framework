using Component.Form.UI.ReqnrollTests.Pages;
using Microsoft.Playwright;

namespace Component.Form.UI.Tests.StepDefinitions
{
    [Binding]
    public class ComponentStepDefinitions(Hooks hooks)
    {
        private readonly IPage _page = hooks.Page;

        [Given(@"the user is on the {string} page")]
        public async Task GivenTheUserIsOnTheStartPage(string url)
        {
            await _page.GotoAsync(url);
        }

        [Given(@"the user can see the {string} question, with text {string}")]
        public async Task GivenTheUserCanSeeTheQuestion(string questionString, string labelString)
        {
            var question = await _page.TextContentAsync($"#{questionString}-label");
            Assert.Equal(labelString, question?.Trim());
        }

        [When(@"the user clicks continue")]
        public async Task WhenTheUserClicksContinue()
        {
            await _page.ClickAsync("button[type='submit']");
        }

        [Then(@"the user sees an error message, {string}")]
        public async Task ThenTheUserSeesAnErrorMessage(string message)
        {
            var errorMessages = await _page.Locator(".govuk-error-message").AllInnerTextsAsync();
            Assert.True(errorMessages.Any(text => text.Contains(message)));
        }

        [When(@"the user enters {string} in the {string} field")]
        public async Task WhenTheUserEntersName(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}", name);
        }

        [When(@"the user enters {string} in the {string} Address Line 1 uk address field")]
        public async Task WhenTheUserEntersAddressLine1(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}-addressLine1", name);
        }

        [When(@"the user enters {string} in the {string} Address Line 2 uk address field")]
        public async Task WhenTheUserEntersAddressLine2(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}-addressLine2", name);
        }

        [When(@"the user enters {string} in the {string} Town uk address field")]
        public async Task WhenTheUserEntersTown(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}-addressTown", name);
        }

        [When(@"the user enters {string} in the {string} County uk address field")]
        public async Task WhenTheUserEntersCounty(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}-addressCounty", name);
        }

        [When(@"the user enters {string} in the {string} Postcode uk address field")]
        public async Task WhenTheUserEntersPostcode(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}-addressPostcode", name);
        }

        [When(@"the user enters day {string} month {string} and year {string} in the {string} date parts field")]
        public async Task WhenTheUserEntersDataInTheDateField(string day, string month, string year, string questionField)
        {
            await _page.FillAsync($"#{questionField}-day", day);
            await _page.FillAsync($"#{questionField}-month", month);
            await _page.FillAsync($"#{questionField}-year", year);
        }

        [When(@"the user enters {string} in the {string} address field")]
        public async Task WhenTheUserEntersDataInTheAddressField(string name, string questionField)
        {
            await _page.FillAsync($"#{questionField}", name);
        }

        [Then(@"the user sees the summary page")]
        public async Task ThenTheUserSeesTheSummaryPage()
        {
            var pageTitle = await _page.TextContentAsync("h1");
            Assert.Equal("Check your answers before sending your application", pageTitle);
        }

        [Then(@"the user sees the stop page")]
        public async Task ThenTheUserSeesTheStopPage()
        {
            var pageTitle = await _page.TextContentAsync("h1");
            Assert.Equal("Then you don't have to :)", pageTitle);
        }

        [Then(@"the summary page shows the answer to {string} is {string}")]
        public async Task ThenTheSummaryPageShowsTheAnswerToNameIs(string question, string name)
        {
            var summaryName = await _page.TextContentAsync($"#{question}-value");
            Assert.Equal(name, summaryName?.Trim());
        }

        [Given(@"the user can see the {string} question, with options {string}")]
        public async Task GivenTheUserCanSeeTheQuestionWithOptions(string questionString, string optionsString)
        {
            var options = optionsString.Split(',').Select(option => option.Trim()).ToList();
            foreach (var option in options)
            {
                var optionElement = await _page.TextContentAsync($"option[value='{option}']");
                Assert.Equal(option, optionElement?.Trim());
            }
        }

        [Given(@"the user can see the {string} question, with radio options {string}")]
        public async Task GivenTheUserCanSeeTheRadioQuestionWithOptions(string questionString, string optionsString)
        {
            var options = optionsString.Split(',').Select(option => option.Trim()).ToList();
            int count = 1;
            foreach (var option in options)
            {
                string forSelector = questionString;
                if (count > 1)
                {
                    forSelector += $"-{count}";
                }
                var optionElement = await _page.TextContentAsync($".govuk-radios__item label[for='{forSelector}']");
                Assert.Equal(option, optionElement?.Trim());
                count++;
            }
        }

        [When(@"the user selects {string} for the {string} question")]
        public async Task WhenTheUserSelectsOptionForTheQuestion(string option, string questionString)
        {
            await _page.SelectOptionAsync($"#{questionString}", option);
        }

        [When(@"the user selects {string} for the {string} radio question")]
        public async Task WhenTheUserSelectsRadioOptionForTheQuestion(string option, string questionString)
        {
            await _page.ClickAsync($"input[type=radio][name='{questionString}'][value='{option}']");
        }

        [Given(@"the user can see the {string} question, with checkbox options {string}")]
        public async Task GivenTheUserCanSeeTheQuestionWithCheckboxOptions(string questionString, string optionsString)
        {
            var options = optionsString.Split(',').Select(option => option.Trim()).ToList();
            int count = 1;
            foreach (var option in options)
            {
                string forSelector = questionString;
                if (count > 1)
                {
                    forSelector += $"-{count}";
                }
                var optionElement = await _page.TextContentAsync($".govuk-checkboxes__item label[for='{forSelector}']");
                Assert.Equal(option, optionElement?.Trim());
                count++;
            }
        }

        [When(@"the user selects {string} for the {string} checkbox question")]
        public async Task WhenTheUserSelectsCheckboxOptionForTheQuestion(string option, string questionString)
        {
            var options = option.Split(',').Select(option => option.Trim()).ToList();
            foreach (var optionValue in options)
            {
                await _page.ClickAsync($"input[name='{questionString}'][value='{optionValue}']");
            }
        }
    }
}
