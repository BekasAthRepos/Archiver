using Archiver.Model;
using Archiver.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAlbumPage : ContentPage
    {
        private EditAlbumViewModel viewModel;

        public EditAlbumPage(Album album, bool IsSync)
        {
            viewModel = new EditAlbumViewModel(album, IsSync);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}