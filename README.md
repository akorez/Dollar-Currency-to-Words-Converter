# Dollar-Currency-to-Words-Converter
This repository contains a Client(WPF) - Server(Web API) based converter codes that converts the dollar currency to words.

![converter](https://user-images.githubusercontent.com/34706028/152647653-4ad71860-c4b8-4273-adea-cf98d43bd400.JPG)

### Features
- <strong>ASP.Net Core 5 </strong>
- <strong>WPF based client</strong>
- <strong>WebAPI based server</strong>
- <strong>Server Unit Tests</strong>
- <strong>The maximum number of dollars is 999 999 999</strong>
- <strong>The maximum number of cents is 99</strong>
- <strong>The separator between dollars and cents is a ‘,’ (comma)</strong>

### Examples

| <strong>INPUT</strong> &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;   | <strong>OUTPUT</strong>        |
| ------------- | ------------- |
|            0  | zero dollars  |
| 1 | one dollar  |
| 25,1 | twenty-five dollars and ten cents  |
| 0,01 | zero dollars and one cent  |
| 45 100 | forty-five thousand one hundred dollars |
| 999 999 999,99 | nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents |
