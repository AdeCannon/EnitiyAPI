using System;
using System.Collections.Generic;

namespace FOA.Entity.DataAccess.Repositories
{
    public partial class AllocationTransmissionRef
    {
        public int AllocationId { get; set; }
        public int TransmissionId { get; set; }

        public Allocations Allocation { get; set; }
        public Transmissions Transmission { get; set; }
    }
}
