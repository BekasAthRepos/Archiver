using Archiver.Model;
using Archiver.ViewModel;
using System;
using System.Collections.Generic;
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

        public AddItemPage(int albumId)
        {
            InitializeComponent();
            addItemViewModel.SetAlbumId(albumId);    
        }
    }
}