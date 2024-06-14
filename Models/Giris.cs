using System.ComponentModel.DataAnnotations;
public class giris
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email Alanı Gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli Bir Email Adresi Giriniz")]
    public string Email { get; set; }

    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Şifre Alanı Gereklidir.")]
    [DataType(DataType.Password)]
    public string Sifre { get; set; }
}