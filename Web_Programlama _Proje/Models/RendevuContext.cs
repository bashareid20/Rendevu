using Microsoft.EntityFrameworkCore;


namespace Web_Programlama__Proje.Models
{
    public class RendevuContext : DbContext
    {
        public DbSet<Rendevu> Rendevular { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-L1KB2B2\SQLEXPRESS; Database=Web_Programlama_Proje;Trusted_Connection=True;");
        }
    }


}
