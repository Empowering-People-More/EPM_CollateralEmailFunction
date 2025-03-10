using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using EPM_AppInsightsLogger;
using Serilog;

namespace CollateralEmailTask
{
    internal class Program
    {
        private static AppInsightsLogger _logger;
        /// <summary>
        /// Completed.  Now handles -2 days if the filter field is not yet completed.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            var appSettings = GetLocalAppSettings();

            _logger = new AppInsightsLogger(appSettings.AppInsightsKey, EPM_AppInsightsLogger.Environment.Prod, "EPM_CollateralEmailer");

            DateTime runDate = DateTime.Now.AddDays(-3);

            _logger.Information("Running collateral emailer for " + runDate.ToString("MM/dd/yyyy") + " (" + runDate.ToString("dddd") + ")");

            ProcessDay(runDate, appSettings, _logger);

            _logger.Information("Completed collateral emailer for " + runDate.ToString("MM/dd/yyyy") + " (" + runDate.ToString("dddd") + ")");

            runDate = DateTime.Now.AddDays(-4);

            _logger.Information("Running collateral emailer for " + runDate.ToString("MM/dd/yyyy") + " (" + runDate.ToString("dddd") + ")");

            ProcessDay(runDate, appSettings, _logger);

            _logger.Information("Completed collateral emailer for " + runDate.ToString("MM/dd/yyyy") + " (" + runDate.ToString("dddd") + ")");

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime prevSundayBatchDate = DateTime.Now.AddDays(-5);
                _logger.Information("Running collateral emailer for " + prevSundayBatchDate.ToString("MM/dd/yyyy")
                    + " (" + prevSundayBatchDate.ToString("dddd") + ")");
                ProcessDay(prevSundayBatchDate, appSettings, _logger);

                _logger.Information("Completed collateral emailer for " + prevSundayBatchDate.ToString("MM/dd/yyyy")
                    + " (" + prevSundayBatchDate.ToString("dddd") + ")");

                DateTime prevSaturdayBatchDate = DateTime.Now.AddDays(-6);
                _logger.Information("Running collateral emailer for " + prevSaturdayBatchDate.ToString("MM/dd/yyyy")
                    + " (" + prevSaturdayBatchDate.ToString("dddd") + ")");
                ProcessDay(prevSaturdayBatchDate, appSettings, _logger);
            }
            //Added completion logging.
            _logger.Information("Completed collateral emailer process.");
        }

        public static void ProcessDay(DateTime runDate, AppSettings appSettings, AppInsightsLogger logger)
        {
            CEF_Request request = CEF_Request.BuildRequest(runDate);
            string requestString = CEF_Request.BuildRequestString(request.requestRoot);

            EncompassApiWrapper api = new EncompassApiWrapper(appSettings.EncompassSettings);
            api.GetToken();
            List<CefResponseRoot> loans = api.GetCollateralEmailLoans(requestString);

            foreach (CefResponseRoot loan in loans)
            {
                logger.Information("Sending email for " + loan.fields.LoanLoanNumber + " - " + loan.fields.LoanBorrowerLastName.ToString());
                try
                {
                    Send3DayEmail(loan, appSettings);
                    logger.Information("Sent email for " + loan.fields.LoanLoanNumber + " - " + loan.fields.LoanBorrowerLastName.ToString());
                }
                catch (Exception ex)
                {
                    logger.Error("Error sending email for " + loan.fields.LoanLoanNumber + ".  Exception: " + ex.ToString());
                }
            }
        }

        public static void Send3DayEmail(CefResponseRoot loan, AppSettings appSettings)
        {
            //_logger.LogInformation("Preparing email for loan " + loan.fields.LoanLoanNumber);
            string body = EmailTemplate.GetBodyTemplate();
            DateTime fundDate = DateTime.Parse(loan.fields.Fields1999);
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
