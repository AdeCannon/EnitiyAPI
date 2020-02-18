using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOA.Entity.DataAccess;
using FOA.Entity.DataAccess.Repositories;
using FOA.Entity.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FOA.Entity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("orders")]
        public IEnumerable<Order> Orders([FromServices] IDataAccess dataAccess, [FromQuery] int orderId, [FromQuery] string omsOrderId, [FromQuery] string omsOrderVersionId, [FromQuery] DateTime createdFrom, [FromQuery] DateTime createdTo)
        {
            return dataAccess.GetOrders(orderId, omsOrderId, omsOrderVersionId, createdFrom, createdTo);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("allocations")]
        //public IEnumerable<Allocation> Allocations([FromServices] IDataAccess dataAccess, [FromQuery] int[] orderIds)
        //{
        //    return dataAccess.GetAllocationsForOrders(orderIds);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("transmissions")]
        //public IEnumerable<Transmission> GetTransmissions([FromServices] IDataAccess dataAccess, [FromQuery] int[] allocationIds)
        //{
        //    return dataAccess.GetTransmissionsForAllocations(allocationIds);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("allocations")]
        public IEnumerable<Allocation> Allocations([FromServices] IDataAccess dataAccess, [FromBody] int[] orderIds)
        {
            return dataAccess.GetAllocationsForOrders(orderIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("transmissions")]
        public IEnumerable<Transmission> GetTransmissions([FromServices] IDataAccess dataAccess, [FromBody] int[] allocationIds)
        {
            return dataAccess.GetTransmissionsForAllocations(allocationIds);
        }
    }
}
