using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CollateralEmailTask
{
    public class CEF_Response
    {

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class CefResponseFields
    {
        [JsonProperty("Loan.LoanFolder")]
        public string LoanLoanFolder { get; set; }

        [JsonProperty("Fields.4002")]
        public string Fields4002 { get; set; }

        [JsonProperty("Loan.LoanNumber")]
        public string LoanLoanNumber { get; set; }

        [JsonProperty("Fields.VEND.X670")]
        public string FieldsVENDX670 { get; set; }

        [JsonProperty("Fields.CX.PC.ORIGINAL.NOTE.DATE")]
        public string FieldsCXPCORIGINALNOTEDATE { get; set; }

        [JsonProperty("Fields.1997")]
        public string Fields1997 { get; set; }

        [JsonProperty("Loan.BorrowerLastName")]
        public string LoanBorrowerLastName { get; set; }

        public CefResponseFields()
        {
            LoanLoanFolder = string.Empty;
            Fields4002 = string.Empty;  
            LoanLoanNumber = string.Empty;
            FieldsVENDX670 = string.Empty;
            FieldsCXPCORIGINALNOTEDATE  = string.Empty;
            Fields1997 = string.Empty;
            LoanBorrowerLastName = string.Empty;
        }
    }

    public class CefResponseRoot
    {
        public string loanId { get; set; }
        public CefResponseFields fields { get; set; }
        public List<object> locks { get; set; }

        public CefResponseRoot()
        {
            loanId = string.Empty;
            fields = new CefResponseFields();
            locks = new List<object>();
        }
    }

}
