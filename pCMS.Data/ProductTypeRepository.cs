using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pCMS.Core;

namespace pCMS.Data
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
    }
    public class ProductTypeRepository : EfRepository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(pCMSEntities context) : base(context) { }
    }


    //public class ProductTypeRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public ProductTypeRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public ProductTypeRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public ProductType GetById(Guid id)
    //    {
    //        return _entities.ProductTypes.FirstOrDefault(q => q.Id == id);
    //    }
    //    public ProductAttribute GetProductAttributeById(Guid id)
    //    {
    //        return _entities.ProductAttributes.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<ProductType> GetAll()
    //    {
    //        return _entities.ProductTypes;
    //    }
    //    public void Add(ProductType productType)
    //    {
    //        _entities.AddToProductTypes(productType);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.ProductTypes.DeleteObject(GetById(id));
    //    }
    //    public void DeleteProductAttribute(Guid id)
    //    {
    //        _entities.ProductAttributes.DeleteObject(GetProductAttributeById(id));
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public void AddProductAttribute(ProductAttribute attribute)
    //    {
    //        _entities.AddToProductAttributes(attribute);
    //    }
    //}
}