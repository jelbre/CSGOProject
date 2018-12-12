using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class CoinflipFactory
    {
        public static ICoinflipContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLCoinflipContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLCoinflipContext();
            }
            return null;
        }
    }
}
