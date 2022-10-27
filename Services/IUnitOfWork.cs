using System.Linq.Expressions;

namespace SchoolApp.Services
{
    public interface IUnitOfWork<T> where T: class
    {
        Task<List<T>> OnLoadItemsAsync();
        Task<T> OnLoadItemAsync(Guid Id);
        Task<T> OnItemCreationAsync(T t);
        Task<T> OnModifyItemAsync(T t);
        void OnRemoveItemAsync(Guid Id);
        Task<int> ItemSaveAsync();
        bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class;

    }
}
