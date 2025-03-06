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
    public partial class CheckUkaddressComponentFeature : object, Xunit.IClassFixture<CheckUkaddressComponentFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "check ukaddress component", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ukaddress.feature"
#line hidden
        
        public CheckUkaddressComponentFeature(CheckUkaddressComponentFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
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
    await testRunner.GivenAsync("the user is on the \'http://localhost:5164/form/ukaddressComponent/start\' page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 5
    await testRunner.AndAsync("the user can see the \'what_is_your_address\' question, with text \'What is your add" +
                    "ress?\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
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
        [Xunit.TraitAttribute("FeatureTitle", "check ukaddress component")]
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
    await testRunner.ThenAsync("the user sees an error message, \'Address Line 1 is required\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 10
    await testRunner.AndAsync("the user sees an error message, \'Postcode is required\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters address and sees the next page")]
        [Xunit.TraitAttribute("FeatureTitle", "check ukaddress component")]
        [Xunit.TraitAttribute("Description", "user enters address and sees the next page")]
        public async System.Threading.Tasks.Task UserEntersAddressAndSeesTheNextPage()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters address and sees the next page", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 12
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
#line 13
    await testRunner.WhenAsync("the user enters \'Fake House\' in the \'what_is_your_address\' Address Line 1 uk addr" +
                        "ess field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 14
    await testRunner.AndAsync("the user enters \'123 Fake St\' in the \'what_is_your_address\' Address Line 2 uk add" +
                        "ress field", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 15
    await testRunner.AndAsync("the user enters \'Faketown\' in the \'what_is_your_address\' Town uk address field", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 16
    await testRunner.AndAsync("the user enters \'Fakeshire\' in the \'what_is_your_address\' County uk address field" +
                        "", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 17
    await testRunner.AndAsync("the user enters \'FA9 9KE\' in the \'what_is_your_address\' Postcode uk address field" +
                        "", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 18
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 19
    await testRunner.ThenAsync("the user sees the summary page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 20
    await testRunner.AndAsync("the summary page shows the answer to \'what_is_your_address\' is \'Fake House, 123 F" +
                        "ake St, Faketown, Fakeshire, FA9 9KE\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
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
                await CheckUkaddressComponentFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await CheckUkaddressComponentFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
