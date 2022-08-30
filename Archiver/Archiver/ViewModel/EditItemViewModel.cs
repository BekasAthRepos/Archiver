using Archiver.Model;
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

        public EditItemViewModel() 
        {
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
                int rows = App.Database.UpdateItem(Item);
                rows += App.Database.UpdateAlbumDate(_item.AlbumId, date);
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
        {
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
            }
        }

        private async void TakePhoto()
        {
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
            }
        }
    }
}
