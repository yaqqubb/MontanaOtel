namespace Otel.DAL.DataContext.Entities
{
    public class SpecialOffer:BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal TotalAmount { get; set; }
        public required DateTime ValidityDate { get; set; }
        public  string? ImagePath { get; set; }
    }
}
