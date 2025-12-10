namespace Blaise.Tests.Behaviour.Transforms
{
    using System.Collections.Generic;
    using Blaise.Tests.Models.Case;
    using Reqnroll;

    [Binding]
    public class CaseStepArgumentTransformations
    {
        [StepArgumentTransformation]
        public IEnumerable<CaseModel> TransformCaseTableIntoListCaseModel(Table table)
        {
            return table.CreateSet<CaseModel>();
        }
    }
}
