using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Archiver.Model
{
    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime InputDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Album() 
        {
            InputDate = DateTime.Now;
            UpdateDate = InputDate;
        }

        public void UpdateAlbum()
        {
            UpdateDate = DateTime.Now;
        }
    }
}
