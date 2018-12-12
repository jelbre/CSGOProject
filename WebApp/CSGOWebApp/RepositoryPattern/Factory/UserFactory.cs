using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class UserFactory
    {
        public static IUserContext GetContext(ContextType contextType)
        {
            switch(contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLUserContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLUserContext();
            }
            return null;
        }
    }
}
