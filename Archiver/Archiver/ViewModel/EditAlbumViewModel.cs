using Archiver.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditAlbumViewModel: INotifyPropertyChanged
    {
        private Album _album;
        public event PropertyChangedEventHandler PropertyChanged;
        public Album Album 
        {   
            get { return _album; }   
            set 
            {
                _album = value;
                OnPropertyChanged(nameof(Album));
            }
        }
        public ICommand SaveClickedCmd => new Command(SaveClicked);

        public EditAlbumViewModel() { }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SaveClicked()
        {
            try
            {
                Album.UpdateAlbum();
                int rows = App.Database.UpdateAlbum(Album);
                if(rows > 0)
                {
                    await App.Current.MainPage.DisplayToastAsync("Success. Changes have been saved", 1500);
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
