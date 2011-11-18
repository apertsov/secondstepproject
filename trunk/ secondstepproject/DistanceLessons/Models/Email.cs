using System.Net.Mail;

namespace DistanceLessons.Models
{
    public static class Email
    {
        public static void SendActivationLink(string ActivationLink,string Email)
        {
            var message = new MailMessage("distancelessons@gmail.com", Email)
            {
                Subject = "Активація акаунту на сайті дистанційного навчання",
                Body = ActivationLink
            };

            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("distancelessons@gmail.com", "distancelesson");

            client.Send(message);
        }
    }
}