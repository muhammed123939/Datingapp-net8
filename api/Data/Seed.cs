using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context){
        if(await context.Users.AnyAsync()) return ; 
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json"); // new data in the file
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true}; //2oltlo an data case sensitve
        var users =JsonSerializer.Deserialize<List<AppUser>>(userData , options); 
        if(users==null)return;
        foreach (var user in users)
        {
            using var hmac= new HMACSHA512();
            user.UserName= user.UserName.ToLower(); 
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("pa$$w0rd"));
            user.PasswordSalt= hmac.Key;

            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}
