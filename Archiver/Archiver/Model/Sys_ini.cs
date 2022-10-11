using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archiver.Model
{
    public class Sys_ini
    {
        // database array for app data and info
        [MaxLength(50)]
        public string Entry {get; set;}
        [MaxLength(50)]
        public string Value { get; set; }

        public Sys_ini() {}

        public Sys_ini(string entry, string value)
        {
            Entry = entry;
            Value = value;
        }
    }
}
