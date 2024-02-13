using Archiver.Model;
using Archiver.View;
using Archiver.ViewModel;
using MarcTron.Plugin;
using MarcTron.Plugin.Controls;
using MarcTron.Plugin.Extra;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
            CrossMTAdmob.Current.LoadRewarded("ca-app-pub-3940256099942544/5224354917");
            CrossMTAdmob.Current.LoadRewardedInterstitial("ca-app-pub-3940256099942544/5354046379");
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

        private void ShowInterstitial(object sender, EventArgs args)
        {
            CrossMTAdmob.Current.ShowInterstitial();
            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
        }

        private void ShowRewarded(object sender, EventArgs args)
        {
            CrossMTAdmob.Current.ShowRewarded();
            CrossMTAdmob.Current.LoadRewarded("ca-app-pub-3940256099942544/5224354917");
        }

        private void ShowRewardedInterstitial(object sender, EventArgs args)
        {
            CrossMTAdmob.Current.ShowRewardedInterstitial();
            CrossMTAdmob.Current.LoadRewardedInterstitial("ca-app-pub-3940256099942544/5354046379");
        }

        private void LoadAd(object sender, EventArgs args)
        {
            //banner.LoadAd();
        }
    }
}
