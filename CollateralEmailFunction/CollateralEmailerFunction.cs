using System.Text;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CollateralEmailFunction.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RestSharp;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json.Linq;

namespace CollateralEmailFunction
{
    public class CollateralEmailerFunction
    {
        private readonly ILogger<CollateralEmailerFunction> _logger;
        private EncompassCredentials encompassCredentials;
        public CollateralEmailerFunction(ILogger<CollateralEmailerFunction> logger, EncompassCredentials creds)
        {
            _logger = logger;
            encompassCredentials = creds;
        }

        public void GetEncompassLoansForNotification()
        {
            _logger.LogInformation("Retrieving loans from Encompass for Collateral Notification.");
            DateTime runDate = DateTime.Now.AddDays(-3);
            CEF_Request request = CEF_Request.BuildRequest(runDate);
            string requestString = CEF_Request.BuildRequestString(request.requestRoot);

            EncompassApiWrapper api = new EncompassApiWrapper(encompassCredentials);
            api.GetToken();
            List<CefResponseRoot> loans = api.GetCollateralEmailLoans(requestString);

            _logger.LogInformation("Retrieved " + loans.Count.ToString() + " loans for emailing.");

            foreach(CefResponseRoot loan in loans)
            {
                Send3DayEmail(loan);
            }
        }

        public void Send3DayEmail(CefResponseRoot loan)
        {
            _logger.LogInformation("Preparing email for loan " + loan.fields.LoanLoanNumber );
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

            Task t = Task.Run( () => new Emailer(encompassCredentials).SendEmailAsync(toList.ToArray(), subject, body, _logger));
            t.Wait();
            //Emailer emailClient = new Emailer();
            //await emailClient.SendEmailAsync(toList.ToArray(), subject, body); 
            
        }

        [Function("CollateralEmailerFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, ExecutionContext context)
        {
            var value = Environment.GetEnvironmentVariable("myKey");

            
            

            GetEncompassLoansForNotification();
            //const string KeyVaultName = "Encompass-Test";
            //const string KeyVaultUri = $"https://encompass-test.vault.azure.net/";
            //SecretClient? azureKeyVaultSecretClient;

            //var credentials = new DefaultAzureCredential();
            //azureKeyVaultSecretClient = new SecretClient(new
            //Uri(KeyVaultUri), credentials);
            ////Console.WriteLine("Displaying all secrets with their values:");
            //var azureKeyVaultSecrets = 
            //azureKeyVaultSecretClient.GetPropertiesOfSecrets();

            //StringBuilder sb = new StringBuilder();

            //foreach (var secret in azureKeyVaultSecrets)
            //{
            //    var secretValue =
            //    azureKeyVaultSecretClient.GetSecret(secret.Name);

            //    sb.AppendLine("Secret: " + secret.Name + "  =  " + secretValue.ToString());
            //    //Console.WriteLine($ "{secret.Name} |
            //    //{ secretValue.Value.Value} |
            //    //{ secretValue.Value.Properties.ContentType}
            //    //");
            //}


            

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to no Functions!");     //+ Environment.NewLine + Environment.NewLine + sb.ToString());
        }
    }
}
