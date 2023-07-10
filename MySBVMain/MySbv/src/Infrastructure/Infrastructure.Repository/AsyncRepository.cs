using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Data.Core;
using Domain.Repository;
using Infrastructure.Repository.Database;

namespace Infrastructure.Repository
{
    [Export(typeof(IRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AsyncRepository : IAsyncRepository
    {
        /// <summary>
        /// Get ALL Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> AllAsync<T>(params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return await includeExpressions.Aggregate(context.Set<T>().Select(e => e),
                    (current, includeExpression) => (DbQuery<T>)current.Include(includeExpression)).ToListAsync();
            }
        }


        /// <summary>
        /// Find Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public async Task<T> FindAsync<T>(int id, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                if (includeExpressions.Length > 0)
                {
                    return includeExpressions.Aggregate(context.Set<T>().Where(a => a.Key == id),
                        (current, expression) => (DbQuery<T>)current.Include(expression)).FirstOrDefault();
                }
                return await context.Set<T>().FindAsync(id);
            }
        }


        /// <summary>
        /// Get collection Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return await includeExpressions.Aggregate(context.Set<T>().Select(o => o).Where(predicate),
                    (current, includeExpression) => (DbQuery<T>)current.Include(includeExpression)).ToListAsync();
            }
        }


        /// <summary>
        /// Get first record or default Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return await includeExpressions.Aggregate(context.Set<T>().Select(o => o).Where(predicate),
                    (current, includeExpression) => (DbQuery<T>)current.Include(includeExpression)).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Add Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                DateTime date = DateTime.Now;
                entity.LastChangedDate = date;
                entity.CreateDate = date;
                context.Entry(entity).State = EntityState.Added;
                await context.SaveChangesAsync();
                return entity.Key;
            }
        }


        /// <summary>
        /// Delete Entity Asyncrounously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                if (entity != null)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                    return await context.SaveChangesAsync() > 0;
                }
                return false;
            }
        }


        /// <summary>
        /// Update Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return await ApplyChangesAsync(entity, context);
            }
        }


        /// <summary>
        /// Check if Any Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return await context.Set<T>().Select(a => a).AnyAsync(predicate);
            }
        }


        /// <summary>
        /// ExecuteQuery Asyncronously
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        public async Task<int> ExecuteQueryAsync(string fullQuery)
        {
            using (var context = new Context())
            {
                return await context.Database.ExecuteSqlCommandAsync(fullQuery);
            }
        }


        #region Internal Methods

        private EntityState ConvertSate(IEntity entity)
        {
            switch (entity.EntityState)
            {
                case State.Added:
                    return EntityState.Added;
                case State.Modified:
                    return EntityState.Modified;
                case State.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        /// <summary>
        /// Apply changes Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<int> ApplyChangesAsync<T>(T root, Context context) where T : class, IEntity
        {
            context.Set<T>().Add(root);

            VerifyEntity(context);

            if (ObjectTreeModified(context))
            {
                foreach (var entry in context.ChangeTracker.Entries<IEntity>())
                {
                    entry.State = ConvertSate(entry.Entity);
                    if (entry.State != EntityState.Unchanged)
                    {
                        entry.Entity.LastChangedDate = DateTime.Now;
                        if (entry.State == EntityState.Added)
                        {
                            entry.Entity.CreateDate = DateTime.Now;
                        }
                    }
                }
                return await context.SaveChangesAsync();
            }
            return -1;
        }

        private void VerifyEntity(Context context)
        {
            IEnumerable<DbEntityEntry> entities = from e in context.ChangeTracker.Entries()
                                                  where !(e.Entity is IEntity)
                                                  select e;

            if (entities.Any())
                throw new NotSupportedException("All entities must inherit from EntityBase or implement IEntity");
        }


        private bool ObjectTreeModified(Context context)
        {
            var entities = from e in context.ChangeTracker.Entries()
                           where ((EntityBase)e.Entity).EntityState != State.Unchanged
                           select e;
            return entities.Any();
        }

        #endregion







        //==================================================================================================================
        // Synchronous Calls
        //==================================================================================================================


        /// <summary>
        /// Add Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                DateTime date = DateTime.Now;
                entity.LastChangedDate = date;
                entity.CreateDate = date;
                context.Entry(entity).State = EntityState.Added;
                context.SaveChangesAsync();
                return entity.Key;
            }
        }



        /// <summary>
        /// Update Entity syncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return ApplyChanges(entity, context);
            }
        }



        /// <summary>
        /// Apply changes Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private int ApplyChanges<T>(T root, Context context) where T : class, IEntity
        {
            context.Set<T>().Add(root);

            VerifyEntity(context);

            if (ObjectTreeModified(context))
            {
                foreach (var entry in context.ChangeTracker.Entries<IEntity>())
                {
                    entry.State = ConvertSate(entry.Entity);
                    if (entry.State != EntityState.Unchanged)
                    {
                        entry.Entity.LastChangedDate = DateTime.Now;
                        if (entry.State == EntityState.Added)
                        {
                            entry.Entity.CreateDate = DateTime.Now;
                        }
                    }
                }
                return context.SaveChanges();
            }
            return -1;
        }



        /// <summary>
        /// Find Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public T Find<T>(int id, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                if (includeExpressions.Length > 0)
                {
                    return includeExpressions.Aggregate(context.Set<T>().Where(a => a.Key == id),
                        (current, expression) => (DbQuery<T>)current.Include(expression)).FirstOrDefault();
                }
                return context.Set<T>().Find(id);
            }
        }



        /// <summary>
        /// Get collection syncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return includeExpressions.Aggregate(context.Set<T>().Select(o => o).Where(predicate),
                    (current, includeExpression) => (DbQuery<T>)current.Include(includeExpression)).ToList();
            }
        }



        /// <summary>
        /// Hard Delete Entity syncrounously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                if (entity != null)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }



        /// <summary>
        /// ExecuteQuery Asyncronously
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        public int ExecuteQuery(string fullQuery)
        {
            using (var context = new Context())
            {
                return context.Database.ExecuteSqlCommand(fullQuery);
            }
        }
    }
}
