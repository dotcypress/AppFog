#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Stats
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("apps")]
        public int Apps { get; set; }

        [JsonProperty("memory")]
        public double Memory { get; set; }

        [JsonProperty("services")]
        public int Services { get; set; }
    }
}