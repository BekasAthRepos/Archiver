using Archiver.Model;
using Archiver.ViewModel;
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
    }
}