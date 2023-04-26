using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PlanIt.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public class AuthRequestBody
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    private class PlanItUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public PlanItUser(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }

    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthRequestBody authRequestBody)
    {
        var user = ValidateCreds(authRequestBody.UserName, authRequestBody.Password);
        if (user == null)
            return Unauthorized();
        
        var securityKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
        claimsForToken.Add(new Claim("username", user.Username));
        claimsForToken.Add(new Claim("email", user.Email));

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private PlanItUser ValidateCreds(string? username, string? password)
    {
        // hardcode for now
        return new PlanItUser(1, "joeblow", "joe@blow.com");
    }
}