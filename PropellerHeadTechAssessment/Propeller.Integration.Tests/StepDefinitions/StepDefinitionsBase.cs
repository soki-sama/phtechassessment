using NUnit.Framework;
using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;

namespace Propeller.Integration.Tests.StepDefinitions
{

    public class StepDefinitionsBase
    {

        protected readonly FeatureContext _featureContext;
        protected readonly ScenarioContext _scenarioContext;

        // protected HttpStatusCode _lastReturnedStatusCode;
        // protected ApiResponse _latestApiResponse;

        public StepDefinitionsBase(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;

        }

        protected void SetScenarioCurrentContact(ContactDto contact)
        {
            Assert.IsNotNull(contact);

            _scenarioContext.Set(contact, ContextKeys.CurrentContact);
        }

        protected ContactDto GetScenarioCurrentContact()
        {
            ContactDto contact;

            if (_scenarioContext.TryGetValue<ContactDto>(ContextKeys.CurrentContact, out contact))
            {
                return contact;
            }

            Assert.Fail("Unable to retrieve Current Contact");
            return contact;
        }

        protected void SetScenarioCurrentCustomer(CustomerDto customer)
        {
            Assert.IsNotNull(customer);

            _scenarioContext.Set(customer, ContextKeys.CurrentCustomerX);
        }

        protected CustomerDto GetScenarioCurrentCustomer()
        {
            CustomerDto customer;

            if (_scenarioContext.TryGetValue<CustomerDto>(ContextKeys.CurrentCustomerX, out customer))
            {
                return customer;
            }

            Assert.Fail("Unable to retrieve Current Customer");
            return customer;
        }

        protected void SetScenarioLatestStatusCode(HttpStatusCode statusCode)
        {
            Assert.IsNotNull(statusCode);

            _scenarioContext.Set(statusCode, ContextKeys.LastReturnedStatusCodeX);
        }

        protected HttpStatusCode GetScenarioLatestStatusCode()
        {
            HttpStatusCode statusCode;

            if (!_scenarioContext.TryGetValue<HttpStatusCode>(ContextKeys.LastReturnedStatusCodeX, out statusCode))
            {
                Assert.Fail($"Unable to Retrieve {ContextKeys.LastReturnedStatusCodeX}");
            }

            return statusCode;
        }


    }
}
