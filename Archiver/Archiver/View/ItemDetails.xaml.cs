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
    public partial class ItemDetails : ContentPage
    {
        private ItemDetailsViewModel viewModel;
        private bool _isSync;
        public ItemDetails(Item item, bool isSync)
        {
            _isSync = isSync;
            viewModel = new ItemDetailsViewModel(item, isSync);
            BindingContext = item;
            InitializeComponent();
        }
    }
}