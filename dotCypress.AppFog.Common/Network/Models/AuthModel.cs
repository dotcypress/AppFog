#region

using Newtonsoft.Json;

#endregion

namespace dotCypress.AppFog.Common.Network.Models
{
    public class AuthModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}