using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sitem.Models;

namespace sitem.Controllers;



public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
      
        ApplicationDbContext context
        )
    {

        _context = context;
    }

    public IActionResult Index()
    {
        var m = new AnasayfaModel();
             m.EtkinlikListesi = _context
            .Etkinlikler
            .Take(6)
            .ToList();

          
            m.DuyuruListesi = _context
                .Duyurular
                .Where(x => x.YayinBaslamaTarihi <= DateTime.Now)
                .Take(6)
                .ToList();

            return View(m);
    }
    
    
    
    public async Task<IActionResult> DuyuruDetay(int? id)
    {
        //Id alanı gelmez ise
        if (!id.HasValue)
        {
            return View("Views\\Shared\\_hata.cshtml", "Id alanı gereklidir!");
        }

        var duyuru = await _context.Duyurular.FindAsync(id.Value);
        //verilen id'e göre duyuru yok ise
        if (duyuru == null)
        {
            return View("Views\\Shared\\_hata.cshtml", "İlgili  duyuru bulunamadı!");
        }
        //buraya geldiyse duyuru bulundu ve duyuru view e gönderiliyor
        return View(duyuru);
    }
    
    public async Task<IActionResult> EtkinlikDetay(int? id)
    {
        //Id alanı gelmez ise
        if (!id.HasValue)
        {
            return View("Views\\Shared\\_hata.cshtml", "Id alanı gereklidir!");
        }

        var Etkinlik = await _context.Etkinlikler.FindAsync(id.Value);
        //verilen id'e göre duyuru yok ise
        if (Etkinlik == null)
        {
            return View("Views\\Shared\\_hata.cshtml", "İlgili  duyuru bulunamadı!");
        }
        //buraya geldiyse duyuru bulundu ve duyuru view e gönderiliyor
        return View(Etkinlik);
    }
}
