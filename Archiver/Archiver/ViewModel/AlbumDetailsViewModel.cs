using Archiver.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AlbumDetailsViewModel
    {
        public Album Album { get; set; }
        public int ItemQty { get; set; }
        public ICommand GetItemQtyCmd => new Command(GetItemQty);

        public AlbumDetailsViewModel() { }

        public AlbumDetailsViewModel(Album album)
        {
            Album = album.Clone();
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
    }
}
