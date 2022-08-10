using Archiver.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AlbumViewModel
    { 
        public ObservableCollection<Album> Albums { get; set; }
        private Command LoadAlbumsCmd;

        public AlbumViewModel()
        {   
            Albums = new ObservableCollection<Album>();
            LoadAlbumsCmd = new Command(async () => await ExcLoadAlbumsCmd());          
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
