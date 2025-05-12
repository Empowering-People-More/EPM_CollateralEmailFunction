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

        [JsonProperty("Fields.1999")]
        public string Fields1999 { get; set; }

        [JsonProperty("Loan.BorrowerLastName")]
        public string LoanBorrowerLastName { get; set; }

        [JsonProperty("Fields.URLA.X73")]
        public string SubjPropStreetAddress { get; set; }

        [JsonProperty("Fields.12")]
        public string SubjPropCity { get; set; }

        [JsonProperty("Fields.URLA.X74")]
        public string SubjPropUnitType { get; set; }

        [JsonProperty("Fields.URLA.X75")]
        public string SubjPropUnitNumber {get;set; }

        [JsonProperty("Fields.14")]
        public string SubjPropState { get; set; }

        [JsonProperty("Fields.15")]
        public string SubjPropZip { get; set; }

        public CefResponseFields()
        {
            LoanLoanFolder = string.Empty;
            Fields4002 = string.Empty;  
            LoanLoanNumber = string.Empty;
            FieldsVENDX670 = string.Empty;
            FieldsCXPCORIGINALNOTEDATE  = string.Empty;
            Fields1999 = string.Empty;
            LoanBorrowerLastName = string.Empty;
            SubjPropStreetAddress = string.Empty;
            SubjPropCity = string.Empty;
            SubjPropUnitType = string.Empty;
            SubjPropUnitNumber = string.Empty;

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
