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

namespace Archiver.ViewModel
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private ImageSource _imgSrc;
        private string _imgPath;
        public Item NewItem { get; set; }
        public ICommand AddItemCmd => new Command(AddItem);
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
            NewItem = new Item();
            ImgSrc = "image.png";
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

        private async void UploadImage()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a picture"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                ImgSrc = ImageSource.FromStream(() => stream);
                //_imgPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            }         
        }

        private async void TakePhoto()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var camera = await result.OpenReadAsync();
                ImgSrc = ImageSource.FromStream(() => camera);

                var newImage = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                using (var stream = await result.OpenReadAsync())
                using (var newStream = File.OpenWrite(newImage))
                    await stream.CopyToAsync(newStream);
                _imgPath = newImage;
            }
        }
    }
}
