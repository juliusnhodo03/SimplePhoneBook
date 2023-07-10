using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Data.Core;

namespace Domain.Repository
{
    public interface IRepository
    {
        /// <summary>
        ///     Get All items of type T Asynchronously
        /// </summary>
        /// <typeparam name="T">The type of entity to get</typeparam>
        /// <returns>Collection of Entity type T</returns>
        IEnumerable<T> All<T>(params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;

        /// <summary>
        ///     Find an object of type T Asynchronously
        /// </summary>
        /// <typeparam name="T">The Entity to get</typeparam>
        /// <param name="id">The primary key id of the entity to get</param>
        /// <param name="includeExpressions"></param>
        /// <returns>An Entity of type T</returns>
        T Find<T>(int id, params Expression<Func<T, object>>[] includeExpressions) where T : class, IEntity;

        /// <summary>
        ///     Get a collection of type T based of the predicate query. Asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object to Get</typeparam>
        /// <param name="predicate">A query to filter object with</param>
        /// <param name="includeExpressions"></param>
        /// <returns>A collection of entity type of T</returns>
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeExpressions)
            where T : class, IEntity;

        /// <summary>
        ///     Add and entity in the db Asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object to add</typeparam>
        /// <param name="entity">the entity to save to the db</param>
        /// <returns>The primary key of the added entity</returns>
        int Add<T>(T entity) where T : class, IEntity;

        /// <summary>
        ///     Delete and entity from the db Asynchronously
        /// </summary>
        /// <typeparam name="T">The type of entity to delete</typeparam>
        /// <param name="id">The primary key of the entity</param>
        /// <param name="lastChangedById"></param>
        /// <returns>True if the object was deleted, False if not.</returns>
        int Delete<T>(int id, int lastChangedById) where T : class, IEntity;

        /// <summary>
        ///     Update an entity in the db Asynchronously
        /// </summary>
        /// <typeparam name="T">The type of entity to update</typeparam>
        /// <param name="entity">The entity to update</param>
        /// <returns>the primary key of the updated entity.</returns>
        int Update<T>(T entity) where T : class, IEntity;

        /// <summary>
        ///     Check if an expression is satisfied Asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;

        /// <summary>
        ///     Execute an SQL string Asynchronously
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        int ExecuteQuery(string fullQuery);

        /// <summary>
        ///     Execute an SQL string Asynchronously
        /// </summary>
        /// <param name="fullQuery">Query to execute</param>
        /// <param name="parameters">parameters to pass to the query</param>
        /// <returns></returns>
        T ExecuteQueryString<T>(string fullQuery, params object[] parameters) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        List<int> ExecuteQueryCommand(string fullQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        List<T> ExecuteQueryCommand<T>(string fullQuery);
    }
}