﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Repo.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity,bool>> where);


        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        TEntity Get(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> GetMany(
                Expression<Func<TEntity,
                bool>> filter,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                string includeEntity
            );

        //----------------------------------------------------------------------------------

        Task InsertAsync(TEntity entity);


        Task<TEntity> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsync(
                Expression<Func<TEntity,
                bool>> filter,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                string includeEntity
            );
        Task<IEnumerable<TEntity>> GetManyAsyncPaging(
            Expression<Func<TEntity, bool>> filter,

            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,

            string includeEntity,
            int count,
            int firstCount,
            int page
        );
    }
}
