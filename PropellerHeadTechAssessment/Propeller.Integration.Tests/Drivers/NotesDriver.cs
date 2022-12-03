using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Propeller.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Propeller.Entities;

namespace Propeller.Integration.Tests.Drivers
{
    public class NotesDriver: DriverBase
    {
        private readonly HttpClient _httpClient;

        private string notesRoute = "notes";

        public NotesDriver(ScenarioContext scenarioContext, FeatureContext featureContext)
            : base(scenarioContext, featureContext)
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpStatusCode> DeleteNote(string customerId, int noteId)
        {
            var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
            Uri uri = new($"{baseUrl}/{notesRoute}/{customerId}/{noteId}");

            string token = RetrieveCurrentUserToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _httpClient.DeleteAsync(uri);
            var statusC = httpResponse.StatusCode;
            return statusC;

            // var content = await httpResponse.Content.ReadAsStringAsync();
            //var testIntegrationResponseModel =
            //    JsonConvert.DeserializeObject<TestIntegrationResponseModel>(content) ??
            //    new TestIntegrationResponseModel();
            //testIntegrationResponseModel.StatusCode = httpResponse.StatusCode;
            //testIntegrationResponseModel.RawContent = content;
            //return testIntegrationResponseModel;
        }

        public async Task<(HttpStatusCode statusCode, IEnumerable<NoteDto> notes)> RetrieveCustomerNotes(
            string customerId)
        {
            var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
            Uri uri = new($"{baseUrl}/{notesRoute}/{customerId}");

            string token = RetrieveCurrentUserToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _httpClient.GetAsync(uri);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            var notes = JsonSerializer.Deserialize<List<NoteDto>>(responseContent);

            var statusC = httpResponse.StatusCode;

            return (statusC, notes);

        }

        public async Task<(ApiResponse apiResponse, NoteDto? note)> 
            CreateNewNote(string customerId, string noteText)
        {
            var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
            Uri uri = new($"{baseUrl}/{notesRoute}/{customerId}");

            string token = RetrieveCurrentUserToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // var payload = JObject.Parse(testIntegrationRequest.Payload).ToString(Formatting.None);
            //_httpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", testIntegrationRequest.Token);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("noteText", noteText)
            });

            var httpResponse = await _httpClient.PostAsync(uri, formContent);
            var statusC = httpResponse.StatusCode;

            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse
            {
                StatusCode = statusC,
                ResponseBody = responseContent
            };

            if (statusC == HttpStatusCode.Created)
            {
                var createdNote = JsonSerializer.Deserialize<NoteDto>(responseContent);
                return (apiResponse, createdNote);
            }

            return (apiResponse, null);

            // var content = await httpResponse.Content.ReadAsStringAsync();
            //var testIntegrationResponseModel =
            //    JsonConvert.DeserializeObject<TestIntegrationResponseModel>(content) ??
            //    new TestIntegrationResponseModel();
            //testIntegrationResponseModel.StatusCode = httpResponse.StatusCode;
            //testIntegrationResponseModel.RawContent = content;
            //return testIntegrationResponseModel;
        }

        //public async void ExecutePost(string api, TestIntegrationRequestModel testIntegrationRequest)
        //    {
        //    var uri =
        //        $"{api}/organizations/{testIntegrationRequest.OrganizationId}/microsoft-cdl/test/{testIntegrationRequest.IntegrationId}?ownerId={testIntegrationRequest.OwnerId}";
        //    var payload = JObject.Parse(testIntegrationRequest.Payload).ToString(Formatting.None);
        //    _httpClient.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", testIntegrationRequest.Token);
        //    var httpResponse = await _httpClient.PostAsync(uri, new StringContent(payload, Encoding.UTF8, "application/json"));

        //    var content = await httpResponse.Content.ReadAsStringAsync();
        //    var testIntegrationResponseModel =
        //        JsonConvert.DeserializeObject<TestIntegrationResponseModel>(content) ??
        //        new TestIntegrationResponseModel();
        //    testIntegrationResponseModel.StatusCode = httpResponse.StatusCode;
        //    testIntegrationResponseModel.RawContent = content;
        //    return testIntegrationResponseModel;
        //}

    }
}
