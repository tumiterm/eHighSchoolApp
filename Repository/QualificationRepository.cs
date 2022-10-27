using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class QualificationRepository : IUnitOfWork<Qualification>
    {
        private readonly ApplicationDbContext _dbContext;
        public QualificationRepository(ApplicationDbContext dbContext)
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

        public async Task<Qualification> OnItemCreationAsync(Qualification qualification)
        {
             await _dbContext.AddAsync(qualification);

             return qualification;
        }

        public async Task<Qualification> OnLoadItemAsync(Guid QualificationId)
        {
            return await _dbContext.Qualification.FirstOrDefaultAsync(m => m.QualificationId == QualificationId);
        }

        public async Task<List<Qualification>> OnLoadItemsAsync()
        {
            var qualifications = await _dbContext.Qualification.ToListAsync();

            var getQualifications = from n in qualifications

                                     where n.IsActive == true

                                     select n;

            return getQualifications.ToList();
        }

        public async Task<Qualification> OnModifyItemAsync(Qualification qualification)
        {
            var results = await _dbContext.Qualification.FirstOrDefaultAsync(x => x.QualificationId == qualification.QualificationId);
            
            if (results != null)
            {
                results.From = qualification.From;

                results.Till = qualification.Till;

                results.Institution = qualification.Institution;

                results.IsActive = qualification.IsActive;

                results.QualificationName = qualification.QualificationName;

                results.QualificationType = qualification.QualificationType;

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Qualification.FirstOrDefaultAsync(m => m.QualificationId == Id);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
