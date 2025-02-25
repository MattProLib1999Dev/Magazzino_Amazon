using Amazon;
using Amazon.Models;
using Amazon.Prodotto;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Prodotto> ListaDeiProdotti { get; set; }
    public DbSet<Titolare> ListaDeiTitolari { get; set; }
    public DbSet<UserModelResponse> ListaDegliUtenti { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prodotto>().HasKey(p => p.Id);
        modelBuilder.Entity<Titolare>().HasKey(p => p.Id);
        modelBuilder.Entity<UserModelResponse>().HasKey(p => p.IdUser);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Stringa di connessione al database SQL Server con nome utente e password
        optionsBuilder.UseSqlServer("Server=FGRML012205\\SQLEXPRESS;Database=Amazon;User Id=sa;Password=1234;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;encrypt=false;Integrated Security=True;");


        // Crea il contesto del database per interagire con il database
        using (var context = new ApplicationDbContext())
        {
            // Usa il context per interagire con il database
        }
    }


}