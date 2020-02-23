using caesarova_sifra_czu.classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caesarova_sifra_czu.repository
{
    public class CipherSettingsRepository
    {
        public SQLiteAsyncConnection database;

        public void CipherSettingsDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<CipherSettings>().Wait();
        }
       

    }
}
