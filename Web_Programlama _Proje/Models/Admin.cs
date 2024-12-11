using System.ComponentModel.DataAnnotations;

namespace Web_Programlama__Proje.Models
{
    public class Admin
    {
        [Key]
        [Display(Name ="Admin ID")]
        public int AdminID { get; set; }
        [Required]
        public string AdminEposta { get; set; }
        [Required]
        public string AdminSifre { get; set; }
        [Required]
        public string AdminYetki { get; set; }

    }
}
