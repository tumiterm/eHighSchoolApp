using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class AddressRepository : IUnitOfWork<Address>
    {
        private readonly ApplicationDbContext _dbContext;
        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Address> OnItemCreationAsync(Address address)
        {
            await _dbContext.AddAsync(address);

            return address;
        }

        public async Task<Address> OnLoadItemAsync(Guid AddressId)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(m => m.AddressId == AddressId);
        }

        public async Task<List<Address>> OnLoadItemsAsync()
        {
            var addresses = await _dbContext.Addresses.ToListAsync();

            var getAddress = from n in addresses

                             where n.IsActive == true

                             select n;

            return getAddress.ToList();

        }

        public async Task<Address> OnModifyItemAsync(Address address)
        {
            var results = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.AddressId == address.AddressId);
           
            if (results != null)
            {
                results.AddressLine1 = address.AddressLine1;

                results.AddressLine2 = address.AddressLine2;

                results.PostalCode2 = address.PostalCode2;

                results.IsActive = address.IsActive;

                results.IsSame = address.IsSame;

                results.PostalAddressLine = address.PostalAddressLine;

                results.PostalAddressLine2 = address.PostalAddressLine2;

                results.PostalCode = address.PostalCode;

                results.PostalProvince = address.PostalProvince;

                results.Province = address.Province;

                return results;
            }

            return null;

        }

        public void OnRemoveItemAsync(Guid AddressId)
        {
            var item = _dbContext.Addresses.FirstOrDefaultAsync(m => m.AddressId == AddressId);

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
