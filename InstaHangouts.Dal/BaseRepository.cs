namespace InstaHangouts.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using InstaHangouts.Dal.Enum;
    using Interface;

    /// <summary>
    /// The database context
    /// </summary>        
    public partial class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The database context
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Using Win32 naming for consistency.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "SA1401 : CSharp.Maintainability : Fields must be declared with private access. Use properties to expose fields.", Justification = "Fields must be declared with private access.")]
        protected instahangoutsEntities dbContext;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        public BaseRepository()
        {
            this.dbContext = new instahangoutsEntities();
        }

        public BaseRepository(DbContext context)
        {
           // dbContext = context;
         
        }
        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.dbContext.Dispose();
                }
            }
            this.isDisposed = true;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>return all data from entity.</returns>
        public IQueryable<TEntity> GetAll()
        {
            return this.dbContext.Set<TEntity>().AsQueryable();
        }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Add(TEntity entity)
        {
            try
            {
                ////SetPropertyValue(entity, nameof(model.Createdon));
                ////SetPropertyValue(entity, nameof(model.Modifiedon));
                this.dbContext.Entry(entity as TEntity).State = EntityState.Added;
            this.dbContext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message1 = string.Format("{0}:{1}:{2}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage, validationError.PropertyName);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message1, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyNames">The property names.</param>
        public void Update(TEntity entity, params object[] propertyNames)
        {
            this.dbContext.Entry(entity as TEntity).State = EntityState.Modified;
         
            ////SetPropertyValue(entity, nameof(model.Modifiedon));             
            foreach (var propertyName in propertyNames)
            {
                this.dbContext.Entry(entity as TEntity).Property(Convert.ToString(propertyName)).IsModified = true;
            }

           this.dbContext.SaveChanges();
        }


        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(TEntity entity)
        {
            this.dbContext.Entry(entity as TEntity).State = EntityState.Deleted;
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes the where.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public void DeleteWhere(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> entities = this.dbContext.Set<TEntity>().AsNoTracking<TEntity>().Where(predicate.Invoke);
            Parallel.ForEach(entities, e => { this.dbContext.Entry(e).State = EntityState.Deleted; });
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>returns TEntity</returns>
        public TEntity Find(Func<TEntity, bool> predicate)
        {
            return this.dbContext.Set<TEntity>().AsNoTracking<TEntity>().FirstOrDefault(predicate.Invoke);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>returns enumerable entity list.</returns>
        public IEnumerable<TEntity> FindAll(Func<TEntity, bool> predicate)
        {
            return this.dbContext.Set<TEntity>().AsNoTracking<TEntity>().Where(predicate.Invoke);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="innerExceptionStackTrace">The inner exception stack trace.</param>
        public void LogError(Exception ex)
        {
            var entity = this.dbContext.ChangeTracker.Entries<TEntity>();
            var type = entity.GetType();   
            using (var db = new instahangoutsEntities())
            {
                var error = new LogData()
                {
                    Application = "InstaHangouts",
                    Message = ex.Message,
                    Exception = ex.StackTrace,
                    Logged = DateTime.Now,
                    LogLevel = "2",
                    Source = Convert.ToString(ex.InnerException)
                };
                db.Entry(error);
                db.Set<LogData>().Add(error as LogData);
                db.SaveChanges();
            }
        }

        public void LogInfo(string message)
        {
            var entity = this.dbContext.ChangeTracker.Entries<TEntity>();
            var type = entity.GetType();
            try
            {
                using (var db = new instahangoutsEntities())
                {
                    var error = new LogData()
                    {
                        Application = "InstaHangouts",
                        Message = message,
                        Exception = message,
                        Logged = DateTime.Now,
                        LogLevel = "2",
                    };
                    db.Entry(error);
                    db.Set<LogData>().Add(error as LogData);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message1 = string.Format("{0}:{1}:{2}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage, validationError.PropertyName);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message1, raise);                        
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exists the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Exists(Func<TEntity, bool> predicate)
        {
            return this.dbContext.Set<TEntity>().AsNoTracking<TEntity>().Any(predicate.Invoke);
        }

        /// <summary>
        /// Databases the execute.
        /// </summary>
        /// <typeparam name="T"> parameter type</typeparam>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="type">The type.</param>
        /// <param name="async">if set to <c>true</c> [asynchronous].</param>
        /// <returns>Returns the entity</returns>
        protected T DbExecute<T>(Func<instahangoutsEntities, T> executeAction, ExType type = ExType.Get, bool async = false) where T : class
        {
            var result = executeAction(this.dbContext);

            if (type == ExType.Set)
            {
                if (async)
                {
                    this.dbContext.SaveChangesAsync();
                }
                else
                {
                    this.dbContext.SaveChanges();
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        private void SetPropertyValue(TEntity entity, string propertyName)
        {
            var type = entity.GetType();
            var date = type.GetProperty(propertyName);
            if (date != null)
            {
                date.SetValue(entity, DateTime.Now);
            }
        }
    }
}
