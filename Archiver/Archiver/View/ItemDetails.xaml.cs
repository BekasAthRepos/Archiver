using Archiver.Model;
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

        public ItemDetails(Item item)
        {
            InitializeComponent();
            itemDetailsViewModel.Item = item; 
        }
    }
}