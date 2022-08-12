using Archiver.Model;
using Archiver.View;
using Archiver.ViewModel;
using System;
using Xamarin.Forms;

namespace Archiver
{
    public partial class MainPage : ContentPage
    {
        private readonly AlbumViewModel vmAlbum; 
        
        public MainPage()
        {
            InitializeComponent();
            vmAlbum = new AlbumViewModel();
            BindingContext = vmAlbum;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vmAlbum.OnAppearing();
        }

        private async void AddAlbumClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAlbumPage());
        }

        private async void AlbumClicked(object sender, ItemTappedEventArgs args)
        {
            if(args.Item == null)
                return;          

            await Navigation.PushAsync(new AlbumDetails(args.Item as Album));
        }

        private async void EditItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Album album = mi.CommandParameter as Album;

            await Navigation.PushAsync(new EditAlbumPage(album)); 
        }

        private async void DeleteItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Album album = mi.CommandParameter as Album;

            DeleteAlbumViewModel dVm = new DeleteAlbumViewModel(album.Id);

            int d = await dVm.DeleteAlbum();
            if (d > 0)
                vmAlbum.Albums.Remove(album);
        }
    }
}
