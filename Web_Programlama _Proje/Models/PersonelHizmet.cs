using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Programlama__Proje.Models
{
    public class PersonelHizmet
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Personel")]
        public int PersonelID { get; set; }
        public Personel Personel { get; set; }

        [ForeignKey("Hizmetler")]
        public int HizmetID { get; set; }
        public Hizmetler Hizmetler { get; set; }
    }
}
