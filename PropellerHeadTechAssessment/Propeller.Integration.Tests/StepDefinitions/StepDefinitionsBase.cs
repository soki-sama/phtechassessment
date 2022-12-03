using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

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

    }
}
