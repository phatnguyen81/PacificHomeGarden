using pCMS.Core;

namespace pCMS.Data
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
    }

    public class ManufacturerRepository : EfRepository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(pCMSEntities context) : base(context) { }
    }

    //public class ManufacturerRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public ManufacturerRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public ManufacturerRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Manufacturer GetById(Guid id)
    //    {
    //        return _entities.Manufacturers.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Manufacturer> GetAll()
    //    {
    //        return _entities.Manufacturers;
    //    }
    //    public void Add(Manufacturer manufacturer)
    //    {
    //        _entities.AddToManufacturers(manufacturer);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Manufacturers.DeleteObject(GetById(id));
    //    }
    //    public bool CheckExistAlias(string alias)
    //    {
    //        return CheckExistAlias(alias, Guid.Empty);
    //    }
    //    public bool CheckExistAlias(string alias, Guid owner)
    //    {
    //        return owner == Guid.Empty ? _entities.Manufacturers.Any(q => q.Alias == alias) : _entities.Manufacturers.Any(q => q.Alias == alias && q.Id != owner);
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

     
    //}
}