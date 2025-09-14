using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.Business.Services.Concrete
{
    public class CustomerService : GenericService<Customer>, ICustomerService
    {
        public CustomerService(IGenericRepository<Customer> customerRepository, AppDbContext dbContext)
            : base(customerRepository, dbContext)
        {
        }
    }
}
