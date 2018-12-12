using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class BetFactory
    {
        public static IBetContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLBetContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLBetContext();
            }
            return null;
        }
    }
}
