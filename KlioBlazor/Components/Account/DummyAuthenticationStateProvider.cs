using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KlioBlazor.Components.Account
{
    public class DummyAuthenticationStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // await Task.Delay(3000);
            var anonymous = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Eugene"),
                // new Claim(ClaimTypes.Role, "Admin")
            });
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
