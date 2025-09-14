using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.Business.Services.Concrete
{
    public class WorkService : GenericService<Work>, IWorkService
    {
        public WorkService(IGenericRepository<Work> workRepository, AppDbContext dbContext)
            : base(workRepository, dbContext)
        {
        }
    }
}
