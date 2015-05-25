using System;
using System.Collections.Generic;
using System.Linq;
using pCMS.Core;

namespace pCMS.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<CategoryResult> GetAllWithOrder();
        IEnumerable<CategoryResult> GetAllWithOrder(int pageIndex, int pageSize);
        IEnumerable<CategoryResult> GetAllExcludeNodeWithOrder(Guid cateogryId);
        void DeleteCategoryPicture(Category_Picture categoryPicture);
        void DeleteCategoryProduct(Product_Category productCategory);
    }

    public class CategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(pCMSEntities context) : base(context) { }

        public IEnumerable<CategoryResult> GetAllWithOrder()
        {
            return Context.sp_GetAllCategories().OrderBy(q=>q.DisplayOrder);
        }

        public IEnumerable<CategoryResult> GetAllWithOrder(int pageIndex, int pageSize)
        {
            return Context.sp_GetAllCategories_Paging(pageIndex, pageSize);
        }

        public IEnumerable<CategoryResult> GetAllExcludeNodeWithOrder(Guid cateogryId)
        {
            return Context.sp_GetAllCategoriesExcludeNode(cateogryId);
        }

        public void DeleteCategoryPicture(Category_Picture categoryPicture)
        {
            Context.Category_Picture.DeleteObject(categoryPicture);
        }

        public void DeleteCategoryProduct(Product_Category productCategory)
        {
            Context.Product_Category.DeleteObject(productCategory);
        }
    }

    //public class CategoryRepository
    //{
    //    private readonly pCMSEntities _entities;

    //    public CategoryRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public CategoryRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Category GetById(Guid id)
    //    {
    //        return _entities.Categories.FirstOrDefault(q => q.Id == id);
    //    }
    //    public Category GetByAlias(string alias)
    //    {
    //        return _entities.Categories.FirstOrDefault(q => q.Alias == alias);
    //    }
    //    public IEnumerable<Category> GetAll()
    //    {
    //        return _entities.Categories;
    //    }
    //    public IEnumerable<CategoryResult> GetAllWithOrder()
    //    {
    //        return _entities.sp_GetAllCategories();
    //    }
    //    public IEnumerable<CategoryResult> GetAllExcludeNodeWithOrder(Guid nodeId)
    //    {
    //        return _entities.sp_GetAllCategoriesExcludeNode(nodeId);
    //    }
    //    public void Add(Category category)
    //    {
    //        _entities.AddToCategories(category);
    //    }
    //    public void Delete(Guid id)
    //    {
    //        _entities.Categories.DeleteObject(GetById(id));
    //    }
    //    public bool CheckExistAlias(string alias)
    //    {
    //        return CheckExistAlias(alias, Guid.Empty);
    //    }
    //    public bool CheckExistAlias(string alias, Guid owner)
    //    {
    //        return owner == Guid.Empty ? _entities.Categories.Any(q => q.Alias == alias) : _entities.Categories.Any(q => q.Alias == alias && q.Id != owner);
    //    }
    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public List<Category_Picture> GetAllPictures(Guid categoryId)
    //    {
    //        var categoryPictures = _entities.Category_Picture.Where(q => q.CategoryId == categoryId).ToList();
    //        foreach (var picture in categoryPictures)
    //        {
    //            picture.PictureReference.Load();
    //        }
    //        return categoryPictures;
    //    }
    //}
}