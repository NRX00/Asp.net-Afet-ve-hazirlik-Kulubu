using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
public class AppUser : IdentityUser
{
    [PersonalData]
    [Display(Name = "Ad")]
    [Required(ErrorMessage = "Ad Boş bırakılamaz")]
    [MinLength(100,ErrorMessage ="En az 2 karakter giriniz")]
    [MaxLength(100,ErrorMessage ="En fazla 20 karakter giriniz")]
    public string? Ad { get; set; }

    [Display(Name = "Soyad")]
    [PersonalData]
    [Required(ErrorMessage = "Soyad Boş bırakılamaz")]
    public string? Soyad { get; set; }

    [Required(ErrorMessage = "Okul Numarası bırakılamaz")]
    [StringLength(5)]
    [Display(Name = "Okul Numarası")]
    public string OkulNumarasi { get; set; }
}