using System.ComponentModel.DataAnnotations;

public class SifreDegistir:SifreTemel
{
   [Required]
    public string Username { get; set; }
}