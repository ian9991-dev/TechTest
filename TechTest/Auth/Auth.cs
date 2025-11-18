using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

using TechTest.Models;
using TechTest.Services;

namespace TechTest.Auth
{
    public static class Auth
    {
        public static string? GetActorName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                                  ?? claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value
                                  ?? claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? claimsPrincipal.Identity?.Name;
        

    }
}
