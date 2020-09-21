using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wallpaper.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            MyUsers = new HashSet<MyUser>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<MyUser> MyUsers { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<MyUser> MyUsers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Similar> Similars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.MyUsers)
                .WithOptional(e => e.ApplicationUser)
                .HasForeignKey(e => e.IdentityID);

            modelBuilder.Entity<Similar>()
                .HasRequired(s => s.Image2)
                .WithMany()
                .HasForeignKey(s => s.Image2Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Similar>()
                .HasRequired(s => s.Image1)
                .WithMany(i => i.SimilarImages)
                .HasForeignKey(s => s.Image1Id)
                .WillCascadeOnDelete(false);
        }
    }
}