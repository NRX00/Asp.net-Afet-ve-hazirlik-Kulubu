using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace sitem.Controllers
{
    public class HesapController : Controller
    {
        //Oturum işlerini yapmamızı sağlar
        private SignInManager<AppUser> signInManager;
        //Kullanıcı işlemlerini yapmamızı sağlar
        private UserManager<AppUser> userManager;
        public HesapController(SignInManager<AppUser> _s, UserManager<AppUser> _u)
        {
            signInManager = _s;
            userManager = _u;
        }
        public IActionResult Kayit()
        {
            return View();
        }
          [HttpPost]
        public async Task<IActionResult> Kayit(Kayit m)
        {
            if (ModelState.IsValid)
            {
                var kullanici = new AppUser
                {
                    UserName = m.Email,
                    Ad = m.Ad,
                    Soyad = m.Soyad,
                    Email = m.Email,
                    OkulNumarasi = m.OkulNumarasi
                };
                var sonuc = await userManager.CreateAsync(kullanici, m.Sifre);
                if (sonuc.Succeeded)
                {
                    ViewBag.OK = "Üye kaydınız başarıyla gerçekleşti, Lütfen giriş yapınız";
                    string icerik = $"Sayın {m.Ad} {m.Soyad}  <br> Sitemize kayıt olduğunuz için teşekkür ederiz ";
                    icerik += $" Giriş yapmak için {this.Request.Scheme}://{this.Request.Host}/Hesap/";
                    Islemler.MailGonder("Hoşgeldin", icerik, m.Email);
                }
                else
                {
                    //kayıt hata ile sonuçlanırsa ön tarafa 
                    //göstermesi için hata ekle
                    ModelState.AddModelError("", string.Join("<br>",
                    sonuc
                    .Errors
                    .Select(x => x.Description)
                    .ToList()));
                }
            }
            return View(m);
        }
           public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(giris m)
        {
            if (ModelState.IsValid)
            {
                    var result = await signInManager.PasswordSignInAsync(m.Email, m.Sifre, true, lockoutOnFailure: true);
                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Hesap kilitli");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Giriş başarısız");
                    }

                }
            }
            return View();
        }
          public IActionResult Cikis()
        {
            signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult SifremiUnuttum()
        {
            // TODO: Your code here
            return View();
        }
         [HttpPost]
        public async Task<IActionResult> SifremiUnuttum(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ViewBag.Hata = "Email alanı boş bırakılamaz";
                return View();
            }

            AppUser kullanici = await userManager.FindByEmailAsync(Email);
            if (kullanici == null)
            {
                ViewBag.Hata = "Girilen email ile sistemde kullanıcı yok";
                return View();
            }

            //şifre sıfırlama linkini oluştur
            var token = await userManager.GeneratePasswordResetTokenAsync(kullanici);
            var link = Url.Action(nameof(SifreSifirla), "Hesap",
            new { token, email = kullanici.Email }, Request.Scheme);
            //email ile gönderiliyor
            string emailmesaj = $"Merhaba {kullanici.Ad} <br> Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayınız <br>";
            Islemler.MailGonder("Şifre sıfırlama bağlantısı", emailmesaj + link, kullanici.Email);
            string mesaj = $" <i class='bi bi-check'></i>Sıfırlama  bağlantısı <b>{Email}</b> adresine gönderildi <br>";

            string l = Url.ActionLink(nameof(Index), "Hesap", null, Request.Scheme);
            mesaj += $"<a href='{l}'>Giriş yapın</a>";
            return View("Views/Shared/_basarili.cshtml", mesaj);


        }
        public IActionResult SifreSifirla()
        {
            return View();
        }
           [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreSifirla(SifreSifirla m)
        {
            if (!ModelState.IsValid)
                return View(m);

            var kullanici = await userManager.FindByEmailAsync(m.Email);
            if (kullanici == null)
                RedirectToAction(nameof(SifremiUnuttum));

            var sifirla = await userManager.ResetPasswordAsync(kullanici, m.Token, m.Sifre);
            if (!sifirla.Succeeded)
            {
                foreach (var error in sifirla.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return View("Views/Shared/_basarili.cshtml", "Şifreniz başarıyla değiştirildi");
        }
           public IActionResult YetkiHatasi()
        {
            return View();
        }

         [Authorize]
        public async Task<IActionResult> Profil()
        {
            ProfilDuzenle m = new ProfilDuzenle();
            //giriş yapan kullanıcı adı alınıyor
            string aktifKullaniciAdi = User.Identity.Name;
            //kullanıcının bütün bilgileri db den alınıyor
            var kullanici = await userManager.FindByNameAsync(aktifKullaniciAdi);
            //model alanları dolduruluyor
            m.Ad = kullanici.Ad;
            m.Soyad = kullanici.Soyad;
            m.OkulNumarasi = kullanici.OkulNumarasi;
            m.Id = kullanici.Id;
            return View(m);
        }

         [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(ProfilDuzenle m)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await userManager.FindByIdAsync(m.Id);
                kullanici.Ad = m.Ad;
                kullanici.Soyad = m.Soyad;
                kullanici.OkulNumarasi = m.OkulNumarasi;
                await userManager.UpdateAsync(kullanici);
                return View(@"Views\Shared\_basarili.cshtml", "Profiliniz başarıyla güncellendi");

            }
            return View(m);
        }

        [Authorize]
        public async Task<IActionResult> SifreDegistir()
        {
            SifreDegistir model = new SifreDegistir();
            //giriş yapmış olan kullanıcının adını modele gönderiyoruz
            model.Username = User.Identity.Name;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SifreDegistir(SifreDegistir model)
        {
            if (ModelState.IsValid)
            {
                //kullanıcı adından kullanıcı bilgilerini alıyoruz
                var kullanici = await userManager.FindByNameAsync(model.Username);
                //Şifre için token oluşturuyoruz
                var token = await userManager.GeneratePasswordResetTokenAsync(kullanici);
                //Şifreyi değiştirmei deniyoruz
                var sonuc = await userManager.ResetPasswordAsync(kullanici, token, model.Sifre);
                if (sonuc.Succeeded)
                {
                    //Hata yok ise , başarılı ekranına gönderiyoruz
                    //Email ile bildir
                    string mesaj=$"Merhaba {kullanici.Ad} {kullanici.Soyad} <br> Şifreniz {DateTime.Now} tarihinde değiştirildi.";
                    Islemler.MailGonder("Şifreniz değiştirildi",mesaj,kullanici.Email);
                    return View(@"Views\Shared\_basarili.cshtml", "Şifreniz başarıyla değiştirildi");
                }
                else
                {
                    // hata var ise aynı ekranda görünmesi için modele oluşan hataları ekliyoruz
                    ModelState.AddModelError("", string.Join("<br>", sonuc.Errors.Select(x => x.Description).ToList()));
                    return View(model);
                }
            }
            return View(model);
        }

           public async Task<IActionResult> Liste()
        {
            return View(userManager.Users.ToList());
        }

        [Authorize(Roles ="Kullanıcı Düzenle")]
        public async Task<IActionResult> Duzenle(string id)
        {            
            ProfilDuzenle model= new ProfilDuzenle();
            var kullanici=await userManager.FindByIdAsync(id);
            if(kullanici==null){
                return View(@"Views\Shared\_hata.cshtml","Kullanıcı bulunamadı!");
            }
            model.Ad=kullanici.Ad;
            model.Soyad=kullanici.Soyad;
            model.OkulNumarasi=kullanici.OkulNumarasi;
            model.Id=kullanici.Id;
            return View(model);
        }


              [HttpPost]
        [Authorize(Roles ="Kullanıcı Düzenle")]
        [ValidateAntiForgeryToken]
        
             public async Task<IActionResult> Duzenle(ProfilDuzenle m)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await userManager.FindByIdAsync(m.Id);
                kullanici.Ad = m.Ad;
                kullanici.Soyad = m.Soyad;
                kullanici.OkulNumarasi = m.OkulNumarasi;
                await userManager.UpdateAsync(kullanici);
                return View(@"Views\Shared\_basarili.cshtml", "Kullanıcı başarıyla güncellendi");

            }
            return View(m);
        }

         [Authorize(Roles ="Kullanıcı Düzenle")]
        public async Task<IActionResult> Sil(string id)
        {                
            var kullanici=await userManager.FindByIdAsync(id);
          var sonuc= await userManager.DeleteAsync(kullanici);
          if(sonuc.Succeeded){
              return View(@"Views\Shared\_basarili.cshtml", "Kullanıcı başarıyla silindi"); 
          } 
          else{
               string mesaj=string.Join("<br>", sonuc.Errors.Select(x=>x.Description).ToList());
               return View(@"Views\Shared\_hata.cshtml", mesaj);
          }            
        }


     }

      

     
}