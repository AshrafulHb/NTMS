using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTMS.DTO
{
    public class EbillingRuleDTO
    {
        public int Id { get; set; }

        public int From1 { get; set; }

        public int From2 { get; set; }

        public int From3 { get; set; }

        public int From4 { get; set; }

        public int To1 { get; set; }

        public int To2 { get; set; }

        public int To3 { get; set; }

        public int To4 { get; set; }

        public decimal Rate1 { get; set; }

        public decimal Rate2 { get; set; }

        public decimal Rate3 { get; set; }

        public decimal Rate4 { get; set; }

        public decimal DemandCharge { get; set; }

        public decimal CommercialRate { get; set; }

        public decimal? CommercialDc { get; set; }

        public decimal MeterRent { get; set; }

        public decimal Vat { get; set; }
    }
}
