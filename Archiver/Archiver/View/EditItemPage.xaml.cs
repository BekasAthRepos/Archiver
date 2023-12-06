using Archiver.Model;
using Archiver.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditItemPage : ContentPage
    {
        private EditItemViewModel viewModel;

        public EditItemPage(Item item, bool isSync)
        {
            viewModel = new EditItemViewModel(item, isSync);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}