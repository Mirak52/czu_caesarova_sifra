using caesarova_sifra_czu.classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caesarova_sifra_czu.repository
{
    //třída pro práci s databází
    public class CipherSettingsRepository
    {
        public SQLiteAsyncConnection database;

        public CipherSettingsRepository(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<CipherSettings>().Wait();
        }
        public Task<int> SaveItemAsync(CipherSettings item)
        {
            return database.InsertAsync(item);
        }

        //Select podle jména, ochrana proti sqlinjection
        public Task<List<CipherSettings>> SelectByName(string name)
        {
            return database.QueryAsync<CipherSettings>("select * FROM [CipherSettings] where name  = @name",name);
        }
        //selectne všechno
        public Task<List<CipherSettings>> Select()
        {
            return database.QueryAsync<CipherSettings>("select * FROM [CipherSettings]");
        }
    }
}
