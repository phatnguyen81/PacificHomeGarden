using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category_Picture> GetAllPictures(Guid categoryId);
        
        Category GetById(Guid categoryId);
        void SaveChanges();
        IEnumerable<CategoryResult> GetAllWithOrder();
        IEnumerable<CategoryResult> GetAllWithOrder(int pageIndex, int pageSize);
        IEnumerable<CategoryResult> GetAllExcludeNodeWithOrder(Guid cateogryId);
        bool CheckExistAlias(string alias);
        bool CheckExistAlias(string alias, Guid excludeId);
        void Add(Category category);
        void Delete(Category category);
        void Delete(Guid categoryId);
        Category GetByAlias(string alias);
        IEnumerable<Category> GetAll();
        int TotalCount();
        IEnumerable<Category> GetAllCategoriesByParentCategoryId(Guid? id);
    }

    public class CategoryService : ICategoryService, IDisposable
    {

        private readonly IDalContext _context;

        public CategoryService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IEnumerable<Category_Picture> GetAllPictures(Guid categoryId)
        {
            return _context.Categories.Find(q => q.Id == categoryId).Category_Picture;
        }


        public Category GetById(Guid categoryId)
        {
            return _context.Categories.Find(q => q.Id == categoryId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<CategoryResult> GetAllWithOrder()
        {
            return _context.Categories.GetAllWithOrder();
        }


        public IEnumerable<CategoryResult> GetAllWithOrder(int pageIndex, int pageSize)
        {
            return _context.Categories.GetAllWithOrder(pageIndex, pageSize);
        }


        public bool CheckExistAlias(string alias)
        {
            return _context.Categories.Contains(q => q.Alias == alias);
        }

        public bool CheckExistAlias(string alias, Guid excludeId)
        {
            return _context.Categories.Contains(q => q.Alias == alias && q.Id != excludeId);
        }

        public void Add(Category category)
        {
            _context.Categories.Create(category);
        }

        public void Delete(Category category)
        {
            while (category.Category_Picture.Count > 0)
            {
                var categoryPicture = category.Category_Picture.First();
                _context.Pictures.Delete(categoryPicture.Picture);
                _context.Categories.DeleteCategoryPicture(categoryPicture);
            }
            while (category.Product_Category.Count > 0)
            {
                _context.Categories.DeleteCategoryProduct(category.Product_Category.First());
            }
            if(category.PictureId != null)
            {
                _context.Pictures.Delete(_context.Pictures.Find(q => q.Id == category.PictureId));
            }
            
            _context.Categories.Delete(category);
        }

        public void Delete(Guid categoryId)
        {
            Delete(_context.Categories.Find(q => q.Id == categoryId));
        }

        public Category GetByAlias(string alias)
        {
            return _context.Categories.Find(q => q.Alias == alias);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.All();
        }

        public int TotalCount()
        {
            return _context.Categories.All().Count();
        }

        public IEnumerable<Category> GetAllCategoriesByParentCategoryId(Guid? id)
        {
            if(id == null || id == Guid.Empty)
            {
                return _context.Categories.Filter(q => q.ParentId == null || q.ParentId == Guid.Empty)
                        .OrderBy(q => q.DisplayOrder);
            }
            return _context.Categories.Filter(q => q.ParentId == id).OrderBy(q => q.DisplayOrder);
        }

        public IEnumerable<CategoryResult> GetAllExcludeNodeWithOrder(Guid cateogryId)
        {
            return _context.Categories.GetAllExcludeNodeWithOrder(cateogryId);
        }
    }
}
