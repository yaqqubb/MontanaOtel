using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.DAL.DataContext.Entities
{
    public class Appointment : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!; 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!; 

        [Required]
        public int RoomTypeId { get; set; } 
        public RoomType? RoomType { get; set; } 

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; } 
        public bool? Paid { get; set; }
    }
}
