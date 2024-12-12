using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTMS.DTO
{
    public class WbillingRuleDTO
    {
        public int Id { get; set; }

        public string UnitPrice { get; set; }

        public string ServiceCharge { get; set; }

        public string Vat { get; set; }
    }
}
