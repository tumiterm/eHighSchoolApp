using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class ParentMessageRepository : IUnitOfWork<ParentMessage>
    {
        private readonly ApplicationDbContext _dbContext;
        public ParentMessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            return data.Any(predicate);
        }
        public async Task<ParentMessage> OnItemCreationAsync(ParentMessage message)
        {
            await _dbContext.AddAsync(message);

            return message;
        }
        public async Task<ParentMessage> OnLoadItemAsync(Guid MessageId)
        {
            return await _dbContext.ParentMessages.FirstOrDefaultAsync(m => m.MessageId == MessageId);
        }
        public async Task<List<ParentMessage>> OnLoadItemsAsync()
        {
            var messages = await _dbContext.ParentMessages.ToListAsync();

            var getActiveAttachments = from n in messages

                                 where n.IsActive == true

                                 select n;

            return getActiveAttachments.ToList();
        }
        public async Task<ParentMessage> OnModifyItemAsync(ParentMessage message)
        {
            var results = await _dbContext.ParentMessages.FirstOrDefaultAsync(x => x.MessageId == message.MessageId);

            if (results != null)
            {
                results.AttachmentType = message.AttachmentType;
                results.IsActive = message.IsActive;
                results.Learner = message.Learner;
                results.Message = message.Message;
                results.MessageType = message.MessageType;

                return results;
            }

            return null;
        }
        public void OnRemoveItemAsync(Guid MessageId)
        {
            var item = _dbContext.ParentMessages.FirstOrDefaultAsync(m => m.MessageId == MessageId);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
