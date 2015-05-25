using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IProductAttributeService
    {
        ProductAttribute GetById(Guid id);
        void SaveChanges();
        IEnumerable<ProductAttribute> GetAll();
        void Add(ProductAttribute productAttribute);
        void Delete(Guid id);
        ProductAttributeOption GetOptionById(Guid id);
        void DeleteOption(Guid id);
    }

    public class ProductAttributeService : IProductAttributeService, IDisposable
    {

        private readonly IDalContext _context;

        public ProductAttributeService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public ProductAttribute GetById(Guid id)
        {
            return _context.ProductAttributes.Find(q => q.Id == id);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ProductAttribute> GetAll()
        {
            return _context.ProductAttributes.All();
        }

        public void Add(ProductAttribute productAttribute)
        {
            _context.ProductAttributes.Create(productAttribute);
        }

        public void Delete(Guid id)
        {
            _context.ProductAttributes.Delete(q => q.Id == id);
        }

        public ProductAttributeOption GetOptionById(Guid id)
        {
            return _context.ProductAttributes.GetOptionById(id);
        }

        public void DeleteOption(Guid id)
        {
            _context.ProductAttributes.DeleteOption(id);
        }
    }
}
