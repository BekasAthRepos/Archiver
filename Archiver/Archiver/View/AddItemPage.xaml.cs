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

        private async void OnUploadCliked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a picture"
            });

            if(result != null)
            {
                var stream = await result.OpenReadAsync();
                imgMain.Source = ImageSource.FromStream(() => stream);
            }   
        }

        private async void OnCameraCliked(object sender, EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();
            
            if(result != null)
            {
                var camera = await result.OpenReadAsync();
                imgMain.Source = ImageSource.FromStream(() => camera);
            }
        }
    }
}