#region

using RestSharp;

#endregion

namespace dotCypress.AppFog.Common.Network
{
    internal class HeaderAuthenticator : IAuthenticator
    {
        private readonly string _auth;

        public HeaderAuthenticator(string auth)
        {
            _auth = auth;
        }

        #region IAuthenticator Members

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("authorization", _auth);
        }

        #endregion
    }
}