using Archiver.Model;
using Archiver.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AddItemViewModel
    {
        public Item NewItem { get; set; }
        public ICommand AddItemCmd => new Command(AddItem);

        public AddItemViewModel() 
        {
            NewItem = new Item();
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

                    if(rows == 2)
                    {
                        App.Current.MainPage.DisplayAlert("Success", "Item has been added.", "Ok");
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
    }
}
