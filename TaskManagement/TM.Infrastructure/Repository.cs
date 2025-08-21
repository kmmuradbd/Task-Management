using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TM.Core;

namespace TM.Infrastructure
{
    public abstract class Repository<TEntity>
   where TEntity : Entity
    {
        protected TMContext Context { get; set; }

        public Repository()
        {
            this.Context = new TMContext();
        }



        public void Add(TEntity entity)
        {
            try
            {
                this.Context.Set<TEntity>().Add(entity);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Context.Database.SqlQuery<TElement>(sql, parameters).AsEnumerable();
        }

        //public void Update(TEntity entity)
        //{
        //    try
        //    {
        //        if (this.Context.Set<TEntity>().Local.Any(r => r.Id == entity.Id))
        //        {
        //            var currentEntity = this.Context.Set<TEntity>().Find(entity.Id);
        //            this.Context.Entry<TEntity>(currentEntity).CurrentValues.SetValues(entity);
        //        }
        //        else
        //        {
        //            this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
        //        }
        //        this.Context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            if (Context.Set<TEntity>().Local.Count > 0)
            {
                var key = GetEntityKeyValue(entity);
                var currentEntity = Find(key.ToArray());
                this.Context.Entry<TEntity>(currentEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }
        public void Update(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        private IEnumerable<object> GetEntityKeyValue(TEntity entity)
        {
            return GetEntityKeyName().Select(r => r.GetValue(entity, null));
        }

        private IEnumerable<PropertyInfo> GetEntityKeyName()
        {
            var type = typeof(TEntity);
            var set = ((IObjectContextAdapter)Context).ObjectContext.CreateObjectSet<TEntity>().EntitySet;
            return set.ElementType.KeyMembers.Select(k => type.GetProperty(k.Name));
        }

        //public void Delete(string id)
        //{
        //    var currentEntity = this.Context.Set<TEntity>().Where(x => x.Id.Equals(id)).FirstOrDefault();
        //    this.Context.Set<TEntity>().Remove(currentEntity);
        //    this.Context.SaveChanges();
        //}

        public virtual TEntity Find(params object[] keyValues)
        {
            return Context.Set<TEntity>().Find(keyValues);
        }

        public void Delete(int id)
        {
            var currentEntity = Find(id);
            this.Context.Set<TEntity>().Remove(currentEntity);
            this.Context.SaveChanges();
        }


        public int Count()
        {
            return Count(x => true);
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Count(expression);
        }

        public TEntity Get(int id)
        {
            return this.Context.Set<TEntity>().Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            TEntity rValue = this.Context.Set<TEntity>().Where(expression).FirstOrDefault();
            if (rValue == null)
            {
                rValue = Activator.CreateInstance<TEntity>();
            }
            return rValue;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetAll(x => true);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Where(expression);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, string include)
        {
            return this.Context.Set<TEntity>().Where(expression).Include(include);
        }

        public bool GetAny(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Any(expression);
        }

        //public string GetAutoNumber()
        //{
        //    string id;
        //    try
        //    {
        //        var data = this.Context.Set<TEntity>().ToList();
        //        id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();
        //    }
        //    catch (Exception)
        //    {
        //        id = "1";
        //    }
        //    return id;
        //}

        public int GetAutoNumber()
        {
            try
            {
                return Convert.ToInt32(Context.Database.SqlQuery<int>("SELECT MAX(CAST(Id AS INT)) + 1 FROM " + GetTableName<TEntity>()).FirstOrDefault().ToString());
            }
            catch (Exception)
            {
                return 1;
            }

        }

        public string GetTableName<T>() where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            string sql = objectContext.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);
            string table = match.Groups["table"].Value;
            return table;
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
