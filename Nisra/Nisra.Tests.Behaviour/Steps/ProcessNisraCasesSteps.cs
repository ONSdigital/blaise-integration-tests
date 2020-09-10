using System;
using System.IO;
using Blaise.Nuget.Api.Contracts.Enums;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class ProcessNisraCasesSteps
    {
        private readonly ScenarioContext _scenarioContext;

        private readonly WebFormStatusType _defaultStatus;
        private readonly int _defaultOutcome;
        private readonly string _bucketName;

        private readonly NisraFileHelper _nisraFileHelper;
        private readonly CaseHelper _caseHelper;
        private readonly BucketHelper _bucketHelper;
        private readonly PubSubHelper _pubSubHelper;

        public ProcessNisraCasesSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _bucketName = "ons-blaise-dev-jam44-nisra";
            _defaultStatus = WebFormStatusType.Complete;
            _defaultOutcome = 110;

            _nisraFileHelper = new NisraFileHelper();
            _caseHelper = new CaseHelper();
            _bucketHelper = new BucketHelper();
            _pubSubHelper = new PubSubHelper();
        }

        [Given(@"there is a Nisra file available")]
        public void GivenThereIsANisraFileAvailable()
        {
            var nisraFilePath = _nisraFileHelper.CreateDatabaseFilesAndFolder();

            _scenarioContext.Set(nisraFilePath, "nisraFilePath");
        }

        [Given(@"the blaise database is empty")]
        public void GivenTheBlaiseDatabaseIsEmpty()
        {
            var numberOfCasesInBlaise = _caseHelper.GetNumberOfCasesInDatabase();

            Assert.AreEqual(0, numberOfCasesInBlaise);
        }


        [Given(@"the file contains '(.*)' cases")]
        public void GivenTheFileContainsCases(int numberOfCases)
        {
            var nisraFilePath = _scenarioContext.Get<string>("nisraFilePath");

            _caseHelper.CreateCase(nisraFilePath, numberOfCases,
                _defaultStatus, _defaultOutcome);

            Assert.IsTrue(true);
        }

        [Given(@"the file contains '(.*)' cases which are '(.*)' with an outcome of '(.*)'")]
        public void GivenTheFileContainsCasesWhichAreWithAnOutcomeOf(int numberOfCases,
            WebFormStatusType status, int outcome)
        {
            var nisraFilePath = _scenarioContext.Get<string>("nisraFilePath");

            _caseHelper.CreateCases(nisraFilePath, numberOfCases, status, outcome);

            Assert.IsTrue(true);
        }


        [When(@"the file is processed")]
        public void WhenTheFileIsProcessed()
        {
            var nisraFilePath = _scenarioContext.Get<string>("nisraFilePath");

            if (string.IsNullOrWhiteSpace(nisraFilePath))
            {
                throw new Exception("The file path to the database is not set");
            }

            var databaseFilePath = Path.GetDirectoryName(nisraFilePath);

            if(string.IsNullOrWhiteSpace(databaseFilePath))
            {
                throw new Exception("The path to the database files does not exist");
            }

            foreach (var file in Directory.GetFiles(databaseFilePath))
            {
                _bucketHelper.UploadToBucket(file, _bucketName);
            }

            _pubSubHelper.PublishMessage(@"{ ""action"": ""process""}");
        }

        [Then(@"'(.*)' cases will be imported into blaise")]
        public void ThenCasesWillBeImportedIntoBlaise(int numberOfCases)
        {
            var numberOfCasesInBlaise = _caseHelper.GetNumberOfCasesInDatabase();

            Assert.AreEqual(numberOfCases, numberOfCasesInBlaise);
        }


        [AfterScenario]
        public static void CleanUpFiles()
        {
            var nisraFileHelper = new NisraFileHelper();
            nisraFileHelper.DeleteDatabaseFilesAndFolder();
        }
    }
}
