using Archiver.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditAlbumViewModel
    {
        public Album Album { get; }
        public ICommand SaveClickedCmd => new Command(SaveClicked);

        public EditAlbumViewModel() { }

        public EditAlbumViewModel(Album album)
        {
            Album = album;
        }

        private async void SaveClicked()
        {
            try
            {
                int rows = App.Database.UpdateAlbum(Album);
                if(rows > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Changes were saved", "Ok");
                }
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
                      
            await App.Current.MainPage.Navigation.PopAsync();
        }   
    }
}
