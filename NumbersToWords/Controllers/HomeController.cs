using Microsoft.AspNetCore.Mvc;
using NumbersToWords.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace NumberToWords.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private static readonly Dictionary<int, string> _numberToWordsMapping = new Dictionary<int, string>
        {
            {1, "ONE"},
            {2, "TWO"},
            {3, "THREE"},
            {4, "FOUR"},
            {5, "FIVE"},
            {6, "SIX"},
            {7, "SEVEN"},
            {8, "EIGHT"},
            {9, "NINE"},
            {10, "TEN"},
            {11, "ELEVEN"},
            {12, "TWELVE"},
            {13, "THIRTEEN"},
            {14, "FOURTEEN"},
            {15, "FIFTEEN"},
            {16, "SIXTEEN"},
            {17, "SEVENTEEN"},
            {18, "EIGHTEEN"},
            {19, "NINETEEN"}
        };

        private static readonly Dictionary<int, string> _numberToWordsMappingTens = new Dictionary<int, string>
        {
            {20, "TWENTY"},
            {30, "THIRTY"},
            {40, "FOURTY"},
            {50, "FIFTY"},
            {60, "SIXTY"},
            {70, "SEVENTY"},
            {80, "EIGHTY"},
            {90, "NINETY"}
        };

        public static string? ConvertNumberToWordBased(int numberBased, string NumberInWordBased)
        {
            string NumberInWords = NumberInWordBased;

            //when the number is more than hundred
            if (numberBased / 100 > 0)
            {
                if (_numberToWordsMapping.TryGetValue(numberBased / 100, out string valueHundred))
                {
                    NumberInWords += " " + valueHundred + " HUNDRED";
                }
                if ((numberBased %= 100) == 0) return NumberInWords;
            }

            //when the number is less than hundred
            if (numberBased < 100)
            {
                //if (NumberInWords != "") NumberInWords += " AND";

                if (numberBased >= 20)
                {
                    if (_numberToWordsMappingTens.TryGetValue((numberBased / 10) * 10, out string valueTens))
                    {

                        NumberInWords += " " + valueTens;

                        if (_numberToWordsMapping.TryGetValue(numberBased % 10, out string valueUnit))
                        {
                            NumberInWords += " " + valueUnit;
                            return NumberInWords;
                        }

                        return NumberInWords;
                    }
                }

                if (numberBased < 20)
                {
                    if (_numberToWordsMapping.TryGetValue(numberBased, out string value))
                    {
                        NumberInWords += " " + value;
                        return NumberInWords;
                    }
                }
            }

            return null;
        }

        public static string? ConvertNumberToWord(int number)
        {
            string NumberInWords = "";

            //check if the round number is empty
            if (number != 0)
            {
                //when the number is more than million
                if (number / 1000000 > 0)
                {
                    NumberInWords = ConvertNumberToWordBased(number / 1000000, NumberInWords);
                    NumberInWords += " MILLION";

                    if ((number %= 1000000) == 0) return NumberInWords + " DOLLARS";
                }

                //when the number is more than thousand
                if (number / 1000 > 0)
                {
                    NumberInWords = ConvertNumberToWordBased(number / 1000, NumberInWords);
                    NumberInWords += " THOUSAND";

                    if ((number %= 1000) == 0) return NumberInWords + " DOLLARS";
                }

                //convert hundreds, tens, ones
                NumberInWords = ConvertNumberToWordBased(number, NumberInWords);

                return NumberInWords + " DOLLARS";
            }

            return null;

        }

        public static string? ConvertNumberToWordDecimal(int number)
        {
            string NumberInWords = "";

            //check if the decimal number is empty
            if (number != 0)
            {
                //convert tens, ones
                NumberInWords = ConvertNumberToWordBased(number, NumberInWords);

                return NumberInWords + " CENTS";
            }

            return null;

        }



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Index(Number number)
        {
            Number model = new Number();

            decimal Num = number.Numbers;

            //Check if the number contains decimal
            if (!Num.ToString().Contains('.')) { Num = Decimal.Add(Num, .00m); }

            //Split the number into 2 parts (Round Number & Decimal Number)
            string[] parts = Num.ToString().Split('.');

            int RoundNum = int.Parse(parts[0]);
            int DecimalNum = int.Parse(parts[1]);

            string Word = "";

            //Convert the number in 2 parts (Round Number & Decimal Number)
            Word = ConvertNumberToWord(RoundNum);
            Word += ConvertNumberToWordDecimal(DecimalNum);

            //Pass the words into string
            model.NumberInWord = Word;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}