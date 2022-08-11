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
        private AlbumDetailsViewModel vmAlbDet;
        public AlbumDetails(Album album)
        {
            InitializeComponent();
            vmAlbDet = albumDetailsViewModel;          
            vmAlbDet.Album = album;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vmAlbDet.OnAppearing();
        }

        private async void AddItemClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int albumId = (int) btn.CommandParameter;

             await Navigation.PushAsync(new AddItemPage(albumId));
        }

        private async void ItemClicked(object sender, ItemTappedEventArgs args)
        {
            if (args.Item == null)
                return;

            await Navigation.PushAsync(new ItemDetails(args.Item as Item));
        }

        private async void EditItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            await Navigation.PushAsync(new EditItemPage(item));
        }

        private async void DeleteItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            DeleteItemViewModel dVm = new DeleteItemViewModel(item.Id, item.AlbumId);

            int d = await dVm.DeleteItem();
            if (d > 0)
               vmAlbDet.Items.Remove(item);
        }
    }
}