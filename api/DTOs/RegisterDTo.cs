using System;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class RegisterDTo
{
[Required]
public required string Username { get; set; }

[Required]
public required string Password { get; set; }
}
