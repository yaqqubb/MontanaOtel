using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.Business.Services.Concrete
{
    public class RoomTypeService:GenericService<RoomType> ,IRoomTypeService
    {
        public RoomTypeService(IGenericRepository<RoomType> roomTypeRepository, AppDbContext dbContext)
            : base(roomTypeRepository, dbContext)
        {
        }
    }
}
