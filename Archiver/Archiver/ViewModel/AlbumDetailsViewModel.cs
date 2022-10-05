using Archiver.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AlbumDetailsViewModel: INotifyPropertyChanged
    {
        private Album _album;
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

        public AlbumDetailsViewModel() 
        {   
            Items = new ObservableCollection<Item>();
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
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OnAppearing()
        {
            await ExcLoadItemsCmd();
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
                Items.Add(item);
            }
        }
    }
}
