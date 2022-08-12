using Archiver.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        private Item _item;
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

        public EditItemViewModel() { }

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
    }
}
