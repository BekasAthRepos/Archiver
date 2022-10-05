using Archiver.Model;
using System;
using System.Collections.Generic;
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

        public DeleteAlbumViewModel(int id)
        {
            _id = id;     
        }

        public async Task<int> DeleteAlbum()
        {
            int recs = 0;
            try
            {
                bool ans = await App.Current.MainPage.DisplayAlert("Warning!", "Delete album?", "Yes", "No");
                if(ans)
                {
                    recs = await App.Database.DeleteAlbumAsync(_id);
                    if (recs > 0)
                    {
                        await App.Current.MainPage.DisplayToastAsync("Success. Album has been deleted", 1500);
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
