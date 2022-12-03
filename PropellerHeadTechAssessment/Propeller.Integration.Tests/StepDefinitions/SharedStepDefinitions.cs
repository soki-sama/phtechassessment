using LivingDoc.Dtos;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Propeller.Integration.Tests.Drivers;
using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Propeller.Integration.Tests.StepDefinitions
{
    [Binding]
    public class SharedStepDefinitions : StepDefinitionsBase
    {
        // TODO: Create a base file for this

        private readonly CustomerStatusDriver _customerStatusDriver;
        string serverAddress = $"https://localhost:7270";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureContext"></param>
        /// <param name="scenarioContext"></param>
        public SharedStepDefinitions(FeatureContext featureContext, ScenarioContext scenarioContext)
            : base(featureContext, scenarioContext)
        {
            _customerStatusDriver = new CustomerStatusDriver(scenarioContext, featureContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        [BeforeScenario]
        public async Task Before()
        {
            //TODO Change this to run once per run
            // Load the customer dtatuses
            (HttpStatusCode StatusCode, IEnumerable<CustomerStatusDto> statuses) result
                = await _customerStatusDriver.RetrieveCurrentStatuses(serverAddress);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                // TODO. maybe change for assert fail
                throw new Exception("Unable to fetch Customer Statuses");
            }

            _featureContext.Set(result.statuses, ContextKeys.CustomerStatuses);


        }



        [Given(@"I use an Authenticated ""([^""]*)"" User")]
        public async void GivenIUseAnAuthenticatedUser(string userProfile)
        {
            // TODO: See about changing this to Task to avoid the warning
            UserProfile cprof;

            if (!Enum.TryParse<UserProfile>(userProfile, true, out cprof))
            {
                Assert.Fail("Invalid User Profile");
            }

            _scenarioContext.Set(cprof, ContextKeys.CurrentUserProfile);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="admin"></param>
        /// <exception cref="PendingStepException"></exception>
        [Given(@"I use and Authenticated ""([^""]*)"" User")]
        public void XGivenIUseAnAuthenticatedUser(string userProfile)
        {
            //UserProfile cprof;

            //if (!Enum.TryParse<UserProfile>(userProfile, out cprof))
            //{
            //    Assert.Fail("Invalid User Profile");
            //}



        }

        [Then(@"I verify the returned Http Status Code was ""(.*)""")]
        public void ThenIVerifyTheReturnedStatusCodeWas(HttpStatusCode expectedStatusCode)
        {
            HttpStatusCode statusCode;

            if (!_scenarioContext.TryGetValue<HttpStatusCode>(ContextKeys.LastReturnedStatusCode, out statusCode))
            {
                Assert.Fail($"Unable to Retrieve {ContextKeys.LastReturnedStatusCode}");
            }

            Assert.AreEqual(expectedStatusCode, statusCode);
        }

        [After]
        public async Task AfterScenarioTestCleanup()
        {
            Console.Out.WriteLine("-AfterScenarioTestCleanup");

            List<Func<Task>> cleanupActions;

            if (!_scenarioContext.TryGetValue(ContextKeys.CleanUp, out cleanupActions))
            {
                return;
            }

            foreach (Func<Task> action in cleanupActions)
            {
                await action();
            }
        }


    }
}