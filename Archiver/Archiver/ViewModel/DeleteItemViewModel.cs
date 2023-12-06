using Archiver.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class DeleteItemViewModel
    {

        private int _ItemId;
        private int _albumId;
        private bool _isSync;
        private ResourceManager _res;

        public DeleteItemViewModel(int ItemId, int albumId, bool isSync)
        {
            _ItemId = ItemId;
            _albumId = albumId;
            _isSync = isSync;

            _res = new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
        }

        public async Task<int> DeleteItem()
        {
            int recs = 0;
            try
            {
                if (!_isSync)
                { 
                    bool ans = await App.Current.MainPage.DisplayAlert("Warning!", "Delete item?", "Yes", "No");
                    if (ans)
                    {
                        DateTime date = DateTime.Now;
                        recs = await App.Database.DeleteItemAsync(_ItemId);
                        recs += await App.Database.UpdateAlbumDateAsync(_albumId, date);
                        if (recs > 0)
                        {
                            await App.Current.MainPage.DisplayToastAsync("Success. Item has been deleted", 1500);
                            MessagingCenter.Send<Object, DateTime>(this, "AlbumChanged", date);
                            MessagingCenter.Send<Object>(this, "ItemDeleted");
                        }
                    }
                }
                else
                {
                    var client = new HttpClient();
                    string endpoint = _res.GetString("APIBaseUrl").ToString() + _res.GetString("DeleteItem").ToString();
                    string url = endpoint + $"?id={_ItemId}";
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        recs = 1;
                        await App.Current.MainPage.DisplayToastAsync("Success. Item has been removed from Desktop.", 1500);
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Error", response.ToString(), "Ok");
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
