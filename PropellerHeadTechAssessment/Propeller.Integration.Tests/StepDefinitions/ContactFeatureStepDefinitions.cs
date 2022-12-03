using NUnit.Framework;
using Propeller.Integration.Tests.Drivers;
using Propeller.Integration.Tests.Support;
using Propeller.Models;
using Propeller.Models.Requests;
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Security.Cryptography;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.CommonModels;

namespace Propeller.Integration.Tests.StepDefinitions
{
    [Binding]
    public class ContactFeatureStepDefinitions : StepDefinitionsBase
    {

        private readonly ContactDriver _contactDriver;

        public ContactFeatureStepDefinitions(ScenarioContext scenarioContext,
            FeatureContext featureContext)
            : base(featureContext, scenarioContext)
        {
            _contactDriver = new ContactDriver(scenarioContext, featureContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        [When(@"I add a new Contact with data")]
        public async Task WhenIAddANewContactWithData(Table dataTable)
        {
            // TODO: Address this possible nullref
            CreateContactRequest newContact = dataTable.CreateSet<CreateContactRequest>().FirstOrDefault();

            if (newContact == null)
            {
                Assert.Fail("Invalid New Contact Data");
            }

            // Customer Id is set as an int on the table, but we need to obfuscate it to send
            if (!string.IsNullOrEmpty(newContact.CustomerID))
            {
                int cid;

                if (!int.TryParse(newContact.CustomerID, out cid))
                {
                    Assert.Fail("Invalid Customer ID");
                }

                newContact.CustomerID = cid.Obfuscate();
            }

            (ApiResponse ApiResponse, ContactDto? Contact) result = await _contactDriver.AddNewContact(newContact);

            if (result.ApiResponse.StatusCode == HttpStatusCode.Created)
            {
                Assert.IsNotNull(result.Contact);

                // Save the Id for the newly created Contact
                // _currentCustomer = result.Customer; // TODO: Should I clean this up and use it like this or inside a context?
                _scenarioContext.Set(result.Contact.Id, ContextKeys.NewContactId);
            }

            _scenarioContext.Set(result.ApiResponse.StatusCode, ContextKeys.LastReturnedStatusCode);
            _scenarioContext.Set(result.ApiResponse, ContextKeys.LastReturnedApiResponse);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0"></param>
        /// <exception cref="PendingStepException"></exception>
        [When(@"I retrieve Contacts with Search Criteria: ""([^""]*)""")]
        public async Task WhenIRetrieveContactsWithSearchCriteria(string searchCriteria)
        {
            (HttpStatusCode statusCode, List<ContactDto>? contacts) result
                       = await _contactDriver.RetrieveContactsBySearch(searchCriteria, string.Empty, 0, 0);

            // Search should return Ok regardless of result
            Assert.AreEqual(HttpStatusCode.OK, result.statusCode);

            _scenarioContext.Set(result.contacts, ContextKeys.FoundCustomers);
        }

        [Then(@"I verify only (.*) record\(s\) were retrieved")]
        public void ThenIVerifyOnlyRecordSWereRetrieved(int recordsExpected)
        {
            List<ContactDto> foundContacts = new List<ContactDto>();

            if (!_scenarioContext.TryGetValue<List<ContactDto>>(ContextKeys.FoundContacts, out foundContacts))
            {
                Assert.Fail("Unable to retrieve Found Contacts");
            }

            foundContacts.Should().HaveCount(recordsExpected);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0"></param>
        /// <exception cref="PendingStepException"></exception>
        [Then(@"I should have retrieved (.*) Contact\(s\)")]
        public void WhenIShouldHaveRetrievedContactS(int contactsExpected)
        {
            List<ContactDto> contacts = _scenarioContext.Get<List<ContactDto>>(ContextKeys.FoundCustomers);

            if (contacts == null)
            {
                throw new Exception("No Contracts in Context");
            }

            contacts.Count.Should().Be(contactsExpected);
        }

        [Then(@"I check the Contact has First Name: ""([^""]*)"" and Last Name: ""([^""]*)""")]
        public void ThenICheckTheContactHasFirstNameAndLastName(string firstName, string lastName)
        {
            List<ContactDto> contacts = _scenarioContext.Get<List<ContactDto>>(ContextKeys.FoundCustomers);

            if (contacts == null)
            {
                throw new Exception("No Contracts in Context");
            }

            if (!contacts.Any())
            {
                throw new Exception("No Contracts found in Context");
            }

            // Retrieve the First row and check
            ContactDto? contact = contacts.FirstOrDefault();

            Assert.IsNotNull(contact);

            contact.FirstName.Should().Be(firstName);
            contact.LastName.Should().Be(lastName);
        }

        [Then(@"I add a new Contact with data for the recently created Customer")]
        public async Task ThenIAddANewContactWithDataForTheRecentlyCreatedCustomer(Table dataTable)
        {

            string recentlyCreatedCustomerId = _scenarioContext.Get<string>(ContextKeys.NewCustomerId);

            if (string.IsNullOrEmpty(recentlyCreatedCustomerId))
            {
                Assert.Fail("Unable to retrieve CustomerID");
            }

            CreateContactRequest newContact = dataTable.CreateSet<CreateContactRequest>().FirstOrDefault();

            if (newContact == null)
            {
                Assert.Fail("Invalid New Contact Data");
            }

            newContact.CustomerID = recentlyCreatedCustomerId;

            (ApiResponse ApiResponse, ContactDto? Contact) result = await _contactDriver.AddNewContact(newContact);

            if (result.ApiResponse.StatusCode == HttpStatusCode.Created)
            {
                Assert.IsNotNull(result.Contact);

                // Save the Id for the newly created Contact
                // _currentCustomer = result.Customer; // TODO: Should I clean this up and use it like this or inside a context?
                _scenarioContext.Set(result.Contact.Id, ContextKeys.NewContactId);
            }

            _scenarioContext.Set(result.ApiResponse.StatusCode, ContextKeys.LastReturnedStatusCode);
            _scenarioContext.Set(result.ApiResponse, ContextKeys.LastReturnedApiResponse);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        [Then(@"I verify it contains a Contact with Email: ""([^""]*)""")]
        public void ThenIVerifyItContainsAContactWithEmail(string email)
        {
            CustomerDto customer = _scenarioContext.Get<CustomerDto>(ContextKeys.CurrentCustomer);

            if (customer == null)
            {
                Assert.Fail("Unable to retrieve Customer"); // TODO: Change this description
            }

            customer.Contacts.Should().HaveCountGreaterThanOrEqualTo(1);

            ContactDto contact = customer.Contacts.FirstOrDefault();

            Assert.AreEqual(email, contact.Email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactEmail"></param>
        /// <returns></returns>
        [When(@"I retrieve a Contact with Email: ""([^""]*)""")]
        public async Task WhenIRetrieveAContactWithEmail(string contactEmail)
        {

            (HttpStatusCode StatusCode, List<ContactDto>? Contacts) result
                = await _contactDriver.RetrieveContactsBySearch(contactEmail, "Email", 0, 0);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                // Regardless of number of records fetched or parameters, Search should always return 200
                Assert.Fail("Error Retrieving Contacts");
            }

            result.Contacts.Should().HaveCountGreaterThanOrEqualTo(1);

            _scenarioContext.Set(result.Contacts, ContextKeys.FoundContacts);
        }

        [When(@"I forcefully remove the recently created Contact")]
        public async Task WhenIRemoveTheRecentlyCreatedContact()
        {
            int newContactId;

            if (!_scenarioContext.TryGetValue<int>(ContextKeys.NewContactId, out newContactId))
            {
                Assert.Fail("Unable to retrieve Newly Created ContactId");
            }

            HttpStatusCode statusCode = await _contactDriver.DeleteContact(newContactId, true);

            Assert.AreEqual(HttpStatusCode.NoContent, statusCode);
        }

        [Then(@"I verify it does not contain a Contact with Email: ""([^""]*)""")]
        public void ThenIVerifyItDoesNotContainAContactWithEmail(string p0)
        {

            CustomerDto customer;

            if (!_scenarioContext.TryGetValue<CustomerDto>(ContextKeys.CurrentCustomer, out customer))
            {
                Assert.Fail("Unable to Retrieve Current Customer");
            }

            customer.Contacts.Should().HaveCount(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0"></param>
        /// <exception cref="PendingStepException"></exception>
        [When(@"I try to Delete a Contact with Id: (.*)")]
        public async Task WhenITryToDeleteAContactWithId(int contactId)
        {
            HttpStatusCode statusCode = await _contactDriver.DeleteContact(contactId, true);
            _scenarioContext.Set(statusCode, ContextKeys.LastReturnedStatusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="PendingStepException"></exception>
        [When(@"I non forcefully try to remove the recently created Contact")]
        public async Task WhenINonForcefullyTryToRemoveTheRecentlyCreatedContact()
        {
            int contactId;

            if (!_scenarioContext.TryGetValue<int>(ContextKeys.NewContactId, out contactId))
            {
                Assert.Fail("Unable to retrieve the last created ContactId");
            }

            HttpStatusCode result = await _contactDriver.DeleteContact(contactId, false);

            _scenarioContext.Set(result, ContextKeys.LastReturnedStatusCode);
        }


        [Given(@"I am working")]
        public void GivenIAmWorking()
        {
            throw new PendingStepException();
        }

        [When(@"I try comething")]
        public void WhenITryComething()
        {
            throw new PendingStepException();
        }

        [Then(@"I check this")]
        public void ThenICheckThis()
        {
            throw new PendingStepException();
        }
    }
}
