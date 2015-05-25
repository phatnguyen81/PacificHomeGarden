using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IProductTypeService
    {
        void SaveChanges();
        IEnumerable<ProductType> GetAll();
        void Add(ProductType productType);
        ProductType GetById(Guid id);
        void Delete(Guid id);
        IPagedList<ProductType> SearchProductTypes(string keywords, bool ascSort, int pageIndex, int pageSize);
    }

    public class ProductTypeService : IProductTypeService, IDisposable
    {

        private readonly IDalContext _context;

        public ProductTypeService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<ProductType> GetAll()
        {
            return _context.ProductTypes.All();
        }

        public void Add(ProductType productType)
        {
            _context.ProductTypes.Create(productType);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ProductType GetById(Guid id)
        {
            return _context.ProductTypes.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.ProductTypes.Delete(q => q.Id == id);
        }

        public IPagedList<ProductType> SearchProductTypes(string keywords, bool ascSort, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(q => q.Title.Contains(keywords) || q.Description.Contains(keywords));
            }
            query = ascSort ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title);
            var productTypes = new PagedList<ProductType>(query, pageIndex, pageSize);
            return productTypes;
        }
    }
}
