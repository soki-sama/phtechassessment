using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Propeller.Entities;
using Propeller.Models.Requests;
// using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Propeller.Models;
using System.Net.Http.Headers;
using LivingDoc.Dtos;
using Microsoft.EntityFrameworkCore;
using Propeller.Integration.Tests.Support;
using NUnit.Framework;

namespace Propeller.Integration.Tests.Drivers
{
    public class CustomerDriver : DriverBase
    {
        private readonly HttpClient _httpClient;
        //private readonly ScenarioContext _scenarioContext;
        //private readonly FeatureContext _featureContext;

        private string customersRoute = "customers";

        private List<CustomerStatusDto> _customerStatuses = new List<CustomerStatusDto>();

        public CustomerDriver(ScenarioContext scenarioContext, FeatureContext featureContext)
            : base(scenarioContext, featureContext)
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<(HttpStatusCode statusCode, CustomerDto customer)>
            RetrieveCustomer(string customerId)
        {
            var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
            Uri uri = new($"{baseUrl}/{customersRoute}/{customerId}");

            string token = RetrieveCurrentUserToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _httpClient.GetAsync(uri);
            var statusCode = httpResponse.StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var customer = JsonSerializer.Deserialize<CustomerDto>(responseContent);
                return (statusCode, customer);
            }

            return (statusCode, null); // statusC;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(HttpStatusCode statusCode, List<CustomerDto> customers)> RetrieveCustomersBySearch(
            string searchCriteria, int pageNumber, int pageSize)
        {

            try
            {
                var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
                Uri uri = new($"{baseUrl}/{customersRoute}?q={searchCriteria}&pn={pageNumber}&ps={pageSize}");

                string token = RetrieveCurrentUserToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponse = await _httpClient.GetAsync(uri);
                var statusCode = httpResponse.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<CustomerDto>>(responseContent);
                    return (statusCode, customers);
                }

                return (statusCode, null);
                // return customerCreated; // statusC;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<HttpStatusCode> ChangeCustomerStatus(string customerId, string statusId)
        {
            try
            {
                var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
                Uri uri = new($"{baseUrl}/{customersRoute}/{customerId}/status/{statusId}");

                string token = RetrieveCurrentUserToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                StringContent data = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PutAsync(uri, data);
                var statusC = httpResponse.StatusCode;
                return statusC;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<HttpStatusCode> DeleteCustomer(string customerId, bool forceDelete)
        {

            try
            {
                var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);



                Uri uri = new($"{baseUrl}/{customersRoute}/{customerId}?fd={(forceDelete ? "y" : "n")}");

                string token = RetrieveCurrentUserToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponse = await _httpClient.DeleteAsync(uri);
                // var customers = JsonSerializer.Deserialize<List<CustomerDto>>(responseContent);
                var statusC = httpResponse.StatusCode;
                return statusC;
                // return customerCreated; // statusC;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<(ApiResponse apiResponse, CustomerDto? customer)>
            AddNewCustomer(CreateCustomerRequest newCustomerRequest)
        {

            // TODO: Return statuscode and response instead or switch to the ApiResponse tuple

            try
            {
                var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
                Uri uri = new($"{baseUrl}/{customersRoute}");

                // var payload = JObject.Parse(testIntegrationRequest.Payload).ToString(Formatting.None);
                //_httpClient.DefaultRequestHeaders.Authorization =
                //    new AuthenticationHeaderValue("Bearer", testIntegrationRequest.Token);

                string jsonString = JsonSerializer.Serialize(newCustomerRequest);

                string token = RetrieveCurrentUserToken();

                // var payload = JObject.Parse(newCustomerRequest).ToString(Formatting.None);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponse = await _httpClient.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                var statusCode = httpResponse.StatusCode;
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                var apiResponse = new ApiResponse
                {
                    StatusCode = statusCode,
                    ResponseBody = responseContent
                };

                if (statusCode == HttpStatusCode.Created)
                {

                    var customerCreated = JsonSerializer.Deserialize<CustomerDto>(responseContent);

                    // This will be done on a per needed basis
                    //if (customerCreated == null)
                    //{
                    //    Assert.Fail("Unable to Create Customer");
                    //}

                    // Hookup cleanup
                    _scenarioContext.AddCleanUpStep(async () =>
                    {
                        await DeleteCustomer(customerCreated.ID, true);
                    });

                    return (apiResponse, customerCreated);
                }

                //var formContent = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("noteText", "Test Data")
                //});

                //var httpResponse = await _httpClient.PostAsync(uri, formContent);

                // var statusC = httpResponse.StatusCode;

                return (apiResponse, null); // statusC;

                //var testIntegrationResponseModel =
                //    JsonConvert.DeserializeObject<TestIntegrationResponseModel>(content) ??
                //    new TestIntegrationResponseModel();
                //testIntegrationResponseModel.StatusCode = httpResponse.StatusCode;
                //testIntegrationResponseModel.RawContent = content;
                //return testIntegrationResponseModel;


            }
            catch (Exception ex)
            {

                throw;
            }


        }


    }
}
