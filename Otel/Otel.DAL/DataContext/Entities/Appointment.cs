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
        public string FullName { get; set; } = null!; // Rezervasiya edən şəxsin adı

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!; // Telefon nömrəsi

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!; // Email ünvanı

        [Required]
        public int RoomTypeId { get; set; } // Seçilən otaq tipi FK
        public RoomType? RoomType { get; set; } // Navigation Property

        [Required]
        public DateTime CheckInDate { get; set; } // Giriş tarixi

        [Required]
        public DateTime CheckOutDate { get; set; } // Çıxış tarixi

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; } // Ümumi qiymət
        public bool? Paid { get; set; }
    }
}
