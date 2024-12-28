using System.ComponentModel.DataAnnotations;

namespace Web_Programlama__Proje.Models
{
    public class Hizmetler
    {
        [Key]
        [Required]
        public int HizmetID { get; set; }
        [Required]
        [Display(Name ="Hizmet Ücreti")]
        public double HizmetUcreti { get; set; }
        [Required(ErrorMessage = "Hizmet adı zorunludur")]
        [StringLength(100)]
        public string HizmetAd { get; set; }
        [Required]
        [Display(Name = "Hizmet Süresi")]
        public int HizmetSuresi { get; set; }
        [Display(Name = "Hizmet Resimi")]
        public string? HizmetResim { get; set; }
        public ICollection<RendevuHizmet>? RendevuHizmetler { get; set; }
        public ICollection<PersonelHizmet>? PersonelHizmetler { get; set; }
    }
}
