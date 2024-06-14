using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace projeApp.Controllers
{
  [Authorize(Roles = "Duyurular")]
    public class DuyuruController : Controller
    {
        //yerel değişken olarak veritabanı işlemleri yapan nesne tanımlanıyor
        private ApplicationDbContext contex;
        public DuyuruController(ApplicationDbContext _context)
        {//Dependecy Injection
            //Veritabanı işlemleri için ilgili sınıfın enjekte diyoruz
            contex = _context;
        }

        public IActionResult Index()
        {
            return View(contex.Duyurular.ToList());
        }
          public IActionResult Ekle()
        {
            Duyuru m = new Duyuru();
            return View(m);
        }
          [HttpPost]
        public async Task<IActionResult> Ekle(Duyuru m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contex.Duyurular.Add(m);
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
            var duyuru = await contex.Duyurular.FindAsync(Id);
            if (duyuru == null) return NotFound();
            return View(duyuru);
        }
        public IActionResult Edit(){
            return View();
        }
         [HttpPost]
        public async Task<IActionResult> Duzenle(Duyuru m)
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
            var duyuru = await contex.Duyurular.FindAsync(id);
            if (duyuru != null)
            {
                contex.Remove(duyuru);
                await contex.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}