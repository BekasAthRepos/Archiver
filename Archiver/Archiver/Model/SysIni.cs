using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archiver.Model
{
    public class SysIni
    {
        // class for system settings, values, versions, etc...

        [PrimaryKey][AutoIncrement]
        public int Id { get; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(50)]
        public string Value { get; set; }

        public SysIni() { }

        public SysIni(string code, string value)
        {
            Code = code;
            Value = value;
        }
    }
}
