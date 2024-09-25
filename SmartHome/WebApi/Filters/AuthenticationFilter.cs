using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace WebApi.Filters
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private const string MissingHeaderMessage = "Authorization header is needed";
        private const string InvalidTokenMessage = "Invalid token format";
        private const string TokenDoesNotCorrespondToUserMessage = "The token does not correspond to a existing user";
        private const string UserDoesNotHavePermissionsMessage = "The user does not have the necessary permissions";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .FirstOrDefault();

            if (allowAnonymousAttribute != null)
            {
                return;
            }
            
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                SetUnauthorizedResult(context, MissingHeaderMessage);
                return;
            }

            if (!Guid.TryParse(token, out Guid parsedToken))
            {
                SetBadRequestResult(context, InvalidTokenMessage);
                return;
            }

            ISessionService sessionService = GetSessionService(context);
            
            Guid uppercaseToken = Guid.Parse(token.ToString().ToUpper());

            if (!sessionService.AuthorizationIsValid(uppercaseToken))
            {
                SetUnauthorizedResult(context, TokenDoesNotCorrespondToUserMessage);
                return;
            }

            var rolesWithPermissionsAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RolesWithPermissionsAttribute>()
                .FirstOrDefault();

            if (rolesWithPermissionsAttribute != null)
            {
                bool correctUser = rolesWithPermissionsAttribute.RolesWithPermissions
                    .Any(role => sessionService.UserHasPermissions(uppercaseToken, role));

                if (!correctUser)
                {
                    SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                }
            }
        }

        private ISessionService GetSessionService(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(ISessionService)) as ISessionService;
        }

        private void SetUnauthorizedResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new ObjectResult(message) { StatusCode = 401 };
        }

        private void SetBadRequestResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new ObjectResult(message) { StatusCode = 400 };
        }

        private void SetForbiddenResult(AuthorizationFilterContext context, string message)
        {
            context.Result = new ObjectResult(message) { StatusCode = 403 };
        }
    }
}