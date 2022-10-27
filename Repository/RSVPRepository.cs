using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class RSVPRepository : IUnitOfWork<RSVP>
    {
        private readonly ApplicationDbContext _dbContext;
        public RSVPRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            return data.Any(predicate);
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<RSVP> OnItemCreationAsync(RSVP rsvp)
        {
            await _dbContext.AddAsync(rsvp);

            return rsvp;

        }

        public async Task<RSVP> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.RSVP.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<List<RSVP>> OnLoadItemsAsync()
        {
            var meetings = await _dbContext.RSVP.ToListAsync();

            var getActiveRSVP = from n in meetings

                                 where n.IsActive == true

                                 select n;

            return getActiveRSVP.ToList();
        }

        public async Task<RSVP> OnModifyItemAsync(RSVP rsvp)
        {
            var results = await _dbContext.RSVP.FirstOrDefaultAsync(x => x.Id == rsvp.Id);

            if (results != null)
            {
                results.Availability = rsvp.Availability;

                results.IsActive = rsvp.IsActive;

                return results;

            }
            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var rsvp = _dbContext.RSVP.FirstOrDefaultAsync(m => m.Id == Id);

            if (rsvp != null)
            {
                _dbContext.Remove(rsvp);

                _dbContext.SaveChangesAsync();
            }
        }
    }
    
}
