using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class AttachmentRepository : IUnitOfWork<Attachment>
    {
        private readonly ApplicationDbContext _dbContext;
        public AttachmentRepository(ApplicationDbContext dbContext)
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
        public async Task<Attachment> OnItemCreationAsync(Attachment attachment)
        {
            await _dbContext.AddAsync(attachment);

            return attachment;
        }
        public async Task<Attachment> OnLoadItemAsync(Guid Id)
        {
            return await _dbContext.Attachments.FirstOrDefaultAsync(m => m.Id == Id);
        }
        public async Task<List<Attachment>> OnLoadItemsAsync()
        {
            var attachments = await _dbContext.Attachments.ToListAsync();

            var getAttachments = from n in attachments

                             where n.IsActive == true

                             select n;

            return getAttachments.ToList();
        }
        public async Task<Attachment> OnModifyItemAsync(Attachment attachment)
        {
            var results = await _dbContext.Attachments.FirstOrDefaultAsync(x => x.Id == attachment.Id);

            if (results != null)
            {
                results.AttachmentType = attachment.AttachmentType;

                results.UserAttachment = attachment.UserAttachment;

                results.IsActive = attachment.IsActive;

                return results;

            }

            return null;
        }
        public void OnRemoveItemAsync(Guid Id)
        {
            var item = _dbContext.Attachments.FirstOrDefaultAsync(m => m.Id == Id);

            if (item != null)
            {
                _dbContext.Remove(item);

                _dbContext.SaveChangesAsync();
            }
        }
    }
}
