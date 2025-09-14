using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.Business.Services.Concrete
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        public EmployeeService(IGenericRepository<Employee> employeeRepository, AppDbContext dbContext)
            : base(employeeRepository, dbContext)
        {
        }
    }
}
