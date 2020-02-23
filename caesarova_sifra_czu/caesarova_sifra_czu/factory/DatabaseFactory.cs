using caesarova_sifra_czu.repository;
using caesarova_sifra_czu.sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace caesarova_sifra_czu.factory
{
    public class DatabaseFactory
    {
        public static CipherSettingsRepository _Item;

        public static CipherSettingsRepository DatabaseCipherSettings
        {
            get
            {
                if (_Item == null)
                {
                    var fileHelper = new FileHelper();
                    _Item = new CipherSettingsRepository(fileHelper.GetLocalFilePath("Database.db3"));
                }
                return _Item;
            }
        }
    }
}
