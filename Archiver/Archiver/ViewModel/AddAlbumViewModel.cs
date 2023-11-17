using Archiver.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Archiver.ViewModel
{
    public class AddAlbumViewModel
    {
        public Album NewAlbum {get; set;}
        public ICommand AddAlbumCmd => new Command(AddAlbum);
        private readonly bool _isSync;
        private readonly ResourceManager _res;

        public AddAlbumViewModel(bool IsSync) 
        {
            _isSync = IsSync;
            NewAlbum = new Album();
            _res = new ResourceManager("Archiver.Resources.Strings", typeof(AddAlbumViewModel).Assembly);
        }

        private async void AddAlbum()
        {
            bool validInputData = true;

            if((string.IsNullOrWhiteSpace(NewAlbum.Name)))
            {
                validInputData = false;
                await App.Current.MainPage.DisplayAlert("Give a name...", "", "ok");
            }

            if (validInputData)
            {
                try
                {
                    if (!_isSync)
                    {
                        int rows = await App.Database.InsertAlbumAsync(NewAlbum);
                        if (rows > 0)
                            await App.Current.MainPage.DisplayToastAsync("Success. Album has been added.", 1500);
                    }else
                    {
                        var client = new HttpClient();
                        string jsonData = JsonConvert.SerializeObject(NewAlbum);
                        StringContent contnet = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        string endpoint = _res.GetString("APIBaseUrl").ToString() + _res.GetString("CreateAlbum").ToString();
                        HttpResponseMessage response = await client.PostAsync(endpoint, contnet);
                        if (response.IsSuccessStatusCode)
                            await App.Current.MainPage.DisplayToastAsync("Success. Album has been uploaded.", 1500);
                        else
                            await App.Current.MainPage.DisplayAlert("Error", response.ToString(), "Ok");
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
}
