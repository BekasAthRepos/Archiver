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
        private const int LAST_VERSION = 4;
        private readonly SQLiteConnection _connection;

        public Database(string dbPath)
        {
            _connection = new SQLiteConnection(dbPath);
            VersionUpdate();
        }


        // -- System functions

        private int GetDBVersion()
        {
            return _connection.ExecuteScalar<int>("PRAGMA user_version");
        }

        private int SetDBVersion(int version)
        {
            return _connection.ExecuteScalar<int>("PRAGMA user_version = " + version);
        }

        // Album functions

        public List<Album>GetAlbumsAsync()
        {            
            return _connection.Table<Album>().ToList();
        }

        public int InsertAlbum(Album album)
        {
            return _connection.Insert(album);
        }

        public Album GetAlbum(int id)
        {
            return _connection.Get<Album>(id);
        }

        public int UpdateAlbum(Album album)
        {
            return _connection.Update(album);
        }

        public int DeleteAlbum(int id)
        {
            return _connection.Delete<Album>(id);
        }

        public int GetItemQty(int id)
        {
            string qItemCount = "select count(*) from Item where AlbumId = ?";
            return _connection.ExecuteScalar<int>(qItemCount, id);         
        }


        // Item functions

        public List<Item> GetItemsAsync()
        {
            return _connection.Table<Item>().ToList();
        }

        public int InsertItem(Item item)
        {
            return _connection.Insert(item);
        }

        public Item GetItem(int id)
        {
            return _connection.Get<Item>(id);
        }

        public int UpdateItem(Item item)
        {
            return _connection.Update(item);
        }

        public int DeleteItem(int id)
        {
            return _connection.Delete(id);
        }


        // -- Database Versioning

        //Version Update
        private void VersionUpdate()
        { 
            int userVersion = GetDBVersion();

            if (userVersion < LAST_VERSION)
            {
                if (userVersion == 0)
                {
                    BuildVersion1();
                    userVersion = GetDBVersion();
                }
            }
        }
        
        // Version 1 - 1.0.0
        private void BuildVersion1() 
        {
            try 
            {
                _connection.CreateTable<Album>();
                _connection.CreateTable<Item>();
                SetDBVersion(1);
            }
            catch
            {
            }
        }
    }
}
