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
        public ObservableCollection<Item> Items { get; set; }
        public int ItemQty { get; set; }
        public ICommand GetItemQtyCmd => new Command(GetItemQty);
        private Command LoadItemsCmd;

        public event PropertyChangedEventHandler PropertyChanged;
        public Album Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged("Album");
            }
        }

        public AlbumDetailsViewModel() 
        {       
            Items = new ObservableCollection<Item>();
            LoadItemsCmd = new Command(async () => await ExcLoadItemsCmd());
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OnAppearing()
        {
            await ExcLoadItemsCmd();
        }

        private void GetItemQty()
        {
            try
            {
                ItemQty = App.Database.GetItemQty(Album.Id);
            }
            catch (Exception e)
            {
                App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }

        private Task ExcLoadItemsCmd()
        {
            Items.Clear();

            var itemList = App.Database.GetItems(Album.Id);
            foreach (var item in itemList)
            {
                Items.Add(item);
            }

            return Task.CompletedTask;
        }
    }
}
