using System.Security.Claims;
using api.DTOs;
using api.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository , IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null) return NotFound();
        return user;
    }

    
    [HttpPut]
    public async Task<ActionResult<MemberDto>> UpdateUser(MemberUpdateDto MemberUpdateDto)
    {
        var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(username==null)return BadRequest("no username found in token");   
        var user =await userRepository.GetUserByUsernameAsync(username);
        if(user==null)return BadRequest("could not find user");
        mapper.Map(MemberUpdateDto , user);
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("failed to update the user");   
    }
}
