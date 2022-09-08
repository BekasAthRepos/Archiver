using Archiver.Model;
using Archiver.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Resources;
using System.Reflection;
using Plugin.Media.Abstractions;
using Plugin.Media;
using static SQLite.SQLite3;
using static Xamarin.Essentials.Permissions;

namespace Archiver.ViewModel
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private ImageSource _imgSrc;
        private ResourceManager rm;

        public Item NewItem { get; set; }
        public ICommand AddItemCmd => new Command(AddItem);
        public ICommand CancelImageCmd => new Command(CancelImage);
        public ICommand UploadImageCmd => new Command(UploadImage);
        public ICommand TakePhotoCmd => new Command(TakePhoto);
        public event PropertyChangedEventHandler PropertyChanged;
        

        public ImageSource ImgSrc
        {
            get { return _imgSrc; }
            set 
            {
                _imgSrc = value;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }

        public AddItemViewModel() 
        {
            rm = new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
            string img = rm.GetString("addItemDefaultImage");

            NewItem = new Item();
            ImgSrc = img;
            NewItem.ImgPath = Path.GetFullPath(img);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetAlbumId(int albumId)
        {
            NewItem.AlbumId = albumId;
        }

        private void AddItem()
        {
            bool validInputData = true;

            if(NewItem.AlbumId <= 0)
            {
                validInputData = false;
                App.Current.MainPage.DisplayAlert("Error with Album Id", "", "ok");
            }

            if ((string.IsNullOrWhiteSpace(NewItem.Name)))
            {
                validInputData = false;
                App.Current.MainPage.DisplayAlert("Give a name...", "", "ok");
            }

            if (validInputData)
            {
                try
                {
                    DateTime date = DateTime.Now;
                    int rows = App.Database.UpdateAlbumDate(NewItem.AlbumId, date);
                    rows += App.Database.InsertItem(NewItem);

                    if (rows == 2)
                    {
                        App.Current.MainPage.DisplayToastAsync("Success. Item has been added.", 1500);
                        MessagingCenter.Send<Object, DateTime>(this, "AlbumChanged", date);
                    }
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
                }

                App.Current.MainPage.Navigation.PopAsync();
            }
        }

        private void CancelImage()
        {
            ImgSrc = rm.GetString("addItemDefaultImage");
            NewItem.ImgPath = rm.GetString("addItemDefaultImage");
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
                ImgSrc = ImageSource.FromStream(() => stream);
                NewItem.ImgPath = result.FullPath;
            } */


            var result = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });

            if (result != null)
            {
                _imgSrc = ImageSource.FromStream(() => result.GetStream());
                NewItem.ImgPath = result.Path;
                OnPropertyChanged(nameof(ImgSrc));
            }
        }

        private async void TakePhoto()
        { /*
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var camera = await result.OpenReadAsync();
                ImgSrc = ImageSource.FromStream(() => camera);

                var newImage = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                var stream = await result.OpenReadAsync();
                var newStream = File.OpenWrite(newImage);
                    await stream.CopyToAsync(newStream);
                NewItem.ImgPath = Path.GetFullPath(newImage);
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
            ImgSrc = ImageSource.FromStream(() => result.GetStream());

            var newImage = Path.Combine(FileSystem.AppDataDirectory, result.Path);
            NewItem.ImgPath = Path.GetFullPath(newImage);
            OnPropertyChanged(nameof(ImgSrc));
        }
    }
}
