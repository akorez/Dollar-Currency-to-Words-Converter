using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.ServerAPI.Abstract;
using Task.ServerAPI.DTO;

namespace Task.ServerAPI.Concrete
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private static string[] ones = { "", " one", " two", " three", " four", " five", " six", " seven", " eight", " nine" };
        private static string[] tens = { "", " ten", " twenty", " thirty", " forty", " fifty", " sixty", " seventy", " eighty", " ninety" };
        private static string[] thousands = { " million", " thousand", "" };

        /// <summary>
        /// It is a method which converts a currency (dollars) from numbers into words
        /// </summary>
        /// <param name="amount">Currency value from client</param>
        /// <returns> Words of currency that is sent</returns>
        public ResultDTO ConvertCurrencyToWords(decimal amount)
        {
            string calculatedResult = "";

            ResultDTO result = new ResultDTO()
            {
                IsSuccess = true,
                Words = ""
            };

            calculatedResult = IsAmountOutOfRange(amount);

            if (calculatedResult != "")
            {
                result.IsSuccess = false;
                result.Words = calculatedResult;

                return result;
            }

            calculatedResult = IsAmountZeroOrOneDollar(amount);

            if (calculatedResult != "")
            {
                result.Words = calculatedResult;

                return result;
            }
    
            string strAmount = amount.ToString("F2").Replace('.', ',');

            calculatedResult = ConvertDollarPartOfAmount(strAmount);
            calculatedResult = ConvertCentPartOfAmount(strAmount, calculatedResult);
            result.Words = calculatedResult.Trim();

            return result;

        }

        private static string IsAmountOutOfRange(decimal amount)
        {
            string result = "";
            int digitCountAfterComma = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];

            if (amount >= 1000000000 || digitCountAfterComma > 2)
            {
                result = "Out of range error!";
            }
            else if (amount < 0)
            {
                result = "Negative value error!";
            }

            return result;
        }

        private static string IsAmountZeroOrOneDollar(decimal amount)
        {
            var result = amount switch
            {
                0 => "zero dollars",
                1 => "one dollar",
                _ => ""
            };

            return result;
        }


        private static string ConvertDollarPartOfAmount(string strAmount)
        {
            var resultWords = "";
            int groupCount = 3;
            string groupValue;
            bool isElevenToNineteen = false;


            string dollarPartOfAmount = strAmount.Substring(0, strAmount.IndexOf(','));
            dollarPartOfAmount = dollarPartOfAmount.PadLeft(groupCount * 3, '0');

            for (int i = 0; i < groupCount * 3; i += 3)
            {
                groupValue = "";
                isElevenToNineteen = false;

                if (dollarPartOfAmount.Substring(i, 1) != "0")
                    groupValue += ones[Convert.ToInt32(dollarPartOfAmount.Substring(i, 1))] + " hundred";

                if (Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 2)) > 10 && Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 2)) < 20)
                {
                    groupValue += ConvertNumbersBetweenTenToTwenty(Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 2))) + " ";
                    isElevenToNineteen = true;
                }
                else if (tens[Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 1))] != "" && ones[Convert.ToInt32(dollarPartOfAmount.Substring(i + 2, 1))] != "")
                {
                    groupValue += tens[Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 1))] + "-" + ones[Convert.ToInt32(dollarPartOfAmount.Substring(i + 2, 1))].Trim();
                }
                else
                {
                    groupValue += tens[Convert.ToInt32(dollarPartOfAmount.Substring(i + 1, 1))];
                    groupValue += ones[Convert.ToInt32(dollarPartOfAmount.Substring(i + 2, 1))];
                }

                if (groupValue != "")
                    groupValue += !isElevenToNineteen ? "" + thousands[i / 3] : "" + thousands[i / 3].Trim();

                resultWords += groupValue;
            }

            if (resultWords != "")
                resultWords += !isElevenToNineteen ? " dollars " : "dollars ";

            return resultWords;


        }

        private static string ConvertCentPartOfAmount(string strAmount, string words)
        {
            string centPartfOfAmount = strAmount.Substring(strAmount.IndexOf(',') + 1, 2);
            int wordsLength = words.Length;
            bool isCent = false;

            if (Convert.ToInt32(centPartfOfAmount) > 10 && Convert.ToInt32(centPartfOfAmount) < 20)
            {
                words += "and" + ConvertNumbersBetweenTenToTwenty(Convert.ToInt32(centPartfOfAmount));
            }
            else
            {
                if (centPartfOfAmount.Substring(0, 1) != "0")
                    words += "and" + tens[Convert.ToInt32(centPartfOfAmount.Substring(0, 1))];

                if (centPartfOfAmount.Substring(1, 1) != "0" && centPartfOfAmount.Substring(0, 1) != "0")
                    words += "-" + ones[Convert.ToInt32(centPartfOfAmount.Substring(1, 1))].Trim();

                if (centPartfOfAmount.Substring(1, 1) != "0" && centPartfOfAmount.Substring(0, 1) == "0")
                {
                    words += "and" + ones[Convert.ToInt32(centPartfOfAmount.Substring(1, 1))];
                    isCent = centPartfOfAmount.Substring(1, 1) == "1" ? true : false;
                }
            }

            if (words.Length > wordsLength)
            {
                words = !words.Contains("dollars") ? "zero dollars " + words : words;
                words += isCent ? " cent" : " cents";
            }
            else
                words += "";

            return words;

        }

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
