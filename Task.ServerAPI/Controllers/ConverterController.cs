using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.ServerAPI.Abstract;
using Task.ServerAPI.Concrete;
using Task.ServerAPI.DTO;

namespace Task.ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public ConverterController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet("{amount:decimal}")]
        public ActionResult<string> Get(decimal amount)
        {
            ResultDTO result = _currencyConverter.ConvertCurrencyToWords(amount);

            return result.IsSuccess ? Ok(result.Words) : BadRequest(result.Words);
        }

    }
}
