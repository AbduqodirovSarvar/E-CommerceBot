using Bot.Application.EntityTypeConfigurations;
using Bot.Application.Interfaces.DbInterfaces;
using Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Infrastructure.Db
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Filial> Filials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeTypeConfiguration());
        }
    }
}
