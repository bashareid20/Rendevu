using Microsoft.AspNetCore.Mvc;

namespace Web_Programlama__Proje.Controllers
{
    public class SorguController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public string Index2()
        {
            string txt = "";
            int[] sayilar = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            var SayiQuery =
                from sayi in sayilar
                where (sayi % 2) == 0
                select sayi;
            sayilar[1] = 8;
            foreach (int deger in SayiQuery)
            {
                txt = txt + " - " + deger;
            }
            return (txt);

        }

        public string Index3()
        {
            string txt = "";
            int[] sayilar = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            var SayiQuery =
                (from sayi in sayilar
                 where (sayi % 2) == 0
                 select sayi).ToList();
            sayilar[1] = 8;
            foreach (int deger in SayiQuery)
            {
                txt = txt + " - " + deger;
            }
            return (txt);

        }
    }
}
