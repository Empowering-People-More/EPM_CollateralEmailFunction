using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollateralEmailFunction
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
                    Please send tracking information for the original note. This loan funded on {0} . As stated in our closing conditions, all original notes must be returned
                    <br />to address before 24 hours after closing. Please provide us with the note’s tracking number or any updates you may have regarding the delay of this delivery.
                    <br />Please Note: There will be a charge of $250.00 per day for the original notes not returned within 48 hours.

                    <br />
                    <br />
                    *Please note our closing instructions have changed. Please only ship the original wet signed note & any needed addendums/power of attorney. The closing
                    <br /> package should be uploaded to Snapdocs only, please do not physically mail the closing package to our office.
                    <br />
                    <br />
                    Please ship collateral to:
                    <br />
                    <br />
                    <b>
                        Equity Prime Mortgage
                        <br />ATTN: Collateral Department
                        <br />
                        <br />5 Concourse Pkwy.
                        <br />Queen Bldg. Suite 2250
                        <br />Atlanta, GA 30328
                    </b>
                    <br />
                    <br />
                    Thank you!
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
