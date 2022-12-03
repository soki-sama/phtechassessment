using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Integration.Tests.Drivers
{
    public class DriverBase
    {

        protected readonly ScenarioContext _scenarioContext;
        protected readonly FeatureContext _featureContext;

        public DriverBase(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            _featureContext = featureContext ?? throw new ArgumentNullException(nameof(featureContext));
        }

        protected string RetrieveCurrentUserToken()
        {
            UserProfile userProfile = _scenarioContext.Get<UserProfile>(ContextKeys.CurrentUserProfile);

            string token = _featureContext.Get<string>((userProfile == UserProfile.Regular) ?
                    ContextKeys.UserBearerToken : ContextKeys.AdminBearerToken);
            return token;
        }
    }
}
