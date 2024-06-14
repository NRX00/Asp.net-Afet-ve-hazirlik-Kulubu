using System.ComponentModel.DataAnnotations;

public class ProfilTemel
{
    [Required(ErrorMessage = "Ad Boş bırakılamaz")]
    [MinLength(2,ErrorMessage ="En az 2 karakter giriniz")]
    [MaxLength(20,ErrorMessage ="En fazla 20 karakter giriniz")]
    [Display(Name = "Adınız")]
    public string Ad { get; set; }

    [Required(ErrorMessage = "Soyad Boş bırakılamaz")]
[MinLength(2,ErrorMessage ="En az 2 karakter giriniz")]
    [MaxLength(20,ErrorMessage ="En fazla 20 karakter giriniz")]
    [Display(Name = "Soyadınız")]
    public string Soyad { get; set; }

    [Required(ErrorMessage = "Okul Numarası Boş bırakılamaz")]
    [Display(Name = "Okul Numarası")]
    [StringLength(5)]
    public string OkulNumarasi { get; set; }
}
