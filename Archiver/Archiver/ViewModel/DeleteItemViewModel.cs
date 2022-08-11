using Archiver.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class DeleteItemViewModel
    {

        private int _ItemId;
        private int _albumId;

        public DeleteItemViewModel(int ItemId, int albumId)
        {
            _ItemId = ItemId;
            _albumId = albumId;
        }

        public async Task<int> DeleteItem()
        {
            int recs = 0;
            try
            {
                bool ans = await App.Current.MainPage.DisplayAlert("Warning!", "Delete item?", "Yes", "No");
                if (ans)
                {
                    DateTime date = DateTime.Now;
                    recs = App.Database.DeleteItem(_ItemId);
                    recs += App.Database.UpdateAlbumDate(_albumId, date);
                    if (recs > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Item was deleted", "Ok");
                        MessagingCenter.Send<Object, DateTime>(this, "AlbumChanged", date);
                    }
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }

            return recs;
        }
    }
}
