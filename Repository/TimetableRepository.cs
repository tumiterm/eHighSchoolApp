using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class TimetableRepository : IUnitOfWork<TimeTable>
    {
        private readonly ApplicationDbContext _dbContext;
        public TimetableRepository(ApplicationDbContext dbContext)
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

        public async Task<TimeTable> OnItemCreationAsync(TimeTable timetable)
        {
            await _dbContext.AddAsync(timetable);

            return timetable;
        }

        public async Task<TimeTable> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.EventTimeTable.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<List<TimeTable>> OnLoadItemsAsync()
        {
            var events = await _dbContext.EventTimeTable.ToListAsync();

            var getActiveTimetables = from n in events

                                  where n.IsActive == true

                                  select n;

            return getActiveTimetables.ToList();
        }

        public async Task<TimeTable> OnModifyItemAsync(TimeTable timetable)
        {
            var results = await _dbContext.EventTimeTable.FirstOrDefaultAsync(x => x.Id == timetable.Id);

            if (results != null)
            {
                results.Subject = timetable.Subject;

                results.Title = timetable.Title;

                results.Grade = timetable.Grade;

                return results;
            }

            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.EventTimeTable.FirstOrDefaultAsync(m => m.Id == Id);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
