using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GraphServiceApi
{
    public class GraphClientAuthProvider
    {
        private DelegateAuthenticationProvider authProvider;

        public string[] Scopes { get; } = { "User.Read", "User.ReadWrite", "User.Read.All" };
        public DelegateAuthenticationProvider AuthProvider 
        { 
            get 
            { 
                GetGraphClientAuthProvider(); 
                return authProvider; 
            } 
        }
        
        public ITokenAcquisition Token { get; }

        public GraphClientAuthProvider(ITokenAcquisition token)
        {
            Token = token;
        }

        private DelegateAuthenticationProvider GetGraphClientAuthProvider()
        {
            if (authProvider != null)
            {
                return authProvider;
            }

            authProvider = new DelegateAuthenticationProvider(async x => {
                var accessToken = await Token.GetAccessTokenForUserAsync(Scopes);
                x.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            });


            return authProvider;
        }

    }
}
