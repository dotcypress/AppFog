#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}