using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        void Add(Product product);
        void SaveChanges();
        Product GetById(Guid id);
        void Delete(Guid id);
        IEnumerable<Product_Category> GetAllProductCategory(Guid productId);
        bool CheckProductCategoryExists(Guid productId, Guid categoryId);
        void InsertProductCategory(Product_Category productCategory);
        Product_Category GetProductCategory(Guid productId, Guid categoryId);
        void DeleteProductCategory(Product_Category productCategory);
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        IEnumerable<Product_Picture> GetAllPictures(Guid productId);
        IEnumerable<Product> GetAllProductsByCategory(Guid categoryId);
        IEnumerable<Product> GetAllProductsByCategory(string categoryAlias);
        Product GetByAlias(string alias);
        Product_Picture GetProductPicture(Guid productId, Guid pictureId);

        IPagedList<Product> Search(Guid? manufacturerId, Guid? categoryId, bool? isPublished, decimal? priceMin, decimal? priceMax, string keywords, int pageIndex, int pageSize);
    }

    public class ProductService : IProductService, IDisposable
    {

        private readonly IDalContext _context;

        public ProductService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.All();
        }

        public void Add(Product product)
        {
            _context.Products.Create(product);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Product GetById(Guid id)
        {
            return _context.Products.Find(q => q.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Products.Delete(q => q.Id == id);
        }

        public IEnumerable<Product_Category> GetAllProductCategory(Guid productId)
        {
            return GetById(productId).Product_Category;
        }

        public bool CheckProductCategoryExists(Guid productId, Guid categoryId)
        {
            return GetById(productId).Product_Category.Any(q => q.CategoryId == categoryId);
        }

        public void InsertProductCategory(Product_Category productCategory)
        {
            GetById(productCategory.ProductId).Product_Category.Add(productCategory);
        }

        public Product_Category GetProductCategory(Guid productId, Guid categoryId)
        {
            return GetById(productId).Product_Category.FirstOrDefault(q => q.CategoryId == categoryId);
        }

        public void DeleteProductCategory(Product_Category productCategory)
        {
            GetById(productCategory.ProductId).Product_Category.Remove(productCategory);
        }
        public bool CheckExistAlias(string alias)
        {
            return _context.Articles.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Articles.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public IEnumerable<Product_Picture> GetAllPictures(Guid productId)
        {
            var product = GetById(productId);
            return product.Product_Picture;
        }

        public IEnumerable<Product> GetAllProductsByCategory(Guid categoryId)
        {
            return _context.Products.GetAllProductsByCategory(categoryId);
        }

        public IEnumerable<Product> GetAllProductsByCategory(string categoryAlias)
        {
            return _context.Products.GetAllProductsByCategory(categoryAlias);
        }

        public Product GetByAlias(string alias)
        {
            return _context.Products.Find(q => q.Alias == alias);
        }

        public Product_Picture GetProductPicture(Guid productId, Guid pictureId)
        {
            return GetById(productId).Product_Picture.FirstOrDefault(q => q.PictureId == pictureId);
        }

        public IPagedList<Product> Search(Guid? manufacturerId, Guid? categoryId, bool? isPublished, decimal? priceMin, decimal? priceMax, string keywords, int pageIndex, int pageSize)
        {
            var query = GetAll().AsQueryable();
            if(manufacturerId != null && manufacturerId != Guid.Empty)
            {
                query = query.Where(q => q.ManufacturerId == manufacturerId.Value);
            }
            if (categoryId != null && categoryId != Guid.Empty)
            {
                query = query.Where(q => q.Product_Category.Select(c => c.CategoryId).Contains(categoryId.Value));
            }
            if(isPublished != null)
            {
                query = query.Where(q => q.IsPublished == isPublished);
            }
            if(priceMin != null)
            {
                query = query.Where(q => q.Price >= priceMin);
            }
            if (priceMax != null)
            {
                query = query.Where(q => q.Price <= priceMax);
            }
            if(!string.IsNullOrWhiteSpace(keywords))
            {
                query =
                    query.Where(
                        q => q.Title.Contains(keywords) || q.Quote.Contains(keywords) || q.Body.Contains(keywords));
            }
            query = query.OrderBy(q => q.Title);
            var products = new PagedList<Product>(query, pageIndex, pageSize);
            return products;
        }
    }
}
