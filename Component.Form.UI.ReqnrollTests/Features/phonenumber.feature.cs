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
    public partial class CheckPhonenumberComponentFeature : object, Xunit.IClassFixture<CheckPhonenumberComponentFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "check phonenumber component", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "phonenumber.feature"
#line hidden
        
        public CheckPhonenumberComponentFeature(CheckPhonenumberComponentFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
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
    await testRunner.GivenAsync("the user is on the \'http://localhost:5164/form/phonenumberComponent/start\' page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 5
    await testRunner.AndAsync("the user can see the \'what_is_your_phone_number\' question, with text \'What is you" +
                    "r phone number?\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
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
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
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
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a space, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters a space, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersASpaceClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a space, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
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
    await testRunner.WhenAsync("the user enters \' \' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 13
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 14
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters not a number, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters not a number, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersNotANumberClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters not a number, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 16
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
#line 17
    await testRunner.WhenAsync("the user enters \'not a number\' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 18
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 19
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a short number, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters a short number, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersAShortNumberClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a short number, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 21
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
#line 22
    await testRunner.WhenAsync("the user enters \'567890\' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 23
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 24
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters a long number, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters a long number, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersALongNumberClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters a long number, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 26
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
#line 27
    await testRunner.WhenAsync("the user enters \'01234 567890 987654\' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 28
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 29
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters an american number, clicks continue and sees a validation error")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters an american number, clicks continue and sees a validation error")]
        public async System.Threading.Tasks.Task UserEntersAnAmericanNumberClicksContinueAndSeesAValidationError()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters an american number, clicks continue and sees a validation error", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 31
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
#line 32
    await testRunner.WhenAsync("the user enters \'+1-555-123-4567\' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 33
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 34
    await testRunner.ThenAsync("the user sees an error message, \'Enter a UK phone number\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="user enters phone number and sees the next page")]
        [Xunit.TraitAttribute("FeatureTitle", "check phonenumber component")]
        [Xunit.TraitAttribute("Description", "user enters phone number and sees the next page")]
        public async System.Threading.Tasks.Task UserEntersPhoneNumberAndSeesTheNextPage()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("user enters phone number and sees the next page", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 36
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
#line 37
    await testRunner.WhenAsync("the user enters \'01234 567890\' in the \'what_is_your_phone_number\' field", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 38
    await testRunner.AndAsync("the user clicks continue", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 39
    await testRunner.ThenAsync("the user sees the summary page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 40
 await testRunner.AndAsync("the summary page shows the answer to \'what_is_your_phone_number\' is \'01234 567890" +
                        "\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
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
                await CheckPhonenumberComponentFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await CheckPhonenumberComponentFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
