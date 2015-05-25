using System;
using pCMS.Core;
using System.Linq;

namespace pCMS.Data
{
    public interface IProductAttributeRepository : IRepository<ProductAttribute>
    {
        ProductAttributeOption GetOptionById(Guid optionId);
        void DeleteOption(Guid id);
    }
    public class ProductAttributeRepository : EfRepository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(pCMSEntities context) : base(context) { }
        public ProductAttributeOption GetOptionById(Guid optionId)
        {
            return Context.ProductAttributeOptions.FirstOrDefault(q => q.Id == optionId);
        }

        public void DeleteOption(Guid id)
        {
            Context.ProductAttributeOptions.DeleteObject(GetOptionById(id));
        }
    }
    //public class ProductAttributeRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public ProductAttributeRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public ProductAttributeRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public ProductAttribute GetById(Guid id)
    //    {
    //        return _entities.ProductAttributes.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<ProductAttribute> GetAll()
    //    {
    //        return _entities.ProductAttributes;
    //    }
    //    public void Add(ProductAttribute attribute)
    //    {
    //        _entities.AddToProductAttributes(attribute);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.ProductAttributes.DeleteObject(GetById(id));
    //    }
    //    public ProductAttributeOption GetOptionById(Guid id)
    //    {
    //        return _entities.ProductAttributeOptions.FirstOrDefault(q => q.Id == id);
    //    }
    //    public void DeleteOption(Guid id)
    //    {
    //        _entities.ProductAttributeOptions.DeleteObject(GetOptionById(id));
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

     
    //}
}