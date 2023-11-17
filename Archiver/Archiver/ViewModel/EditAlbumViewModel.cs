using Archiver.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditAlbumViewModel: INotifyPropertyChanged
    {
        private Album _album;
        private bool _isSync;
        private readonly ResourceManager _res;
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

        public EditAlbumViewModel(Album album, bool IsSync) 
        {
            Album = album;
            _isSync = IsSync;
            _res = new ResourceManager("Archiver.Resources.Strings", typeof(EditAlbumViewModel).Assembly);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SaveClicked()
        {
            try
            {
                Album.UpdateAlbum();
                if (!_isSync)
                {
                    int rows = await App.Database.UpdateAlbumAsync(Album);
                    if(rows > 0)
                        await App.Current.MainPage.DisplayToastAsync("Success. Changes have been saved", 1500);
                }
                else
                {
                    var client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(Album);
                    StringContent contnet = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    string endpoint = _res.GetString("APIBaseUrl").ToString() + _res.GetString("UpdateAlbum").ToString();
                    HttpResponseMessage response = await client.PutAsync(endpoint, contnet);
                    if (response.IsSuccessStatusCode)
                        await App.Current.MainPage.DisplayToastAsync("Success. Album has been synchronized.", 1500);
                    else
                        await App.Current.MainPage.DisplayAlert("Error", response.ToString(), "Ok");
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
