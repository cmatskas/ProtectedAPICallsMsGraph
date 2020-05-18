using Microsoft.Graph;

namespace GraphServiceApi
{
    public class GraphClient
    {
        private GraphServiceClient serviceClient;
        public GraphServiceClient ServiceClient
        {
            get
            {
                if (serviceClient != null)
                {
                    return serviceClient;
                }

                serviceClient = new GraphServiceClient(AuthenticationProvider.AuthProvider);
                return serviceClient;
            }
        }

        public GraphClientAuthProvider AuthenticationProvider { get; }
        public GraphClient(GraphClientAuthProvider authProvider)
        {
            AuthenticationProvider = authProvider;
        }        
    }
}
