using System.Threading.Tasks;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterfaceLibrary.API
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}