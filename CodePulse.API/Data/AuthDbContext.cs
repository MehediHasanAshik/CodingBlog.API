using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleid = "061ab9e3-5c98-45fd-a0b0-bd24fd3dd73b";
            var writerRoleid = "a6deb4ae-a5b8-4463-b110-813ef81ae7c5";

            var roles = new List<IdentityRole>
            {
                //Creeate reader and writer role
                new IdentityRole()
                {
                    Id = readerRoleid,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleid
                },

                new IdentityRole()
                {
                    Id = writerRoleid,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleid
                }
            };

            //Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create admin User
            var adminUserId = "c91c1fd2-a009-48fa-9336-b6f225c21a3c";

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedUserName = "admin@codepulse.com".ToUpper(),
                NormalizedEmail = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@1234");

            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles to Admin 
            var adminRole = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleid
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleid
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRole);
        }
    }
}
