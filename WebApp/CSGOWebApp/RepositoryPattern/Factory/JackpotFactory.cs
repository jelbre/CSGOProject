using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class JackpotFactory
    {
        public static IJackpotContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLJackpotContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLJackpotContext();
            }
            return null;
        }
    }
}
