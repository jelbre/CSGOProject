using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class SkinFactory
    {
        public static ISkinContext GetContext(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.MSSQL:
                    return new MSSQLSkinContext();

                case ContextType.Test:
                    //Veranderen als aangemaakt
                    return new MSSQLSkinContext();
            }
            return null;
        }
    }
}
