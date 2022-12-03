using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Integration.Tests
{
    public static class ContextKeys
    {
        // Used in external packages
        public const string AuthenticationResponse = "AuthenticationResponse";

        public const string MongoDatabase = "mongoDatabase";
        public const string ApiBaseUrl = "apiBaseUrl";
        public const string MicrosoftLead = "microsoftLead";
        public const string AckPayload = "ackPayload";
        public const string Configuration = "configuration";
        public const string Integration = "integration";
        public const string TestIntegrationRequest = "testIntegrationRequest";
        public const string TestIntegrationResponse = "testIntegrationResponse";
        public const string ConfigurationType = "microsoft-cdl-configuration";
        public const string ConfigurationId = "configurationId";
        public const string EntityId = "entityId";
        public const string CreatedCampaignId = "createdCampaignId";
        public const string CreatedContractId = "createdContractId";
        // public const string CleanUp = "CleanUpAsync";
        public const string UploadLeadRequest = "uploadLeadRequest";

        public const string AdminUserName = "AdminUserName";
        public const string AdminPassword = "AdminPassword";

        public const string PowerUserName = "PowerUserName";
        public const string PowerPassword = "PowerPassword";

        public const string RegularUserName = "RegularUserName";
        public const string RegularPassword = "RegularPassword";

        public const string CustomerID = "CustomerId";
        // public const string NewCustomer = "NewCustomer";
        public const string NewNote = "NewNote";
        // public const string ExistingCustomer = "ExistingCustomer";

        // public const string BearerToken = "BearerToken";

        public const string AdminBearerToken = "AdminBearerToken";
        public const string PowerBearerToken = "PowerBearerToken";
        public const string UserBearerToken = "UserBearerToken";

        public const string NewCustomerId = "NewCustomerId";
        public const string CreatedNoteId = "CreatedNoteId";

        public const string NewContactId = "NewContactId";

        public const string FoundCustomers = "FoundCustomers";
        public const string FoundContacts = "FoundContacts";

        public const string LastReturnedStatusCode = "LastReturnedStatusCode";
        // public const string LastReturnedResponse = "LastReturnedResponse";
        public const string LastReturnedApiResponse = "LastReturnedApiResponse";
        public const string CurrentCustomer = "CurrentCustomer";

        public const string CustomerStatuses = "CustomerStatuses";


        public const string CleanUp = "CleanUp";

        public static string CurrentUserProfile = "CurrentUserProfile";
    }
}
