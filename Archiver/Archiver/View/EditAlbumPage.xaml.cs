using Archiver.Model;
using Archiver.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAlbumPage : ContentPage
    {
        public EditAlbumPage(Album album)
        {
            InitializeComponent();
            BindingContext = new EditAlbumViewModel(album);
        }
    }
}