namespace Web_Programlama__Proje.Models
{


    public class RendevuHizmet
    {
        public int RendevuID { get; set; }
        public Rendevu Rendevu { get; set; }

        public int HizmetID { get; set; }
        public Hizmetler Hizmetler { get; set; }
    }

}
