
using Microsoft.EntityFrameworkCore;
using ApiWebApp.Model;
using WebApp.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class WebAppContext : IdentityDbContext<IdentityUser>
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        //public DbSet<AppUsers> AppUsers { get; set; } = default!;
        //public DbSet<AppRole> AppRoles { get; set; } = default!;
    }
