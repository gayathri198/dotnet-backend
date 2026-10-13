using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Data;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<User> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    // READ ALL
    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // READ ONE
    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    // UPDATE
    public async Task<User?> UpdateUser(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.FullName = user.FullName;
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.MobileNumber = user.MobileNumber;

        await _context.SaveChangesAsync();

        return existingUser;
    }

    public async Task<User?> UpdateProfileAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.FullName = user.FullName;
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.MobileNumber = user.MobileNumber;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> UpdatePasswordAsync(int id, string passwordHash)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        user.Password = passwordHash;
        await _context.SaveChangesAsync();
        return true;
    }

    // DELETE
    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }
}