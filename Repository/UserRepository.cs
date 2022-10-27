using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class UserRepository : IUnitOfWork<User>
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
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

        public async Task<User> OnItemCreationAsync(User user)
        {
            await _dbContext.AddAsync(user);

            return user;
        }

        public async Task<User> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<List<User>> OnLoadItemsAsync()
        {
            var users = await _dbContext.Users.ToListAsync();

            var getActiveUsers = from n in users

                                    where n.IsActive == true

                                    select n;

            return getActiveUsers.ToList();
        }

        public async Task<User> OnModifyItemAsync(User user)
        {
            var results = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (results != null)
            {
                results.LastName = user.LastName;

                results.Name = user.Name;

                results.Password = user.Password;

                results.ConfirmPassword = user.ConfirmPassword;

                results.Username = user.Username;

                results.RoleManager = user.RoleManager;

                await _dbContext.SaveChangesAsync();

                return results;

            }

            return null;
        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var user = _dbContext.Users.FirstOrDefaultAsync(m => m.Id == Id);

            if (user != null)
            {
                _dbContext.Remove(user);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
