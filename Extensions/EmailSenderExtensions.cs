using System.Text.Encodings.Web;

namespace BusinessERP.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm Your Email: Business ERP Solution",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>"
                 + "<br /><br />Thanks<br />Admin<br />Email: admin@gmail.com");
        }
        public static Task SendEmailForgotPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Reset Password: Business ERP Solution",
                   $"Please reset your password by clicking here: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>"
                   + "<br /><br />Thanks<br />Admin<br />Email: admin@gmail.com");
        }
    }
}
