namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Models.Case;
    using NUnit.Framework;
    using Reqnroll;
    using Blaise.Tests.Helpers.Cati.Pages;

    [Binding]
    public sealed class DaybatchSteps
    {
        [Then(@"the sample cases are present on the daybatch page")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchPage(IEnumerable<CaseModel> cases)
        {
            var daybatchPage = new DaybatchPage();

            Console.WriteLine("Navigating to the Daybatch page...");
            daybatchPage.NavigateToVersionSpecificPage();

            if (daybatchPage.IsUsingNewSelectors)
            {
                // Logic for the new dashboard
                var entriesText = daybatchPage.GetDaybatchEntriesText();

                Assert.That(
                    entriesText,
                    Is.Not.Null.And.Not.Empty,
                    "The daybatch entries text should not be null or empty (new dashboard)");
            }
            else
            {
                // Logic for the old dashboard
                var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();

                Assert.That(
                    entriesText,
                    Is.Not.Null.And.Not.Empty,
                    "The daybatch entries text should not be null or empty (old dashboard)");
            }
        }
    }
}
