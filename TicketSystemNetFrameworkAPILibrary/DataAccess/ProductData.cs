using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystemNetFrameworkAPILibrary.Internal.DataAccess;
using TicketSystemNetFrameworkAPILibrary.Models;

namespace TicketSystemNetFrameworkAPILibrary.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TSDatabase");

            return output;


        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, 
                dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TSDatabase")
                .FirstOrDefault();

            return output;
        }
    }
}
