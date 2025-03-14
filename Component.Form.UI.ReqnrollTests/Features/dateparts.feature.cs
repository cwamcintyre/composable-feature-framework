﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Component.Form.UI.ReqnrollTests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class CheckDatepartsComponentFeature : object, Xunit.IClassFixture<CheckDatepartsComponentFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "check dateparts component", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "dateparts.feature"
#line hidden
        
        public CheckDatepartsComponentFeature(CheckDatepartsComponentFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }
        
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 3
#line hidden
#line 4
    await testRunner.GivenAsync("the user is on the \'http://localhost:5164/form/datepartsComponent/start\' page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 5
    await testRunner.AndAsync("the user can see the \'give_me_a_date\' question, with text \'Give me a date. Any da" +
                    "te.\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
        {
            await this.TestInitializeAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
        {
            await this.TestTearDownAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check dateparts component")]
        [Xunit.TraitAttribute("Description", "user clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 7
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 8
    await testRunner.WhenAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 9
    await testRunner.ThenAsync("the user sees an error message, \'Day is between 1 and 31\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a day, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check dateparts component")]
        [Xunit.TraitAttribute("Description", "user enters a day, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersADayClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a day, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 11
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 12
    await testRunner.WhenAsync("the user enters day \'01\' month \'\' and year \'\' in the \'give_me_a_date\' date parts " +
                        "field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 13
    await testRunner.WhenAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 14
    await testRunner.ThenAsync("the user sees an error message, \'Month is between 1 and 12\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 15
 await testRunner.AndAsync("the user sees an error message, \'Year is between 1900 and 2100\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a month, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check dateparts component")]
        [Xunit.TraitAttribute("Description", "user enters a month, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersAMonthClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a month, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 17
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 18
    await testRunner.WhenAsync("the user enters day \'\' month \'01\' and year \'\' in the \'give_me_a_date\' date parts " +
                        "field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 19
    await testRunner.WhenAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 20
    await testRunner.ThenAsync("the user sees an error message, \'Day is between 1 and 31\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 21
    await testRunner.AndAsync("the user sees an error message, \'Year is between 1900 and 2100\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a year, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check dateparts component")]
        [Xunit.TraitAttribute("Description", "user enters a year, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersAYearClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a year, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 24
    await testRunner.WhenAsync("the user enters day \'\' month \'\' and year \'20\' in the \'give_me_a_date\' date parts " +
                        "field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 25
    await testRunner.WhenAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 26
    await testRunner.ThenAsync("the user sees an error message, \'Day is between 1 and 31\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 27
    await testRunner.AndAsync("the user sees an error message, \'Month is between 1 and 12\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters date and sees the next page")]
        [Xunit.TraitAttribute("FeatureTitle", "check dateparts component")]
        [Xunit.TraitAttribute("Description", "user enters date and sees the next page")]
        public async System.Threading.Tasks.Task UserEntersDateAndSeesTheNextPage()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters date and sees the next page", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 29
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 30
    await testRunner.WhenAsync("the user enters day \'01\' month \'01\' and year \'2020\' in the \'give_me_a_date\' date " +
                        "parts field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 31
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 32
    await testRunner.ThenAsync("the user sees the summary page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 33
    await testRunner.AndAsync("the summary page shows the answer to \'give_me_a_date\' is \'1/1/2020\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : object, Xunit.IAsyncLifetime
        {
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
            {
                await CheckDatepartsComponentFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await CheckDatepartsComponentFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
