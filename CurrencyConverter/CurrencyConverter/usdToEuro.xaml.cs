using Android.Content;
using Android.Util;
using Android.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CurrencyConverter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class usdToEuro : ContentPage
	{
		public usdToEuro ()
		{
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

        }



        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<decimal> RunEnchanchgeSync()
        {
            decimal test2 = decimal.Parse(number.Text);
            var result = await CurrencyConversionAsync(test2, "USD", "EUR");
            return result;
        }

        static ConcurrentDictionary<string, decimal> cachedDownloads =
       new ConcurrentDictionary<string, decimal>();
        private const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        public async Task<decimal> CurrencyConversionAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            string content = "";
            string url = string.Format(urlPattern, fromCurrency, toCurrency);
            Decimal result = 0;
            decimal exchangeRate = 0;
            if (CheckForInternetConnection() == false)
            {
                result = amount * decimal.Parse("1.11");
                return result;
            }

            if (cachedDownloads.TryGetValue(content, out exchangeRate))
            {
                result = (amount * exchangeRate);
                return result;

            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(url).ConfigureAwait(false);

                    // Now parse with JSON.Net

                    Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                    exchangeRate = (decimal)token.SelectToken("rate");
                    result = (amount * exchangeRate);
                    cachedDownloads.TryAdd(content, exchangeRate);
                    return result;
                }
            }
        }

        public async Task<decimal> ConversionRateAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            string content = "";
            string url = string.Format(urlPattern, fromCurrency, toCurrency);
            Decimal result = 0;
            decimal exchangeRate = 0;
            if (CheckForInternetConnection() == false)
            {
                result = amount * decimal.Parse("1.11");
                return result;
            }

            if (cachedDownloads.TryGetValue(content, out exchangeRate))
            {
                result = (amount * exchangeRate);
                return exchangeRate;

            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(url).ConfigureAwait(false);

                    // Now parse with JSON.Net

                    Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                    exchangeRate = (decimal)token.SelectToken("rate");
                    cachedDownloads.TryAdd(content, exchangeRate);
                    return exchangeRate;
                }
            }
        }


        //Basic Conversion Async
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                decimal test2 = decimal.Parse(number.Text);
                var result = CurrencyConversionAsync(test2, "USD", "EUR");
                var conversion = ConversionRateAsync(test2, "USD", "EUR");
                answer.Text = result.Result.ToString("C2", CultureInfo.CreateSpecificCulture("eu-EUR"));
                var exchange = "Hello, today rate is " + conversion.Result.ToString("C2", CultureInfo.CreateSpecificCulture("eu-EUR")); ;
                CompImg.Text = exchange;

                async void useit()
                {

                    await CompImg.TranslateTo(125, 0, 800, Easing.SpringIn);// Move to down Element  
                    await CompImg.ScaleTo(2, 2000, Easing.CubicIn);          // Resize the Element  
                    await CompImg.FadeTo(0, 1000);                           //Opacity Change to 0 meanse element fully Hide     
                    await CompImg.FadeTo(1, 1000);                           //Opacity Change to 1 meanse element fully Appear  
                    await CompImg.RotateTo(360, 2000, Easing.SinInOut);      //Element Rotate to 360 degree  
                    await CompImg.ScaleTo(1, 2000, Easing.CubicOut);         //Again Resize the Element  
                    await CompImg.TranslateTo(0, 0, 2000, Easing.BounceOut); // Move to up the Element  
                }
                useit();
            }
            catch (System.FormatException)
            {
                CompImg.Text = "Hello, today rate is 1.11";
                answer.Text = "0"; // or other default value as appropriate in context.
            }

        }

    }
}