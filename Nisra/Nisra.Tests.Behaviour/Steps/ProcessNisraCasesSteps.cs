using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Enums;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers;
using BlaiseNisraCaseProcessor.Tests.Behaviour.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class ProcessNisraCasesSteps
    {
        private readonly ScenarioContext _scenarioContext;

        private readonly int _defaultOutcome;
        private readonly ModeType _defaultMode;
        private readonly string _bucketName;

        private readonly NisraFileHelper _nisraFileHelper;
        private readonly CaseHelper _caseHelper;
        private readonly BucketHelper _bucketHelper;
        private readonly PubSubHelper _pubSubHelper;

        public ProcessNisraCasesSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _bucketName = "ons-blaise-dev-jam44-nisra";
            _defaultOutcome = 110;
            _defaultMode = ModeType.Web;

            _nisraFileHelper = new NisraFileHelper();
            _caseHelper = new CaseHelper();
            _bucketHelper = new BucketHelper();
            _pubSubHelper = new PubSubHelper();
        }

        [Given(@"there is a not a Nisra file available")]
        public void GivenThereIsANotANisraFileAvailable()
        {
        }

        [Given(@"there is a Nisra file that contains '(.*)' cases")]
        public void GivenThereIsANisraFileAvailable(int numberOfCases)
        {
            var nisraFilePath = _nisraFileHelper.CreateDatabaseFilesAndFolder();

            _caseHelper.CreateCases(nisraFilePath, numberOfCases, _defaultOutcome, _defaultMode);
            UploadNisraFile(nisraFilePath);

            _scenarioContext.Set(nisraFilePath, "nisraFilePath");
        }

        [Given(@"blaise contains no cases")]
        public void GivenTheBlaiseDatabaseIsEmpty()
        {
            _caseHelper.DeleteCasesInDatabase();
        }

        [Given(@"blaise contains '(.*)' cases")]
        public void GivenBlaiseContainsCases(int numberOfCases)
        {
            _caseHelper.CreateCasesInDatabase(numberOfCases, _defaultOutcome, _defaultMode);
        }


        [Given(@"there is a Nisra file that contains the following cases")]
        public void GivenTheNisraFileContainsCasesToProcess(IEnumerable<CaseModel> cases)
        {
            var nisraFilePath = _nisraFileHelper.CreateDatabaseFilesAndFolder();

            _caseHelper.CreateCases(nisraFilePath, cases);
            UploadNisraFile(nisraFilePath);

            _scenarioContext.Set(nisraFilePath, "nisraFilePath");

        }

        [Given(@"blaise contains the following cases")]
        public void GivenTheBlaiseDatabaseAlreadyContainsCases(IEnumerable<CaseModel> cases)
        {
            _caseHelper.DeleteCasesInDatabase();

            _caseHelper.CreateCasesInDatabase(cases);
        }


        [When(@"the nisra process is triggered")]
        public void UploadNisraFileAndTriggerProcess()
        {
            _pubSubHelper.PublishMessage(@"{ ""action"": ""process""}");

            Thread.Sleep(60000);
        }

        [When(@"the nisra file is triggered every '(.*)' minutes for '(.*)' hour\(s\)")]
        public void WhenTheNisraFileIsProcessedEveryMinutesForHours(int minutes, int hours)
        {
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalHours < hours)
            {
                Console.WriteLine("Start process at" + DateTime.Now);
                UploadNisraFileAndTriggerProcess();

                Console.WriteLine("Sleep for " + minutes + " minutes");
                Thread.Sleep(minutes * 60 * 1000);
            }

            Console.WriteLine("Finished after " + hours + " hour(s)");
        }
        
        [Then(@"blaise will contain '(.*)' cases")]
        public void ThenCasesWillBeImportedIntoBlaise(int numberOfCases)
        {
            var numberOfCasesInBlaise = _caseHelper.GetNumberOfCasesInDatabase();

            Assert.AreEqual(numberOfCases, numberOfCasesInBlaise);
        }

        [Then(@"blaise will contain the following cases")]
        public void ThenTheBlaiseDatabaseWillContainTheFollowingCases(IEnumerable<CaseModel> cases)
        {
            var numberOfCasesInDatabase = _caseHelper.GetNumberOfCasesInDatabase();
            var casesExpected = cases.ToList();

            if (casesExpected.Count != numberOfCasesInDatabase)
            {
                Assert.Fail($"Expected '{casesExpected.Count}' cases in the database, but {numberOfCasesInDatabase} cases were found");
            }

            var casesInDatabase = _caseHelper.GetCasesInDatabase();

            foreach (var caseModel in casesInDatabase)
            {
                var caseRecordExpected = casesExpected.FirstOrDefault(c => c.PrimaryKey == caseModel.PrimaryKey);

                if (caseRecordExpected == null)
                {
                    Assert.Fail($"Case {caseModel.PrimaryKey} was in the database but not found in expected cases");
                }

                Assert.AreEqual(caseRecordExpected.Outcome, caseModel.Outcome, $"expected an outcome of '{caseRecordExpected.Outcome}' for case '{caseModel.PrimaryKey}'," +
                                                                               $"but was '{caseModel.Outcome}'");
                Assert.AreEqual(caseRecordExpected.Mode, caseModel.Mode, $"expected an version of '{caseRecordExpected.Mode}' for case '{caseModel.PrimaryKey}'," +
                                                                               $"but was '{caseModel.Mode}'");
            }
        }

        [AfterScenario]
        public static void CleanUpFiles()
        {
            var nisraFileHelper = new NisraFileHelper();
            nisraFileHelper.DeleteDatabaseFilesAndFolder();

            var caseHelper = new CaseHelper();
            caseHelper.DeleteCasesInDatabase();
        }

        private void UploadNisraFile(string nisraFilePath)
        {
            var databaseFilePath = Path.GetDirectoryName(nisraFilePath);

            if (string.IsNullOrWhiteSpace(databaseFilePath))
            {
                throw new Exception("The path to the database files does not exist");
            }

            foreach (var file in Directory.GetFiles(databaseFilePath))
            {
                _bucketHelper.UploadToBucket(file, _bucketName);
            }
        }
    }
}
