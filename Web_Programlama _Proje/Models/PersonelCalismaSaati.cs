using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programlama__Proje.Models
{
    public class PersonelCalismaSaati
    {
        [Key]
        public int CalismaSaatiID { get; set; }

        [Required]
        public int PersonelID { get; set; } // Foreign Key

        [Required]
        public TimeSpan CalismaSaati { get; set; } // Çalışma Saati
        [Required]
        public DateTime Tarih { get; set; } // Çalışma Saatine Ait Tarih (Yeni ekleniyor)
        public bool IsDeleted { get; set; } // Silinmiş saatleri işaretlemek için
        [ForeignKey("PersonelID")]
        public Personel Personel { get; set; } // Navigasyon Özelliği

    }
}
