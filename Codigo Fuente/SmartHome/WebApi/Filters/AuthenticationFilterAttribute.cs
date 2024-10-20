using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using WebApi.Attributes;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

namespace WebApi.Filters
{
    public abstract class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private const string MissingHeaderMessage = "Authorization header is needed";
        private const string InvalidTokenMessage = "Invalid token format";
        private const string TokenDoesNotCorrespondToUserMessage = "The token does not correspond to a existing user";
        private const string UserDoesNotHavePermissionsMessage = "The user does not have the necessary permissions";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (HandleAnnonymousAttribute(context))
            {
                return;
            }
            
            (bool isValid, Guid uppercaseToken) = HandleAuthorizationToken(context);
            if (!isValid)
            {
                return;
            }

            if (!HandleRolesWithPermissionsAttribute(context, uppercaseToken))
            {
                return;
            }

            if (!HandlePrivilegedMembersAttribute(context, uppercaseToken))
            {
                return;
            }
        }
        
        private static bool HandleAnnonymousAttribute(AuthorizationFilterContext context)
        {
            AllowAnonymousAttribute? allowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .FirstOrDefault();

            return allowAnonymousAttribute != null;
        }

        private (bool, Guid) HandleAuthorizationToken(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                SetUnauthorizedResult(context, MissingHeaderMessage);
                return (false, Guid.Empty);
            }

            if (!Guid.TryParse(token, out Guid parsedToken))
            {
                SetBadRequestResult(context, InvalidTokenMessage);
                return (false, Guid.Empty);
            }

            ISessionService sessionService = GetSessionService(context);

            Guid uppercaseToken = Guid.Parse(token.ToString().ToUpper());

            if (!sessionService.AuthorizationIsValid(uppercaseToken))
            {
                SetUnauthorizedResult(context, TokenDoesNotCorrespondToUserMessage);
                return (false, Guid.Empty);
            }
            
            return (true, uppercaseToken);
        }
        
        private bool HandleRolesWithPermissionsAttribute(AuthorizationFilterContext context, Guid uppercaseToken)
        {
            RolesWithPermissionsAttribute? rolesWithPermissionsAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RolesWithPermissionsAttribute>()
                .FirstOrDefault();

            if (rolesWithPermissionsAttribute != null)
            {
                return RestrictToRolesWithPermissions(context, rolesWithPermissionsAttribute, uppercaseToken);
            }
            
            return true;
        }
        
        private bool RestrictToRolesWithPermissions(AuthorizationFilterContext context, 
            RolesWithPermissionsAttribute attribute, Guid token)
        {
            ISessionService sessionService = GetSessionService(context);
            
            bool correctUser = attribute.RolesWithPermissions
                .Exists(role => sessionService.UserHasCorrectRole(token, role));

            if (correctUser) return true;
            
            SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
            return false;

        }
        
        private bool HandlePrivilegedMembersAttribute(AuthorizationFilterContext context, Guid uppercaseToken)
        {
            string homeIdStr = context.HttpContext.Request.RouteValues["id"]?.ToString() ?? "-1";
            int homeId = int.Parse(homeIdStr);
            
            if (homeId != -1)
            {
                RestrictToPrivilegedMembersAttribute? restrictToPrivilegedMembersAttribute = context
                    .ActionDescriptor.EndpointMetadata
                    .OfType<RestrictToPrivilegedMembersAttribute>()
                    .FirstOrDefault();
                
                if (restrictToPrivilegedMembersAttribute != null)
                {
                    return RestrictToPrivilegedMembers
                        (context, restrictToPrivilegedMembersAttribute, uppercaseToken, homeId);
                }
            }
            
            return true;
        }

        private bool RestrictToPrivilegedMembers(AuthorizationFilterContext context, 
            RestrictToPrivilegedMembersAttribute attribute, Guid token, long homeId)
        {
            return RestrictToPrivilegedMembersWithAddDevicesPermission(context, attribute, token, homeId) &&
                   RestrictToPrivilegedMembersWithListDevicesPermission(context, attribute, token, homeId);
        }
        
        private bool RestrictToPrivilegedMembersWithAddDevicesPermission(AuthorizationFilterContext context, 
            RestrictToPrivilegedMembersAttribute attribute, Guid token, long homeId)
        {
            IHomeService homeService = GetHomeService(context);
            ISessionService sessionService = GetSessionService(context);

            try
            {
                if (attribute.WithAddPermissions && 
                    !sessionService.UserCanAddDevicesInHome(token, homeService.GetHomeById(homeId)))
                {
                    SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                }
            } catch (ElementNotFound)
            {
                return false;
            }
            
            return true;
        }
        
        private bool RestrictToPrivilegedMembersWithListDevicesPermission(AuthorizationFilterContext context, 
            RestrictToPrivilegedMembersAttribute attribute, Guid token, long homeId)
        {
            IHomeService homeService = GetHomeService(context);
            ISessionService sessionService = GetSessionService(context);

            try 
            {
                if (attribute.WithListPermissions && 
                    !sessionService.UserCanListDevicesInHome(token, homeService.GetHomeById(homeId)))
                {
                    SetForbiddenResult(context, UserDoesNotHavePermissionsMessage);
                }
            } catch (ElementNotFound)
            {
                return false;
            }
            
            return true;
        }

        private static ISessionService GetSessionService(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(ISessionService)) as ISessionService;
        }
        
        private static IHomeService GetHomeService(AuthorizationFilterContext context)
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