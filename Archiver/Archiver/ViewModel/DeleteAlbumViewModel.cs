using Archiver.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Archiver.ViewModel
{
    public class DeleteAlbumViewModel
    {
        private int _id;
        private bool _isSync;
        private ResourceManager _res;

        public DeleteAlbumViewModel(int id, bool IsSync)
        {
            _id = id;     
            _isSync = IsSync;
            _res = new ResourceManager("Archiver.Resources.Strings", typeof(DeleteAlbumViewModel).Assembly);
        }

        public async Task<int> DeleteAlbum()
        {
            int recs = 0;
            try
            {
                bool ans = await App.Current.MainPage.DisplayAlert("Warning!", "Delete album?", "Yes", "No");
                if(ans)
                {
                    if(!_isSync)
                    {
                        recs = await App.Database.DeleteAlbumAsync(_id);

                        if (recs > 0)
                        {
                            await App.Current.MainPage.DisplayToastAsync("Success. Album has been deleted", 1500);
                        }
                    }
                    else
                    {
                        var client = new HttpClient();
                        string endpoint = _res.GetString("APIBaseUrl").ToString() + _res.GetString("DeleteAlbum").ToString();
                        string url = endpoint + $"?id={_id}";
                        HttpResponseMessage response = await client.DeleteAsync(url);
                        if (response.IsSuccessStatusCode)
                            await App.Current.MainPage.DisplayToastAsync("Success. Album has been removed from Desktop.", 1500);
                        else
                            await App.Current.MainPage.DisplayAlert("Error", response.ToString(), "Ok");
                    }
                }
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            } 
            
            return recs;
        }
    }
}
