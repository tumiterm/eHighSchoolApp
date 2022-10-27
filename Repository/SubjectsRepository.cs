using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class SubjectsRepository : IUnitOfWork<Subject>
    {
        private readonly ApplicationDbContext _dbContext;
        public SubjectsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Subject> OnItemCreationAsync(Subject subject)
        {
            await _dbContext.AddAsync(subject);

            return subject;
        }

        public async Task<Subject> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Subjects.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<List<Subject>> OnLoadItemsAsync()
        {
            var school = await _dbContext.Subjects.ToListAsync();

            var getActiveSubjects = from n in school

                                   where n.IsActive == true

                                   select n;

            return getActiveSubjects.ToList();
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Subject> OnModifyItemAsync(Subject subject)
        {
            var results = await _dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == subject.Id);

            if (results != null)
            {
                results.Date = subject.Date;

                results.IsActive = subject.IsActive;

                results.Subj = subject.Subj;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Subjects.Where(m => m.Id == Id).FirstOrDefault();

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

