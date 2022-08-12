using Archiver.Model;
using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AddAlbumViewModel
    {
        public Album NewAlbum {get; set;}
        public ICommand AddAlbumCmd => new Command(AddAlbum);

        public AddAlbumViewModel() 
        {
            NewAlbum = new Album(); 
        }

        private void AddAlbum()
        {
            bool validInputData = true;

            if((string.IsNullOrWhiteSpace(NewAlbum.Name)))
            {
                validInputData = false;
                App.Current.MainPage.DisplayAlert("Give a name...", "", "ok");
            }

            if (validInputData)
            {
                try
                {
                    int rows = App.Database.InsertAlbum(NewAlbum);
                    if (rows > 0)
                        App.Current.MainPage.DisplayToastAsync("Success. Album has been added.", 1500);
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
                }
                
                App.Current.MainPage.Navigation.PopAsync();
            }
        }

    }
}
