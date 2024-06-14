using System.ComponentModel.DataAnnotations;
public class Duyuru : Temel
{
    [Required]
    public string Baslik { get; set; }
    [Required]
    [Display(Name = "Kısa Bilgi")]
    public string KisaBilgi { get; set; }

    [Required]
    public DateTime YayinBaslamaTarihi { get; set; }

    [Required]
    public DateTime EklenmeTarihi { get; set; }
    public Duyuru()
    {
        this.EklenmeTarihi = DateTime.Now;
        this.YayinBaslamaTarihi = DateTime.Now;
    }

}