using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class Email
    {
        public bool SendEmail(EmailObject Email, bool CCEmail)
        {
            bool isSuccess = false;

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(Email.From);
                message.To.Add(Email.To);

                if(CCEmail)
                    message.CC.Add(Email.CC);

                message.Subject = Email.Subject;
                message.Body = Email.Body;

                message.SubjectEncoding = System.Text.Encoding.Default;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = Configs.SMTPHost;
                smtp.Send(message);
                isSuccess = true;
            }
            catch
            {

            }

            return isSuccess;
        }
    }
}
