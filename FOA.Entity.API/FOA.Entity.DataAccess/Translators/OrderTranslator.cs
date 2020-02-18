using FOA.Entity.DataAccess.Repositories;
using FOA.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOA.Entity.DataAccess.Translators
{
    public static class OrderTranslator
    {
        public static Order ModelToDomain(Orders model)
        {
            return new Order 
            { 
                OrderId = model.OrderId,
                OmsOrderId = model.OmsOrderId,
                OmsOrderVersionId = model.OmsOrderVersionId,
                SecurityType = model.SecurityType,
                GamId = model.GamId,
                TradeStatusListitemCode = model.TradeStatusListitemCode,
                SourceListitemCode = model.SourceListitemCode
            };
        }
    }
}
