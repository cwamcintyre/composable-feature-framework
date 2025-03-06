using System;
using Component.Form.UI.Tests.StepDefinitions;
using Microsoft.Playwright;

namespace Component.Form.UI.ReqnrollTests.Pages;

public class StartPage(Hooks hooks)
{
    private readonly IPage _page = hooks.Page;

    public async Task GoTo(string component)
    {
        await _page.GotoAsync($"http://localhost:5164/form/{component}/start");
    }
}