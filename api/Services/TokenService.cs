using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Entities;
using api.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenService(IConfiguration config) : ITokenService
{
   public string CreateToken (AppUser user)
    {
        var tokenkey = config["TokenKey"] ?? throw new Exception("cannot access tokenkey from appsetting");
        if (tokenkey.Length<64) throw new Exception ("your tokenkey needs to be longer ");
        var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        var claims= new List<Claim>
        {
            new (ClaimTypes.NameIdentifier , user.UserName)
        };
        var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.UtcNow.AddDays(7),
            SigningCredentials=creds
        };
        var tokenhandler = new JwtSecurityTokenHandler();
        var token = tokenhandler.CreateToken(tokenDescriptor);
        return tokenhandler.WriteToken(token);
    }
}
 