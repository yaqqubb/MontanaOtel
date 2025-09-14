using Otel.Business.Services.Abstract;
using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;
using Otel.DAL.Repositories.Abstract;

namespace Otel.Business.Services.Concrete
{
    public class AppointmentService : GenericService<Appointment>, IAppointmentService
    {
        public AppointmentService(IGenericRepository<Appointment> appointmentRepository, AppDbContext dbContext)
            : base(appointmentRepository, dbContext)
        {
        }
    }
}
