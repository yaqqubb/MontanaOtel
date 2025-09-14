using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.DAL.DataContext.Entities
{
    public class Employee:BaseEntity
    {
        public required string Name { get; set; }
        public int WorkId { get; set; }
        [ForeignKey("WorkId")]
        public Work? Work { get; set; }
        public  string? ImagePath { get; set; }
    }
}
