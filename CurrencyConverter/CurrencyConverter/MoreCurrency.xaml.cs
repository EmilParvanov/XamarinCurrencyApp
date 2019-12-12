using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Util;
using Android.Views;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CurrencyConverter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoreCurrency : ContentPage
	{
		public MoreCurrency ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            void addValueToFromPicker(){
                fromPicker.Items.Add("USD");
                fromPicker.Items.Add("EUR");
                fromPicker.Items.Add("JPY");
                fromPicker.Items.Add("BGN");
                fromPicker.Items.Add("CZK");
                fromPicker.Items.Add("DKK");
                fromPicker.Items.Add("GBP");
                fromPicker.Items.Add("HUF");
                fromPicker.Items.Add("PLN");
                fromPicker.Items.Add("RON");
                fromPicker.Items.Add("SEK");
                fromPicker.Items.Add("CHF");
                fromPicker.Items.Add("ISK");
                fromPicker.Items.Add("NOK");
                fromPicker.Items.Add("HRK");
                fromPicker.Items.Add("RUB");
                fromPicker.Items.Add("TRY");
                fromPicker.Items.Add("AUD");
                fromPicker.Items.Add("BRL");
                fromPicker.Items.Add("CAD");
                fromPicker.Items.Add("CNY");
                fromPicker.Items.Add("HKD");
                fromPicker.Items.Add("IDR");
                fromPicker.Items.Add("ILS");
                fromPicker.Items.Add("INR");
                fromPicker.Items.Add("KRW");
                fromPicker.Items.Add("MXN");
                fromPicker.Items.Add("MYR");
                fromPicker.Items.Add("NZD");
                fromPicker.Items.Add("PHP");
                fromPicker.Items.Add("SGD");
                fromPicker.Items.Add("THB");
                fromPicker.Items.Add("ZAR");
            }
            void addValueToPicker()
            {
                toPicker.Items.Add("USD");
                toPicker.Items.Add("EUR");
                toPicker.Items.Add("JPY");
                toPicker.Items.Add("BGN");
                toPicker.Items.Add("CZK");
                toPicker.Items.Add("DKK");
                toPicker.Items.Add("GBP");
                toPicker.Items.Add("HUF");
                toPicker.Items.Add("PLN");
                toPicker.Items.Add("RON");
                toPicker.Items.Add("SEK");
                toPicker.Items.Add("CHF");
                toPicker.Items.Add("ISK");
                toPicker.Items.Add("NOK");
                toPicker.Items.Add("HRK");
                toPicker.Items.Add("RUB");
                toPicker.Items.Add("TRY");
                toPicker.Items.Add("AUD");
                toPicker.Items.Add("BRL");
                toPicker.Items.Add("CAD");
                toPicker.Items.Add("CNY");
                toPicker.Items.Add("HKD");
                toPicker.Items.Add("IDR");
                toPicker.Items.Add("ILS");
                toPicker.Items.Add("INR");
                toPicker.Items.Add("KRW");
                toPicker.Items.Add("MXN");
                toPicker.Items.Add("MYR");
                toPicker.Items.Add("NZD");
                toPicker.Items.Add("PHP");
                toPicker.Items.Add("SGD");
                toPicker.Items.Add("THB");
                toPicker.Items.Add("ZAR");
            }
            addValueToFromPicker();
            addValueToPicker();

        }


        //creating few varibles for from to picker
        String from, to;

        // from picker used for getting the value from the "from" picker box
        private void FromPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
           from = fromPicker.Items[fromPicker.SelectedIndex];
            cachedDownloads.TryRemove(content, out exchangeRate);
            number.Placeholder = "Please enter " + from;
        }
        // to picker used for getting the value from the to picker box
        private void ToPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            to = toPicker.Items[toPicker.SelectedIndex];
            cachedDownloads.TryRemove(content, out exchangeRate);
        }

        //checking if the device have a internet
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

        string content = "";
        decimal exchangeRate = 0;
        static ConcurrentDictionary<string, decimal> cachedDownloads =
        new ConcurrentDictionary<string, decimal>();
        private const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        //async method for calculating from currency a to currency b in real time 
        public async Task<decimal> CurrencyConversionAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            
            string url = string.Format(urlPattern, fromCurrency, toCurrency);
            Decimal result = 0;
            
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

        //async method for calculating the rate
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


        //when text change in the input box its calling that method
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
               
                decimal test2 = decimal.Parse(number.Text);
                if (to == null)
                    to = "EUR";
                if (from == null)
                    from = "EUR";
                var result = CurrencyConversionAsync(test2, from, to);
                var conversion = ConversionRateAsync(test2, from, to);
 
                string firstTwoChar = new string(to.Take(2).ToArray());
                // checking if the firstwocharacter is gb and changing it to en so the cultureinfo symbol would work
                if (firstTwoChar == "GB") { 
                    firstTwoChar = "en";
                    to = "GB";
                }
                if (to == "USD")
                {
                    firstTwoChar = "en";

                }
                answer.Text = result.Result.ToString("C2", CultureInfo.CreateSpecificCulture(firstTwoChar + "-" + to));
                var exchange = "Hello, today rate is " + conversion.Result.ToString("C3", CultureInfo.CreateSpecificCulture(firstTwoChar + "-" + to)); ;
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
                CompImg.Text = "Hello, today rate is ";
                answer.Text = "0"; // or other default value as appropriate in context.
            }

        }

    }
}