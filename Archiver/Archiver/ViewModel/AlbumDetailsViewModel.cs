using Archiver.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Archiver.ViewModel
{
    public class AlbumDetailsViewModel: INotifyPropertyChanged
    {
        private Album _album;
        private bool _isSync;
        private ResourceManager _res;
        public Album Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged(nameof(Album));
            }
        }
        public ObservableCollection<Item> Items { get; set; }
        private int _itemQty;
        public int ItemQty 
        {
            get { return _itemQty; }
            set
            {
                _itemQty = value;
                OnPropertyChanged(nameof(ItemQty));
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AlbumDetailsViewModel(Album album, bool isSync) 
        {   
            _album = album;
            _isSync = isSync;
            Items = new ObservableCollection<Item>();
            _res = new ResourceManager("Archiver.Resources.Strings", typeof(AlbumDetailsViewModel).Assembly);

            MessagingCenter.Subscribe<Object, DateTime>(this, "AlbumChanged", (o, date) =>
            {
                Album.UpdateDate = date;
                OnPropertyChanged(nameof(Album));
            });

            MessagingCenter.Subscribe<Object>(this, "ItemDeleted", (o) =>
            {
                ItemQty--;
                OnPropertyChanged(nameof(ItemQty));
            });

            Task.Run(async () => await LoadItems());
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadItems()
        {
            if (!_isSync)
                await ExcLoadItemsCmd();
            else
                await SyncGetAllItems();
            ItemQty = GetItemQty();
        }

        private int GetItemQty()
        {
            return Items.Count;
        }

        private async Task ExcLoadItemsCmd()
        {
            Items.Clear();
            var itemList = await App.Database.GetItemsAsync(Album.Id);
            foreach (var item in itemList)
            {
                item.ImageSource = item.ImgPath;
                Items.Add(item);
            }
        }

        private async Task SyncGetAllItems()
        {
            try
            {
                string endpoint = _res.GetString("APIBaseUrl").ToString() + _res.GetString("GetAllItems").ToString();
                string url = endpoint + $"/{_album.Id}";
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    var items = JsonConvert.DeserializeObject<List<Item>>(result);
                    
                    Items.Clear();
                    foreach (var item in items)
                    {
                        if (item.ImageB64 != null)
                        {
                            byte[] ImgByte;
                            ImgByte = Convert.FromBase64String(item.ImageB64);
                            item.ImageSource = ImageSource.FromStream(() => new MemoryStream(ImgByte));
                        }
                        Items.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }
    }
}
