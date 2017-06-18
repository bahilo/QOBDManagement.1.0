using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using MoneyClass = Money.Money<decimal>;
using MoneyCurrency = Money.Currency;

namespace QOBDManagement.Models
{
    public class CurrencyModel : BindBase
    {
        private Currency _currency;

        public CurrencyModel()
        {
            _currency = new Currency();
        }

        public Currency Currency
        {
            get { return _currency; }
            set { setProperty(ref _currency, value); }
        }

        public MoneyCurrency CurrencyEnum
        {
            get { return getCurrencyByCurrencyCode(TxtCurrencyCode); }
        }

        public string TxtID
        {
            get { return _currency.ID.ToString(); }
            set { _currency.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _currency.Date.ToString("dd/MM/yyyy H:mm:ss"); }
            set { _currency.Date = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public bool IsDefault
        {
            get { return _currency.IsDefault; }
            set { _currency.IsDefault = value; onPropertyChange(); }
        }

        public string TxtName
        {
            get { return _currency.Name; }
            set { _currency.Name = value; onPropertyChange(); }
        }

        public string TxtRate
        {
            get { return _currency.Rate.ToString(); }
            set { _currency.Rate = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtSymbol
        {
            get { return _currency.Symbol; }
            set { _currency.Symbol = value; onPropertyChange(); }
        }

        public string TxtCurrencyCode
        {
            get { return (_currency.CurrencyCode ?? "").ToString(); }
            set { _currency.CurrencyCode = value; onPropertyChange(); }
        }

        public string TxtCurrencyCode_symbol
        {
            get { return _currency.CurrencyCode + ":" + _currency.Symbol; }
            set
            {
                if (value != null)
                {
                    TxtCurrencyCode = value.Split(':')[0];

                    if (value.Split(':').Count() > 1)
                        TxtSymbol = value.Split(':')[1];
                    
                    onPropertyChange();
                }                
            }
        }

        public MoneyClass CurrencyMoney
        {
            get
            {
                if(!string.IsNullOrEmpty(TxtCurrencyCode))
                    return new MoneyClass(_currency.Rate, getCurrencyByCurrencyCode(TxtCurrencyCode));
                return null;
            }
        }

        //=====================[ Static Actions ]

        public static List<string> getCurrencyCodes()
        {
            return getCurrencies().Select(x => x.ToString() + ":" + getCurrencySymbolByCurrencyString(x.ToString())).ToList();
        }

        public static string getCurrencyCodeByCurrencyEnumIntegerValue(int currencyEnumInteger)
        {
            return getCurrencies().Where(x => ((int)x) == currencyEnumInteger).Select(x => x.ToString()).SingleOrDefault();
        }

        public static List<int> getCurrencyEnumIntegerValues()
        {
            return getCurrencies().Select(x => (int)x).ToList();
        }

        public static int getCurrencyEnumIntegerValueByCurrencyCode(string currencyString)
        {
            return getCurrencies().Where(x => x.ToString() == currencyString).Select(x=>(int)x).SingleOrDefault();
        }

        public static Money.Currency getCurrencyByCurrencyCode(string currencyString)
        {
            return getCurrencies().Where(x => x.ToString() == currencyString).SingleOrDefault();
        }

        public static List<Money.Currency> getCurrencies()
        {
            List<Money.Currency> output = new List<Money.Currency>();
            foreach (Money.Currency currency in Enum.GetValues(typeof(Money.Currency)).Cast<Money.Currency>())
                output.Add(currency);
            return output;
        }

        public static MoneyClass ConvertToCurrency(decimal valueToConvert, Money.Currency destinationCurrency, decimal defaultCurrencyRate)
        {
            decimal newValue = decimal.Divide(valueToConvert, defaultCurrencyRate);
            decimal newValueWithPrecision = decimal.Round(newValue, 7);
            return new MoneyClass(newValueWithPrecision, destinationCurrency);
        }

        public MoneyClass getCurrencyMoney(decimal value)
        {
            return new MoneyClass(value, CurrencyEnum);
        }

        public static string getCurrencySymbolByCurrencyString(string currencyString)
        {
            System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                          where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                          let region = new System.Globalization.RegionInfo(culture.LCID)
                                                          where String.Equals(region.ISOCurrencySymbol, currencyString, StringComparison.InvariantCultureIgnoreCase)
                                                          select region).FirstOrDefault();
            if (regionInfo != null)
                return regionInfo.CurrencySymbol;
            return null;
        }

        public static string getCurrencySymbolCurrencyStringByCurrencySymbol(string currencySymbol)
        {
            System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                          where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                          let region = new System.Globalization.RegionInfo(culture.LCID)
                                                          where String.Equals(region.CurrencySymbol, currencySymbol, StringComparison.InvariantCultureIgnoreCase)
                                                          select region).FirstOrDefault();

            if(regionInfo != null)
                return regionInfo.ISOCurrencySymbol;
            return null;
        }
        
    }
}
