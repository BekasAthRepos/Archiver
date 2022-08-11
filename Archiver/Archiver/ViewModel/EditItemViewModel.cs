using Archiver.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class EditItemViewModel
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
                //OnPropertyChanged("Album");
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
                _item.UpdateItem();
                //update album updatetime...
                int rows = App.Database.UpdateItem(Item);
                if (rows > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Changes were saved", "Ok");
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
