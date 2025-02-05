using UserManager.Models;

namespace UserManager.Data
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        void AddUser(User user);
        bool ValidateUser(string username, string password);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
