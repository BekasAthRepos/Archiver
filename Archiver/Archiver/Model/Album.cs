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
        //public DateTime InputDate { get; set; }
        //public DateTime EditDate { get; set; }
        //public List<Item> Items { get; set; }

        public Album() { }

        public Album(int id, string name, string description)
        {
            //, DateTime inputDate, DateTime editDate, List<Item> items
            Id = id;
            Name = name;
            Description = description;
            //this.InputDate = inputDate;
            //this.EditDate = editDate;
            //this.Items = items;
        }

        public Album Clone()
        {
            return MemberwiseClone() as Album;
        }
    }
}
