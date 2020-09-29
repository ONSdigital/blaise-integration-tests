using System.Collections.Generic;
using System.Linq;
using StatNeth.Blaise.Shared.Collections;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.StepArgumentTransformations.Case
{
    [Binding]
    public class ProcessNisraCasesStepArgumentTransformations
    {
        [StepArgumentTransformation]
        public Dictionary<string, string> TransformFieldDataTableIntoListOfCaseModels(Table table)
        {
            return table.Rows.ToDictionary(r => r[0], r => r[1]);
        }

        [StepArgumentTransformation]
        public IEnumerable<string> TransformFieldDataTableIntoListOfStrings(Table table)
        {
            return table.Rows.ToList(r => r[0]);
        }
    }
}
