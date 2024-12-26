using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Programlama__Proje.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class RestApiController : ControllerBase
    {
        RendevuContext _context = new RendevuContext();
        // GET: api/<RestApiController>
        [HttpGet]
        public List<Rendevu> Get()//yazdırma select
        {
            var rendevular = _context.Rendevular.ToList();
            return rendevular;
        }

        // GET api/<RestApiController>/5
        [HttpGet("{id}")]
        public ActionResult<Rendevu> Get(int id)
        {
            var rendevu= _context.Rendevular.FirstOrDefault(x=>x.MusteriiID==id);
            if (rendevu == null) 
            {
                return NoContent();
            }
            return rendevu;
        }

        // POST api/<RestApiController>
        [HttpPost]
        public ActionResult Post([FromBody] Rendevu rendevu)//Ekleme
        {
            _context.Rendevular.Add(rendevu);
            _context.SaveChanges();
            return Ok(rendevu.MusteriAd+": Rendevusu Başarıyla alındı");
        }

        // PUT api/<RestApiController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Rendevu ren)//Güncelleme
        {
            var  rendevu=_context.Rendevular.FirstOrDefault(x=>x.MusteriiID == id);
            if(rendevu == null) 
            {
                return NotFound();
            }
            rendevu.MusteriAd = ren.MusteriAd;
            rendevu.MusteriSoyAd = ren.MusteriSoyAd;
            _context.Update(rendevu);
            _context.SaveChanges();
            return Ok(rendevu.MusteriAd + ": Rendevusu Başarıyla Güncellendi");

        }

        // DELETE api/<RestApiController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)//silme
        {
            var rendevu = _context.Rendevular.FirstOrDefault(x => x.MusteriiID == id);
            if (rendevu == null)
            {
                return NotFound();

            }
            
            _context.Rendevular.Remove(rendevu);
            _context.SaveChanges();
            return Ok(rendevu.MusteriAd + ":  Rendevusu Başarıyla silindi");
        }
    }
}
