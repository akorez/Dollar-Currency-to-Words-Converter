using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using Task.ServerAPI.Controllers;
using Xunit;

namespace Task.ServerAPITest
{
    public class CalculatorControllerTest
    {
        [Theory]
        [InlineData("1000000000")]
        [InlineData("0.001")]
        public async void InputValueIsOutOfRange_GetServiceTest_ReturnBadRequestandOutOfRangeResult(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "Out of range error!";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();
           
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("-25")]
        public async void InputValueIsNegative_GetServiceTest_ReturnBadRequestandNegativeValueError(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "Negative value error!";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("0", "zero dollars")]
        [InlineData("0.00", "zero dollars")]
        [InlineData(".00", "zero dollars")]
        [InlineData("1", "one dollar")]
        [InlineData("01", "one dollar")]
        [InlineData("1.00", "one dollar")]
        [InlineData("25.1", "twenty-five dollars and ten cents")]
        [InlineData("025.1", "twenty-five dollars and ten cents")]
        [InlineData("025.10", "twenty-five dollars and ten cents")]
        [InlineData("0.01", "zero dollars and one cent")]
        [InlineData("00.01", "zero dollars and one cent")]
        [InlineData("45100", "forty-five thousand one hundred dollars")]
        [InlineData("45100.00", "forty-five thousand one hundred dollars")]
        [InlineData("045100", "forty-five thousand one hundred dollars")]
        [InlineData("999999999.99", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [InlineData("0999999999.99", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [InlineData("011012013.14", "eleven million twelve thousand thirteen dollars and fourteen cents")]
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue(string amount, string expected)
        {
            //Arrange
            var client = new TestClientProvider().Client;

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expected, resultContent.Result);
        }

    }
}
