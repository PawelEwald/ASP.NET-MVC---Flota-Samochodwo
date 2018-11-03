using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracaWSIZ.Helpers
{
    public class RefuelingHelper
    {
        public bool IsMilageOk(int? previous, int? current)
        {
            if (previous < current || current == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}