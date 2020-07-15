using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystemNetFrameworkAPILibrary.Internal.DataAccess;
using TicketSystemNetFrameworkAPILibrary.Models;

namespace TicketSystemNetFrameworkAPILibrary.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);

                if(productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);

            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId

            };

            sale.Total = sale.SubTotal + sale.Tax;


            //Wrap the code below in a transaction. So all of the process complete or none of it complete.
            //This is to avoid having incomplete/corrupt data in the SQL server incase part of the process
            //completes and another part fails.
            //Uses a C# transaction, meaning C# will handle the opening and closing of the transaction.
            //This is to be used sparingly. Most of the time you do not want to open transaction in C#
            //Instead transaction should be open in the SQL side. As you can leave a connection open
            //and forget to close it, making proformance to be reduced significantly.

            
            //using make it so all of the calls and made together.
            //The end of the using statement will close the connection and use the dispose method
            //However, we place the commitTransaction at the end anyways to help visability
            //if the transaction fail then we catch the failure and do a rollback.
            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("TSDatabase");
                    sql.SaveData("dbo.spSale_Insert", sale, "TSDatabase");

                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new
                    {
                        sale.CashierId,
                        sale.SaleDate
                    }).FirstOrDefault();

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            } 


        }



        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<SaleReportModel, dynamic>(
                "dbo.spSale_SaleReport", new { }, "TSDatabase");

            return output;
        }
    }
}
