using System.Net;
using System.Net.Mail;
using System.Text;



namespace SchoolApp.Helper
{
    public static class Utility
    {
        static Random rand = new Random();

        private static string _username = $"apprentice@forek.co.za";

        private static string _password = "P@55w0rd2022";

        public const string Alphabet =
        "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string loggedInUser = "";
        public static string ValueEncryption(string value)
        {
            return Convert.ToBase64String(

                System.Security.Cryptography.SHA256.Create()

                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }

        public static HttpClient Initialize(string baseAddress)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseAddress);

            return client;
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();

            var ext = Path.GetExtension(path).ToLowerInvariant();

            return types[ext];
        }
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        public static string RandomStringGenerator(int size)
        {
            char[] chars = new char[size];

            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }

            return new string(chars);

        }

        public static string OnGetCurrentDateTime()
        {
            return DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
        }

        public static string OnConfirmationMessage(string name,string learnerNum, string Id, string grade)
        {
            return $"Confirmation of Enrollment<br/><hr/> " +
                $"Dear {name} it is with great pleasure to confirm & acknowledge reciept of your application<br/>" +
                $" Your details are recorded as follows:<br/><hr/> Name: {name}<br/>" +
                $"ID Number: {Id}<br/> Grade: {grade}<br/>Learner Reference Number: {learnerNum}<br/><br/>Warm Regards";
                
        }

        public static string OnAssessmentSchedule(string grade, string subject, string date, string time, string type)
        {
            return $"Good day {grade}'s this notification servers to inform you of our upcoming assessment<br/>" +
                $"The details for the assessment is as follow:<hr/>" +
                $"1) Subject: {subject}<br/>" +
                $"2) Assessment Type: {type}<br/>" +
                $"3) Date of Assessment: {date}<br/>" +
                $"4) Time: {time} exactly<hr/>" +
                $"Warm Regards";
        }

        public static void OnSendMailNotification(string reciever, string subject, string message, string header)
        {
                var senderMail = new MailAddress(_username, $"iCode E-Tutor");

                var recieverMail = new MailAddress(reciever, header);
            
                var password = _password;

                var sub = subject;

                var body = message;

                var smtp = new SmtpClient
                {
                    Host = "smtp.forek.co.za",

                    Port = 587,

                    EnableSsl = true,

                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(senderMail.Address, password)
                };

                using (var mess = new MailMessage(senderMail, recieverMail)
                {
                    Subject = subject,

                    Body = body,

                    IsBodyHtml = true,

                })

                {
                    //mess.Attachments.Add(new Attachment("C:\\file.zip"));

                    smtp.Send(mess);
                }
            }
    }
}
