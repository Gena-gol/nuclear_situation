using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqliteLib.Model
{
    public class Note
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public double Indication { get; set; }

    }

    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
