using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Footer : ContentView
    {
        public Footer()
        {
            InitializeComponent();
            footerText.Text = "© " + DateTime.Now.Year + " Bekas Athanasios. All rights reserved";
        }

        private async void OnHelpClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new HelpPage());
        }

        private async void OnInfoClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new InfoPage());
        }
    }
}