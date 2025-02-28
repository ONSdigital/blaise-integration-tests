﻿using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CreateCasesSteps
    {
        [Given(@"I have created sample cases for the questionnaire")]
        [When(@"I create cases for the questionnaire")]
        public void WhenICreateCasesForTheQuestionnaire(IEnumerable<CaseModel> caseModels)
        {
            CaseHelper.GetInstance().DeleteCases();
            CaseHelper.GetInstance().CreateCases(caseModels);
        }

        [Then(@"the cases are available in the questionnaire")]
        public void ThenTheCasesAreAvailableInTheQuestionnaire(IEnumerable<CaseModel> cases)
        {
            var expectedCases = cases.ToList();
            CheckNumberOfCasesMatch(expectedCases.Count);
            CheckCasesMatch(expectedCases);
        }

        private void CheckNumberOfCasesMatch(int expectedNumberOfCases)
        {
            var actualNumberOfCases = CaseHelper.GetInstance().NumberOfCasesInQuestionnaire();

            if (expectedNumberOfCases != actualNumberOfCases)
            {
                Assert.Fail($"Expected '{expectedNumberOfCases}' cases in Blaise, but {actualNumberOfCases} cases were found");
            }
        }

        private void CheckCasesMatch(IEnumerable<CaseModel> expectedCases)
        {
            var actualCases = CaseHelper.GetInstance().GetCasesInBlaise().ToList();

            foreach (var expectedCase in expectedCases)
            {
                var actualCase = actualCases.FirstOrDefault(c => c.PrimaryKey == expectedCase.PrimaryKey);

                Assert.That(actualCase, Is.Not.Null,
                    $"Case '{expectedCase.PrimaryKey}' was not found in Blaise");

                Assert.That(actualCase, Is.EqualTo(expectedCase),
                    $"Case '{expectedCase.PrimaryKey}' did not match the case in Blaise");
            }
        }

        [AfterScenario("create-cases")]
        public void AfterScenario()
        {
            CaseHelper.GetInstance().DeleteCases();
        }

        [AfterFeature("create-cases")]
        public static void AfterFeature()
        {
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }
    }
}
