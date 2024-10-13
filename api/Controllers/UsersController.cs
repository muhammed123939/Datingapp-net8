using System;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;
 [Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()    
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }
     
     [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser (string username)    
    {
        var user = await userRepository.GetMemberAsync(username);
        if(user==null) return NotFound();
        return user;         
    }

}
