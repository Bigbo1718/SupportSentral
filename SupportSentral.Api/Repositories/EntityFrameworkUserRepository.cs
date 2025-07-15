using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Entities;
using SupportSentral.Api.Mappings;

namespace SupportSentral.Api.Repositories;

public class EntityFrameworkUserRepository : IUserRepository
{
    private SupportContext _dbContext;

    public EntityFrameworkUserRepository(SupportContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserContract>> GetAllAsync()
    {
       return await _dbContext
            .Users
            .Select(user=> user.MapToContract())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserContract?> GetByEmailIdAsync(string email)
    {
        var user = _dbContext.Users
            .SingleOrDefault(x => x.Email == email);

        if (user != null)
        {
            return await Task.FromResult(user.MapToContract());
        }
        return null;
    }

    public async Task<UserContract?> GetByIdAsync(Guid Id)
    {
        var user = _dbContext.Users
            .SingleOrDefault(x => x.Id == Id);

        if (user != null)
        {
            return await Task.FromResult(user.MapToContract());
        }
        return null;
    }

    public async Task<UserContract?> CreateUserAsync(UserContract user)
    {
        if (ValidateUserValues(user.Email)) return null;
        User createdUser = new User
        {
            Email = user.Email,
            Name = user.Name,
        };
        var createdEntity = _dbContext.Users.Add(createdUser);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity.MapToContract();
    }

    private static bool ValidateUserValues(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return true;
        };

        var isValidEmail = ValidateUsingRegex(email);

        if (!isValidEmail)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateUser(Guid id, UpdateUserContract user)
    {
        var existingUser = await _dbContext.Users.FindAsync(id);
        if (existingUser == null)
        {
            return false;
        }
            
        _dbContext.Entry(existingUser)
            .CurrentValues
            .SetValues(user.MapToEntity(existingUser.Id));
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public static bool ValidateUsingRegex(string emailAddress)
    {
        var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            
        var regex = new Regex(pattern);

        return regex.IsMatch(emailAddress);
    }
}