using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programlama__Proje.Models
{
    public class Rendevu
    {
        [Key]
        [Display(Name = "Müşteri ID")]
        public int MusteriiID { get; set; }

        [Display(Name = "Müşteri Adı")]
        [Required(ErrorMessage = "Lütfen adınızı giriniz")]
        [StringLength(50)]
        public string MusteriAd { get; set; }

        [Display(Name = "Müşteri Soy Adı")]
        [Required(ErrorMessage = "Lütfen soy adınızı giriniz")]
        [StringLength(50)]
        public string MusteriSoyAd { get; set; }

        [Display(Name = "Telefon Numarası")]
        [RegularExpression(@"^0\d{3}\d{3}\d{2}\d{2}$", ErrorMessage = "Lütfen '05448980922' formatında bir telefon numarası giriniz.")]
        public string MusteriTelefonNo { get; set; }

        [Display(Name = "Mail Hesapı")]
        [Required(ErrorMessage = "Lütefen Mail Adressiniz giriniz")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli Mail Hesapı giriniz ")]
        public string MusteriMail { get; set; }

        [Required(ErrorMessage = "Rendevu zamanı boş bırakılmaz")]
        [Display(Name = "Rendevu Tarihi")]
        public DateTime RendevuZaman { get; set; }

        [Display(Name = "Rendevu Onay Durumu")]
        public Boolean RendevuOnayDurumu { get; set; }

        // Foreign Key
        [ForeignKey("Personel")]
        public int? PersonelID { get; set; }

        public Personel? Personel { get; set; }
    }
}
