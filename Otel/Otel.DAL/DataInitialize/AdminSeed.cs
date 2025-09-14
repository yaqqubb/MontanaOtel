using Otel.DAL.DataContext.Entities;
using Otel.DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otel.DAL.DataInitialize
{
    public static class AdminSeed
    {
        public static void SeedAdmins(AppDbContext context)
        {
            if (!context.AdminUsers.Any(u => u.Role == AdminRole.SuperAdmin))
            {
                var superAdmin = new AdminUser
                {
                    UserName = "superadmin",
                    Password = "Super123!", 
                    Role = AdminRole.SuperAdmin
                };
                context.AdminUsers.Add(superAdmin);
            }
            if (!context.AdminUsers.Any(u => u.Role == AdminRole.Admin))
            {
                var admin = new AdminUser
                {
                    UserName = "admin1",
                    Password = "Admin123!", 
                    Role = AdminRole.Admin
                };
                context.AdminUsers.Add(admin);
            }

            context.SaveChanges();
        }
    }
}
