using Archiver.Model;
using Archiver.View;
using Archiver.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver
{
    public partial class MainPage : ContentPage
    {
        private readonly AlbumViewModel vmAlbum; 
        private readonly ObservableCollection<Album> albums;
        
        public MainPage()
        {
            InitializeComponent();
            vmAlbum = new AlbumViewModel();
            BindingContext = vmAlbum;
            albums = lvAlbums.ItemsSource as ObservableCollection<Album>; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vmAlbum.OnAppearing();
        }

        private async void AddAlbumClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAlbumPage(false));
        }

        private async void AlbumClicked(object sender, ItemTappedEventArgs args)
        {
            if(args.Item == null)
                return;          

            await Navigation.PushAsync(new AlbumDetails(args.Item as Album, false));
        }

        private async void EditItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Album album = mi.CommandParameter as Album;

            await Navigation.PushAsync(new EditAlbumPage(album, false)); 
        }

        private async void DeleteItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Album album = mi.CommandParameter as Album;

            DeleteAlbumViewModel dVm = new DeleteAlbumViewModel(album.Id, false);
            await dVm.DeleteAlbum();
            vmAlbum.OnAppearing();
        }

        private async void OnHelpClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new HelpPage());
        }

        private async void OnInfoClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new InfoPage());
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && e.NewTextValue != "")
            {         
                var newList = albums.Where(a => a.Name.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase));
                lvAlbums.ItemsSource = newList;
            }
            else
            {
                lvAlbums.ItemsSource = albums;
            }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            vmAlbum.OnAppearing();
        }

        private async void ShowAd(object sender, EventArgs args)
        {
            
        }
    }
}
