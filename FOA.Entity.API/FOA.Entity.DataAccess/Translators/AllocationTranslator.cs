using FOA.Entity.DataAccess.Repositories;
using FOA.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOA.Entity.DataAccess.Translators
{
    public static class AllocationTranslator
    {
        public static Allocation ModelToDomain(Allocations model)
        {
            return new Allocation
            {
                AllocationId = model.AllocationId,
                OrderId = model.OrderId,
                SecurityId = model.SecurityId,
                SecurityDesc = model.SecurityDesc,
                SecurityCcy = model.SecurityCcy,
                PortfolioCode = model.PortfolioCode,
                Quantity = model.Quantity,
                Price = model.Price,
                GamFundCode = model.GamFundCode                
            };
        }
    }
}
