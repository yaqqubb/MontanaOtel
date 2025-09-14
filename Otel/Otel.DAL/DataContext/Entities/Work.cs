using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.DAL.DataContext.Entities
{
    public class Work:BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
