#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Instance
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("stats")]
        public InstanceStats Stats { get; set; }
    }
}