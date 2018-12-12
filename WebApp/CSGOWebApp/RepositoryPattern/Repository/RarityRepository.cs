using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class RarityRepository
    {
        IRarityContext context;
        public RarityRepository(IRarityContext Context)
        {
            context = Context;
        }
    }
}
