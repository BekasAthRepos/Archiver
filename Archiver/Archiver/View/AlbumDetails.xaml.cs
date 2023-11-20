using Archiver.Model;
using Archiver.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumDetails : ContentPage
    {
        private AlbumDetailsViewModel vmAlbDet;
        private readonly ObservableCollection<Item> items;
        private readonly bool _isSync;

        public AlbumDetails(Album album, bool isSync)
        {
            _isSync = isSync;
            vmAlbDet = new AlbumDetailsViewModel(album, _isSync);         
            BindingContext = vmAlbDet;
            InitializeComponent();
        }

        private async void AddItemClicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            int albumId = (int) btn.CommandParameter;

            await Navigation.PushAsync(new AddItemPage(albumId, _isSync, vmAlbDet.Items));
        }

        private async void ItemClicked(object sender, ItemTappedEventArgs args)
        {
            if (args.Item == null)
                return;

            await Navigation.PushAsync(new ItemDetails(args.Item as Item, _isSync));
        }

        private async void EditItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            await Navigation.PushAsync(new EditItemPage(item, _isSync));
        }

        private async void DeleteItemClicked(object sender, EventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            Item item = mi.CommandParameter as Item;

            DeleteItemViewModel dVm = new DeleteItemViewModel(item.Id, item.AlbumId, _isSync);

            int d = await dVm.DeleteItem();
            if (d > 0)
               vmAlbDet.Items.Remove(item);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && e.NewTextValue != "")
            {
                var newList = items.Where(a => a.Name.StartsWith(e.NewTextValue, StringComparison.InvariantCultureIgnoreCase));
                lvitems.ItemsSource = newList;
            }
            else
            {
                lvitems.ItemsSource = items;
            }
        }
    }
}