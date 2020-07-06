using System.Collections.Generic;
using System.Threading.Tasks;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterfaceLibrary.API
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}