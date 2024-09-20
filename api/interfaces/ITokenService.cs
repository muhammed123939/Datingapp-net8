using System;
using api.Entities;

namespace api.interfaces;

public interface ITokenService
{
    string CreateToken (AppUser user);

}
 