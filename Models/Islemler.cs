using System.Net;
using System.Net.Mail;

public static class Islemler
{
    public static void MailGonder(string baslik, string icerik, string alici)
    {
        MailMessage ePosta = new MailMessage();
        ePosta.From = new MailAddress("Afet Hazırlık Kulubu <okulsaide@yandex.com>");
        ePosta.Subject = baslik;
        ePosta.Body = icerik;
        ePosta.IsBodyHtml = true;
        ePosta.To.Add(alici);

        var _host = "smtp.yandex.com";
        var _port = 587;
        var _defaultCredentials = false;
        var _enableSsl = true;
        var _emailfrom = "okulsaide@yandex.com";//yandex email adresiniz
        var _password = "ejfwkxfmkpvrstvm";//uygulama şifreniz
        using (var smtpClient = new SmtpClient(_host, _port))
        {
            smtpClient.EnableSsl = _enableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = _defaultCredentials;
            if (_defaultCredentials == false)
            {
                smtpClient.Credentials = new NetworkCredential(_emailfrom, _password);
            }

            smtpClient.Send(ePosta);
        }
    }
}