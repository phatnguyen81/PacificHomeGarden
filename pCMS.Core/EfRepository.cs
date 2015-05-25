using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace pCMS.Core
{

    public class EfRepository<TObject> : IRepository<TObject>
            where TObject : class
    {
        private readonly bool _shareContext = false;

        public EfRepository()
        {
            Context = new pCMSEntities();
        }

        public EfRepository(pCMSEntities context)
        {
            Context = context;
            _shareContext = true;
        }

        protected pCMSEntities Context = null;

        protected ObjectSet<TObject> DbSet
        {
            get { return Context.CreateObjectSet<TObject>(); }
        }

        public void Dispose()
        {
            if (_shareContext && (Context != null))
                Context.Dispose();
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        //public virtual TObject Find(params object[] keys)
        //{
        //    return DbSet.Where("",keys);
        //}

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual void Create(TObject TObject)
        {
            DbSet.AddObject(TObject);
            Context.SaveChanges();
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual void Delete(TObject obj)
        {
            DbSet.DeleteObject(obj);
            Context.SaveChanges();
        }

        public virtual void Update(TObject TObject)
        {
            Context.SaveChanges();
        }

        public virtual void Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var o in objects)
            {
                DbSet.DeleteObject(o);
            }
            Context.SaveChanges();
        }
    }
}