using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.DAL.Repositories.Concrete
{
    public class SpecialOfferRepository:GenericRepository<SpecialOffer>, ISpecialOfferRepository
    {
        public SpecialOfferRepository(AppDbContext context) : base(context) { }
    }


}
