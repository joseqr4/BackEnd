using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InterestsUsers>().HasKey(x => new { x.InterestsId, x.IdUser });
            builder.Entity<CommerceDiscounts>().HasKey(x => new { x.CommerceID, x.DiscountsID });
            builder.Entity<DiscountsInterests>().HasKey(x => new { x.DiscountsID, x.InterestsId });
            builder.Entity<BussinessCommerce>().HasKey(x => new { x.Commerce, x.Bussines });
            builder.Entity<UserCommerce>().HasKey(x => new { x.CommerceID, x.IdUser });


            base.OnModelCreating(builder);

        }


        public DbSet<Commerce> Commerce { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<Interests> Interests { get; set; }

        public DbSet<InterestsUsers> InterestsUsers { get; set; }

        public DbSet<Discounts> Discounts { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<CommerceDiscounts> CommerceDiscounts { get; set; }

        public DbSet<DiscountsInterests> DiscountsInterests { get; set; }

        public DbSet<QrCode> QrCode { get; set; }

        public DbSet<UserDiscountConsumed> UserDiscountConsumed { get; set; }

        public DbSet<Review> Review { get; set; }

        public DbSet<BussinessCommerce> BussinessCommerce { get; set; }

        public DbSet<BusinessModel> BusinessModels { get; set; }

        public DbSet<UsersCompany> UsersCompany { get; set; }

        public DbSet<CompanyModelBussines> CompanyModelBussines { get; set; }

        public DbSet<UserCommerce> UserCommerce { get; set; }

    }
}

