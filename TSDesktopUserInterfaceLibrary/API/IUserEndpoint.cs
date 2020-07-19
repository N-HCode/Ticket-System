using System.Collections.Generic;
using System.Threading.Tasks;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterfaceLibrary.API
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
        Task<Dictionary<string, string>> GetAllRoles();
        Task AddUserToRole(string userId, string roleName);
        Task RemoveUserFromRole(string userId, string roleName);
    }
}