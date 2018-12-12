using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class InventoryFactory
    {
        public static IInventoryContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLInventoryContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLInventoryContext();
            }
            return null;
        }
    }
}
