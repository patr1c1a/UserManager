using System.Collections.Generic;
using UserManager.Models;

namespace UserManager.Data
{
    public interface IRoleRepository
    {
        List<User> GetAllRoles();
        User GetRoleById(int id);
        void AddRole(User user);
        void UpdateRole(User user);
        void DeleteRole(int id);
    }
}
