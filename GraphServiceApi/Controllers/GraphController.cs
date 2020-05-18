using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace GraphServiceApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GraphController : ControllerBase
    {
        private GraphClient graphClient;

        public GraphController(GraphClient client)
        {
            graphClient = client;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            IEnumerable<User> users;

            try
            {
                users = await graphClient.ServiceClient.Users.Request().GetAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                  graphClient.AuthenticationProvider.Token.ReplyForbiddenWithWwwAuthenticateHeader(graphClient.AuthenticationProvider.Scopes, ex);
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }

            return users.Select(x => new
            {
                name = x.DisplayName,
                upn = x.UserPrincipalName,
                mail = x.Mail,
                city = x.City,
                jobTitle = x.JobTitle,
                image=x.Photo
            });
        }

        [HttpGet("{upn}")]
        public async Task<object> Get(string upn = "me")
        {
            User user;
            try
            {
                if (upn.Equals("me"))
                {
                    user = await graphClient.ServiceClient.Me.Request().GetAsync();
                }
                else
                {
                    user = await graphClient.ServiceClient.Users[upn].Request().GetAsync();
                }
            }
            catch (MsalUiRequiredException ex)
            {
                graphClient.AuthenticationProvider.Token.ReplyForbiddenWithWwwAuthenticateHeader(graphClient.AuthenticationProvider.Scopes, ex);
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }

            return new
            {
                name = user.DisplayName,
                upn = user.UserPrincipalName,
                mail = user.Mail,
                city = user.City,
                jobTitle = user.JobTitle,
                image = user.Photo
            };
        }
    }
}
