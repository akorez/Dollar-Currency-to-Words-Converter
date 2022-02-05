using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.ServerAPI.DTO;

namespace Task.ServerAPI.Abstract
{
    public interface ICurrencyConverter
    {
        ResultDTO CurrencyToWords(decimal amount);
    }
}
