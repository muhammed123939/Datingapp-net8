using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using System.Text;
using api.Data;
using api.DTOs;
using Microsoft.EntityFrameworkCore;
using api.interfaces;
namespace api.Controllers;

public class AccountController(DataContext context , ITokenService tokenService) :BaseApiController
{ 
    [HttpPost("register")] //acount/register     
    public async Task<ActionResult<UserDto>> Register (RegisterDTo registerDTo)    
    {
        if (await UserExists(registerDTo.Username) ) return BadRequest("username is taken");
        using var hmac=new HMACSHA512();
        var user = new AppUser
        {
            UserName=registerDTo.Username.ToLower(),
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTo.Password)), 
            PasswordSalt=hmac.Key 
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDto{
            Username =user.UserName ,
            Token = tokenService.CreateToken(user)
        };
    }

    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login (loginDTo loginDTo)    
    {
        var user =await context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower()==loginDTo.Username.ToLower());
        if (user==null) return Unauthorized("invalid username");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedhash =hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTo.Password));
        for (int i = 0; i < computedhash.Length; i++)
        {
            if(computedhash[i]!=user.PasswordHash[i]) return Unauthorized("invalid password !");
        }
        return new UserDto{
            Username =user.UserName ,
            Token = tokenService.CreateToken(user)
        };
    }    
     private async Task<bool>UserExists(string username)
     {
        return await context.Users.AnyAsync(x=> x.UserName.ToLower()==username.ToLower());
     }


}
