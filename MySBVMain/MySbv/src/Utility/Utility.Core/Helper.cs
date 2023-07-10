using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Utility.Core
{
    public static class Helper
    {
        /// <summary>
        ///     Method is used to execute code that returns a result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="codeToExecute"></param>
        /// <param name="resultMessage"></param>
        /// <param name="objectTag"></param>
        /// <returns></returns>
        public static MethodResult<T> HandleException<T>(Func<T> codeToExecute, string resultMessage = "",
            Object objectTag = null) where T : new()
        {
            MethodResult<T> response = null;

            try
            {
                T codeToExecuteResult = codeToExecute.Invoke();
                response = new MethodResult<T>(MethodStatus.Successful, codeToExecuteResult, resultMessage, objectTag,
                    new ErrorDetails()
                    {
                        ErrorMessage = string.Empty,
                        ExecuteDateAndTime = DateTime.Now,
                        HasError = false,
                        MethodName = MethodBase.GetCurrentMethod().Name,
                        StackTrace = string.Empty,
                        ExceptionType = null
                    });
            }
            catch (Exception ex)
            {
                response = new MethodResult<T>(MethodStatus.Error, new T() // Result <T> is null
                    ,
                    string.Format("An Exception Of type {0} has occurred.\nFor more details, look in to error details",
                        ex.GetType()), null // Tag is null
                    , new ErrorDetails
                    {
                        ErrorMessage = ex.Message,
                        ExecuteDateAndTime = DateTime.Now,
                        HasError = true,
                        MethodName = MethodBase.GetCurrentMethod().Name,
                        StackTrace = ex.StackTrace,
                        ExceptionType = ex.GetType()
                    });
            }
            return response;
        }

        /// <summary>
        ///     Method is used to execute code that does not return anything. E.g a void Method
        ///     Method is suited if you execute a void method but you want to receive back exception
        ///     details should the method fail due to and exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="codeToExecute"></param>
        /// <param name="resultMessage"></param>
        /// <param name="objectTag"></param>
        /// <returns></returns>
        public static MethodResult<T> HandleException<T>(Action codeToExecute, string resultMessage = "",
            Object objectTag = null) where T : new()
        {
            MethodResult<T> response = null;

            try
            {
                codeToExecute.Invoke();
                response = new MethodResult<T>(MethodStatus.Successful, new T(), resultMessage, objectTag,
                    new ErrorDetails()
                    {
                        ErrorMessage = string.Empty,
                        ExecuteDateAndTime = DateTime.Now,
                        HasError = false,
                        MethodName = MethodBase.GetCurrentMethod().Name,
                        StackTrace = string.Empty,
                        ExceptionType = null
                    });
            }
            catch (Exception ex)
            {
                response = new MethodResult<T>(MethodStatus.Error, new T() // Result <T> is null
                    ,
                    string.Format("An Exception Of type {0} has occurred.\nFor more details, look in to error details",
                        ex.GetType()), null // Tag is null
                    , new ErrorDetails
                    {
                        ErrorMessage = ex.Message,
                        ExecuteDateAndTime = DateTime.Now,
                        HasError = true,
                        MethodName = MethodBase.GetCurrentMethod().Name,
                        StackTrace = ex.StackTrace,
                        ExceptionType = ex.GetType()
                    });
            }
            return response;
        }

        /// <summary>
        /// Method is used to execute code that does not return anything. E.g a void Method
        /// The method is suited for fire and forget situations. where you do not care about any exception details
        /// if there's any.
        /// </summary>
        /// <param name="codeToExecute"></param>
        public static void HandleException(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                /// TODO - LOG EXCEPTION DETAILS
            }
        }

        /// <summary>
        /// Get an Enum id,value pair in a form of a selectList 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObject"></param>
        /// <returns></returns>
        public static SelectList GetEnumValues<TEnum>(TEnum enumObject)
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObject);
        }

        /// <summary>
        /// Get the name of an Enum property
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObject"></param>
        /// <returns></returns>
        public static string Name<TEnum>(this TEnum enumObject)
        {
            return Enum.GetName(typeof(TEnum), enumObject);
        }

		/// <summary>
		/// Convert an IEnumerable of type<t> in to a collection of type<T>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static Collection<T> ToCollection<T>(this IEnumerable<T> items)
		{
			var collection = new Collection<T>();

			foreach (T t in items)
			{
				collection.Add(t);
			}

			return collection;
		}

        public static string GetLast(this string source, int tailLength)
        {
            if (tailLength >= source.Length)
                return source;
            return source.Substring(source.Length - tailLength);
        }

    }
}