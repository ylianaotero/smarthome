using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using WebApi.Attributes;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

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
            AllowAnonymousAttribute? allowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata
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

            RolesWithPermissionsAttribute? rolesWithPermissionsAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RolesWithPermissionsAttribute>()
                .FirstOrDefault();

            if (rolesWithPermissionsAttribute != null)
            {
                bool correctUser = rolesWithPermissionsAttribute.RolesWithPermissions
                    .Any(role => sessionService.UserHasCorrectRole(uppercaseToken, role));

                if (!correctUser)
                {
                    SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                }
            }

            long homeId = context.HttpContext.Request.RouteValues["{id}"] as long? ?? -1;
            if (homeId != -1)
            {
                RestrictToPrivilegedMembersAttribute? restrictToPrivilegedMembersAttribute = context.ActionDescriptor.EndpointMetadata
                    .OfType<RestrictToPrivilegedMembersAttribute>()
                    .FirstOrDefault();
                
                if (restrictToPrivilegedMembersAttribute != null)
                {
                    IHomeService homeService = GetHomeService(context);

                    try
                    {
                        if (restrictToPrivilegedMembersAttribute.WithAddPermissions && 
                            !sessionService.UserCanAddDevicesInHome(uppercaseToken, homeService.GetHomeById(homeId)))
                        {
                            SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                        }
                    } catch (ElementNotFound)
                    {
                        return;
                    }
                    
                    try 
                    {
                        if (restrictToPrivilegedMembersAttribute.WithListPermissions && 
                            !sessionService.UserCanListDevicesInHome(uppercaseToken, homeService.GetHomeById(homeId)))
                        {
                            SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                        }
                    } catch (ElementNotFound)
                    {
                        return;
                    }
                }
            }

        
        }

        private ISessionService GetSessionService(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(ISessionService)) as ISessionService;
        }
        
        private IHomeService GetHomeService(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(IHomeService)) as IHomeService;
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