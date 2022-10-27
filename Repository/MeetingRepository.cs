using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class MeetingRepository : IUnitOfWork<Meeting>
    {
        private readonly ApplicationDbContext _dbContext;
        public MeetingRepository(ApplicationDbContext dbContext)
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
        public async Task<Meeting> OnItemCreationAsync(Meeting meeting)
        {
            await _dbContext.AddAsync(meeting);

            return meeting;
        }
        public async Task<Meeting> OnLoadItemAsync(Guid MeetingId)
        {
            return await _dbContext.Meeting.FirstOrDefaultAsync(m => m.MeetingId == MeetingId);
        }
        public async Task<List<Meeting>> OnLoadItemsAsync()
        {
            var meetings = await _dbContext.Meeting.ToListAsync();

            var getMeetings = from n in meetings

                             where n.IsActive == true

                             select n;

            return getMeetings.ToList();
        }
        public async Task<Meeting> OnModifyItemAsync(Meeting meeting)
        {
            var results = await _dbContext.Meeting.FirstOrDefaultAsync(x => x.MeetingId == meeting.MeetingId);

            if (results != null)
            {
                results.Date = meeting.Date;

                results.Grade = meeting.Grade;

                results.IsActive = meeting.IsActive;

                results.Message = meeting.Message;

                results.Parent = meeting.Parent;

                results.Time = meeting.Time;

                results.Urgency = meeting.Urgency;

                results.Venue = meeting.Venue;

                return results;
            }

            return null;
        }
        public void OnRemoveItemAsync(Guid MeetingId)
        {
            var item = _dbContext.Meeting.FirstOrDefaultAsync(m => m.MeetingId == MeetingId);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
