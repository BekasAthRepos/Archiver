using Archiver.Model;
using Archiver.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {
        private AddItemViewModel viewModel;
        private bool _isSync;
        private int _albumId;

        public AddItemPage(int albumId, bool isSync)
        {
            _isSync = isSync;
            _albumId = albumId;
            viewModel = new AddItemViewModel(albumId, isSync);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}