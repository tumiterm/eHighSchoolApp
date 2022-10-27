using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class AttendanceRepository : IUnitOfWork<Attendance>
    {
        private readonly ApplicationDbContext _dbContext;
        public AttendanceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Attendance> OnItemCreationAsync(Attendance attendance)
        {
            await _dbContext.AddAsync(attendance);

            return attendance;
        }

        public async Task<Attendance> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Attendances.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<List<Attendance>> OnLoadItemsAsync()
        {
            var attendance = await _dbContext.Attendances.ToListAsync();

            var getAttendanceList = from n in attendance

                                    where n.IsActive == true

                                    select n;

            return getAttendanceList.ToList();
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Attendance> OnModifyItemAsync(Attendance attendance)
        {
            var results = await _dbContext.Attendances.FirstOrDefaultAsync(x => x.Id == attendance.Id);

            if (results != null)
            {
                results.Comment = attendance.Comment;

                results.IsActive = attendance.IsActive;

                results.Cycle = attendance.Cycle;

                results.From = attendance.From;

                results.Till = attendance.Till;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Attendances.FirstOrDefaultAsync(m => m.Id == Id);

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
