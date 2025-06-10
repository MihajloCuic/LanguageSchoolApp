using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LanguageSchoolApp.service.Helpers
{
    public class EmailService : IEmailService
    {
        private readonly string senderEmail;
        private readonly string password;
        private readonly string smtpHost;
        private readonly int port;

        public EmailService()
        {
            senderEmail = "gsalingua@gmail.com";
            password = "xdew jvqw xvtk txmy";
            smtpHost = "smtp.gmail.com";
            port = 587;
        }

        public void SendCourseResults(List<Student> students, Course course) 
        {
            SmtpClient client = new SmtpClient(smtpHost, port) 
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            foreach (Student student in students) 
            {
                string body = SetEmailBody(student.Name, course.LanguageProficiency);
                MailMessage message = new MailMessage(senderEmail, student.Email, "Course Results", body)
                {
                    IsBodyHtml = true
                };

                client.Send(message);
            }
        }

        private string SetEmailBody(string studentName, LanguageProficiency proficiency) 
        {
            StringBuilder template = new();
            template.AppendLine("<div style='margin: auto; width: 200px; font-family: Cambria;'>");
            template.Append("<div style='text-align: center;'><img src='https://i.postimg.cc/kgc8YK2k/world-blue.png'/></div>");
            template.AppendLine($"Dear {studentName},<br>");
            template.AppendLine($"<p>Congratulations,<br>I am glad to inform you that you finished course <b>{proficiency.LanguageName} {proficiency.LanguageLevel.ToString()}</b> as one of top students !</p>");
            template.AppendLine("Sincerely,<br> Global Speak Academy Lingua</div>");
            return template.ToString();
        }
    }
}
