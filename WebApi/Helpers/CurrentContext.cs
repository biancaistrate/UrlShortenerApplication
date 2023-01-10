using Azure.Core;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

namespace WebApi.Helpers
{
    public class CurrentContext
    {
        private readonly HttpRequest _request;
        private readonly ClaimsPrincipal _user;

        public CurrentContext(HttpContext httpContext)
        {
            _request = httpContext.Request;
            _user = httpContext.User;
        }

        public bool IsUserAuthenticated()
        {
            return _user.Identity.IsAuthenticated;
        }
        public string GetUserClaim()
        {
            return _user.Claims.FirstOrDefault().Value;
        }

        public Uri GetDisplayUrl()
        {
            return new Uri(_request.GetDisplayUrl());
        }

        public Uri GetDefaultDomain()
        {
            var builder = new UriBuilder();

            builder.Scheme = _request.Scheme;
            builder.Host = _request.Host.Host;
            builder.Port = _request.Host.Port.HasValue ? _request.Host.Port.Value : _request.IsHttps ? 443 : 80;

            var hostname = _request.Host.ToUriComponent();
            var path = _request.Path.ToUriComponent();
            return new Uri($"{hostname}{path}");

        }

        public bool HasClaims()
        {
            return _user.Claims.Any();
        }
    }
}
