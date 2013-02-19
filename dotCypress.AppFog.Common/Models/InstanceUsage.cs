#region

using System;
using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class InstanceUsage
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("cpu")]
        public double Cpu { get; set; }

        [JsonProperty("Mem")]
        public int Mem { get; set; }

        [JsonProperty("disk")]
        public int Disk { get; set; }
    }
}