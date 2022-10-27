using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class ParentRepository : IUnitOfWork<Parent>
    {
        private readonly ApplicationDbContext _dbContext;

        public ParentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Parent> OnItemCreationAsync(Parent parent)
        {
            await _dbContext.AddAsync(parent);

            return parent;
        }

        public async Task<Parent> OnLoadItemAsync(Guid ParentId)
        {
            return await _dbContext.Parents.FirstOrDefaultAsync(m => m.ParentId == ParentId);

        }

        public async Task<List<Parent>> OnLoadItemsAsync()
        {
            var parent = await _dbContext.Parents.ToListAsync();

            var getActiveParent = from n in parent

                                    where n.IsActive == true

                                    select n;

            return getActiveParent.ToList();

        }

        public async Task<Parent> OnModifyItemAsync(Parent parent)
        {
            var results = await _dbContext.Parents.FirstOrDefaultAsync(x => x.ParentId == parent.ParentId);

            if (results != null)
            {
                results.Cellphone = parent.Cellphone;

                results.IsActive = parent.IsActive;

                results.LastName = parent.LastName;

                results.Name = parent.Name;

                results.Email = parent.Email;

                results.Relationship = parent.Relationship;

                results.RSAID = parent.RSAID;

                results.Telephone = parent.Telephone;

                results.WorkNumber = parent.WorkNumber;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid ParentId)
        {
            var item = _dbContext.Parents.FirstOrDefaultAsync(m => m.ParentId == ParentId);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            return data.Any(predicate);
        }

    }
}

