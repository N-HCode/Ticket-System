using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TicketSystemNetFrameworkAPILibrary.DataAccess;
using TicketSystemNetFrameworkAPILibrary.Models;

//This adds the standards status code conventions like 404 etc.
//This will help with swagger documentation.
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace TicketSystemDotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData(_config);
            //.NET version: string userId = RequestContext.Principal.Identity.GetUserId();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            data.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        [HttpGet]
        public List<SaleReportModel> GetSalesReport()
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    // Do admin stuff
            //}
            //else if (RequestContext.Principal.IsInRole("Manager"))
            //{
            //    // Do Manager stuff
            //}
            SaleData data = new SaleData(_config);
            return data.GetSaleReport();
        }

    }
}
