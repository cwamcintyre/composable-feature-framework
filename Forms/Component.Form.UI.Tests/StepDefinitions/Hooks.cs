using System;
using Microsoft.Playwright;
using Reqnroll;

namespace Component.Form.UI.Tests.StepDefinitions;

[Binding]
public class Hooks(ScenarioContext scenarioContext)
{
    public IPage Page { get; private set; } = null!;
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    [BeforeScenario]
    public async Task SetupTestAsync()
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
        IBrowserContext context = await browser.NewContextAsync();

        Page = await context.NewPageAsync();
    }
}
