using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        /// <summary>
        /// It is a method which converts a currency (dollars) from numbers into words
        /// </summary>
        /// <param name="amount">Currency value from client</param>
        /// <returns> Words of currency that is sent</returns>
        [HttpGet("{amount:decimal}")]
        public ActionResult<string> Get(decimal amount)
        {
            int digitCountAfterComma = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];
            //Initial controls
            if (amount >= 1000000000 || digitCountAfterComma > 2)
            {
                return BadRequest("Out of range error!");
            }
            else if (amount < 0)
            {
                return BadRequest("Negative value error!");
            }
            else
            {
                switch (amount)
                {
                    case 0:
                        return Ok("zero dollars");
                    case 1:
                        return Ok("one dollar");
                }
            }

            string words = "";
            string[] ones = { "", " one", " two", " three", " four", " five", " six", " seven", " eight", " nine" };
            string[] tens = { "", " ten", " twenty", " thirty", " forty", " fifty", " sixty", " seventy", " eighty", " ninety" };
            string[] thousands = { " million", " thousand", "" };
            int groupCount = 3;
            string groupValue;
            bool isElevenToNineteen = false;

            string strAmount = amount.ToString("F2").Replace('.', ',');    // Replace '.' to ','            
            string dolarValue = strAmount.Substring(0, strAmount.IndexOf(',')); //dollar part of amount
            string centValue = strAmount.Substring(strAmount.IndexOf(',') + 1, 2); //cent part of amount
            dolarValue = dolarValue.PadLeft(groupCount * 3, '0'); //by adding '0' to the left of the amount, the amount is made with 'group number x 3' digits

            for (int i = 0; i < groupCount * 3; i += 3) //The amount is handled in groups of 3.
            {
                groupValue = "";
                isElevenToNineteen = false;

                if (dolarValue.Substring(i, 1) != "0")
                    groupValue += ones[Convert.ToInt32(dolarValue.Substring(i, 1))] + " hundred"; //dollar hundreds

                if (Convert.ToInt32(dolarValue.Substring(i + 1, 2)) > 10 && Convert.ToInt32(dolarValue.Substring(i + 1, 2)) < 20) //Eleven to Nineteen control
                {
                    groupValue += ConvertNumbersBetweenTenToTwenty(Convert.ToInt32(dolarValue.Substring(i + 1, 2))) + " ";
                    isElevenToNineteen = true;
                }
                else if (tens[Convert.ToInt32(dolarValue.Substring(i + 1, 1))] != "" && ones[Convert.ToInt32(dolarValue.Substring(i + 2, 1))] != "") //if both tens and ones, add "-" between tens and ones
                {
                    groupValue += tens[Convert.ToInt32(dolarValue.Substring(i + 1, 1))] + "-" + ones[Convert.ToInt32(dolarValue.Substring(i + 2, 1))].Trim();
                }
                else
                {
                    groupValue += tens[Convert.ToInt32(dolarValue.Substring(i + 1, 1))]; //dollar tens
                    groupValue += ones[Convert.ToInt32(dolarValue.Substring(i + 2, 1))]; //dollar ones
                }

                if (groupValue != "")
                    groupValue += !isElevenToNineteen ? "" + thousands[i / 3] : "" + thousands[i / 3].Trim(); // ElevenToNineteen blank remove 

                words += groupValue;
            }

            if (words != "")
                words += !isElevenToNineteen ? " dollars " : "dollars "; // ElevenToNineteen blank remove and add "dollars" keyword else only add "dollars" keyword


            int wordsLength = words.Length;
            bool isCent = false;

            if (Convert.ToInt32(centValue) > 10 && Convert.ToInt32(centValue) < 20) //Eleven to Nineteen control
            {
                words += "and" + ConvertNumbersBetweenTenToTwenty(Convert.ToInt32(centValue));
            }
            else
            {
                if (centValue.Substring(0, 1) != "0") //cent tens
                    words += "and" + tens[Convert.ToInt32(centValue.Substring(0, 1))];

                if (centValue.Substring(1, 1) != "0" && centValue.Substring(0, 1) != "0") //cent ones
                    words += "-" + ones[Convert.ToInt32(centValue.Substring(1, 1))].Trim();

                if (centValue.Substring(1, 1) != "0" && centValue.Substring(0, 1) == "0") //if tens is absent, add "and" keyword before cent ones
                { 
                    words += "and" + ones[Convert.ToInt32(centValue.Substring(1, 1))];
                    isCent = centValue.Substring(1, 1) == "1" ? true : false; // if tens is absent and ones equals "1", write cent (singular type)
                }
            }

            if (words.Length > wordsLength) // amount contains cents
            {
                words = !words.Contains("dollars") ? "zero dollars " + words : words; //if dollar part of amount equals zero, add "zero dollars" to words
                words += isCent ? " cent" : " cents";
            }
            else
                words += ""; // amount doesn't contain cents

            return Ok(words.Trim());
        }

        /// <summary>
        /// It is a method that converts numbers between 11 and 19 to text
        /// </summary>
        /// <param name="number">number beetween 11 and 19</param>
        /// <returns>Returns text of number</returns>
        private static string ConvertNumbersBetweenTenToTwenty(int number)
        {
            var result = number switch
            {
                11 => " eleven",
                12 => " twelve",
                13 => " thirteen",
                14 => " fourteen",
                15 => " fifteen",
                16 => " sixteen",
                17 => " seventeen",
                18 => " eighteen",
                19 => " nineteen",
                _ => ""
            };

            return result;
        }
    }
}
