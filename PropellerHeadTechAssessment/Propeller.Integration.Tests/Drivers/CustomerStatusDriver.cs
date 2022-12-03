﻿using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Propeller.Entities;
using TechTalk.SpecFlow;

namespace Propeller.Integration.Tests.Drivers
{
    public class CustomerStatusDriver : DriverBase
    {
        private readonly HttpClient _httpClient;

        private string statusRoute = "status";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scenarioContext"></param>
        /// <param name="featureContext"></param>
        public CustomerStatusDriver(ScenarioContext scenarioContext, FeatureContext featureContext)
                   : base(scenarioContext, featureContext)
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <returns></returns>
        public async Task<(HttpStatusCode statusCode, IEnumerable<CustomerStatusDto> statuses)> RetrieveCurrentStatuses(string serverAddress)
        {
            var baseUrl = _featureContext.Get<string>(ContextKeys.ApiBaseUrl);
            Uri uri = new($"{baseUrl}/{statusRoute}");

            var httpResponse = await _httpClient.GetAsync(uri);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            var statuses = JsonSerializer.Deserialize<List<CustomerStatusDto>>(responseContent);
            var statusC = httpResponse.StatusCode;

            return (statusC, statuses);
        }

    }
}
