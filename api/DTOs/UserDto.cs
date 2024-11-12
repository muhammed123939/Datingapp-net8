using System;

namespace api.DTOs;

public class UserDto
{
public required string Username { get; set; }
public required string knownAs { get; set; }

public required string Token { get; set; }

public string? photoUrl {get;set;}
}
