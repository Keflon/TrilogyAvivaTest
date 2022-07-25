using System;
using System.Collections.Generic;
using System.Text;

namespace TrilogyAvivaTest.Bootstrap
{
    internal class ApiConstants
    {
        // TODO: Read these from a config file?
        public const string BaseApiUrl = "https://api.openweathermap.org";
        public const string WeatherServiceEndpoint = "data/2.5/weather";
        // TODO: This should be secret. Ssshh!
        public const string ApiKey = "62e10f56766a241ebda3e5b25ec8c173";
    }
}
