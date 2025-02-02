using System.Collections.Generic;
using UserManager.Models;

namespace UserManager.Data
{
    public interface IRoleRepository
    {
        List<Role> GetAllRoles();
        Role? GetRoleById(int id);
        Role? GetRoleByName(string name);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int id);
    }
}
