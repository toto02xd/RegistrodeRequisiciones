using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RegistrodeRequisiciones.Models.Entities;

namespace RegistrodeRequisiciones.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LSGT-IT\\FARMANDINA; Database=Farmandina; User Id=sa; Password=Guatemala1.; Trust Server Certificate=true");
            }
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("TblUsers");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.UserName)
                .HasMaxLength(255);
                entity.Property(x => x.Password)
                .HasMaxLength(255);
                entity.Property(x => x.Email)
                .HasMaxLength(255);
            });

            modelBuilder.Entity<Loan>(entity => {
                entity.ToTable("TblLoans");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Article)
                .HasMaxLength(255);
                entity.Property(x => x.Observations)
                .HasMaxLength(255);

                entity.HasOne(x => x.Applicant)
                .WithMany(x => x.ApplicantLoans)
                .HasForeignKey(x => x.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Responsible)
                .WithMany(x => x.ResponsibleLoans)
                .HasForeignKey(x => x.ResponsibleId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
