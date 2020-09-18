﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Repo.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {

        #region ctor

        private readonly DbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        #endregion

        #region normal

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new AggregateException("Thre is no entity!!");
            }
            _dbSet.Update(entity);
        }

        public void Delete(object id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                throw new AggregateException("Thre is no entity!!");
            }
            _dbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objs = _dbSet.Where(where).AsEnumerable();
            foreach (TEntity item in objs)
            {
                _dbSet.Remove(item);
            }
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetMany(
                Expression<Func<TEntity,
                bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeEntity = ""
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        #endregion

        #region async

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }

        }

        public async Task<IEnumerable<TEntity>> GetManyAsyncPaging(Expression<Func<TEntity,
            bool>> filter, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy,
            string includeEntity, int count, int firstCount, int page)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.Skip(firstCount).Skip(count * page).Take(count).ToListAsync();
        }

        #endregion

        #region dispose
        private bool dispose = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!dispose)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            dispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }
        #endregion
    }
}
