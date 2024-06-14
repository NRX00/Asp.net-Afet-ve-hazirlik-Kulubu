using Microsoft.AspNetCore.Identity;

public class YetkiliKullanicilarModel
{
    public string RoleAd {get;set;}
    public List<AppUser> ButunKullanicilar{get;set;}
    public IList<AppUser> YetkiliKullanicilar{get;set;}
}