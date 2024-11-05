using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SwipeWords.Data;

namespace SwipeWords.Models;

public class TokenProvider(IConfiguration configuration)
{
    public string Create(User user)
    {
        string secretKey = configuration["Jwt:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var TokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(TokenDescriptor);
        
        return token;
    }
    
}