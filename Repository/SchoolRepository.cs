using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class SchoolRepository : IUnitOfWork<School>
    {
        private readonly ApplicationDbContext _dbContext;
        public SchoolRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<School> OnItemCreationAsync(School school)
        {
            await _dbContext.AddAsync(school);

            return school;
        }

        public async Task<School> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Schools.FirstOrDefaultAsync(m => m.SchoolId == Id);
        }

        public async Task<List<School>> OnLoadItemsAsync()
        {
            var school = await _dbContext.Schools.ToListAsync();

            var getActiveSchools = from n in school

                                    where n.IsActive == true

                                    select n;

            return getActiveSchools.ToList();
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<School> OnModifyItemAsync(School school)
        {
            var results = await _dbContext.Schools.FirstOrDefaultAsync(x => x.SchoolId == school.SchoolId);

            if (results != null)
            {
                results.Email = school.Email;

                results.IsActive = school.IsActive;

                results.Logo = school.Logo;

                results.Name = school.Name;

                results.Tel = school.Tel;

                results.Type = school.Type;

                results.Website = school.Website;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Schools.FirstOrDefaultAsync(m => m.SchoolId == Id);

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
