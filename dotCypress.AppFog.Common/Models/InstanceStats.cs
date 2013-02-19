#region

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class InstanceStats
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("uris")]
        public List<string> Uris { get; set; }

        [JsonProperty("uptime")]
        public double Uptime { get; set; }

        [JsonProperty("mem_quota")]
        public int MemQuota { get; set; }

        [JsonProperty("disk_quota")]
        public int DiskQuota { get; set; }

        [JsonProperty("fds_quota")]
        public int FdsQuota { get; set; }

        [JsonProperty("cores")]
        public int Cores { get; set; }

        [JsonProperty("usage")]
        public InstanceUsage Usage { get; set; }
    }
}