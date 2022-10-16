using System.Net;
using System.Net.Mail;

namespace Digital_Signatues.Helpers
{
    public class SendEmailHelper
    {
        public static void SendEmail(string email, string otp)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("your email");
                mail.To.Add(email);
                mail.Subject = "Mã OTP";
                mail.Body = "<h5>Bạn đang sử dụng chức năng quên mật khẩu</h5>" +
                    "<p>Đây là mã OTP xác nhận của bạn: " + otp + "</p>" +
                    "<p>Chúc bạn một ngày tốt lành</p>" +
                    "<p><i style='color:red'>Lưu ý: mã OTP chỉ có hiệu lực trong 2 phút<i></p>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("your email", "pasword");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
