using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);

    Task<List<User>> GetAllUsers();

    Task<User?> GetUserById(int id);

    Task<User?> GetUserByUsername(string username);

    Task<User?> GetUserByEmail(string email);

    Task<User?> UpdateUser(int id, User user);

    Task<User?> UpdateProfileAsync(int id, User user);

    Task<bool> UpdatePasswordAsync(int id, string passwordHash);

    Task<bool> DeleteUser(int id);
}