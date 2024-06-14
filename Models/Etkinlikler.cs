using System.ComponentModel.DataAnnotations;
public class Etkinlik : Temel
{
    [Required]
    public string EtkinlikAd { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime TarihSaat { get; set; }
    [Required]
    public string Yer { get; set; }

    [Required]
    [DataType(DataType.ImageUrl)]
    public string Gorsel { get; set; }
   // public DateTime YayinBaslamaTarihi { get;  set; }

    public Etkinlik()
    {
        this.TarihSaat = DateTime.Now; ;
    }

}