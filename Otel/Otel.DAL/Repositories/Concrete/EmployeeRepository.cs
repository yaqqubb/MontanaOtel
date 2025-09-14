using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.DAL.Repositories.Concrete
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context) { }
    }


}
