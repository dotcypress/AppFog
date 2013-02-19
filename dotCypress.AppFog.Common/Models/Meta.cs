#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Meta
    {
        [JsonProperty("debug")]
        public string Debug { get; set; }
    }
}