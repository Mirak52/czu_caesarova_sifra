using System;
using System.Collections.Generic;
using System.Text;

namespace caesarova_sifra_czu.sqlite
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
