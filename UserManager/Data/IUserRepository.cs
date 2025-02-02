using System.Collections.Generic;
using UserManager.Models;

namespace UserManager.Data
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByUsername(string name);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
