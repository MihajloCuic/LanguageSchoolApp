using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
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
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("data/appConfig.json", optional: false, reloadOnChange: true)
                .Build();

            senderEmail = config["EmailSettings:SenderEmail"];
            password = config["EmailSettings:Password"];
            smtpHost = config["EmailSettings:SmtpHost"];
            port = int.Parse(config["EmailSettings:Port"]);
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
                string body = SetEmailBodyForCourseResults(student.Name, course.LanguageProficiency);
                MailMessage message = new MailMessage(senderEmail, student.Email, "Course Results", body)
                {
                    IsBodyHtml = true
                };

                client.Send(message);
            }
        }

        public void SendExamResults(List<Student> students, Exam exam)
        {
            SmtpClient client = new SmtpClient(smtpHost, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            foreach (Student student in students)
            {
                string body = SetEmailBodyForExamResults(student.Name, exam.LanguageProficiency);
                MailMessage message = new MailMessage(senderEmail, student.Email, "Exam Results", body)
                {
                    IsBodyHtml = true
                };

                client.Send(message);
            }
        }

        private string SetEmailBodyForCourseResults(string studentName, LanguageProficiency proficiency) 
        {
            StringBuilder template = new();
            template.AppendLine("<div style='margin: auto; width: 200px; font-family: Cambria;'>");
            template.Append("<div style='text-align: center;'><img src='https://i.postimg.cc/kgc8YK2k/world-blue.png'/></div>");
            template.AppendLine($"Dear {studentName},<br>");
            template.AppendLine($"<p>Congratulations,<br>I am glad to inform you that you finished course <b>{proficiency.LanguageName} {proficiency.LanguageLevel.ToString()}</b> as one of top students !</p>");
            template.AppendLine("Sincerely,<br> Global Speak Academy Lingua</div>");
            return template.ToString();
        }

        private string SetEmailBodyForExamResults(string studentName, LanguageProficiency proficiency)
        {
            StringBuilder template = new();
            template.AppendLine("<div style='margin: auto; width: 200px; font-family: Cambria;'>");
            template.Append("<div style='text-align: center;'><img src='https://i.postimg.cc/kgc8YK2k/world-blue.png'/></div>");
            template.AppendLine($"Dear {studentName},<br>");
            template.AppendLine($"<p>The results of exam <b>{proficiency.LanguageName} {proficiency.LanguageLevel.ToString()}</b> are out !<br>");
            template.AppendLine("To see your details please visit the application in section <i>My Exams</i>.</p>");
            template.AppendLine("Sincerely,<br> Global Speak Academy Lingua</div>");
            return template.ToString();
        }
    }
}
