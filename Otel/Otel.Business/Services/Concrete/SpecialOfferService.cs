using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.Business.Services.Concrete
{
    public class SpecialOfferService : GenericService<SpecialOffer>, ISpecialOfferService
    {
        public SpecialOfferService(IGenericRepository<SpecialOffer> specialOfferRepository, AppDbContext dbContext)
            : base(specialOfferRepository, dbContext)
        {
        }
    }
}
