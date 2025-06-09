using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly FunewsManagementContext _context;

        public GenericRepository(FunewsManagementContext context)
        {
            _context = context;
        }


        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = _context.Set<TEntity>();

            if (includeProperties.Any())
            {
                foreach (var property in includeProperties)
                {
                    items = items.Include(property);
                }
            }


            if (predicate != null)
            {
                items = items.Where(predicate);
            }

            return items;
        }


        public async Task<TEntity?> FindById(TKey id, string idPropertyName, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, idPropertyName).Equals(id));
        }




        public void Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }


        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
       
    }
}
