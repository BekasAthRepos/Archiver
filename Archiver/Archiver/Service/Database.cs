using Archiver.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Service
{
    public class Database
    {
        private const int LAST_VERSION = 2;
        private readonly SQLiteAsyncConnection _connection;

        public Database(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            VersionUpdate();
        }


        // -- System functions

        private Task<int> GetDBVersion()
        {
            return _connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        private Task<int> SetDBVersion(int version)
        {
            return _connection.ExecuteScalarAsync<int>("PRAGMA user_version = " + version);
        }

        // Album functions

        public Task<List<Album>>GetAlbumsAsync()
        {            
            return _connection.Table<Album>().ToListAsync();
        }

        public Task<int> InsertAlbum(Album album)
        {
            return _connection.InsertAsync(album);
        }

        public Task<Album> GetAlbum(int id)
        {
            return _connection.GetAsync<Album>(id);
        }

        public Task<int> UpdateAlbum(Album album)
        {
            return _connection.UpdateAsync(album);
        }

        public Task<int> DeleteAlbum(int id)
        {
            return _connection.DeleteAsync<Album>(id);
        }

        public Task<int> GetItemQty(int id)
        {
            string qItemCount = "select count(*) from Item where AlbumId = ?";
            return _connection.ExecuteScalarAsync<int>(qItemCount, id);         
        }


        // Item functions

        public Task<List<Item>> GetItemsAsync()
        {
            return _connection.Table<Item>().ToListAsync();
        }

        public Task<int> InsertItem(Item item)
        {
            return _connection.InsertAsync(item);
        }

        public Task<Item> GetItem(int id)
        {
            return _connection.GetAsync<Item>(id);
        }

        public Task<int> UpdateItem(Item item)
        {
            return _connection.UpdateAsync(item);
        }

        public Task<int> DeleteItem(int id)
        {
            return _connection.DeleteAsync(id);
        }


        // - Database Versions

        //Version Update
        private async void VersionUpdate()
        {
            int userVersion = await GetDBVersion();

            if (userVersion < LAST_VERSION)
            {
                if (userVersion == 0)
                {
                    BuildVersion1();
                    userVersion = await GetDBVersion();
                }

                if (userVersion == 1)
                {
                    userVersion = BuildVersion2();
                    userVersion = await GetDBVersion();
                }
                /*
                if (userVersion == 2)
                {
                    BuildVersion3();
                    userVersion = await GetDBVersion();
                }

                if (userVersion == 3)
                {
                    BuildVersion4();
                    userVersion = await GetDBVersion();
                } */
            
            }
        }
        
        // Version 1 - 1.0.0
        private int BuildVersion1() 
        {
            int newVersion = -1;
            try {
                _connection.CreateTableAsync<Album>();
                _connection.CreateTableAsync<Item>();
                SetDBVersion(1);
                newVersion = 1;
                App.Current.MainPage.DisplayAlert("success", "database version 1", "Ok");
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Error in database version 1", "Ok");
            }
            
            return newVersion;
        }

        // Version 2 - 1.0.1
        private int BuildVersion2()
        {
            int newVersion = -1;
            try
            {
                SetDBVersion(2);
                newVersion = 2;
                App.Current.MainPage.DisplayAlert("success", "database version 2", "Ok");
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Error in database version 2", "Ok");
            }

            return newVersion;
        }

        // Version 3 - 1.0.2
        private int BuildVersion3()
        {
            int newVersion = -1;
            try
            {
                SetDBVersion(3);
                newVersion = 3;
                App.Current.MainPage.DisplayAlert("success", "database version 3", "Ok");
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Error in database version 3", "Ok");
            }

            return newVersion;
        }

        private int BuildVersion4()
        {
            int newVersion = -1;
            try
            {
                SetDBVersion(4);
                newVersion = 4;
                App.Current.MainPage.DisplayAlert("success", "database version 4", "Ok");
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Error in database version 4", "Ok");
            }

            return newVersion;
        }
    }
}
