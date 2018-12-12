using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class RarityFactory
    {
        public static IRarityContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLRarityContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLRarityContext();
            }
            return null;
        }
    }
}
