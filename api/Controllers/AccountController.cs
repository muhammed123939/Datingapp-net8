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

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] //acount/register     
    public async Task<ActionResult<UserDto>> Register(RegisterDTo registerDTo)
    {
        if (await UserExists(registerDTo.Username)) return BadRequest("username is taken");
        return Ok();

    //     using var hmac = new HMACSHA512();
    //     var user = new AppUser
    //     {
    //         UserName = registerDTo.Username.ToLower(),
    //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTo.Password)),
    //         PasswordSalt = hmac.Key
    //     };
    //     context.Users.Add(user);
    //     await context.SaveChangesAsync();
    //     return new UserDto
    //     {
    //         Username = user.UserName,
    //         Token = tokenService.CreateToken(user)
    //     };
     }


    [HttpPost("login")] //function 3mlt check login w b3tt check for login 
    public async Task<ActionResult<UserDto>> Login(loginDTo loginDTo)
    {
        var user = await context.Users
        .Include(p=>p.Photos ) //3shan tfw2 el database b relation
        .FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTo.Username.ToLower());//get first username exist
        if (user == null) return Unauthorized("invalid username");
        using var hmac = new HMACSHA512(user.PasswordSalt);//5at salt mn database
        var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTo.Password));//compute hash elsalt m3 elpassword entered
        for (int i = 0; i < computedhash.Length; i++)
        {
            if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("invalid password !");//karen password hash in db m3 computed hash
        }
           return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user) ,
            photoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url
        };
    }
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); //check 3la kol el users
    }


}
