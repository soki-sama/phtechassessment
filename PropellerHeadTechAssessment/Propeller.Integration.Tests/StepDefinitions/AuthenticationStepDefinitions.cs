using NUnit.Framework;
using Propeller.Integration.Tests.Drivers;
using Propeller.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Integration.Tests.StepDefinitions
{
    [Binding]
    internal class AuthenticationStepDefinitions : StepDefinitionsBase
    {
        //private readonly ScenarioContext _scenarioContext;
        //private readonly FeatureContext _featureContext;
        private readonly AuthenticationDriver _authDriver;

        public AuthenticationStepDefinitions(ScenarioContext scenarioContext,
            FeatureContext featureContext) :
            base(featureContext, scenarioContext)
        {
            //_scenarioContext = scenarioContext;
            //_featureContext = featureContext;
            _authDriver = new AuthenticationDriver(scenarioContext, featureContext);
        }

        [Given(@"I am Authenticated as ""([^""]*)""")]
        public async Task GivenIAmAuthenticatedAs(string userProfile)
        {
            throw new NotImplementedException();
            //string userName;
            //string password;

            //if (userProfile == "admin")
            //{
            //    userName = _featureContext.Get<string>(ContextKeys.AdminUserName);
            //    password = _featureContext.Get<string>(ContextKeys.AdminPassword);
            //}
            //else
            //{
            //    userName = _featureContext.Get<string>(ContextKeys.RegularUserName);
            //    password = _featureContext.Get<string>(ContextKeys.RegularPassword);
            //}

            //AuthRequest request = new AuthRequest
            //{
            //    UserId = userName,
            //    Password = password
            //};

            //var result = await _authDriver.Authenticate(request);

            //Assert.AreEqual(HttpStatusCode.OK, result.statusCode);

            //_scenarioContext.Set(result.token, ContextKeys.BearerToken);

        }



    }
}
