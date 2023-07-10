using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Data.Core;

namespace Domain.Repository
{
    public interface IAsyncRepository 
    {
        /// <summary>
        /// Get ALL Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> AllAsync<T>(params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;


        /// <summary>
        /// Find Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(int id, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;


        /// <summary>
        /// Get collection Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;


        /// <summary>
        /// Get first record or default Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        Task<T> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;

        /// <summary>
        /// Add Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddAsync<T>(T entity) where T : class, IEntity;


        /// <summary>
        /// Delete Entity Asyncrounously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T entity) where T : class, IEntity;


        /// <summary>
        /// Update Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(T entity) where T : class, IEntity;


        /// <summary>
        /// Check if Any Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;


        /// <summary>
        /// ExecuteQuery Asyncronously
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        Task<int> ExecuteQueryAsync(string fullQuery);



        //===================================================================================================================================
        // Synchronous Calls
        //===================================================================================================================================


        /// <summary>
        /// Add Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add<T>(T entity) where T : class, IEntity;


        /// <summary>
        /// Update Entity Asyncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update<T>(T entity) where T : class, IEntity;


        /// <summary>
        /// Find Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        T Find<T>(int id, params Expression<Func<T, object>>[] includeExpressions)
            where T : class, IEntity;



        /// <summary>
        /// Get collection syncronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;



        /// <summary>
        /// Delete Entity syncrounously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<T>(T entity) where T : class, IEntity;



        /// <summary>
        /// ExecuteQuery Asyncronously
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        int ExecuteQuery(string fullQuery);
    }
}
