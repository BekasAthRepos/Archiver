using Archiver.Model;
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

        private Task ExcLoadAlbumsCmd()
        {
            Albums.Clear();
          
            var albumList = App.Database.GetAlbums();
            foreach (var album in albumList)
            {
                Albums.Add(album);
            }

            return Task.CompletedTask;
        }
    }
}
