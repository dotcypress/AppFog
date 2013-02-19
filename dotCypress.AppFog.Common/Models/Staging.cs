#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Staging
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("stack")]
        public string Stack { get; set; }
    }
}