using Archiver.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAlbumPage : ContentPage
    {
        private AddAlbumViewModel viewModel;
        public AddAlbumPage(bool IsSync)
        {
            viewModel = new AddAlbumViewModel(IsSync);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}