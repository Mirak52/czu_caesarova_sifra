﻿using System;
using System.Collections.Generic;
using System.Text;

namespace caesarova_sifra_czu.models
{
    public class Numbers
    {
        //kontrole sudého lichého čísla
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
