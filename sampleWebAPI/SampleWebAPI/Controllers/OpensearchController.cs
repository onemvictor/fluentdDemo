using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SampleWebAPI.Controllers
{
    public class OpensearchClient
    {
        private readonly HttpClient _httpClient;
        
        public OpensearchClient(HttpClient client)
        {
            _httpClient = client;
        }

        public HttpClient Gateway
        {
            get
            {
                return _httpClient;
            }
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class OpensearchController : ControllerBase
    {
        private readonly OpensearchClient _opensearchClient;
        private readonly ILogger<OpensearchController> _logger;

        public OpensearchController(OpensearchClient opensearchClient, ILogger<OpensearchController> logger)
        {
            _logger = logger;
            _opensearchClient = opensearchClient;
        }

        [HttpGet]
        public async Task<ContentResult> Get([FromQuery]string url)
        {
            _logger.LogInformation("Getting opensearch");
            var message = await _opensearchClient.Gateway.GetAsync(url, HttpCompletionOption.ResponseContentRead);
            var content = await message.Content.ReadAsStringAsync(); 
            return Content(content, "application/json");
        }
    }
}
