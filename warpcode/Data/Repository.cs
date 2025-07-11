using CCIMIGRATION.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCIMIGRATION.Data
{
    public class Repository
    {
        #region fields

        private static OptraxDataBase database;

        #endregion

        #region constructor

        public Repository()
        {
            database = new OptraxDataBase();
        }

        #endregion

        #region public methods

        public async Task<List<T>> GetAsync<T>() where T : BaseEntity, new()
        {
            return await database.Database.Table<T>().ToListAsync();
        }

        public async Task<T> GetAsync<T>(int id) where T : BaseEntity, new()
        {
            return await database.Database.FindAsync<T>(id);
        }

        public async Task<int> CreateAsync<T>(T entity, bool synced = false) where T : BaseEntity, new()
        {
            var date = DateTime.UtcNow;
            entity.CreatedOn = date;
            entity.ModifiedOn = date;
            if (synced)
            {
                entity.SyncDate = date;
            }
            return await database.Database.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity, bool synced = false) where T : BaseEntity, new()
        {
            var date = DateTime.UtcNow;
            entity.ModifiedOn = date;
            if (synced)
            {
                entity.SyncDate = date;
            }
            return await database.Database.UpdateAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(List<T> entities) where T : BaseEntity, new()
        {
            foreach (var item in entities)
            {
                item.ModifiedOn = DateTime.UtcNow;
            }

            return await database.Database.UpdateAllAsync(entities, true);
        }

        public Task<int> DeleteAsync<T>(T entity) where T : BaseEntity, new()
        {
            return database.Database.DeleteAsync(entity);
        }

        public Task<int> DeleteAllAsync<T>() where T : BaseEntity, new()
        {
            return database.Database.DeleteAllAsync<T>();
        }

        #endregion
    }
}
