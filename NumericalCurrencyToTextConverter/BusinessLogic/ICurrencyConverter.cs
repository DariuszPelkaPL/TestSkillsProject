using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalCurrencyToTextConverter.BusinessLogic
{
    public interface ICurrencyConverter
    {
        string ConvertToWords(string currencyAmount);
    }
}
