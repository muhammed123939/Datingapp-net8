using api.DTOs;
using api.Entities;
using api.Extensions;
using api.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository , IMapper mapper , 
IphotoService photoService) : BaseApiController
{
    [HttpGet]//function will get all users
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

        var user =await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user==null)return BadRequest("could not find user");
        mapper.Map(MemberUpdateDto , user); //7ot member update f user 
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("failed to update the user");   
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        //7atena await w zaharet photos l user line 54 , User interface updated after login 
        var user= await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user==null) return BadRequest("cannot update user");
        var result=await photoService.AddPhotoAsync(file);
        if(result.Error!=null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url=result.SecureUrl.AbsoluteUri , 
            PublicId=result.PublicId
        };
         
        if(user.Photos.Count==0)photo.IsMain=true;

        user.Photos.Add(photo);

        if(await userRepository.SaveAllAsync()) 
        return CreatedAtAction(nameof(GetUser) , 
        new {username=user.UserName} ,mapper.Map<PhotoDto>(photo)); 
      
        return BadRequest ("problem adding photo");

    }
    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        // lw m7ttsh await MatchCasing hytl3ly photos in line 68
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername()); 
        if(user==null) return BadRequest("could not find user");
        var photo = user.Photos.FirstOrDefault(x=>x.Id==photoId);
        if(photo==null||photo.IsMain) return BadRequest("cannot use this as main photo");
        var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
        if(currentMain!=null) currentMain.IsMain=false;
        photo.IsMain=true;
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("problem setting main photo");
    }
        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername()); //it take instance from database
            if(user==null) return BadRequest("could not find user");
            var photo = user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if(photo==null||photo.IsMain) return BadRequest("this photo cannot be deleted");
            if(photo.PublicId!=null)
            {
                var result= await photoService.DeletePhotoAsync(photo.PublicId); //photo deleted in cloudinary
                if(result.Error!=null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if(await userRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem deleting photo");
        }
}
