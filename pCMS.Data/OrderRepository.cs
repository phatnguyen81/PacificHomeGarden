using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IOrderRepository : IRepository<Order>
    {
        void DeleteOrderItem(Guid id);
        void DeleteOrderItem(OrderDetail orderDetail);
    }
    class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(pCMSEntities context) : base(context) { }
        public void DeleteOrderItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrderItem(OrderDetail orderDetail)
        {
            Context.OrderDetails.DeleteObject(orderDetail);
        }
    }
}
