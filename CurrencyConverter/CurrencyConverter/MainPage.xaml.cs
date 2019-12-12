using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CurrencyConverter
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // when the button is clicked open new navigation page Morecurrency
        private void Button_Clicked_4(object sender, EventArgs e)
        {
           Navigation.PushAsync(new MoreCurrency());
        }

        // when the button is clicked open new navigation page EuroToPounds
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EuroToPounds());

        }

        // when the button is clicked open new navigation page PoundsToEuro
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PoundsToEuro());
        }

        // when the button is clicked open new navigation page usdToEuro
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new usdToEuro());
        }

        // when the button is clicked open new navigation page EuroToUsd
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EuroToUsd());
        }
    }
}
