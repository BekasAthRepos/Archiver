using Archiver.Model;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        private Item _item;
        private ImageSource _imgSrc;
        private ResourceManager rm;
        private bool _isSync;

        public ICommand CancelImageCmd => new Command(CancelImage);
        public ICommand UploadImageCmd => new Command(UploadImage);
        public ICommand TakePhotoCmd => new Command(TakePhoto);
        public event PropertyChangedEventHandler PropertyChanged;

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public ICommand SaveClickedCmd => new Command(SaveClicked);

        public EditItemViewModel(Item item, bool isSync) 
        {
            _item = item;
            _isSync = isSync; 
            rm = new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SaveClicked()
        {
            try
            {
                DateTime date = DateTime.Now;
                _item.UpdateItem();
                int rows = await App.Database.UpdateItemAsync(Item);
                rows += await App.Database.UpdateAlbumDateAsync(_item.AlbumId, date);
                if (rows >= 2)
                {
                    await App.Current.MainPage.DisplayToastAsync("Success. Changes have been saved", 1500);
                    MessagingCenter.Send<Object, DateTime>(this, "AlbumChanged", date);
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }

            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void CancelImage()
        {
            Item.ImgPath = rm.GetString("addItemDefaultImage");
            OnPropertyChanged(nameof(Item));
        }

        private async void UploadImage()
        {   /*
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a picture"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                _imgSrc = ImageSource.FromStream(() => stream);
                Item.ImgPath = result.FullPath;
                OnPropertyChanged(nameof(Item));
            }   */

            var result = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });

            if (result != null)
            {                
                _imgSrc = ImageSource.FromStream(() => result.GetStream());
                Item.ImgPath = result.Path;
                OnPropertyChanged(nameof(Item));

            }
        }

        private async void TakePhoto()
        {   /*
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var camera = await result.OpenReadAsync();
                _imgSrc = ImageSource.FromStream(() => camera);

                var newImage = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                var stream = await result.OpenReadAsync();
                var newStream = File.OpenWrite(newImage);
                await stream.CopyToAsync(newStream);
                Item.ImgPath = Path.GetFullPath(newImage);
                OnPropertyChanged(nameof(Item));
            } */

            var cameraMediaOptions = new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Rear,

                // Set the value to true if you want to save the photo to your public storage.
                SaveToAlbum = false,

                // Give the name of the folder you want to save to
                //Directory = null,

                // Give a photo name of your choice,
                // or set it to null if you want to use the default naming convention
                //Name = null,

                // Set the compression quality
                // 0 = Maximum compression but worse quality
                // 100 = Minimum compression but best quality
                CompressionQuality = 100
            };
            MediaFile result = await CrossMedia.Current.TakePhotoAsync(cameraMediaOptions);
            if (result == null) return;
            _imgSrc = ImageSource.FromStream(() => result.GetStream());

            var newImage = Path.Combine(FileSystem.AppDataDirectory, result.Path);
            Item.ImgPath = Path.GetFullPath(newImage);
            OnPropertyChanged(nameof(Item));
        }
    }
}
