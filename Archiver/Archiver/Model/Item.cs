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
        [MaxLength(200)]
        public string Description {get; set;}
        public double Qty {get; set;}
        //public ImageSource Picture { get; set; }
       // public DateTime InputDate { get; set;}
        //public DateTime EditDate { get; set; }

        public Item() { }

        public Item(int anId, int albumId, string name, string description, double qty)
        {
            //, ImageSource picture, DateTime inputDate, DateTime editDate
            Id = anId;
            AlbumId = albumId;
            Name = name;
            Description = description;
            Qty = qty;
            //this.Picture = picture;
            //this.InputDate = inputDate;
            //this.EditDate = editDate;
        }
    }
}
