using Archiver.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AlbumViewModel
    { 
        public ObservableCollection<Album> Albums { get; set; }

        public AlbumViewModel()
        {   
            Albums = new ObservableCollection<Album>();         
        }

        public async void OnAppearing()
        {
            await ExcLoadAlbumsCmd();
        }

        private async Task ExcLoadAlbumsCmd()
        {
            Albums.Clear();
          
            var albumList = await App.Database.GetAlbumsAsync();

            foreach (var album in albumList)
            {
                Albums.Add(album);
            }
        }
    }
}
