using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Archiver.Model
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AlbumId {get; set;}
        [MaxLength(50)]
        public string Name {get; set;}
        [MaxLength(400)]
        public string Description {get; set;}
        public double Qty {get; set;}
        public string imgPath { get; set; } 
        public DateTime InputDate { get; set;}
        public DateTime UpdateDate { get; set; }

        public Item() 
        {
            InputDate = DateTime.Now;
            UpdateDate = InputDate;
        }

        public void UpdateItem()
        {
            UpdateDate = DateTime.Now;
        }
    }
}
