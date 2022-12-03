using NUnit.Framework;
using Propeller.Integration.Tests.Drivers;
using Propeller.Integration.Tests.Support;
using Propeller.Models;
using Propeller.Models.Requests;
using System;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;

namespace Propeller.Integration.Tests.StepDefinitions
{
    [Binding]
    public class NoteManagementStepDefinitions : StepDefinitionsBase
    {
        private readonly NotesDriver _notesDriver;
        // private readonly CustomerDriver _customerDriver;

        public NoteManagementStepDefinitions(ScenarioContext scenarioContext,
            FeatureContext featureContext)
            : base(featureContext, scenarioContext)
        {
            //_scenarioContext = scenarioContext;
            //_featureContext = featureContext;
            _notesDriver = new NotesDriver(scenarioContext, featureContext);
            // _customerDriver = new CustomerDriver(scenarioContext, featureContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteText"></param>
        /// <returns></returns>
        [Given(@"I add a new Note with text: ""([^""]*)""")]
        [Then(@"I add a new Note with text: ""([^""]*)""")]
        public async Task WhenIAddANewNoteWithId(string noteText)
        {
            string customerId = _scenarioContext.Get<string>(ContextKeys.NewCustomerId);
            var result = await _notesDriver.CreateNewNote(customerId, noteText);

            Assert.AreEqual(HttpStatusCode.Created, result.apiResponse.StatusCode);
            Assert.IsNotNull(result.note);

            _scenarioContext.Add(ContextKeys.NewNote, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteText"></param>
        [Then(@"I verify the note with text: ""([^""]*)"" exists")]
        public void ThenIVerifyTheNoteWithTextExists(string noteText)
        {
            CustomerDto customer = GetScenarioCurrentCustomer();
            var note = customer.Notes.Where(x => x.Text.Equals(noteText)).FirstOrDefault();

            Assert.IsNotNull(note);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteText"></param>
        /// <returns></returns>
        [Then(@"I delete the Note with text: ""([^""]*)""")]
        public async Task ThenIDeleteTheNoteWithText(string noteText)
        {
            CustomerDto customer = GetScenarioCurrentCustomer();

            var note = customer.Notes.Where(x => x.Text.Equals(noteText)).FirstOrDefault();

            Assert.IsNotNull(note);

            var statusCode = await _notesDriver.DeleteNote(customer.ID, note.ID);

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            // throw new PendingStepException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        [Given(@"A Customer Exists")]
        public async void GivenACustomerExists(Table table)
        {

            var noteDto = _scenarioContext.Get<NoteDto>(ContextKeys.NewNote);

            CreateCustomerRequest request = new CreateCustomerRequest
            {
                Name = "New Client",
                Status = ""
            };


            //var result = await _notesDriver.ExecutePost("https://localhost:7270/api/notes/MQ==", "Test Note");

            //Assert.IsNotNull(result);

            // throw new PendingStepException();
        }


        [Then(@"I verify the Customer does not have any Notes")]
        public async Task ThenIVerifyTheCustomerDoesNotHaveAnyNotes()
        {
            var customerId = _scenarioContext.Get<string>(ContextKeys.NewCustomerId);

            var result = await _notesDriver.RetrieveCustomerNotes(customerId);

            // TODO: This might not be useful since it's a tuple
            result.Should().NotBeNull();

            Assert.AreEqual(HttpStatusCode.OK, result.statusCode);
            Assert.IsEmpty(result.notes);
        }



        [When(@"I try to add a new Note with Text: ""([^""]*)"" to a Client with Id: (.*)")]
        public async Task WhenITryToAddANewNoteWithTextToAClientWithId(string noteText, int customerId)
        {
            string cid = Obfuscator.ObfuscateId(customerId);
            (ApiResponse ApiResponse, NoteDto? Note) result = await _notesDriver.CreateNewNote(cid, noteText);

            SetScenarioLatestStatusCode(result.ApiResponse.StatusCode);

            _scenarioContext.Set<ApiResponse>(result.ApiResponse, ContextKeys.LastReturnedApiResponse);

            // _lastReturnedStatusCode = result.apiResponse.StatusCode;

            // Assert.AreEqual(HttpStatusCode.OK, result.statusCode);
            // Assert.IsNotNull(result.note);

            // _scenarioContext.Add(ContextKeys.NewNote, result);
        }



        [Then(@"I try to add a New Note with Text: ""([^""]*)""")]
        public async Task ThenITryToAddANewNoteWithText(string noteText)
        {
            (ApiResponse ApiResponse, NoteDto? Note) result = await AddNote(noteText);

            // TODO: Maybe remove this next one and just use the ApiResponse
            SetScenarioLatestStatusCode(result.ApiResponse.StatusCode);
            _scenarioContext.Set<ApiResponse>(result.ApiResponse, ContextKeys.LastReturnedApiResponse);

            // var latestReturnedApiResponse = _scenarioContext.Get<ApiResponse>(ContextKeys.LastReturnedApiResponse);


            //Assert.AreEqual(HttpStatusCode.OK, result.statusCode);
            //Assert.IsNotNull(result.note);

            //_scenarioContext.Add(ContextKeys.NewNote, result);
        }

        private async Task<(ApiResponse apiResponse, NoteDto? note)> AddNote(string noteText)
        {
            string customerId = _scenarioContext.Get<string>(ContextKeys.NewCustomerId);
            return await _notesDriver.CreateNewNote(customerId, noteText);
            //_latestApiResponse = result.apiResponse;
            //_lastReturnedStatusCode = result.apiResponse.StatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringLength"></param>
        [Then(@"I try to add a New Note with (.*) characters")]
        public async Task ThenITryToAddANewNoteWithCharacters(int stringLength)
        {
            string longString = new string('*', stringLength);
            (ApiResponse ApiResponse, NoteDto? Note) result = await AddNote(longString);

            // TODO: Maybe remove this next one and just use the ApiResponse
            SetScenarioLatestStatusCode(result.ApiResponse.StatusCode);
            _scenarioContext.Set<ApiResponse>(result.ApiResponse, ContextKeys.LastReturnedApiResponse);
        }

        [Then(@"I verify the Response is ""([^""]*)""")]
        public void ThenIVerifyTheResponseIs(string expectedRespoonse)
        {
            var latestReturnedApiResponse = _scenarioContext.Get<ApiResponse>(ContextKeys.LastReturnedApiResponse);
           Assert.AreEqual(latestReturnedApiResponse.ResponseBody, expectedRespoonse);
        }


        /// <summary>
        /// 
        /// </summary>
        [AfterScenario]
        public void AfterScenarioNoteManagement()
        {
            Console.Out.WriteLine("-AfterScenarioNoteManagement");

            // Delete Newly created note

            // string insertedCustomerId = 

            Console.Out.WriteLine("Here Notes");
            //if (_context.TryGetValue<Guid>(ContextKeys.ConfigurationId, out Guid configurationId))
            //{
            //    _configureIntegrationDriver.RemoveIntegrationConfiguration(configurationId);
            //}
        }

    }
}
