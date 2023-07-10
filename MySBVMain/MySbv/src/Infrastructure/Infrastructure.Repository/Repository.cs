using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Domain.Data.Core;
using Domain.Repository;
using Infrastructure.Repository.Database;

namespace Infrastructure.Repository
{
    [Export(typeof (IRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Repository : IRepository
    {
        #region IRepository

        public IEnumerable<T> All<T>(params Expression<Func<T, object>>[] includeExpressions)
            where T : class, IEntity
        {
            using (var context = new Context())
            {
                return includeExpressions.Aggregate(context.Set<T>().Where(a => a.IsNotDeleted),
                    (current, includeExpression) => (DbQuery<T>) current.Include(includeExpression)).ToList();
            }
        }

        public T Find<T>(int id, params Expression<Func<T, object>>[] includeExpressions)
            where T : class, IEntity
        {
            using (var context = new Context())
            {
                if (includeExpressions.Length > 0)
                {
                    return includeExpressions.Aggregate(context.Set<T>().Where(a => a.Key == id && a.IsNotDeleted),
                        (current, expression) => (DbQuery<T>) current.Include(expression)).FirstOrDefault();
                }
                return context.Set<T>().Find(id);
            }
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeExpressions)
            where T : class, IEntity
        {
            using (var context = new Context())
            {
                return includeExpressions.Aggregate(context.Set<T>().Where(a => a.IsNotDeleted).Where(predicate),
                    (current, includeExpression) => (DbQuery<T>) current.Include(includeExpression)).ToList();
            }
        }

        public int Add<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                DateTime date = DateTime.Now;
                entity.LastChangedDate = date;
                entity.CreateDate = date;
                entity.IsNotDeleted = true;
                context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();
                return entity.Key;
            }
        }

        public int Delete<T>(int id, int lastChangedById) where T : class, IEntity
        {
            using (var context = new Context())
            {
                T entity = context.Set<T>().Find(id);
                if (entity != null)
                {
                    DateTime date = DateTime.Now;
                    entity.WalkObjectGraph(o =>
                    {
                        o.IsNotDeleted = false;
                        o.LastChangedDate = date;
                        o.LastChangedById = lastChangedById;
                        return false;
                    }, a => { });

                    return (context.SaveChanges());
                }
                return -1;
            }
        }

        public int Update<T>(T entity) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return ApplyChanges(entity, context);
            }
        }

        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            using (var context = new Context())
            {
                return context.Set<T>().Where(a => a.IsNotDeleted).Any(predicate);
            }
        }

        public int ExecuteQuery(string fullQuery)
        {
            using (var context = new Context())
            {
                return context.Database.ExecuteSqlCommand(fullQuery);
            }
        }

		public T ExecuteQueryString<T>(string fullQuery, params object[] parameters) where T : class
        {
            using (var context = new Context())
            {
                return context.Database.SqlQuery(typeof(T), fullQuery, parameters) as T;
            }
        }




        public List<int> ExecuteQueryCommand(string fullQuery)
        {
            using (var context = new Context())
            {
                return context.Database.SqlQuery<int>(fullQuery).ToList();
            }
        }

        public List<T> ExecuteQueryCommand<T>(string fullQuery)
        {
            using (var context = new Context())
            {
                return context.Database.SqlQuery<T>(fullQuery).ToList();
            }
        }

        #endregion

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
                            entry.Entity.IsNotDeleted = true;
                            entry.Entity.CreateDate = DateTime.Now;
                        }
                    }
                }

                return (context.SaveChanges());
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
            IEnumerable<DbEntityEntry> entities = from e in context.ChangeTracker.Entries()
                where ((EntityBase) e.Entity).EntityState != State.Unchanged
                select e;
            return entities.Any();
        }

        #endregion
    }
}