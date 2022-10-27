using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class MessageRepository : IUnitOfWork<Message>
    {
        private readonly ApplicationDbContext _dbContext;
        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Message> OnItemCreationAsync(Message message)
        {
            await _dbContext.AddAsync(message);

            return message;
        }

        public async Task<Message> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == Id);

        }

        public async Task<List<Message>> OnLoadItemsAsync()
        {
            var messages = await _dbContext.Messages.ToListAsync();

            var getMessages = from n in messages

                                     where n.IsActive == true

                                     select n;

            return getMessages.ToList();
        }

        public async Task<Message> OnModifyItemAsync(Message message)
        {
            Message results = await _dbContext.Messages.FirstOrDefaultAsync(x => x.MessageId == message.MessageId);

            if (results != null)
            {
                results.IsActive = message.IsActive;

                results.IsAllLearners = message.IsAllLearners;

                results.Learners = message.Learners;

                results.MessageId = message.MessageId;

                results.MessageInfo = message.MessageInfo;

                return results;

            }

            return null;

        }

        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == Id);

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
