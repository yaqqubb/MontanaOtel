using Otel.DAL.DataContext;
using Otel.DAL.DataContext.Entities;

namespace Otel.DAL.DataInitialize
{
    public static class DataInitializer
    {
        public static void SeedData(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Works.Any() || context.Employees.Any())
                return;

            var works = new List<Work>
        {
            new Work { Name = "Software Development", Description = "Develops software solutions" },
            new Work { Name = "HR Management", Description = "Handles employee relations and HR tasks" },
            new Work { Name = "Marketing", Description = "Promotes the company's products" }
        };

            context.Works.AddRange(works);
            context.SaveChanges();

            var employees = new List<Employee>
        {
            new Employee { Name = "Alice Johnson", WorkId = works[0].Id, ImagePath = "images/employees/alice.jpg" },
            new Employee { Name = "Bob Smith", WorkId = works[0].Id, ImagePath = "images/employees/bob.jpg" },
            new Employee { Name = "Carol Williams", WorkId = works[1].Id, ImagePath = "images/employees/carol.jpg" },
            new Employee { Name = "David Brown", WorkId = works[2].Id, ImagePath = "images/employees/david.jpg" }
        };

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
    }

}
