using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Diagnostics;
using ZZLibModel;

namespace zzLibrary.DAOs
{
    public class BaseDAO<TObject> where TObject : class
    {
        public zzLibEntities db { get; set; }
        //public zzLibraryEntities db { get; set; }

        public BaseDAO()
        {
            db = new zzLibEntities();
        }

        ~BaseDAO()
        {
            db.Dispose();
        }
        
        public ICollection<TObject> GetAll()
        {
            return db.Set<TObject>().ToList();
        }

        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await db.Set<TObject>().ToListAsync();
        }

        public TObject Get(Object id)
        {
            try
            {
                return db.Set<TObject>().Find(id);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return null;
            }
        }

        public async Task<TObject> GetAsync(Object id)
        {
            try
            {
                return await db.Set<TObject>().FindAsync(id);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return null;
            }
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return db.Set<TObject>().SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await db.Set<TObject>().SingleOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return db.Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await db.Set<TObject>().Where(match).ToListAsync();
        }

        public TObject Add(TObject t)
        {
            try
            {
                db.Set<TObject>().Add(t);
                db.SaveChanges();
                return t;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                return null;
            }
        }

        public async Task<TObject> AddAsync(TObject t)
        {
            try
            {
                db.Set<TObject>().Add(t);
                await db.SaveChangesAsync();
                return t;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                return null;
            }
        }

        public TObject Update(TObject updated, Object key)
        {
            if (updated == null)
                return null;

            TObject existing = db.Set<TObject>().Find(key);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(updated);
                db.SaveChanges();
            }
            return existing;
        }

        public async Task<TObject> UpdateAsync(TObject updated, Object key)
        {
            if (updated == null)
                return null;

            TObject existing = await db.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(updated);
                await db.SaveChangesAsync();
            }
            return existing;
        }

        public void Delete(TObject t)
        {
            db.Set<TObject>().Remove(t);
            db.SaveChanges();
        }

        public async Task<int> DeleteAsync(TObject t)
        {
            db.Set<TObject>().Remove(t);
            return await db.SaveChangesAsync();
        }

        public int Count()
        {
            return db.Set<TObject>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await db.Set<TObject>().CountAsync();
        }
    }
}