using FOA.Entity.DataAccess.Repositories;
using FOA.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOA.Entity.DataAccess
{
    public interface IDataAccess
    {
        IEnumerable<Order> GetOrders(int orderId, string omsOrderId, string omsOrderVersionId, DateTime createdFrom, DateTime createdTo);

        IEnumerable<Allocation> GetAllocationsForOrders(IEnumerable<int> OrderIds);

        IEnumerable<Transmission> GetTransmissionsForAllocations(IEnumerable<int> AllocationIds);
    }
}
