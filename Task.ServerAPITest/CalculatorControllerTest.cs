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
        [InlineData("0")]
        [InlineData("0.00")]
        [InlineData(".00")]
        public async void InputValueIsZero_GetServiceTest_ReturnOKandZeroDollars(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "zero dollars";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("01")]
        [InlineData("1.00")]
        public async void InputValueIsOne_GetServiceTest_ReturnOKandOneDollar(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "one dollar";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("25.1")]
        [InlineData("025.1")]
        [InlineData("025.10")]
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue_1(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "twenty-five dollars and ten cents";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("0.01")]
        [InlineData("00.01")]
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue_2(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "zero dollars and one cent";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("45100")]
        [InlineData("45100.00")]
        [InlineData("045100")]
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue_3(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "forty-five thousand one hundred dollars";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("999999999.99")]
        [InlineData("0999999999.99")]        
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue_4(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }

        [Theory]
        [InlineData("011012013.14")]
        public async void InputValueIsValid_GetServiceTest_ReturnOKandValue_5(string amount)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            string expectedResult = "eleven million twelve thousand thirteen dollars and fourteen cents";

            //Act
            var result = await client.GetAsync($"api/calculator/{amount}/");
            var resultContent = result.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult, resultContent.Result);
        }
    }
}
