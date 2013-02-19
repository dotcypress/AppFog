#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Info
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("usage")]
        public Stats Usage { get; set; }

        [JsonProperty("limits")]
        public Stats Limits { get; set; }
    }
}