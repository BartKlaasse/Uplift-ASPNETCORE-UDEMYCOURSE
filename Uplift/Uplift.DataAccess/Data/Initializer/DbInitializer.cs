using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // implement exception catching here
            }
            if (_db.Roles.Any(r => r.Name == SD.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Manager)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    Name = "Admin account"
            }, "Welkom123!").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "admin@admin.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();
        }
    }
}