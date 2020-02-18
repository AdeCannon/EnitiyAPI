using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOA.Entity.DataAccess.Repositories;
using FOA.Entity.DataAccess.Translators;
using FOA.Entity.Domain;
using Microsoft.EntityFrameworkCore;

namespace FOA.Entity.DataAccess
{
    public class DataAccess : IDataAccess
    {
        protected readonly GTPSContext _dbContext;

        public DataAccess(GTPSContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Order> GetOrders(int orderId, string omsOrderId, string omsOrderVersionId, DateTime createdFrom, DateTime createdTo)
        {
            var orders = (from ord in _dbContext.Orders
                        select ord);

            if (orderId != 0)
            {
                orders = orders.Where(x => x.OrderId.ToString().Contains(orderId.ToString()));
            }

            if (!string.IsNullOrEmpty(omsOrderId))
            {
                orders = orders.Where(x => x.OmsOrderId.Contains(omsOrderId));
            }

            if (!string.IsNullOrEmpty(omsOrderVersionId))
            {
                orders = orders.Where(x => x.OmsOrderVersionId.Contains(omsOrderVersionId));
            }

            //orders = orders.GroupBy(x => x.OrderId).Select(g => g.FirstOrDefault());

            //var orders2 = from f in orders select f;

            var orderList = new List<Order>();

            orders.ToList().ForEach(o => { orderList.Add(OrderTranslator.ModelToDomain(o)); });

            return orderList;

            //var orders = _dbContext.Orders.OrderBy(o => o.OrderId).Take(100).ToList();

            //var orderList = new List<Order>();

            //orders.ForEach(o => { orderList.Add(OrderTranslator.ModelToDomain(o)); });

            //return orderList;
        }

        public IEnumerable<Allocation> GetAllocationsForOrders(IEnumerable<int> OrderIds)
        {
            var allocations = _dbContext.Allocations.Where(alloc => OrderIds.Contains(alloc.OrderId)).ToList();

            var allocationList = new List<Allocation>();

            allocations.ForEach(a => { allocationList.Add(AllocationTranslator.ModelToDomain(a)); });

            return allocationList;
        }

        public IEnumerable<Transmission> GetTransmissionsForAllocations(IEnumerable<int> AllocationIds)
        {            
            var transmissions = (from alloc in _dbContext.Allocations.Where(alloc => AllocationIds.Contains(alloc.AllocationId))
                              join tRef in _dbContext.AllocationTransmissionRef on alloc.AllocationId equals tRef.AllocationId
                              join tr in _dbContext.Transmissions on tRef.TransmissionId equals tr.TransmissionId
                              select new Transmission 
                              { 
                                  TransmissionId = tr.TransmissionId, 
                                  AllocationId = alloc.AllocationId, 
                                  DestinationListitemCode = tr.DestinationListitemCode,
                                  TypeListitemCode = tr.TypeListitemCode,
                                  ProcListitemCode = tr.ProcListitemCode,
                                  StatusListitemCode = tr.StatusListitemCode,
                                  PurposeListCode = tr.PurposeListitemCode,
                                  Comment = tr.Comment }
                              ).ToList();
                     
            return transmissions;
        }
    }
}
