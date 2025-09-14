using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.DAL.Repositories.Concrete
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
      public WorkRepository(AppDbContext context) : base(context) { }
    }
}
