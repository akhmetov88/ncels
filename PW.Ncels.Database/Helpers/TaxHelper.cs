using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Ncels.Database.Helpers
{
    //НДС
    public class TaxHelper
    {
        public const double taxNDS = 0.12;
        public const int taxNDSint = 12;


        public static double GetCalculationTax(double price)
        {
            return price * taxNDS + price;
        }

        public static decimal GetTaxWithPrice(decimal price)
        {
            return price * (decimal)taxNDS;
        }
    }
}
