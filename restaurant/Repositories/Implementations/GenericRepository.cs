using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Repositories.Interfaces;

namespace restaurant.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #region GetAllAsync
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var result = await _dbSet.ToListAsync();
                if (result == null || !result.Any())
                    throw new ArgumentNullException("تمت محاولة الوصول إلى كائن null");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء جلب البيانات.", ex);
            }
        }
        #endregion

        #region GetByIdAsync
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        #endregion

        #region AddAsync
        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "لا يوجد بيانات لإضافتها.");

            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء عملية الإضافة.", ex);
            }
        }
        #endregion

        #region UpdateAsync
        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء عملية التحديث.", ex);
            }
        }
        #endregion

        #region Delete
        public virtual async Task Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء عملية الحذف.", ex);
            }
        }
        #endregion

        #region SaveAsync
        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
