using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace CollateralEmailTask
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            var appSettings = GetLocalAppSettings();

            DateTime runDate = DateTime.Now.AddDays(-3);

            ProcessDay(runDate, appSettings);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime prevSundayBatchDate = DateTime.Now.AddDays(-4);
                ProcessDay(prevSundayBatchDate, appSettings);

                DateTime prevSaturdayBatchDate = DateTime.Now.AddDays(-5);
                ProcessDay(prevSaturdayBatchDate, appSettings);
            }
        }

        public static void ProcessDay(DateTime runDate, AppSettings appSettings)
        {
            CEF_Request request = CEF_Request.BuildRequest(runDate);
            string requestString = CEF_Request.BuildRequestString(request.requestRoot);

            EncompassApiWrapper api = new EncompassApiWrapper(appSettings.EncompassSettings);
            api.GetToken();
            List<CefResponseRoot> loans = api.GetCollateralEmailLoans(requestString);

            foreach (CefResponseRoot loan in loans)
            {
                Send3DayEmail(loan, appSettings);
            }
        }

        public static void Send3DayEmail(CefResponseRoot loan, AppSettings appSettings)
        {
            //_logger.LogInformation("Preparing email for loan " + loan.fields.LoanLoanNumber);
            string body = EmailTemplate.GetBodyTemplate();
            DateTime fundDate = DateTime.Parse(loan.fields.Fields1997);
            string fundDateString = fundDate.ToString("MM/dd/yyyy");

            body = string.Format(body, fundDateString);
            string subject = EmailTemplate.GetSubjectTemplate();
            subject = string.Format(subject, loan.fields.LoanLoanNumber, loan.fields.LoanBorrowerLastName);
            string from = EmailTemplate.GetFromTemplate();
            string to = loan.fields.FieldsVENDX670;
            //to = "craig.mcsweeney@epm.net";

            List<string> fromList = new List<string>();
            fromList.Add(from);

            List<string> toList = new List<string>();
            toList.Add(to);

            List<string> ccList = new List<string>();

            Task t = Task.Run(() => new Emailer(appSettings).SendEmailAsync(toList.ToArray(), subject, body));
            t.Wait();
            //Emailer emailClient = new Emailer();
            //await emailClient.SendEmailAsync(toList.ToArray(), subject, body); 

        }

        public static AppSettings GetLocalAppSettings()
        {
            var appSettingsText = System.IO.File.ReadAllText("appsettings.json");
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsText);

            return appSettings;
        }


    }
    public class AppSettings
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SendGridKey { get; set; }
        public string AppInsightsKey { get; set; }
        public EncompassSettings EncompassSettings { get; set; }
    }

    public class EncompassSettings
    {
        public string InstanceId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SmartClientUsername { get; set; }
        public string SmartClientPassword { get; set; }
    }
}
