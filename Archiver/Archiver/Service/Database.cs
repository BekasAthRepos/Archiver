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

        private Task<int> GetDBVersionAsync()
        {
            return _connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        private Task<int> SetDBVersionAsync(int version)
        {
            return _connection.ExecuteScalarAsync<int>("PRAGMA user_version = " + version);
        }

        // Album functions

        public Task<List<Album>> GetAlbumsAsync()
        {            
            return _connection.Table<Album>().ToListAsync();
        }

        public Task<int> InsertAlbumAsync(Album album)
        {
            return _connection.InsertAsync(album);
        }

        public Task<Album> GetAlbumAsync(int id)
        {
            return _connection.Table<Album>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAlbumAsync(Album album)
        {
            return _connection.UpdateAsync(album);
        }

        public Task<int> DeleteAlbumAsync(int id)
        {
            return _connection.DeleteAsync<Album>(id);
        }

        public Task<int> UpdateAlbumDateAsync(int albumId, DateTime date)
        {
            string query = "update Album set UpdateDate = ? where Id = ?;";
            return _connection.ExecuteAsync(query, date, albumId);
        }


        // Item functions

        public Task<List<Item>> GetItemsAsync(int albumId)
        {
            string query = "select * from Item where AlbumId = ?";
            return _connection.QueryAsync<Item>(query, albumId);
        }

        public Task<int> InsertItemAsync(Item item)
        {
            return _connection.InsertAsync(item);
        }

        public Task<Item> GetItemAsync(int id)
        {
            return _connection.Table<Item>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateItemAsync(Item item)
        {
            return _connection.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(int id)
        {
            return _connection.DeleteAsync<Item>(id);
        }


        // -- Database Versioning

        //Version Update
        private async void VersionUpdate()
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
        private void BuildVersion1() 
        {
            try 
            {
                _connection.CreateTableAsync<Album>();
                _connection.CreateTableAsync<Item>();
                SetDBVersionAsync(1);
            }
            catch
            {
            }
        }

        // Version 2 - 1.0.1
        private void BuildVersion2()
        {
            try
            {
                _connection.CreateTableAsync<Item>();
                SetDBVersionAsync(2);
                //string sql = "alter table Item add ImgPath varchar";
                //int affRows = _connection.ExecuteScalar<int>(sql);
                //if (affRows > 0)
                //    SetDBVersion(2);
            }
            catch
            {
            }
        }
    }
}
