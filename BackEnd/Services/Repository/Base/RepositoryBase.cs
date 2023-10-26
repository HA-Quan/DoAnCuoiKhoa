﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Services.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void UpdateMultiple(List<T> listEntity);
        void Delete(T entity);
        void Save();
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _repositoryContext;
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() => _repositoryContext.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _repositoryContext.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => _repositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => _repositoryContext.Set<T>().Update(entity);
        public void UpdateMultiple(List<T> listEntity) => _repositoryContext.Set<T>().UpdateRange(listEntity);
        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);
        public Task<bool> CheckExist(Expression<Func<T, bool>> expression) => _repositoryContext.Set<T>().AnyAsync(expression);
        public void Save() => _repositoryContext.SaveChanges();
    }
}
