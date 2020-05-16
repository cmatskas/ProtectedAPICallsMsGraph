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
        private GraphClientAuthProvider graphClientAuth;
        private readonly GraphServiceClient graphClient;

        public GraphController(GraphClientAuthProvider auth)
        {
            graphClientAuth = auth;
            graphClient = new GraphServiceClient(graphClientAuth.AuthProvider);
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            IEnumerable<User> users;

            try
            {
                users = await graphClient.Users.Request().GetAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                graphClientAuth.Token.ReplyForbiddenWithWwwAuthenticateHeader(graphClientAuth.Scopes, ex);
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
                    user = await graphClient.Me.Request().GetAsync();
                }
                else
                {
                    user = await graphClient.Users[upn].Request().GetAsync();
                }
            }
            catch (MsalUiRequiredException ex)
            {
                graphClientAuth.Token.ReplyForbiddenWithWwwAuthenticateHeader(graphClientAuth.Scopes, ex);
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
