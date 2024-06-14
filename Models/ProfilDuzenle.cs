using System.ComponentModel.DataAnnotations;

public class ProfilDuzenle:ProfilTemel
{
    [Required]
    public string Id { get; set; }   
}