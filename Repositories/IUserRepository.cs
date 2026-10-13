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

    Task<bool> DeleteUser(int id);
}