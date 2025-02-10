using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollateralEmailTask
{
    public  class EmailTemplate
    {
        /// <summary>
        /// 0 = Funding Date
        /// </summary>
        /// <returns></returns>
        public static string GetBodyTemplate()
        {
            string template = @"
               <html>

<body>
    Hello!
    <br />
    <br />
    Please send tracking information for the original note. This loan funded on {0} . As stated in our closing instructions, all original notes must be returned to
    <br /> our office within 24 hours of closing, or you are subject to a charge of <b>$250.00 per day delayed</b>.  Please provide us with the note’s tracking number or any
    <br />updates you may have regarding the delay of this delivery.
    <br />
    <br /> <b>Important Information Below!</br>
	<br/>
	Please only ship the original wet signed note & any needed addendums/power of attorney/checks. The closing package should be uploaded to 
	<br />
	Snapdocs within 24 hours, please do not physically mail the closing package to our office. All closing packages not uploaded within 24 hours are
	<br />
	subject to charge of $250.00 per day.
	<br />
	Please note that for any missing documents or signatures, you as the Closing Agent, will be responsible to reach back out to the buyer or borrower
	<br />
	to have the missing/unsigned document(s) corrected.
	<br />
	<br />
	Please ship collateral to:
	<br />
	<br />
	Equity Prime Mortgage
	<br />
	<br />
	ATTN: Collateral Department
	<br />
	<br />
	5 Concourse Pkwy. 
	<br />
	Queen Bldg. Suite 2250
	<br />
	Atlanta, GA 30328
	<br />
	</b>
</body>
</html>

                ";

            return template;
        }

        /// <summary>
        /// 0 = Loan Number
        /// 1 = Borrower Last Name
        /// </summary>
        /// <returns>Collateral Follow-up : {0}- {1}</returns>
        public static string GetSubjectTemplate()
        {
            return "Collateral Follow-up : {0}- {1}";
        }

        public static string GetFromTemplate()
        {
            return "collateral.pc@epm.net";
        }
    }
}
