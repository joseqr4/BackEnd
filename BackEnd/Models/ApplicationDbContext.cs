using BackEnd.Models.Intereses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }
        public DbSet<Comercios> Comercios { get; set; }

        public DbSet<intereses> Intereses { get; set; }

        public DbSet<InteresesUsuarios> InteresesUsuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InteresesUsuarios>()
                    .HasKey(o => new { o.Usuario.Id, o.InteresesFK });

        }
    }
}

