using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TM.Core
{
    public interface IRepository<T> : IDisposable
             where T : Entity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        int Count();
        int Count(Expression<Func<T, bool>> expression);
        T Get(int id);

        /// <summary>
        /// Get any by expression first or default
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, string include);
        bool GetAny(Expression<Func<T, bool>> expression);
        int GetAutoNumber();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

    }
}
