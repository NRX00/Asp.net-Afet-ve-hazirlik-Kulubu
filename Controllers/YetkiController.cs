using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace YesilayKlubu.Controllers
{
  [Authorize(Roles ="Yetkiler")]
    public class YetkiController : Controller
    {
          private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public YetkiController(
                                UserManager<AppUser> _userManager,
                                RoleManager<IdentityRole> _roleManager
                                )
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            var yetkiler = roleManager.Roles.ToList();
            return View(yetkiler);
        }
          public IActionResult Yeni()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Yeni(string Ad)
        {
            // yetki adını kontrol et
            if (string.IsNullOrEmpty(Ad))
            {
                ViewBag.Hata = "Yetki adı boş bırakılamaz";
                return View();
            }
            //veritabanına yetkiyi ekle
            var sonuc = await roleManager.CreateAsync(new IdentityRole() { Name = Ad });
            if (sonuc.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Hata = sonuc.Errors.First().Description;
            }
            return View();
        }
          public async Task<IActionResult> YetkiliKullanicilar(string id)
        {
             var roleAdi = await roleManager.FindByIdAsync(id);            
            YetkiliKullanicilarModel m= new YetkiliKullanicilarModel();
            m.ButunKullanicilar=userManager.Users.ToList();
            m.RoleAd=roleAdi.Name;
            m.YetkiliKullanicilar=await userManager.GetUsersInRoleAsync(roleAdi.Name);
            return View(m);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YetkiliKullanicilar(List<string> kullaniciId,
                                                                     string roleAd)
        {
            //bütün kullanıcıları rolden çıkar, aşağıda yenilerinmi ekleyeceğiz
            foreach (var item in userManager.Users.ToList())
            {
                await userManager.RemoveFromRoleAsync(item, roleAd);
            }
            //seçilenleri ekle
            foreach (var item in kullaniciId)
            {
                //önce id e göre kullanıcıyı bul
                var user = await userManager.FindByIdAsync(item);
                //kullancıya rolü ekle
                await userManager.AddToRoleAsync(user, roleAd);
            }

            return RedirectToAction(nameof(Index));
        }
         [HttpGet]
        public async Task<IActionResult> Duzenle(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null) return NotFound();
            return View(role);
        }
      [HttpPost]
        public async Task<IActionResult> Duzenle(IdentityRole m)
        {
            if (ModelState.IsValid)
            {
                var sonuc = await roleManager.UpdateAsync(m);
                if (!sonuc.Succeeded)
                {
                    ModelState.AddModelError("Name",
                    string.Join("<br>",
                    sonuc.Errors.Select(x => x.Description).ToList()));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
    }
}