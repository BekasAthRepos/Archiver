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
        private const int LAST_VERSION = 1;
        private readonly SQLiteAsyncConnection _connection;

        public Database(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            VersionUpdate();
        } 
        

        // -- Database version control

        private Task<int> GetDataBaseVersion()
        {
            return _connection.ExecuteAsync("PRAGMA user_version");
        }

        public Task<int> SetDatabaseVersion(int version)
        {
            return _connection.ExecuteAsync("PRAGMA user_version = " + version);
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


        // -- Database Versions

        //Version Update
        private async void VersionUpdate()
        {
            Version1();
            
            int userVersion = await GetDataBaseVersion();
            if(userVersion < LAST_VERSION)
            {
                
            }
            
        }

        private void Version1()
        {
            _connection.CreateTableAsync<Album>();
            _connection.CreateTableAsync<Item>();
        }
    }
}
