using Newtonsoft.Json;
using piris.DomainService.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace piris.DomainService.lpml
{
    

    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "CentralBankService" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы CentralBankService.svc или CentralBankService.svc.cs в обозревателе решений и начните отладку.
    public class CentralBankService : ICentralBankService
    {
        public class CurrencyInfo
        {
            public double Value { get; set; }

        }

        public class Valute
        {
            public CurrencyInfo EUR { get; set; }
            public CurrencyInfo USD { get; set; }
            public CurrencyInfo CNY { get; set; }

           
        }

        public class ResultRoot
        {
            public Dictionary<string, CurrencyInfo> Valute { get; set; }
            // Другие свойства, если они есть в вашем JSON-ответе (Date и т.д.)

            // Конструктор для инициализации словаря, если это необходимо
            public ResultRoot()
            {
                Valute = new Dictionary<string, CurrencyInfo>();
            }
        }

        private string _apiUrl = "https://www.cbr-xml-daily.ru/daily_json.js";


            public ConverterObject ConvertValue(double value, string currencyName)
            {
                ConverterObject converterRes = new ConverterObject();

                using (HttpClient _httpClient = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = _httpClient.GetAsync(_apiUrl).Result;
                        Console.WriteLine(response.Content);
                        Console.WriteLine("1");

                        if (response.IsSuccessStatusCode)
                        {
                            converterRes.requestRes = response.StatusCode.ToString();
                            var rawRes = response.Content.ReadAsStringAsync().Result;
                            var desResult = JsonConvert.DeserializeObject<ResultRoot>(rawRes);

                            switch (currencyName.ToUpper())
                            {
                                case "EUR":
                                    converterRes.currencyValue = value / desResult.Valute["EUR"].Value;
                                    converterRes.currencyName = "EUR";
                                    break;
                                case "USD":
                                    converterRes.currencyValue = value / desResult.Valute["USD"].Value;
                                    converterRes.currencyName = "USD";
                                    break;
                                case "CNY":
                                    converterRes.currencyValue = value / desResult.Valute["CNY"].Value;
                                    converterRes.currencyName = "CNY";
                                    break;
                            default:
                                    converterRes.currencyValue = value;
                                    converterRes.currencyName = "RUB";
                                    break;
                            }

                            Console.WriteLine("Successfully fetched currency data.");
                        }
                        else
                        {
                            converterRes.currencyValue = value;
                            converterRes.currencyName = "RUB";
                            converterRes.requestRes = response.StatusCode.ToString();
                            Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        converterRes.currencyValue = value;
                        converterRes.currencyName = "RUB";
                        converterRes.requestRes = $"Error: {ex.Message}";
                        Console.WriteLine($"Error fetching currency data: {ex.Message}");
                    }
                }

                return converterRes;
            }
        }

}
