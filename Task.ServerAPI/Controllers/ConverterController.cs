using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.ServerAPI.Concrete;
using Task.ServerAPI.DTO;

namespace Task.ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        [HttpGet("{amount:decimal}")]
        public ActionResult<string> Get(decimal amount)
        {
            CurrencyConverter currencyConverter = new CurrencyConverter();
            ResultDTO result = currencyConverter.ConvertCurrencyToWords(amount);
            
            return result.IsSuccess ? Ok(result.Words) : BadRequest(result.Words);
        }

    }
}
