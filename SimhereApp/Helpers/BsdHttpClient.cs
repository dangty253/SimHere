using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SimhereApp.Portable.Helpers
{
    sealed class BsdHttpClient
    {
        private static HttpClient _httpClient = null;
        static internal HttpClient Instance()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(Configuration.AppConfig.API_IP);
            }

            return _httpClient;
        }
    }
}
