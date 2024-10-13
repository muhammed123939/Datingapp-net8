using System;

namespace api.DTOs;

public class MemberDto
{
    //inside Dto we make returning  string optional
    public int Id { get; set; }
    //change Name to name to differs in appuser
    public string? Username { get; set; } 
    public int Age { get; set; }
    public string? PhotoUrl { get; set; }
    public string? KnownAs {get; set;} 
    public DateTime Created{get;set;}
    public DateTime LastActive{get;set;}
    public string? Gender {get; set;} 
    public string? Introduction {get; set;} 
    public string? Interests {get; set;} 
    public string? LookingFor {get; set;} 
    public string? City {get; set;} 
    public string? Country {get; set;} 

    //generate class photoDto the move it 
public List<PhotoDto>? Photos {get; set;}

}
