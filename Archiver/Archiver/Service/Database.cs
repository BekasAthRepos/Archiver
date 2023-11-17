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
            Task.Run(async () => await VersionUpdate());
        }


        // -- System functions

        private async Task<int> GetDBVersionAsync()
        {
            return await _connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        private async Task<int> SetDBVersionAsync(int version)
        {
            return await _connection.ExecuteScalarAsync<int>("PRAGMA user_version = " + version);
        }

        // Album functions

        public async Task<List<Album>> GetAlbumsAsync()
        {            
            return await _connection.Table<Album>().ToListAsync();
        }

        public async Task<int> InsertAlbumAsync(Album album)
        {
            return await _connection.InsertAsync(album);
        }

        public async Task<Album> GetAlbumAsync(int id)
        {
            return await _connection.Table<Album>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAlbumAsync(Album album)
        {
            return await _connection.UpdateAsync(album);
        }

        public async Task<int> DeleteAlbumAsync(int id)
        {
            return await _connection.DeleteAsync<Album>(id);
        }

        public async Task<int> UpdateAlbumDateAsync(int albumId, DateTime date)
        {
            string query = "update Album set UpdateDate = ? where Id = ?;";
            return await _connection.ExecuteAsync(query, date, albumId);
        }


        // Item functions

        public async Task<List<Item>> GetItemsAsync(int albumId)
        {
            string query = "select * from Item where AlbumId = ?";
            return await _connection.QueryAsync<Item>(query, albumId);
        }

        public async Task<int> InsertItemAsync(Item item)
        {
            return await _connection.InsertAsync(item);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await _connection.Table<Item>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            return await _connection.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            return await _connection.DeleteAsync<Item>(id);
        }


        // -- Database Versioning

        //Version Update
        private async Task VersionUpdate()
        { 
            int userVersion = await GetDBVersionAsync();

            if (userVersion < LAST_VERSION)
            {
                if (userVersion == 0)
                {
                    BuildVersion1();
                    userVersion = await GetDBVersionAsync();
                }
                if (userVersion == 1)
                {
                    BuildVersion2();
                    userVersion = await GetDBVersionAsync();
                }
            }
        }
        
        // Version 1 - 1.0.0
        private async void BuildVersion1() 
        {
            try 
            {
                await _connection.CreateTableAsync<Album>();
                await _connection.CreateTableAsync<Item>();
                await SetDBVersionAsync(1);
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }

        // Version 2 - 1.0.1
        private async void BuildVersion2()
        {
            try
            {
                await _connection.CreateTableAsync<Item>();
                await SetDBVersionAsync(2);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }
    }
}
