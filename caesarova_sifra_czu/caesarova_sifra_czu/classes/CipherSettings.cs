using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace caesarova_sifra_czu.classes
{
    public class CipherSettings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string hash { get; set; }
        public int index { get; set; }
    }
}
