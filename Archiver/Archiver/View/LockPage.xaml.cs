using Archiver.Model;
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
	public partial class LockPage : ContentPage
	{
		private int passTries;
		private const int MaxTries = 3;
		public LockPage ()
		{
			InitializeComponent ();
			passTries = 0;
		}

		private async void LogInClicked(object sender, EventArgs args)
        {
			btnLogIn.IsEnabled = false;
			string attempt = txtPass.Text;
			if (CheckPassword(attempt))
			{
				Sys_ini data = new Sys_ini("LOCKED", "FALSE");
				int rows = await App.Database.SetIniValueAsync(data);
				if (rows > 0)
					Application.Current.MainPage = new NavigationPage(new MainPage());
				else
					await App.Current.MainPage.DisplayAlert("Error", "Something went wong", "OK");
			}
			else
			{
				passTries++;
				int times = MaxTries - passTries;
				if (passTries < 3)
				{
					btnLogIn.IsEnabled = true;
					await App.Current.MainPage.DisplayAlert("Warning", times.ToString() + " time(s) left", "OK");
				}
				else
				{
					await App.Current.MainPage.DisplayAlert("Block", "App has been locked", "OK");
				}
			}
        }

		private bool CheckPassword(string pass)
        {
			if (pass.Length < 9) { return false; }

			DateTime date = DateTime.Now;
			int day = date.Day;
			int month = date.Month;
			int digit1;
			int digit2;

			// get sum of day
			if (day / 10 >= 1)
			{
				int d1 = (int)(day / 10);
				int d2 = (int)(day % 10);
				digit1 = d1 + d2;
			}
			else
				digit1 = day;

			// get sum of month
			if (month / 10 >= 1)
			{
				int d1 = (int)(month / 10);
				int d2 = (int)(month % 10);
				digit2 = d1 + d2;
			}
			else
				digit2 = month;

			//check password 
			char a = digit1.ToString()[0];
			char b = digit2.ToString()[0];
            if ((pass[1] == a) && (pass[7] == b))
			{ return true; } else { return false; }
        }
	}
}