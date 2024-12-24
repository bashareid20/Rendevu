using Microsoft.EntityFrameworkCore;


namespace Web_Programlama__Proje.Models
{
    public class RendevuContext : DbContext
    {
        public DbSet<Rendevu> Rendevular { get; set; }
        public DbSet<Personel> Personaller { get; set; }
        public DbSet<Hizmetler> Hizmetler { get; set; }
        public DbSet<PersonelHizmet> PersonelHizmetler { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-L1KB2B2\SQLEXPRESS; Database=Web_Programlama_Proje;Trusted_Connection=True;");
        }

   
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Çok'a Çok İlişkiyi Tanımlayın
                modelBuilder.Entity<PersonelHizmet>()
                    .HasOne(ph => ph.Personel)
                    .WithMany(p => p.PersonelHizmetler)
                    .HasForeignKey(ph => ph.PersonelID);

                modelBuilder.Entity<PersonelHizmet>()
                    .HasOne(ph => ph.Hizmetler)
                    .WithMany(h => h.PersonelHizmetler)
                    .HasForeignKey(ph => ph.HizmetID);
            }
        }
    




}
