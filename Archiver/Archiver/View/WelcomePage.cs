using Archiver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Archiver.View
{
    public class WelcomePage : ContentPage
    {
        private Image welcomeImg; 

        public WelcomePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var lo = new AbsoluteLayout();
            welcomeImg = new Image
            {
                Source = "logo.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            AbsoluteLayout.SetLayoutFlags(welcomeImg, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(welcomeImg, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            lo.Children.Add(welcomeImg);

            this.BackgroundColor = Color.FromHex("#EBEEFF");
            this.Content = lo;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await welcomeImg.ScaleTo(0.8, 1000, Easing.Linear);
            await welcomeImg.ScaleTo(3, 1000, Easing.Linear);

            // check if app is locked
            Sys_ini isLocked = await App.Database.GetIniValueAsync("LOCKED");
            
            if (isLocked != null)
            {
                if (isLocked.Value.Equals("TRUE"))
                    Application.Current.MainPage = new NavigationPage(new LockPage());
                else
                    Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong...", "OK");
            }
            
        }
    }
}