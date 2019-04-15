using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalCurrencyToTextConverter.Models
{

    public class ConverterResponse
    {
        public string ConvertedText { get; set; }

        public int Status { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum ConversionStatusCode
    {
        OK = 0,
        Error = 1
    }
}
