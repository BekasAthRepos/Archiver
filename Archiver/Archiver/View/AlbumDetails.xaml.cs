using Archiver.Model;
using Archiver.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumDetails : ContentPage
    {
        public AlbumDetails(Album album)
        {
            InitializeComponent();
            BindingContext = new AlbumDetailsViewModel(album);
        }

        private async void ItemClicked(object sender, ItemTappedEventArgs args)
        {
            if (args.Item == null)
                return;

            //await Navigation.PushAsync(new ItemDetails(args.Item as Item));
        }

        private async void EditItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            //await Navigation.PushAsync(new EditItemPage(item));
        }

        private async void DeleteItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            DeleteAlbumViewModel dVm = new DeleteAlbumViewModel(item.Id);

            int d = await dVm.DeleteAlbum();
            //if (d > 0)
              //  vmAlbum.Albums.Remove(Item);
        }
    }
}