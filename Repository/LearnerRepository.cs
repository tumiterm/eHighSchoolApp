using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class LearnerRepository : IUnitOfWork<Learner>
    {
        private readonly ApplicationDbContext _dbContext;

        public LearnerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Learner> OnItemCreationAsync(Learner learner)
        {
            await _dbContext.AddAsync(learner);

            return learner;
        }

        public async Task<Learner> OnLoadItemAsync(Guid LearnerId)
        {
            return await _dbContext.Learners.FirstOrDefaultAsync(m => m.LearnerId == LearnerId);
        }

        public async Task<List<Learner>> OnLoadItemsAsync()
        {
            var learners = await _dbContext.Learners.ToListAsync();

            var getActiveLearners = from n in learners

                                    where n.IsActive == true

                                    select n;

            return getActiveLearners.ToList();
            
        }

        public async Task<Learner> OnModifyItemAsync(Learner learner)
        {
            var results = await _dbContext.Learners.FirstOrDefaultAsync(x => x.LearnerId == learner.LearnerId);

            if (results != null)
            {
                results.LearnerReference = learner.LearnerReference;

                results.AddressLine1 = learner.AddressLine1;

                results.AddressLine2 = learner.AddressLine2;

                results.Province = learner.Province;

                results.AddressType = learner.AddressType;

                results.Cellphone = learner.Cellphone;

                results.DateOfBirth = learner.DateOfBirth;

                results.Disability = learner.Disability;

                results.Email = learner.Email;

                results.Grade = learner.Grade;

                results.HasDisability = results.HasDisability;

                results.RSAIDNumber = learner.RSAIDNumber;

                results.IntakeCycle = learner.IntakeCycle;

                results.IsActive = learner.IsActive;

                results.IsSaCitizen = learner.IsSaCitizen;

                results.LastName = learner.LastName;

                results.LearnerReference = learner.LearnerReference;

                results.Nationality = learner.Nationality;

                results.PassportNumber = learner.PassportNumber;

                results.Postal = learner.Postal;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid LearnerId)
        {
            var item = _dbContext.Learners.FirstOrDefaultAsync(m => m.LearnerId == LearnerId);

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



