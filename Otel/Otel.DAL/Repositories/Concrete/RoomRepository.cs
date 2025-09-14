using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.DAL.Repositories.Concrete
{
    public class RoomRepository:GenericRepository<Room>,IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }
    }
}
