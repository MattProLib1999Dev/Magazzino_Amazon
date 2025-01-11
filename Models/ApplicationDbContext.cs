using Amazon;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ProdottoDto> ListaDeiProdotti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    modelBuilder.Entity<ProdottoDto>().HasNoKey(); // Dichiarazione per entità senza chiave primaria
    }

}
