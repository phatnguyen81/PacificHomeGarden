using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IOrderService
    {
        Order GetById(Guid id);
        void Add(Order obj);
        void Delete(Guid id);
        void Delete(Order obj);
        IEnumerable<Order> GetAll(bool withBeingShopping = false);
        void DeleteOrderItem(OrderDetail orderDetail);
        void SaveChanges();
        Order GetCurrentOrderWaiting(string username);
    }
    public class OrderService : IOrderService
    {
        private readonly IDalContext _context;

        public OrderService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void DeleteOrderItem(OrderDetail orderDetail)
        {
            _context.Orders.DeleteOrderItem(orderDetail);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Order GetCurrentOrderWaiting(string username)
        {
            return _context.Orders.Find(q => q.Status == "W" && q.UserName == username);
        }

        public Order GetById(Guid id)
        {
            return _context.Orders.Find(q => q.Id == id);
        }

        public void Add(Order obj)
        {
            _context.Orders.Create(obj);
        }

        public void Delete(Guid id)
        {
            _context.Orders.Delete(q => q.Id == id);
        }

        public void Delete(Order obj)
        {
            _context.Orders.Delete(obj);
        }

        public IEnumerable<Order> GetAll(bool withBeingShopping = false)
        {
            return !withBeingShopping ? _context.Orders.Filter(q => "AP".Contains(q.Status)) : _context.Orders.All();
        }
    }
}
