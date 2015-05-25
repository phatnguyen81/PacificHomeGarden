using System;
using System.Collections.Generic;
using pCMS.Core;
using System.Linq;

namespace pCMS.Data
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllProductsByCategory(Guid categoryId);
        IEnumerable<Product> GetAllProductsByCategory(string categoryAlias);
    }
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(pCMSEntities context) : base(context) { }
        public IEnumerable<Product> GetAllProductsByCategory(Guid categoryId)
        {
            return Context.Product_Category.Where(q => q.CategoryId == categoryId).Select(q => q.Product);
        }

        public IEnumerable<Product> GetAllProductsByCategory(string categoryAlias)
        {
            return Context.Product_Category.Where(q => q.Category.Alias == categoryAlias).Select(q => q.Product);
        }
    }
    //public class ProductRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public ProductRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public ProductRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Product GetById(Guid id)
    //    {
    //        return _entities.Products.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Product> GetAll()
    //    {
    //        return _entities.Products;
    //    }
    //    public void Add(Product product)
    //    {
    //        _entities.AddToProducts(product);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Products.DeleteObject(GetById(id));
    //    }
    //    public bool CheckExistAlias(string alias)
    //    {
    //        return CheckExistAlias(alias, Guid.Empty);
    //    }
    //    public bool CheckExistAlias(string alias, Guid owner)
    //    {
    //        return owner == Guid.Empty ? _entities.Products.Any(q => q.Alias == alias) : _entities.Products.Any(q => q.Alias == alias && q.Id != owner);
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public IEnumerable<Product_Category> GetAllProductCategory(Guid productId)
    //    {
    //        var product = _entities.Products.FirstOrDefault(q => q.Id == productId);
            
    //        return product == null? null : product.Product_Category; 
    //    }

    //    public bool CheckProductCategoryExists(Guid productId, Guid categoryId)
    //    {
    //        return _entities.Product_Category.Any(q => q.ProductId == productId && q.CategoryId == categoryId);
    //    }

    //    public void InsertProductCategory(Product_Category productCategory)
    //    {
    //        _entities.AddToProduct_Category(productCategory);
    //    }

    //    public Product_Category GetProductCategory(Guid productId, Guid categoryId)
    //    {
    //        return _entities.Product_Category.FirstOrDefault(q => q.ProductId == productId && q.CategoryId == categoryId);
    //    }

    //    public void DeleteProductCategory(Product_Category productCategory)
    //    {
    //        _entities.DeleteObject(productCategory);
    //    }
    //}
}