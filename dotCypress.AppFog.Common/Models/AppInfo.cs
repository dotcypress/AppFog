#region

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class AppInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("uris")]
        public List<string> Uris { get; set; }

        [JsonProperty("services")]
        public List<string> Services { get; set; }

        [JsonProperty("env")]
        public List<string> Env { get; set; }

        [JsonProperty("instances")]
        public int Instances { get; set; }

        [JsonProperty("runningInstances")]
        public int RunningInstances { get; set; }

        [JsonProperty("staging")]
        public Staging Staging { get; set; }

        [JsonProperty("infra")]
        public Infrastructure Infra { get; set; }

        [JsonProperty("resources")]
        public Resources Resources { get; set; }
        
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
}