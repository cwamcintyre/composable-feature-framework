using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Reqnroll;

namespace Component.Form.UI.Tests.StepDefinitions;

[Binding]
public class Hooks(ScenarioContext scenarioContext)
{
    public IPage Page { get; private set; } = null!;
    private IBrowserContext _context;
    private readonly ScenarioContext _scenarioContext = scenarioContext;   

    [BeforeScenario]
    public async Task SetupTestAsync()
    {
        IPlaywright _playwright = await Playwright.CreateAsync();
        IBrowser _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        _context = await _browser.NewContextAsync();

        Page = await _context.NewPageAsync();
        //Page.SetDefaultTimeout(5000);
    }

    [AfterScenario]
    public async Task TearDownTestAsync()
    {
        await _context.CloseAsync();
        await Page.CloseAsync();
    }
}
