using Lab2OOP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab2OOP.models;

public class TheatreContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{

    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<Film> Films { get; set; } = default!;
    public DbSet<Hall> Halls { get; set; } = default!;
    public DbSet<Purchase> Purchases { get; set; } = default!;
    public DbSet<Ticket> Tickets { get; set; } = default!;
    //public DbSet<PurchaseTicket> PurchaseTickets { get; set; } = default!; // Include the junction table

    public TheatreContext(DbContextOptions<TheatreContext> options)
        : base(options)
    {
    }
    
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}