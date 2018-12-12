using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class MoneyfaucetFactory
    {
        public static IMoneyfaucetContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLMoneyfaucetContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLMoneyfaucetContext();
            }
            return null;
        }
    }
}
