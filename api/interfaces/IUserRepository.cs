using System;
using api.DTOs;
using api.Entities;

namespace api.interfaces;

public interface IUserRepository
{
    void Update (AppUser user);
   Task <bool> SaveAllAsync();
    Task<IEnumerable <AppUser>> GetUsersAsync();
    //? to handle the null values
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task <IEnumerable<MemberDto>> GetMembersAsync();
    Task  <MemberDto?> GetMemberAsync(string username);

}
