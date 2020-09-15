using System.Collections.Generic;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.StepArgumentTransformations
{
    [Binding]
    public class ProcessNisraCasesStepArgumentTransformations
    {
        [StepArgumentTransformation]
        public IEnumerable<CaseModel> TransformCasesTableIntoListOfCaseModels(Table table)
        {
            return table.CreateSet<CaseModel>();
        }
    }
}
