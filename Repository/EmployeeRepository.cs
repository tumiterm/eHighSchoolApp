using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Services;
using System.Linq.Expressions;

namespace SchoolApp.Repository
{
    public class EmployeeRepository : IUnitOfWork<Employee>
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Employee> OnItemCreationAsync(Employee employee)
        {
            await _dbContext.AddAsync(employee);

            return employee;
        }

        public async Task<Employee> OnLoadItemAsync(Guid EmployeeId)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(m => m.EmployeeId == EmployeeId);
        }

        public async Task<List<Employee>> OnLoadItemsAsync()
        {
            var employees = await _dbContext.Employees.ToListAsync();

            var getActiveEmployees = from n in employees

                                    where n.IsActive == true

                                    select n;

            return getActiveEmployees.ToList();
        }

        public async Task<Employee> OnModifyItemAsync(Employee employee)
        {
            var results = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId);

            if (results != null)
            {
                results.AlternativeCell = employee.AlternativeCell;

                results.Cellphone = employee.Cellphone;

                results.Disability = employee.Disability;

                results.Email = employee.Email;

                results.EmployeeStatus = employee.EmployeeStatus;

                results.EmployeeType = employee.EmployeeType;

                results.HomeLanguage = employee.HomeLanguage;

                results.IsActive = employee.IsActive;

                results.LastName = employee.LastName;

                results.Nationality = employee.Nationality;

                results.Race = employee.Race;

                results.RSAIdNumber = employee.RSAIdNumber;

                results.PassportNum = employee.PassportNum;

                results.Title = employee.Title;

                await _dbContext.SaveChangesAsync();

                return results;
            }
            return null;
        }

        public void OnRemoveItemAsync(Guid EmployeeId)
        {
            var item = _dbContext.Employees.FirstOrDefaultAsync(m => m.EmployeeId == EmployeeId);

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
