using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Kayit : ProfilTemel
{
   /* [PersonalData]
    [Display(Name = "Ad")]
    [Required(ErrorMessage = "Ad Boş bırakılamaz")]
    [MinLength(3,ErrorMessage ="En az 2 karakter giriniz")]
    [MaxLength(10,ErrorMessage ="En fazla 20 karakter giriniz")]
    public string? Ad { get; set; }

    [Display(Name = "Soyad")]
    [PersonalData]
    [Required(ErrorMessage = "Soyad Boş bırakılamaz")]
    public string? Soyad { get; set; }

    [Required(ErrorMessage = "Okul Numarası bırakılamaz")]
    [StringLength(5)]
    [Display(Name = "Okul Numarası")]
    public string OkulNumarasi { get; set; }*/

    [Required(ErrorMessage = "Email Boş bırakılamaz")]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Şifre Boş bırakılamaz")]
    public string Sifre { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Şifre Tekrarı Boş bırakılamaz")]
    [Compare("Sifre")]
    [Display(Name = "Şifre Takrarı")]
    public string SifreTekrari { get; set; }
}