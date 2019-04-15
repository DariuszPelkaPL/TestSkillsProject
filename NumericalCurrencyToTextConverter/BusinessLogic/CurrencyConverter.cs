using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalCurrencyToTextConverter.BusinessLogic
{
    using Microsoft.Extensions.Logging;

    public class CurrencyConverter : ICurrencyConverter
    {
        private const char DecimalSeparator = ',';
        private const char ThousandSeparator = ' ';
        private const int NumberOfPartsInStringifiedAmount = 2;
        private const int DefaultValueBeforeParsing = 0;
        private const string WrongFormatErrorMessage = "Provided amount has incorrect format";
        private const int MaxDecimalPart = 999999999;
        private const int MaxFractionPart = 99;
        private const string TooBigAmountErrorMessage = "Too large amount";
        private const string TooBigFractionalPartErrorMessage = "Too large fractional part";
        private const int Highest3DigitsNumber = 999;
        private const int Highest6DigitsNumber = 999999;
        private const string Dollars = "dollars";
        private const string Dollar = "dollar";
        private const string Cents = "cents";
        private const string Space = " ";
        private const string Es = "s";
        private const string Hundred = "hundred";
        private const string Hundreds = "hundred"; // Should not be "hundreds"?!
        private const string Zero = "0";
        private const string Thousand = "thousand";
        private const string Thousands = "thousand"; // Should not be "thousands"?!
        private const string Million = "million";
        private const string Millions = "million"; // Should not be "million"!?
        private const int SixDigits = 6;
        private const int ThreeDigits = 3;
        private const int TwoDigits = 2;
        private const int OneDigits = 1;
        private const string BetweenDigits = "-";
        private const string And = "and";
        private const string One = "1";
        private const string Cent = "cent";

        public string ConvertToWords(string currencyAmount)
        {
            string amountInWords = string.Empty;
            int integerPart = DefaultValueBeforeParsing;
            int fractionPart = DefaultValueBeforeParsing;
            ParseAmount(currencyAmount, out integerPart, out fractionPart);

            if (integerPart > MaxDecimalPart)
            {
                throw new ArgumentException(TooBigAmountErrorMessage);
            }

            if (fractionPart > MaxFractionPart)
            {
                throw new ArgumentException(TooBigFractionalPartErrorMessage);
            }

            var stringifiedIntegerPart = integerPart.ToString();
            var stringifiedFractionPart = fractionPart.ToString();

            amountInWords = ConvertWholeNumber(stringifiedIntegerPart) + Space + (stringifiedIntegerPart == One ? Dollar : Dollars);

            if (fractionPart > 0)
            {
                amountInWords += Space + And + Space + ConvertWholeNumber(stringifiedFractionPart) + Space + (stringifiedFractionPart == One ? Cent : Cents);
            }

            return amountInWords;
        }

        private void ParseAmount(string stringifiedAmoutn, out int integerPart, out int fractionPart)
        {
            string stringifiedIntegerPart = string.Empty;
            string stringifiedFractionPart = string.Empty;
            integerPart = DefaultValueBeforeParsing;
            fractionPart = DefaultValueBeforeParsing;

            if (stringifiedAmoutn.Contains(DecimalSeparator))
            {
                var parts = stringifiedAmoutn.Split(DecimalSeparator);

                if (parts.Length != NumberOfPartsInStringifiedAmount)
                {
                    throw new ArgumentException(WrongFormatErrorMessage);
                }

                stringifiedIntegerPart = parts[0];
                stringifiedFractionPart = parts[1].Length == 2 ? parts[1] : parts[1] + Zero;
            }
            else
            {
                stringifiedIntegerPart = stringifiedAmoutn;
            }

            stringifiedIntegerPart = stringifiedIntegerPart.Replace(ThousandSeparator.ToString(), string.Empty);

            if (!int.TryParse(stringifiedIntegerPart, out integerPart))
            {
                throw new ArgumentException(WrongFormatErrorMessage);
            }

            if (!string.IsNullOrEmpty(stringifiedFractionPart))
            {
                if (stringifiedFractionPart[0].ToString() == Zero)
                {
                    stringifiedFractionPart = stringifiedFractionPart.Replace(Zero, string.Empty);
                }

                if (!int.TryParse(stringifiedFractionPart, out fractionPart))
                {
                    throw new ArgumentException(WrongFormatErrorMessage);
                }
            }
        }

        private string ConvertWholeNumber(string number)
        {
            var ret = string.Empty;

            // At east 7 to 9 digits number
            if (number.Length > SixDigits)
            {
                var numberOfMillions = number.Substring(0, number.Length - SixDigits);
                var theRest = number.Substring(number.Length - SixDigits);
                ret = ConvertWholeNumber(numberOfMillions) + Space + (numberOfMillions == One ? Million : Millions) + Space
                      + ConvertWholeNumber(theRest);
            }
            else if (number.Length > ThreeDigits)
            {
                // At east 4 to 6 digits number    
                var numberOfThousands = number.Substring(0, number.Length - ThreeDigits);
                var theRest = number.Substring(number.Length - ThreeDigits);
                ret = ConvertWholeNumber(numberOfThousands) 
                     + Space + (numberOfThousands == One ? Thousand : Thousands) + Space
                     + ConvertWholeNumber(theRest);
            }
            else
            {
                // At east 1 to 3 digits number 
                ret = ConvertPartialNumber(number);
            }

            return ret;
        }

        private string ConvertPartialNumber(string number)
        {
            var result = string.Empty;
            var firstDigit = number[0].ToString();
            var secondDigit = number.Length > 1 ? number[1].ToString() : string.Empty;
            var thirdDigit = number.Length > 2 ? number[2].ToString() : string.Empty;

            if (number.Length == ThreeDigits)
            {
                var stringifiedFirstDigit = ConvertPartialNumber(firstDigit.ToString());
                result = stringifiedFirstDigit + Space + (firstDigit == One ? Hundred : Hundreds) + Space + ConvertPartialNumber((secondDigit + thirdDigit).ToString());
            }
            else if (number.Length == TwoDigits)
            {
                switch (number)
                {
                    case "10":
                        result = "ten";
                        break;
                    case "11":
                        result = "eleven";
                        break;
                    case "12":
                        result = "twelve";
                        break;
                    case "13":
                        result = "thirteen";
                        break;
                    case "14":
                        result = "fourteen";
                        break;
                    case "15":
                        result = "fifteen";
                        break;
                    case "16":
                        result = "sixteen";
                        break;
                    case "17":
                        result = "seventeen";
                        break;
                    case "18":
                        result = "eighteen";
                        break;
                    case "19":
                        result = "nineteen";
                        break;
                    default:
                        switch (firstDigit)
                        {
                            case "2":
                                result = "twenty";
                                break;
                            case "3":
                                result = "thirty";
                                break;
                            case "4":
                                result = "fourty";
                                break;
                            case "5":
                                result = "fifty";
                                break;
                            case "6":
                                result = "sixty";
                                break;
                            case "7":
                                result = "seventy";
                                break;
                            case "8":
                                result = "eighty";
                                break;
                            case "9":
                                result = "ninety";
                                break;
                            default:
                                result = string.Empty;
                                break;
                        }

                        if (secondDigit != Zero)
                        {
                            result = result + BetweenDigits + ConvertPartialNumber(secondDigit);
                        }
                        break;
                }
            }
            else
            {
                switch (number)
                {
                    case "0":
                        result = "zero";
                        break;
                    case "1":
                        result = "one";
                        break;
                    case "2":
                        result = "two";
                        break;
                    case "3":
                        result = "three";
                        break;
                    case "4":
                        result = "four";
                        break;
                    case "5":
                        result = "five";
                        break;
                    case "6":
                        result = "six";
                        break;
                    case "7":
                        result = "seven";
                        break;
                    case "8":
                        result = "eight";
                        break;
                    case "9":
                        result = "nine";
                        break;
                }
            }

            return result;
        }
    }
}
