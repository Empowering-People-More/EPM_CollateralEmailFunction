using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace CollateralEmailTask
{
    internal class Emailer
    {
        private readonly string _sendGridKey;
        private readonly bool _isDevEnvironment;

        public Emailer(AppSettings creds)
        {
            //var text = File.ReadAllText("local.settings.json");
            //var jObject = JObject.Parse(text);
            //_sendGridKey = jObject.GetValue("SendGridKey")?.ToString();
            //_isDevEnvironment = jObject.GetValue("Instance")?.ToString() != "BE11177782";
            _sendGridKey = creds.SendGridKey;
            _isDevEnvironment = creds.EncompassSettings.InstanceId != "BE11177782" && creds.EncompassSettings.InstanceId != "BE11370673";
        }

        /// <summary>
        /// Sends am email using SendGrid
        /// </summary>
        /// <param name="to">Recipient string array</param>
        /// <param name="cc">Cc string array</param>
        /// <param name="subject">Email subject line</param>
        /// <param name="body">Body of the Email</param>
        /// <exception cref="Exception">Thrown if the api call is not successful.</exception>
        public void SendEmailAsync(string[] to, string subject, string body)
        {
            var tos = to.Where(x => !string.IsNullOrEmpty(x)).Select(x => new EmailAddress(x)).ToList();
            //var ccs = cc.Where(x => !string.IsNullOrEmpty(x) && !tos.Any(toAddress => toAddress.Email == x))
              //  .Select(x => new EmailAddress(x)).ToList(); // remove the email if it already exists in the To (will throw error) Common in DEV
            if (_isDevEnvironment)
            {
                tos.Clear();
                //tos.Add(new EmailAddress("jed@epm.net"));
                //tos.Add(new EmailAddress("kelly@epm.net"));
                //tos.Add(new EmailAddress("kristie.cupitt@epm.net"));
                //tos.Add(new EmailAddress("apayton@epm.net"));
                //tos.Add(new EmailAddress("arivera@epm.net"));
                tos.Add(new EmailAddress("encompassdev@epm.net"));

                //ccs.Clear();
                //ccs.Add(new EmailAddress("luis@epm.net"));
            }
            //logger.LogInformation("Preparing email to " + tos);
            var from = new EmailAddress("noreply@epm.net", "No Reply");
            var ccs = new List<EmailAddress>();
            //Added cc to collateral.pc@epm.net
            ccs.Add(new EmailAddress("collateral.pc@epm.net"));
            var message = new SendGridMessage
            {
                From = from,
                Subject = subject,
                HtmlContent = body,
            };
            message.AddTos(tos);
            message.AddCcs(ccs);
            var client = new SendGridClient(_sendGridKey);
            var response = SendMail(message, client).Result;
           
            
            if (response.IsSuccessStatusCode)
            {
                //logger.LogInformation("Email sent successfully to " + tos);
            }
            else
            {
                //logger.LogError("Error sending email to " + tos + ".  Status Code: " +
                //    Environment.NewLine + response.StatusCode + Environment.NewLine +
                //    "Details:" + Environment.NewLine +
                //    response.Body);
            }
        }

        public async Task<Response> SendMail(SendGridMessage m, SendGridClient c)
        {
            var response = await c.SendEmailAsync(m);

            return response;
        }
    }
}
