using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ado.net
{
    public static class Extension
    {
        public static bool IsVAlidChoice(this char symbol)
        {
            if (symbol.ToString().ToLower().Equals("y") || symbol.ToString().ToLower().ToLower().Equals("n"))
                return true;


            return false;
        }
    }
}
