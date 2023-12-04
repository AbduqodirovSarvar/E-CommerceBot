using Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Interfaces.DbInterfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Filial> Filials { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
