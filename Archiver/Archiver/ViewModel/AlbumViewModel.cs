using Archiver.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshAlbumsCmd => new Command(RefreshAlbums);
        public ObservableCollection<Album> Albums { get; set; }
        public bool IsSync
        {
            get { return _isSync; }
            set
            {
                _isSync = value;
                OnPropertyChanged(nameof(IsSync));
            }
        }

        public bool IsRefreshing
        {
            get { return _isRef; }
            set
            {
                _isRef = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ResourceManager _res;
        private bool _isSync;
        private bool _isRef;

        public AlbumViewModel()
        {   
            Albums = new ObservableCollection<Album>();
            IsSync = false;

            _res= new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
        }

        public async void OnAppearing()
        {
            if (IsSync)
                await SyncGetAllAlbums();
            else 
                await ExcLoadAlbumsCmd();
        }

        private async Task ExcLoadAlbumsCmd()
        {
            Albums.Clear();
            var albumList = await App.Database.GetAlbumsAsync();
            foreach (var album in albumList)
                Albums.Add(album);
        }

        private async Task SyncGetAllAlbums()
        {
            try
            {
                string _getAlbumsUrl = _res.GetString("APIBaseUrl").ToString() + _res.GetString("GetAllAlbums").ToString();
                using (HttpClient client = new HttpClient())
                {
                    var resultJson = await client.GetStringAsync(_getAlbumsUrl);
                    var albums = JsonConvert.DeserializeObject<List<Album>>(resultJson);
                    Albums.Clear();
                    foreach (var album in albums)
                        Albums.Add(album);
                }
            }catch (Exception e) 
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }

        private async void RefreshAlbums()
        {
            IsRefreshing = true;
            if (IsSync)
                await SyncGetAllAlbums();
            else
                await ExcLoadAlbumsCmd();
            IsRefreshing = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
