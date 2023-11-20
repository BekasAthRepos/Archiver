using Archiver.Model;
using Archiver.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditItemPage : ContentPage
    {
        private EditItemViewModel viewModel;
        private bool _isSync;
        public EditItemPage(Item item, bool isSync)
        {
            _isSync = isSync;
            viewModel = new EditItemViewModel(item, isSync);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}