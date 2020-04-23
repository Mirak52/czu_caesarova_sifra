using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace caesarova_sifra_czu.sqlite
{
    class FileHelper : IFileHelper
    {
        //funkce na vytvoření souboru
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine("", filename);
        }
    }
}
