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
        private const int LAST_VERSION_COUNT = 1;  // ανεβαίνει χειροκίνητα κατά 1 σε κάθε ενημέρωση 
        private readonly SQLiteAsyncConnection _connection;

        public Database(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            Task.Run(async () => await VersionUpdate());
        }


        // -- System functions

        private async Task<int> GetDBVersionCountAsync()
        {
            return await _connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        private async Task<int> SetDBVersionCountAsync(int version)
        {
            return await _connection.ExecuteScalarAsync<int>("PRAGMA user_version = " + version);
        }

        public async Task<bool> InsertSysIniAsync(SysIni ini)
        {
            return await _connection.InsertAsync(ini) == 1;
        }

        public async Task<SysIni> GetSysIniAsync(string Code)
        {
            return await _connection.Table<SysIni>().Where(ini => ini.Code == Code).FirstOrDefaultAsync();
        }

        public async Task<SysIni> UpdateSysIniAsync(SysIni ini)
        {
            return await _connection.ExecuteScalarAsync<SysIni>("update SysIni set Value = ? where Code = ?", ini.Value, ini.Code);
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
            int userVersionCount = await GetDBVersionCountAsync();
            if (userVersionCount < LAST_VERSION_COUNT)
            {
                if (userVersionCount == 0)
                {
                    BuildVersionCount1();
                    userVersionCount = await GetDBVersionCountAsync();
                }
                /*
                if (userVersion == 1)
                {
                    BuildVersion2();
                    userVersion = await GetDBVersionAsync();
                }
                */
            }
        }
        
        // Version Count 1 - v1.1.0
        private async void BuildVersionCount1() 
        {
            try 
            {
                _connection.CreateTableAsync<Album>().Wait();
                _connection.CreateTableAsync<Item>().Wait();
                _connection.CreateTableAsync<SysIni>().Wait();
                await InsertSysIniAsync(new SysIni { Code = "AdInterCount", Value = "0" });
                await InsertSysIniAsync(new SysIni { Code = "AdRewardCount", Value = "0" });
                await InsertSysIniAsync(new SysIni { Code = "Version", Value = "1.1.0" });
                await SetDBVersionCountAsync(1);
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }

        // Version 2 - 1.1.1
        /*
        private async void BuildVersionCount2()
        {
            try
            {
                // changes
                await SetDBVersionCountAsync(2);
                await UpdateSysIniAsync(new SysIni { Code = "Version", Value = "1.1.1" });
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "Ok");
            }
        }
        */

    }
}
