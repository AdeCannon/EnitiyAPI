using System;
using System.Collections.Generic;

namespace FOA.Entity.DataAccess.Repositories
{
    public partial class ScheduledTaskTransmissionRef
    {
        public int ScheduledTaskId { get; set; }
        public int TransmissionId { get; set; }

        public ScheduledTask ScheduledTask { get; set; }
        public Transmissions Transmission { get; set; }
    }
}
