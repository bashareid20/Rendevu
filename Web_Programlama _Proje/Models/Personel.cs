using System.ComponentModel.DataAnnotations;

namespace Web_Programlama__Proje.Models
{
    public class Personel
    {
        [Key]
        [Display(Name = "Personel ID")]
        public int PersonelID { get; set; }
        [Required(ErrorMessage = "Lütefen Personel Adını Giriniz")]
        [Display(Name = "Personel Adı")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ten küçük 50 büyük olamaz")]
        public string PersonelAd { get; set; }
        [Required(ErrorMessage = "Lütefen Personel Soy Adını Giriniz")]
        [Display(Name = "Personel Soy Adı")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ten küçük 50 büyük olamaz")]
        public string PersonelSoyAd { get; set; }
        [Required(ErrorMessage = "Lütefen Personel yetenekleri Giriniz")]
        [Display(Name = "Personel Yetenekleri")]
        public string PersonelYetenekleri { get; set; }
    }
}
