using System.ComponentModel.DataAnnotations;

public class SifreTemel{
     [Display(Name ="Şifre")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage ="Şifre boş bırakılamaz")]
    public string Sifre { get; set; }
    
    [Compare("Sifre")]    
    [Display(Name ="Şifre tekrarı")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage ="Şifre tekrarı boş bırakılamaz")]
    public string SifreTekrari { get; set; }
}