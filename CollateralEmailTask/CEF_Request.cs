using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralEmailTask
{
    internal class CEF_Request
    {
        public Root requestRoot = new Root();

        public static CEF_Request BuildRequest(DateTime fundingDate)
        {
            CEF_Request request = new CEF_Request();
            request.requestRoot = Root.BuildRoot(fundingDate);
            

            return request;
        }

        public static string BuildRequestString( Root r)
        {
            var json =  JsonSerializer.Serialize(r);

            return json.ToString();
        }

        

        public class Filter
        {
            public string @operator { get; set; }
            public List<Term> terms { get; set; }

            public Filter()
            {
                @operator = string.Empty;
                terms = new List<Term>();
            }

            public static Filter BuildFilter(DateTime fundDate)
            {
                Filter filter = new Filter();
                filter.@operator = "and";
                filter.terms.Add(Term.BuildFundingDateTerm(fundDate));
                filter.terms.Add(Term.BuildOriginalNoteDateTerm());

                return filter;
            }

         
        }

        public class Root
        {
            public List<string> fields { get; set; }
            public Filter filter { get; set; }
            public List<SortOrder> sortOrder { get; set; }

            public Root() {
                fields = new List<string>();
                filter = new Filter();
                sortOrder = new List<SortOrder>();
            }

            public static Root BuildRoot(DateTime fundDate)
            {
                Root r = new Root();
                r.fields = GetFieldList();
                r.filter = Filter.BuildFilter(fundDate);
                r.sortOrder = SortOrder.BuildSortOrder();

                return r;
            }

            public static List<string> GetFieldList()
            {
                var list = new List<string>();
                list.Add("Loan.LoanFolder");
                list.Add("Fields.4002");
                list.Add("Loan.LoanNumber");
                list.Add("Fields.VEND.X670");
                list.Add("Fields.CX.PC.ORIGINAL.NOTE.DATE");
                list.Add("Fields.1999");
                list.Add("Loan.BorrowerLastName");
                list.Add("Fields.URLA.X73");
                list.Add("Fields.URLA.X74");
                list.Add("Fields.URLA.X75");
                list.Add("Fields.12");
                list.Add("Fields.14");
                list.Add("Fields.15");
                return list;
            }
        }

        public class SortOrder
        {
            public string canonicalName { get; set; }
            public string order { get; set; }

            public SortOrder() 
            { 
                canonicalName = string.Empty;
                order = string.Empty;
            }

            public static List<SortOrder> BuildSortOrder()
            {
                SortOrder s = new SortOrder();
                s.canonicalName = "Fields.1999";
                s.order = "Descending";

                List<SortOrder> list = new List<SortOrder>();
                list.Add(s);

                return list;
            }
        }

        public class Term
        {
            public string canonicalName { get; set; }
            public string matchType { get; set; }
            //public bool include { get; set; }
            public string value { get; set; }
            public string precision { get; set; }

            public static Term BuildOriginalNoteDateTerm()
            {
                Term t = new Term();
                t.canonicalName = "Fields.CX.PC.ORIGINAL.NOTE.DATE";
                t.matchType = "IsEmpty";
                //t.include = true;

                return t;
            }

            public static Term BuildFundingDateTerm(DateTime fundDate)
            {
                Term t = new Term();
                t.canonicalName = "Fields.1999";
                t.value = fundDate.ToString("MM/dd/yyyy");
                t.matchType = "Equals";
                t.precision = "Day";

                return t;
            }
        }
    }
}
