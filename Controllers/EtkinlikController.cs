using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace projeApp.Controllers
{
   [Authorize(Roles = "Etkinlikler")]
    public class EtkinlikController : Controller
    {
        //yerel değişken olarak veritabanı işlemleri yapan nesne tanımlanıyor
        private ApplicationDbContext contex;
       
        public EtkinlikController(ApplicationDbContext _context)
        {//Dependecy Injection
            //Veritabanı işlemleri için ilgili sınıfın enjekte diyoruz
            contex = _context;
        }

        public IActionResult Index()
        {
            return View(contex.Etkinlikler.ToList());
        }
         public IActionResult Ekle()
        {
            Etkinlik m = new Etkinlik();
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Ekle(Etkinlik m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contex.Etkinlikler.Add(m);
                    await contex.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (System.Exception istisna)
            {
                string mesaj = (istisna.InnerException == null)
                ? istisna.Message : istisna.InnerException.Message;
                ModelState.AddModelError("", mesaj);
            }
            return View(m);
        }
            [HttpGet]
        public async Task<IActionResult> Duzenle(int Id)
        {
            var etkinlik = await contex.Etkinlikler.FindAsync(Id);
            if (etkinlik == null) return NotFound();
            return View(etkinlik);
        }
        [HttpPost]
        public async Task<IActionResult> Duzenle(Etkinlik m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contex.Update(m);
                    await contex.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (System.Exception istisna)
            {
                string mesaj = (istisna.InnerException == null)
                ? istisna.Message : istisna.InnerException.Message;
                ModelState.AddModelError("", mesaj);
            }
            return View();
        }
         public async Task<IActionResult> Sil(int id)
        {
            var etkinlik = await contex.Etkinlikler.FindAsync(id);
            if (etkinlik != null)
            {
                contex.Remove(etkinlik);
                await contex.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    } 
}