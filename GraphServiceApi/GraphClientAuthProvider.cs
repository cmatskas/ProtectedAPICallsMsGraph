using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GraphServiceApi
{
    public class GraphClientAuthProvider
    {
        public string[] Scopes { get; } = { "User.Read", "User.ReadWrite", "User.Read.All" };
        public DelegateAuthenticationProvider AuthProvider { get; }
        public ITokenAcquisition Token { get; }

        public GraphClientAuthProvider(ITokenAcquisition token)
        {
            Token = token;
            var accessToken = token.GetAccessTokenForUserAsync(Scopes).GetAwaiter().GetResult();
            AuthProvider = new DelegateAuthenticationProvider(x => { 
                x.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); 
                return Task.FromResult(0); });
        }

    }
}
