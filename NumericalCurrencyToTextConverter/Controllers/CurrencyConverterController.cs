using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NumericalCurrencyToTextConverter.BusinessLogic;
using NumericalCurrencyToTextConverter.Models;

namespace NumericalCurrencyToTextConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private ICurrencyConverter _converter;

        public CurrencyConverterController(ICurrencyConverter converter)
        {
            this._converter = converter;
        }

        [HttpGet("[action]")]
        public ConverterResponse Convert(string currency)
        {
            var response = new ConverterResponse();

            try
            {
                response.ConvertedText = this._converter.ConvertToWords(currency);
                response.Status = (int)ConversionStatusCode.OK;
                response.ErrorMessage = string.Empty;
            }
            catch (ArgumentException argumentException)
            {
                response.Status = (int)ConversionStatusCode.Error;
                response.ErrorMessage = argumentException.Message;
            }
            catch (Exception)
            {
                response.Status = (int)ConversionStatusCode.Error;
                response.ErrorMessage = "Error while doing conversion";
                // Some logging stuff here log.Error(exception);
            }
            
            return response;
        }
    }
}