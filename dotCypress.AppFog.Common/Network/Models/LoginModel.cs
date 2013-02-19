#region

using System.Runtime.Serialization;

#endregion

namespace dotCypress.AppFog.Common.Network.Models
{
    [DataContract]
    public class LoginModel
    {
        public LoginModel(string password)
        {
            this.password = password;
        }

        [DataMember(Name = "password")]
        public string password { get; set; }
    }
}