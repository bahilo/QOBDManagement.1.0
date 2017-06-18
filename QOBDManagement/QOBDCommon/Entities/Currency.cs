using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Entities
{
    public class Currency
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal Rate { get; set; }

        public bool IsDefault { get; set; }

        public string CountryCode { get; set; }

        public string CurrencyCode { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }
    }
}
