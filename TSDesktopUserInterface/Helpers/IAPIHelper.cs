using System.Threading.Tasks;
using TSDesktopUserInterface.Models;

namespace TSDesktopUserInterface.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}