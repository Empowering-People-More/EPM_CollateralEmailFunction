using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace CollateralEmailTask
{
    public class EncompassApiWrapper
    {
        private  string _token;
        //private  ILogger _logger;
        public EncompassSettings Creds;

        public EncompassApiWrapper(EncompassSettings creds)
        {
            Creds = creds;
            _token = string.Empty;

        }
        
        public void GetToken( string overrideUsername = "")
        {
            var client = new RestClient("https://api.elliemae.com");
            
            RestRequest req = GetTokenRequest(overrideUsername);

            var response = client.Execute(req);

            object obj = JsonConvert.DeserializeObject(response.Content);
            
            if (obj == null)
                _token = "";
            else
                _token = ((Newtonsoft.Json.Linq.JObject)obj).Property("access_token").Value.ToString();
        }

        public RestRequest GetTokenRequest(string overrideUsername = "")
        {
            //var username = overrideUsername switch
            //{
            //    "bsdUser" => Creds?.DisclosureDeskUsername,
            //    _ => Creds?.Username
            //};

            var username = Creds.SmartClientUsername;
            var password = Creds.SmartClientPassword;


            //var password = overrideUsername switch
            //{
            //    "bsdUser" => Creds?.DisclosureDeskPassword,
            //    _ => Creds?.Password
            //};

           

            var req = BuildRequest("oauth2/v1/token", Method.Post);

            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            req.AddParameter("grant_type", "password");
            req.AddParameter("username", username + "@encompass:" + Creds.InstanceId);
            req.AddParameter("password", password);
            req.AddParameter("client_id", Creds.ClientId);
            req.AddParameter("client_secret", Creds.ClientSecret);

            return req;
        }

        public RestRequest BuildRequest(string resource, Method httpMethodToInvoke)
        {
            var request = new RestRequest(resource, httpMethodToInvoke);
            request.RequestFormat = DataFormat.Json;
            return request;
        }

        // public void GetToken() { }

        public List<CefResponseRoot> GetCollateralEmailLoans(string requestBody) 
        {
            var options = new RestClientOptions("https://api.elliemae.com");

            var client = new RestClient(options);
            var request = new RestRequest("/encompass/v3/loanPipeline?limit=2000&include=LockInfo", Method.Post);
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-Type", "application/json");
            
            request.AddStringBody(requestBody, DataFormat.Json);
            RestResponse response = client.Execute(request);
            
            Console.WriteLine(response.Content);
            return JsonConvert.DeserializeObject<List<CefResponseRoot>>(response.Content);
        }

        public void SendEmail() { }
    }}
