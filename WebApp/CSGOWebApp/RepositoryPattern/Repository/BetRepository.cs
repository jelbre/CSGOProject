using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class BetRepository
    {
        IBetContext context;
        public BetRepository(IBetContext Context)
        {
            context = Context;
        }
    }
}
