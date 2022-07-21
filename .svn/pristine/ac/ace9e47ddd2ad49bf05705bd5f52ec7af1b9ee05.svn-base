using Archiver.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class DeleteAlbumViewModel
    {
        private int _id;

        public DeleteAlbumViewModel() { }

        public DeleteAlbumViewModel(int id)
        {
            _id = id;     
        }

        public async Task<int> DeleteAlbum()
        {
            int items = 0;
            try
            {
                bool ans = await App.Current.MainPage.DisplayAlert("Warning!", "Delete album?", "Yes", "No");
                if(ans)
                {
                    items = await App.Database.DeleteAlbum(_id);
                    if (items > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Album was deleted", "Ok");
                    }
                }
                
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            } 
            
            return items;
        }
    }
}
