#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Models
{
    public class Resources
    {
        [JsonProperty("memory")]
        public int Memory { get; set; }

        [JsonProperty("disk")]
        public int Disk { get; set; }

        [JsonProperty("fds")]
        public int Fds { get; set; }
    }
}